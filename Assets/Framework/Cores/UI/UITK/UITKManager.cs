using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Dada.Cores
{
    /// <summary>
    /// UITK 屏幕管理器 —— 实现 IScreenManager。
    /// 通过 VContainer 注入 UITKConfig + IObjectResolver，自动发现 UIDocument。
    ///
    /// 使用:
    ///   [Inject] IScreenManager _ui;  // 一套接口，UITK/uGUI 通用
    ///   var screen = _ui.GetOrCreateScreen{MyScreen}("DetailPanel");
    ///   _ui.ShowScreenAsync("DetailPanel").Forget();
    /// </summary>
    public class UITKManager : MonoBehaviour, IScreenManager
    {
        [SerializeField] private UIDocument _uiDocument;

        private UITKConfig _config;
        private IObjectResolver _resolver;
        private readonly Dictionary<string, UIScreen> _screens = new();
        private readonly Dictionary<string, Func<UIScreen>> _factories = new();

        public UIScreen ActiveScreen { get; private set; }
        public IReadOnlyDictionary<string, UIScreen> Screens => _screens;
        public UITKConfig Config => _config;
        public VisualElement RootElement => _uiDocument?.rootVisualElement;

        public event Action<string> OnScreenShown;
        public event Action<string> OnScreenHidden;
        public event Action<string, string, string> OnScreenStateChanged;

        [Inject]
        private void Construct(UITKConfig config, IObjectResolver resolver)
        {
            _config = config;
            _resolver = resolver;

            foreach (var def in config.screens)
            {
                if (string.IsNullOrEmpty(def.screenType)) continue;

                var type = Type.GetType(def.screenType, throwOnError: false);
                if (type != null && typeof(UIScreen).IsAssignableFrom(type))
                {
                    var screenId = def.screenId;
                    _factories[screenId] = () => (UIScreen)resolver.Resolve(type);
                }
            }
        }

        private void Awake()
        {
            if (_uiDocument == null) _uiDocument = GetComponent<UIDocument>();
            if (_uiDocument == null) _uiDocument = FindAnyObjectByType<UIDocument>();
        }

        private void OnDestroy() => _screens.Clear();

        // ──── IScreenManager ────

        public void RegisterScreen(IScreen screen)
        {
            if (screen is not UIScreen uitkScreen || string.IsNullOrEmpty(uitkScreen.ScreenId)) return;
            _screens[uitkScreen.ScreenId] = uitkScreen;
            uitkScreen.StateMachine.OnStateChanged += (oldS, newS) =>
                OnScreenStateChanged?.Invoke(uitkScreen.ScreenId, oldS, newS);
        }

        public T GetScreen<T>(string screenId) where T : class, IScreen
        {
            _screens.TryGetValue(screenId, out var s);
            return s as T;
        }

        /// <summary>
        /// 获取或创建屏幕（非接口方法——外部需知道具体类型来创建）
        /// </summary>
        public T GetOrCreateScreen<T>(string screenId) where T : UIScreen
        {
            if (_screens.TryGetValue(screenId, out var existing))
                return existing as T;

            var def = _config?.Find(screenId);
            if (def == null)
            {
                Debug.LogError($"[UITKManager] 未在 Config 中找到屏幕: '{screenId}'");
                return null;
            }

            UIScreen screen;
            if (_factories.TryGetValue(screenId, out var factory))
                screen = factory();
            else if (_resolver != null)
                screen = _resolver.Resolve<T>();
            else
                screen = Activator.CreateInstance<T>();

            if (screen == null) return null;

            screen.Initialize(def, RootElement);
            screen.StateMachine.OnStateChanged += (oldS, newS) =>
                OnScreenStateChanged?.Invoke(screenId, oldS, newS);
            _screens[screenId] = screen;

            return screen as T;
        }

        public async UniTask ShowScreenAsync(string screenId, string initialState = null)
        {
            if (RootElement == null)
            {
                Debug.LogError("[UITKManager] UIDocument.rootVisualElement 为空");
                return;
            }

            if (!_screens.TryGetValue(screenId, out var screen))
            {
                Debug.LogError($"[UITKManager] 屏幕 '{screenId}' 未创建，请先 GetOrCreateScreen");
                return;
            }

            if (ActiveScreen != null && ActiveScreen != screen)
            {
                await ActiveScreen.HideAsync();
                OnScreenHidden?.Invoke(ActiveScreen.ScreenId);
            }

            await screen.ShowAsync();
            ActiveScreen = screen;

            if (initialState != null)
                screen.SetState(initialState);

            OnScreenShown?.Invoke(screenId);
        }

        public async UniTask DestroyScreenAsync(string screenId)
        {
            if (!_screens.TryGetValue(screenId, out var screen)) return;

            await screen.HideAsync();
            screen.Dispose();
            _screens.Remove(screenId);

            if (ActiveScreen == screen)
                ActiveScreen = null;
        }

        public async UniTask HideAllAsync()
        {
            if (ActiveScreen != null)
            {
                await ActiveScreen.HideAsync();
                OnScreenHidden?.Invoke(ActiveScreen.ScreenId);
                ActiveScreen = null;
            }
        }
    }
}
