using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Dada.Cores
{
    /// <summary>
    /// uGUI 屏幕管理器 —— 实现 IScreenManager。
    /// 与 UITKManager 共享同一套接口，通过 VContainer 注入时无需关心底层实现。
    ///
    /// 使用:
    ///   [Inject] IScreenManager _ui;  // UITK 切 uGUI 只改 DI 注册，业务代码不动
    /// </summary>
    public class UGUIManager : MonoBehaviour, IScreenManager
    {
        [SerializeField] private Canvas _rootCanvas;
        [SerializeField] private UGUIConfig _config;

        private IObjectResolver _resolver;
        private readonly Dictionary<string, UGUIScreen> _screens = new();
        private readonly Dictionary<string, Func<UGUIScreen>> _factories = new();

        public UGUIScreen ActiveScreen { get; private set; }
        public IReadOnlyDictionary<string, UGUIScreen> Screens => _screens;
        public UGUIConfig Config => _config;
        public Transform ScreenLayer { get; private set; }

        public event Action<string> OnScreenShown;
        public event Action<string> OnScreenHidden;
        public event Action<string, string, string> OnScreenStateChanged;

        [Inject]
        private void Construct(UGUIConfig config, IObjectResolver resolver)
        {
            _config = config;
            _resolver = resolver;

            foreach (var def in config.screens)
            {
                if (string.IsNullOrEmpty(def.screenType)) continue;

                var type = Type.GetType(def.screenType, throwOnError: false);
                if (type != null && typeof(UGUIScreen).IsAssignableFrom(type))
                {
                    var screenId = def.screenId;
                    _factories[screenId] = () => (UGUIScreen)resolver.Resolve(type);
                }
            }
        }

        private void Awake()
        {
            if (_rootCanvas == null) _rootCanvas = FindAnyObjectByType<Canvas>();

            var layerGo = new GameObject("ScreenLayer", typeof(RectTransform));
            layerGo.transform.SetParent(_rootCanvas.transform, false);
            var rt = layerGo.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.sizeDelta = Vector2.zero;
            ScreenLayer = layerGo.transform;
        }

        private void OnDestroy() => _screens.Clear();

        // ──── IScreenManager ────

        public void RegisterScreen(IScreen screen)
        {
            if (screen is not UGUIScreen uguiScreen || string.IsNullOrEmpty(uguiScreen.ScreenId)) return;
            _screens[uguiScreen.ScreenId] = uguiScreen;
            uguiScreen.StateMachine.OnStateChanged += (oldS, newS) =>
                OnScreenStateChanged?.Invoke(uguiScreen.ScreenId, oldS, newS);
        }

        public T GetScreen<T>(string screenId) where T : class, IScreen
        {
            _screens.TryGetValue(screenId, out var s);
            return s as T;
        }

        public T GetOrCreateScreen<T>(string screenId) where T : UGUIScreen
        {
            if (_screens.TryGetValue(screenId, out var existing))
                return existing as T;

            var def = _config?.Find(screenId);
            if (def == null)
            {
                Debug.LogError($"[UGUIManager] 未在 Config 中找到屏幕: '{screenId}'");
                return null;
            }

            UGUIScreen screen;
            if (_factories.TryGetValue(screenId, out var factory))
                screen = factory();
            else if (_resolver != null)
                screen = _resolver.Resolve<T>();
            else
                screen = Activator.CreateInstance<T>();

            if (screen == null) return null;

            screen.Initialize(def, ScreenLayer);
            screen.StateMachine.OnStateChanged += (oldS, newS) =>
                OnScreenStateChanged?.Invoke(screenId, oldS, newS);
            _screens[screenId] = screen;

            return screen as T;
        }

        public async UniTask ShowScreenAsync(string screenId, string initialState = null)
        {
            if (!_screens.TryGetValue(screenId, out var screen))
            {
                Debug.LogError($"[UGUIManager] 屏幕 '{screenId}' 未创建");
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

            if (ActiveScreen == screen) ActiveScreen = null;
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
