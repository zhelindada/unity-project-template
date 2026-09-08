using System;
using System.Collections.Generic;
using System.Linq;
using Dada.Foundations;
using UnityEngine;
using UnityEngine.UI;

namespace Dada.Cores
{
    /// <summary>
    /// 全局UI管理器。层级化Canvas管理、模块生命周期、对象池、通知队列、ESC栈处理。
    ///
    /// 用法：
    ///   1. 启动时注册各模块Prefab：UIManager.Instance.RegisterPrefab(myPopupPrefab);
    ///   2. 打开UI：var popup = UIManager.Instance.Show&lt;PopupUI&gt;(data);
    ///   3. 快捷方法：UIManager.Instance.Notify("操作成功");
    ///   4. ESC关闭顶层UI由管理器自动处理。
    /// </summary>
    public class UIManager : MonoSingleton<UIManager>
    {
        [Serializable]
        public class LayerSetup
        {
            public UILayerType Type;
            public Canvas Canvas;
            public int BaseSortOrder;
            public bool IsModal;
        }

        [Header("层级配置（留空则自动创建）")]
        [SerializeField] private LayerSetup[] _layerSetups;

        [Header("预设注册（可选）")]
        [SerializeField] private List<UIModule> _initialPrefabs = new();

        // 内部状态
        private readonly Dictionary<UILayerType, Canvas> _canvases = new();
        private readonly Dictionary<UILayerType, Stack<UIModule>> _stacks = new();
        private readonly Dictionary<Type, UIModule> _prefabs = new();
        private readonly List<UIModule> _activeModules = new();
        private UIPool _pool;
        private GameObject _poolRoot;

        // 通知队列
        private readonly Queue<NotificationUI> _pendingNotifications = new();
        private NotificationUI _activeNotification;

        // 单例Tooltip
        private TooltipUI _activeTooltip;

        // 事件
        public event Action<UIModule> ModuleOpened;
        public event Action<UIModule> ModuleClosed;

        // ==================== 初始化 ====================

        protected override void OnAwake()
        {
            base.OnAwake();
            _poolRoot = new GameObject("[UIPool]") { hideFlags = HideFlags.HideInHierarchy };
            _poolRoot.transform.SetParent(transform, false);
            _pool = new UIPool(_poolRoot.transform);

            BuildLayers();
            RegisterInitialPrefabs();
        }

        private void BuildLayers()
        {
            var defaults = new (UILayerType type, int order, bool modal)[]
            {
                (UILayerType.Background, -100, false),
                (UILayerType.Main, 0, false),
                (UILayerType.Panel, 100, true),
                (UILayerType.Popup, 200, true),
                (UILayerType.Notification, 300, false),
                (UILayerType.Tooltip, 400, false),
                (UILayerType.Overlay, 500, true),
            };

            // 合并配置（用户配置优先）
            var merged = new Dictionary<UILayerType, LayerSetup>();
            if (_layerSetups != null)
            {
                foreach (var s in _layerSetups)
                    merged[s.Type] = s;
            }

            foreach (var (type, order, modal) in defaults)
            {
                if (!merged.TryGetValue(type, out var setup))
                {
                    setup = new LayerSetup { Type = type, BaseSortOrder = order, IsModal = modal };
                }

                var canvas = setup.Canvas;
                if (canvas == null)
                    canvas = CreateLayerCanvas(setup);

                _canvases[type] = canvas;
                if (setup.IsModal)
                    _stacks[type] = new Stack<UIModule>();
            }
        }

        private Canvas CreateLayerCanvas(LayerSetup setup)
        {
            var go = new GameObject($"[{setup.Type}Layer]");
            go.transform.SetParent(transform, false);
            go.layer = LayerMask.NameToLayer("UI");

            var canvas = go.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = setup.BaseSortOrder;
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = go.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            go.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private void RegisterInitialPrefabs()
        {
            foreach (var p in _initialPrefabs)
            {
                if (p != null) _prefabs[p.GetType()] = p;
            }
        }

        // ==================== Prefab注册 ====================

        /// <summary>注册模块Prefab，之后可通过Show&lt;T&gt;()按类型打开。</summary>
        public void RegisterPrefab<T>(T prefab) where T : UIModule
        {
            if (prefab != null)
                _prefabs[typeof(T)] = prefab;
        }

        /// <summary>批量注册Prefab。</summary>
        public void RegisterPrefabs(params UIModule[] prefabs)
        {
            foreach (var p in prefabs)
            {
                if (p != null) _prefabs[p.GetType()] = p;
            }
        }

        /// <summary>预热对象池。</summary>
        public void Warmup<T>(int count) where T : UIModule
        {
            if (_prefabs.TryGetValue(typeof(T), out var prefab))
                _pool.Warmup((T)prefab, count);
        }

        // ==================== Show API ====================

        /// <summary>按类型打开模块（需先RegisterPrefab）。</summary>
        public T Show<T>(object data = null) where T : UIModule
        {
            if (!_prefabs.TryGetValue(typeof(T), out var prefab))
            {
                Debug.LogError($"[UIManager] {typeof(T).Name} 未注册Prefab，请先调用 RegisterPrefab<{typeof(T).Name}>()");
                return null;
            }
            return Show((T)prefab, data);
        }

        /// <summary>使用指定Prefab打开模块。</summary>
        public T Show<T>(T prefab, object data = null) where T : UIModule
        {
            var layer = ResolveLayer<T>();
            var canvas = GetCanvas(layer);
            if (canvas == null)
            {
                Debug.LogError($"[UIManager] 层级 {layer} 的Canvas不存在");
                return null;
            }
            return CreateAndOpen(prefab, canvas.transform, layer, data);
        }

        /// <summary>在指定层级打开模块。</summary>
        public T ShowAt<T>(T prefab, UILayerType layer, object data = null) where T : UIModule
        {
            var canvas = GetCanvas(layer);
            if (canvas == null)
            {
                Debug.LogError($"[UIManager] 层级 {layer} 的Canvas不存在");
                return null;
            }
            return CreateAndOpen(prefab, canvas.transform, layer, data);
        }

        /// <summary>内部方法：创建（或从池获取）、配置、打开模块。</summary>
        internal T CreateAndOpen<T>(T prefab, Transform parent, UILayerType layer, object data = null) where T : UIModule
        {
            // Tooltip单例处理
            if (typeof(T) == typeof(TooltipUI))
                HideActiveTooltip();

            // 从池获取或实例化
            var instance = _pool.Get<T>();
            if (instance == null)
            {
                instance = Instantiate(prefab, parent);
                instance.name = prefab.name;
            }
            else
            {
                instance.transform.SetParent(parent, false);
            }

            instance.LayerType = layer;
            _activeModules.Add(instance);

            // 模态层入栈
            if (_stacks.TryGetValue(layer, out var stack))
                stack.Push(instance);

            // 特殊模块处理
            if (instance is TooltipUI t) _activeTooltip = t;
            if (instance is NotificationUI n) HandleNotificationQueue(n);

            instance.DoOpen(data);
            ModuleOpened?.Invoke(instance);
            return instance;
        }

        // ==================== Hide / Close API ====================

        /// <summary>关闭指定模块。</summary>
        public void Hide(UIModule module)
        {
            if (module == null || !module.IsOpen) return;

            RemoveFromStack(module);
            _activeModules.Remove(module);
            module.DoClose();

            if (module == _activeTooltip) _activeTooltip = null;

            ModuleClosed?.Invoke(module);
            _pool.Return(module);
        }

        /// <summary>关闭指定类型的所有模块。</summary>
        public void HideAll<T>() where T : UIModule
        {
            var targets = _activeModules.OfType<T>().ToList();
            foreach (var m in targets) Hide(m);
        }

        /// <summary>关闭某层所有模块。</summary>
        public void HideLayer(UILayerType layer)
        {
            var targets = _activeModules.Where(m => m.LayerType == layer).ToList();
            foreach (var m in targets) Hide(m);
        }

        /// <summary>立即关闭所有模块（跳过动画）。</summary>
        public void HideAllImmediate()
        {
            foreach (var m in _activeModules.ToList())
                m.DoCloseImmediate();
            _activeModules.Clear();
            _stacks.Clear();
            _activeTooltip = null;
            _activeNotification = null;
            _pendingNotifications.Clear();
        }

        /// <summary>关闭指定层级栈顶模块（ESC行为）。</summary>
        public bool CloseTopLayer(UILayerType layer)
        {
            if (!_stacks.TryGetValue(layer, out var stack) || stack.Count == 0)
                return false;

            var top = stack.Peek();
            if (top == null || !top.IsOpen) return false;
            return top.Cancel();
        }

        /// <summary>ESC全局处理：从高到底依次尝试关闭栈顶。</summary>
        public bool CloseTop()
        {
            var priority = new[]
            {
                UILayerType.Overlay, UILayerType.Popup, UILayerType.Tooltip, UILayerType.Panel
            };
            foreach (var layer in priority)
            {
                if (CloseTopLayer(layer))
                    return true;
            }
            return false;
        }

        // ==================== 查询 API ====================

        /// <summary>某类型模块是否处于打开状态。</summary>
        public bool IsOpen<T>() where T : UIModule => _activeModules.Any(m => m is T);

        /// <summary>获取某类型的活跃模块实例（多个则返回首个）。</summary>
        public T GetModule<T>() where T : UIModule => _activeModules.OfType<T>().FirstOrDefault();

        /// <summary>获取所有活跃模块。</summary>
        public IReadOnlyList<UIModule> ActiveModules => _activeModules;

        /// <summary>获取指定层级的Canvas。</summary>
        public Canvas GetCanvas(UILayerType layer)
        {
            _canvases.TryGetValue(layer, out var c);
            return c;
        }

        /// <summary>是否有任何模态UI打开（弹出层/覆盖层有内容）。</summary>
        public bool HasModalOpen =>
            (_stacks.TryGetValue(UILayerType.Popup, out var ps) && ps.Count > 0) ||
            (_stacks.TryGetValue(UILayerType.Overlay, out var os) && os.Count > 0);

        // ==================== 快捷方法 ====================

        /// <summary>显示确认弹窗。</summary>
        public PopupUI ShowPopup(string title, string body,
            Action onConfirm = null, Action onCancel = null,
            string confirmLabel = "确认", string cancelLabel = "取消")
        {
            if (!_prefabs.TryGetValue(typeof(PopupUI), out var prefab))
            {
                Debug.LogError("[UIManager] 未注册PopupUI Prefab");
                return null;
            }
            return Show((PopupUI)prefab)
                ?.SetTitle(title).SetBody(body)
                .SetConfirm(confirmLabel, onConfirm)
                .SetCancel(cancelLabel, onCancel);
        }

        /// <summary>显示CK3风格事件弹窗。</summary>
        public EventDialogUI ShowEvent(string title, string body,
            List<EventDialogUI.Choice> choices, Sprite portrait = null)
        {
            if (!_prefabs.TryGetValue(typeof(EventDialogUI), out var prefab))
            {
                Debug.LogError("[UIManager] 未注册EventDialogUI Prefab");
                return null;
            }
            return Show((EventDialogUI)prefab)
                ?.SetTitle(title).SetBody(body)
                .SetPortrait(portrait).SetChoices(choices);
        }

        /// <summary>发送Toast通知。</summary>
        public NotificationUI Notify(string message,
            NotificationUI.NotificationType type = NotificationUI.NotificationType.Info,
            float duration = 3f)
        {
            if (!_prefabs.TryGetValue(typeof(NotificationUI), out var prefab))
            {
                Debug.Log($"[Notify] {message}");
                return null;
            }
            var n = Show((NotificationUI)prefab);
            n?.Configure(message, type, duration);
            return n;
        }

        /// <summary>显示工具提示。</summary>
        public TooltipUI ShowTooltip(string text, Vector2? screenPosition = null)
        {
            if (!_prefabs.TryGetValue(typeof(TooltipUI), out var prefab))
                return null;

            var t = Show((TooltipUI)prefab);
            if (t != null)
            {
                t.SetText(text);
                if (screenPosition.HasValue)
                    t.SetPosition(screenPosition.Value);
            }
            return t;
        }

        /// <summary>隐藏当前Tooltip。</summary>
        public void HideTooltip()
        {
            HideActiveTooltip();
        }

        // ==================== 内部 ====================

        private UILayerType ResolveLayer<T>() where T : UIModule
        {
            var t = typeof(T);
            if (t == typeof(PopupUI) || t == typeof(EventDialogUI)) return UILayerType.Popup;
            if (t == typeof(NotificationUI)) return UILayerType.Notification;
            if (t == typeof(TooltipUI)) return UILayerType.Tooltip;
            if (t.IsSubclassOf(typeof(PanelUI))) return UILayerType.Main;
            if (t.IsSubclassOf(typeof(PopupUI))) return UILayerType.Popup;
            return UILayerType.Panel;
        }

        private void HandleNotificationQueue(NotificationUI notification)
        {
            if (_activeNotification == null || !_activeNotification.IsOpen)
            {
                _activeNotification = notification;
                return;
            }

            // 队列中等待
            notification.gameObject.SetActive(false);
            _pendingNotifications.Enqueue(notification);
        }

        private void ProcessNotificationQueue()
        {
            if (_pendingNotifications.Count > 0 &&
                (_activeNotification == null || !_activeNotification.IsOpen))
            {
                var next = _pendingNotifications.Dequeue();
                var canvas = GetCanvas(UILayerType.Notification);
                next.transform.SetParent(canvas.transform, false);
                next.gameObject.SetActive(true);
                next.DoOpen();
                _activeNotification = next;
            }
        }

        private void HideActiveTooltip()
        {
            if (_activeTooltip != null)
                Hide(_activeTooltip);
        }

        private void RemoveFromStack(UIModule module)
        {
            foreach (var kv in _stacks)
            {
                var stack = kv.Value;
                if (!stack.Contains(module)) continue;
                var temp = new Stack<UIModule>();
                while (stack.TryPop(out var m))
                    if (m != module) temp.Push(m);
                while (temp.TryPop(out var m))
                    stack.Push(m);
                break;
            }
        }

        internal void UnregisterModule(UIModule module)
        {
            _activeModules.Remove(module);
        }

        // ==================== MonoBehaviour ====================

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseTop();

            ProcessNotificationQueue();
        }
    }
}
