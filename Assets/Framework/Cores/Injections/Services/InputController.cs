using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dada.Cores;

/// <summary>
/// 框架层输入控制器 — 观察者模式实现。
///
/// 每帧通过 InputDriver.Update 读取原始输入：
///   持续状态（Move/MousePos/Held）→ 更新属性
///   瞬时输入（wasPressedThisFrame/wasReleasedThisFrame）→ 触发事件
///
/// ReadInput / ReadKeyboard / ReadMouse 为 virtual，游戏层可覆写。
/// </summary>
public class InputController : IInputController
{
    private readonly InputBindings _bindings;
    private readonly GameObject _root;
    private readonly InputDriver _driver;

    // ── 持续状态 ──────────────────────────────────

    private Vector2 _move;
    private Vector2 _look;
    private float _zoom;
    private Vector2 _mouseScreenPos;

    private bool _sprintHeld;
    private bool _interactHeld;
    private bool _leftMouseHeld;
    private bool _rightMouseHeld;
    private bool _middleMouseHeld;

    private bool _interactWasHeld;

    public Vector2 Move => _move;
    public Vector2 Look => _look;
    public float Zoom => _zoom;
    public Vector2 MouseScreenPos => _mouseScreenPos;
    public bool SprintHeld => _sprintHeld;
    public bool InteractHeld => _interactHeld;
    public bool LeftMouseHeld => _leftMouseHeld;
    public bool RightMouseHeld => _rightMouseHeld;
    public bool MiddleMouseHeld => _middleMouseHeld;

    // ── 事件 ──────────────────────────────────────

    public event Action OnInteractPressed;
    public event Action OnInteractReleased;
    public event Action OnCancelPressed;
    public event Action OnMenuPressed;
    public event Action OnPausePressed;

    public event Action OnLeftMousePressed;
    public event Action OnRightMousePressed;
    public event Action OnMiddleMousePressed;

    public event Action<int> OnTimeSpeedPressed;

    public event Action OnAction1Pressed;
    public event Action OnAction2Pressed;
    public event Action OnAction3Pressed;
    public event Action OnAction4Pressed;
    public event Action OnAction5Pressed;
    public event Action OnAction6Pressed;
    public event Action OnAction7Pressed;

    public InputBindings Bindings => _bindings;

    public InputController(InputBindings bindings = null)
    {
        _bindings = bindings ?? InputBindings.Default;

        _root = new GameObject("InputController");
        UnityEngine.Object.DontDestroyOnLoad(_root);

        _driver = _root.AddComponent<InputDriver>();
        _driver.OnUpdate += ReadInput;
    }

    public void Dispose()
    {
        _driver.OnUpdate -= ReadInput;
        UnityEngine.Object.Destroy(_root);
    }

    // ── 每帧读取 ──────────────────────────────────

    protected virtual void ReadInput(float dt)
    {
        var kb = Keyboard.current;
        var mouse = Mouse.current;

        // 每帧清零持续值
        _move = Vector2.zero;
        _look = Vector2.zero;
        _zoom = 0f;

        if (kb != null) ReadKeyboard(kb);
        if (mouse != null) ReadMouse(mouse);
    }

    protected virtual void ReadKeyboard(Keyboard kb)
    {
        // ── 移动 ──
        if (kb[_bindings.MoveUp].isPressed)    _move.y += 1;
        if (kb[_bindings.MoveDown].isPressed)  _move.y -= 1;
        if (kb[_bindings.MoveLeft].isPressed)  _move.x -= 1;
        if (kb[_bindings.MoveRight].isPressed) _move.x += 1;

        // ── 视角（方向键平移） ──
        if (kb[_bindings.LookUp].isPressed)    _look.y += 1;
        if (kb[_bindings.LookDown].isPressed)  _look.y -= 1;
        if (kb[_bindings.LookLeft].isPressed)  _look.x -= 1;
        if (kb[_bindings.LookRight].isPressed) _look.x += 1;

        // ── 交互键（Held 状态 + Press/Release 事件） ──
        bool fHeld = kb[_bindings.Interact].isPressed;
        _interactHeld = fHeld;

        if (fHeld && !_interactWasHeld)
            OnInteractPressed?.Invoke();
        if (!fHeld && _interactWasHeld)
            OnInteractReleased?.Invoke();

        _interactWasHeld = fHeld;

        // ── 功能键（wasPressedThisFrame → 事件） ──
        if (kb[_bindings.Cancel].wasPressedThisFrame) OnCancelPressed?.Invoke();
        if (kb[_bindings.Menu].wasPressedThisFrame)   OnMenuPressed?.Invoke();
        if (kb[_bindings.Pause].wasPressedThisFrame)  OnPausePressed?.Invoke();

        _sprintHeld = kb[_bindings.Sprint].isPressed;

        // ── 时间速度 ──
        var tsKeys = _bindings.GetTimeSpeedKeys();
        for (int i = 0; i < tsKeys.Count; i++)
        {
            if (kb[tsKeys[i]].wasPressedThisFrame)
                OnTimeSpeedPressed?.Invoke(i + 1);
        }

        // ── 动作键 ──
        if (kb[_bindings.Action1].wasPressedThisFrame) OnAction1Pressed?.Invoke();
        if (kb[_bindings.Action2].wasPressedThisFrame) OnAction2Pressed?.Invoke();
        if (kb[_bindings.Action3].wasPressedThisFrame) OnAction3Pressed?.Invoke();
        if (kb[_bindings.Action4].wasPressedThisFrame) OnAction4Pressed?.Invoke();
        if (kb[_bindings.Action5].wasPressedThisFrame) { Debug.Log("[Input] E (Action5) pressed"); OnAction5Pressed?.Invoke(); }
        if (kb[_bindings.Action6].wasPressedThisFrame) OnAction6Pressed?.Invoke();
        if (kb[_bindings.Action7].wasPressedThisFrame) OnAction7Pressed?.Invoke();
    }

    protected virtual void ReadMouse(Mouse mouse)
    {
        _zoom = mouse.scroll.ReadValue().y;
        _mouseScreenPos = mouse.position.ReadValue();

        _leftMouseHeld   = mouse.leftButton.isPressed;
        _rightMouseHeld  = mouse.rightButton.isPressed;
        _middleMouseHeld = mouse.middleButton.isPressed;

        if (mouse.leftButton.wasPressedThisFrame)   OnLeftMousePressed?.Invoke();
        if (mouse.rightButton.wasPressedThisFrame)  OnRightMousePressed?.Invoke();
        if (mouse.middleButton.wasPressedThisFrame) OnMiddleMousePressed?.Invoke();
    }

    // ── Internal driver ────────────────────────────

    internal class InputDriver : MonoBehaviour
    {
        public event Action<float> OnUpdate;
        private void Update() => OnUpdate?.Invoke(Time.deltaTime);
    }
}
