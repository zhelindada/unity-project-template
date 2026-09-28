using UnityEngine;

namespace Dada.Cores;

/// <summary>
/// 框架层相机控制器 — 提供俯视角/策略类相机的默认实现。
/// 通过 IInputController 读取输入，与具体输入方案解耦。
/// 核心方法均为 virtual，游戏层可通过继承定制相机行为。
/// 如需完全不同的相机模式（FPS/TPS），实现 ICameraController 即可。
/// </summary>
public class CameraController : ICameraController
{
    // ── Configuration ──────────────────────────────
    protected readonly CameraSettings _settings;
    protected readonly IInputController _input;
    protected readonly GameObject _root;
    protected readonly Transform _yawPivot;
    protected readonly Transform _pitchPivot;
    protected readonly Transform _zoomArm;
    protected readonly Camera _camera;
    private readonly CameraDriver _driver;

    // ── Runtime state ──────────────────────────────
    private Transform _followTarget;
    private Vector3 _targetWorldPos;
    protected float _currentZoom;
    protected float _currentYaw;
    protected float _targetZoom;
    protected float _targetYaw;
    private Vector3 _velocity;
    private Bounds? _bounds;
    private Vector2 _dragOrigin;
    private bool _isDragging;

    // ── Properties ─────────────────────────────────

    public UnityEngine.Camera Camera => _camera;

    public float Zoom
    {
        get => _currentZoom;
        set => _targetZoom = Mathf.Clamp(value, _settings.minZoom, _settings.maxZoom);
    }

    public float Yaw
    {
        get => _currentYaw;
        set => _targetYaw = value;
    }

    /// <summary>
    /// @param settings 相机参数，null 则使用默认值
    /// @param input 输入控制器（框架通过 DI 注入）
    /// </summary>
    public CameraController(CameraSettings settings = null, IInputController input = null)
    {
        _settings = settings ?? new CameraSettings();
        _input = input;

        if (_input != null)
            _input.OnMiddleMousePressed += OnMiddleDragStart;

        _root = new GameObject("CameraRig");
        Object.DontDestroyOnLoad(_root);

        _yawPivot = new GameObject("YawPivot").transform;
        _yawPivot.SetParent(_root.transform, false);

        _pitchPivot = new GameObject("PitchPivot").transform;
        _pitchPivot.SetParent(_yawPivot, false);
        _pitchPivot.localRotation = Quaternion.Euler(_settings.pitchAngle, 0f, 0f);

        _zoomArm = new GameObject("ZoomArm").transform;
        _zoomArm.SetParent(_pitchPivot, false);
        _zoomArm.localPosition = Vector3.back * _settings.defaultZoom;

        _camera = new GameObject("MainCamera").AddComponent<Camera>();
        _camera.transform.SetParent(_zoomArm, false);
        _camera.transform.localPosition = Vector3.zero;
        _camera.transform.localRotation = Quaternion.identity;
        _camera.nearClipPlane = 0.3f;
        _camera.farClipPlane = 1000f;

        if (_settings.orthographic)
        {
            _camera.orthographic = true;
            _camera.orthographicSize = _settings.defaultZoom;
        }

        _targetZoom  = _settings.defaultZoom;
        _currentZoom = _settings.defaultZoom;
        _targetYaw   = _settings.defaultYaw;
        _currentYaw  = _settings.defaultYaw;

        _driver = _root.AddComponent<CameraDriver>();
        _driver.OnUpdate     += DriverUpdate;
        _driver.OnLateUpdate += DriverLateUpdate;
    }

    public void Follow(Transform target)
    {
        _followTarget = target;
        if (target != null)
            _targetWorldPos = target.position;
    }

    public void StopFollow()
    {
        _followTarget = null;
    }

    public void MoveTo(Vector3 worldPosition)
    {
        _targetWorldPos = worldPosition;
    }

    public void SetBounds(Bounds bounds)
    {
        _bounds = bounds;
    }

    public void ClearBounds()
    {
        _bounds = null;
    }

    public void Dispose()
    {
        if (_input != null)
            _input.OnMiddleMousePressed -= OnMiddleDragStart;
        _driver.OnUpdate     -= DriverUpdate;
        _driver.OnLateUpdate -= DriverLateUpdate;
        Object.Destroy(_root);
    }

    // ── Driver callbacks ──────────────────────────

    private void DriverUpdate(float dt)
    {
        ProcessInput(dt);
    }

    private void DriverLateUpdate(float dt)
    {
        ApplyZoom(dt);
        ApplyYaw(dt);
        UpdateTargetPosition(dt);
        ApplyPosition(dt);
        ApplyBounds();
    }

    // ── Input (virtual — 游戏层可覆写) ───────────

    /// <summary>
    /// 从 IInputController 读取输入并驱动相机。覆写以添加游戏特有的相机输入。
    /// </summary>
    protected virtual void ProcessInput(float dt)
    {
        if (_input == null) return;

        // 滚轮缩放
        float scroll = _input.Zoom;
        if (Mathf.Abs(scroll) > 0.001f)
        {
            _targetZoom -= scroll * _settings.zoomSpeed * (_currentZoom * 0.1f);
            _targetZoom = Mathf.Clamp(_targetZoom, _settings.minZoom, _settings.maxZoom);
        }

        // 摄像机旋转已禁用 — Q/E 由游戏层 SelectionController 接管

        // WASD / 方向键平移
        var panDir = _input.Move + _input.Look;
        if (panDir != Vector2.zero)
            ApplyPan(panDir, dt);

        // 边缘滚动
        if (_settings.edgePanEnabled)
        {
            ProcessEdgePan(_input.MouseScreenPos, dt);
        }

        // 中键拖拽
        ProcessMiddleDrag(_input, dt);
    }

    /// <summary>
    /// 边缘滚动检测 — 覆盖此方法以自定义边缘行为。
    /// </summary>
    protected virtual void ProcessEdgePan(Vector2 screenPos, float dt)
    {
        float edge = _settings.edgePanThreshold;
        bool panLeft  = screenPos.x <= edge;
        bool panRight = screenPos.x >= Screen.width - edge;
        bool panDown  = screenPos.y <= edge;
        bool panUp    = screenPos.y >= Screen.height - edge;

        if (!panLeft && !panRight && !panDown && !panUp) return;

        var edgeDir = Vector2.zero;
        if (panLeft)  edgeDir.x -= 1;
        if (panRight) edgeDir.x += 1;
        if (panDown)  edgeDir.y -= 1;
        if (panUp)    edgeDir.y += 1;

        ApplyPan(edgeDir, dt);
    }

    /// <summary>
    /// 中键拖拽 — 覆盖此方法以自定义拖拽行为。
    /// </summary>
    protected virtual void ProcessMiddleDrag(IInputController input, float dt)
    {
        var screenPos = input.MouseScreenPos;

        if (input.MiddleMouseHeld && _isDragging)
        {
            Vector3 delta = _dragOrigin - screenPos;
            if (delta.magnitude > 0.5f)
            {
                delta *= _currentZoom * 0.002f;
                var camRight   = _camera.transform.right;
                var camForward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
                _targetWorldPos += camRight * delta.x + camForward * delta.y;
                _dragOrigin = screenPos;
            }
        }

        if (!input.MiddleMouseHeld)
            _isDragging = false;
    }

    private void OnMiddleDragStart()
    {
        _dragOrigin = _input.MouseScreenPos;
        _isDragging = true;
    }

    protected virtual void ApplyPan(Vector2 direction, float dt)
    {
        float speed = _settings.panSpeed * _currentZoom * dt;
        var camRight   = _camera.transform.right;
        var camForward = Vector3.ProjectOnPlane(_camera.transform.forward, Vector3.up).normalized;
        _targetWorldPos += (camRight * direction.x + camForward * direction.y).normalized * speed;
    }

    // ── Zoom ───────────────────────────────────────

    protected virtual void ApplyZoom(float dt)
    {
        _currentZoom = Mathf.Lerp(_currentZoom, _targetZoom, _settings.zoomDamping * dt);
        if (_settings.orthographic)
        {
            _camera.orthographicSize = _currentZoom;
        }
        else
        {
            _zoomArm.localPosition = Vector3.back * _currentZoom;
        }
    }

    // ── Yaw ────────────────────────────────────────

    protected virtual void ApplyYaw(float dt)
    {
        _currentYaw = Mathf.Lerp(_currentYaw, _targetYaw, _settings.rotateDamping * dt);
        if (Mathf.Abs(_currentYaw - _targetYaw) < 0.01f)
            _currentYaw = _targetYaw;
        _yawPivot.rotation = Quaternion.Euler(0f, _currentYaw, 0f);
    }

    // ── Position ───────────────────────────────────

    protected virtual void UpdateTargetPosition(float dt)
    {
        if (_followTarget == null) return;

        Vector3 targetPos = _followTarget.position;

        if (_settings.deadZoneRadius > 0f)
        {
            Vector3 toTarget = targetPos - _targetWorldPos;
            toTarget.y = 0f;
            if (toTarget.magnitude > _settings.deadZoneRadius)
            {
                float excess = toTarget.magnitude - _settings.deadZoneRadius;
                _targetWorldPos += toTarget.normalized * excess;
            }
        }
        else
        {
            _targetWorldPos = Vector3.Lerp(_targetWorldPos, targetPos, 0.5f);
        }
    }

    protected virtual void ApplyPosition(float dt)
    {
        Vector3 next = Vector3.SmoothDamp(
            _root.transform.position,
            _targetWorldPos,
            ref _velocity,
            _settings.moveSmoothTime,
            float.MaxValue,
            dt);

        _root.transform.position = next;
    }

    protected virtual void ApplyBounds()
    {
        if (_bounds == null) return;

        var b = _bounds.Value;
        Vector3 pos = _root.transform.position;

        float halfHeight, halfWidth;
        if (_settings.orthographic)
        {
            halfHeight = _currentZoom;
            halfWidth  = halfHeight * _camera.aspect;
        }
        else
        {
            halfHeight = _currentZoom * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            halfWidth  = halfHeight * _camera.aspect;
        }

        pos.x = Mathf.Clamp(pos.x, b.min.x + halfWidth,  b.max.x - halfWidth);
        pos.z = Mathf.Clamp(pos.z, b.min.z + halfHeight, b.max.z - halfHeight);
        _root.transform.position = pos;
    }

    // ── Internal driver ────────────────────────────

    internal class CameraDriver : MonoBehaviour
    {
        public event System.Action<float> OnUpdate;
        public event System.Action<float> OnLateUpdate;

        private void Update()      => OnUpdate?.Invoke(Time.deltaTime);
        private void LateUpdate()  => OnLateUpdate?.Invoke(Time.deltaTime);
    }

    public class CameraSettings
    {
        public float pitchAngle   = 55f;
        public float defaultZoom  = 20f;
        public float minZoom      = 5f;
        public float maxZoom      = 50f;
        public float defaultYaw;

        public float zoomSpeed    = 1f;
        public float zoomDamping  = 8f;
        public float rotateSpeed  = 90f;
        public float rotateDamping = 10f;
        public float panSpeed     = 1.5f;
        public float moveSmoothTime = 0.3f;

        public bool edgePanEnabled = true;
        public float edgePanThreshold = 8f;
        public float deadZoneRadius;

        /// <summary>
        /// 正交模式 — top-down 2D 视角
        /// </summary>
        public bool orthographic = false;
    }
}
