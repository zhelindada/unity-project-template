using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Dada.Cores
{
    /// <summary>
    /// uGUI 屏幕基类——实现 IScreen，组合 ScreenStateMachine + GameObject。
    /// 与 UIScreen 共享同一套状态机核心和接口。
    /// </summary>
    public abstract class UGUIScreen : IScreen, IDisposable
    {
        public string ScreenId => _definition?.screenId;
        public GameObject RootObject { get; private set; }
        public RectTransform RectTransform { get; private set; }
        public bool IsVisible { get; private set; }

        public ScreenStateMachine StateMachine { get; private set; }
        public string CurrentState => StateMachine?.CurrentState;
        public string[] AvailableStates => StateMachine?.ValidStates;

        protected UGUIScreenDefinition _definition;
        private Transform _parentContainer;

        public event Action<string, string> OnStateChanged;

        // ──── 初始化（由 UGUIManager 调用） ────

        internal void Initialize(UGUIScreenDefinition definition, Transform parentContainer)
        {
            _definition = definition;
            _parentContainer = parentContainer;

            StateMachine = new ScreenStateMachine(definition.states, definition.defaultState);
            StateMachine.OnStateChanged += (oldS, newS) => OnStateChanged?.Invoke(oldS, newS);
            StateMachine.OnStateEnterAsync = OnStateEnterAsync;
            StateMachine.OnStateExitAsync = OnStateExitAsync;

            var parent = parentContainer;
            if (!string.IsNullOrEmpty(definition.parentPath))
            {
                var go = GameObject.Find(definition.parentPath);
                if (go != null) parent = go.transform;
            }

            RootObject = UnityEngine.Object.Instantiate(definition.prefab, parent);
            RootObject.name = definition.screenId;
            RectTransform = RootObject.GetComponent<RectTransform>();
            if (RectTransform == null)
                RectTransform = RootObject.AddComponent<RectTransform>();

            RootObject.SetActive(false);
            OnInitialize();
        }

        public void SetState(string newState) => StateMachine.SetState(newState);
        public UniTask SetStateAsync(string newState) => StateMachine.SetStateAsync(newState);

        // ──── 显示/隐藏（无参，parent 由 Manager 初始化时注入） ────

        public async UniTask ShowAsync()
        {
            if (RootObject.transform.parent != _parentContainer)
                RootObject.transform.SetParent(_parentContainer, false);

            RootObject.SetActive(true);
            IsVisible = true;

            await OnShowAsync();
            var targetState = CurrentState ?? _definition.defaultState;
            await StateMachine.SetStateAsync(targetState);
        }

        public async UniTask HideAsync()
        {
            await OnHideAsync();
            RootObject.SetActive(false);
            IsVisible = false;
        }

        public virtual void Dispose()
        {
            if (RootObject != null)
            {
                UnityEngine.Object.Destroy(RootObject);
                RootObject = null;
            }
            OnDispose();
        }

        // ──── 子类覆写点 ────

        protected virtual void OnInitialize() { }
        protected virtual UniTask OnShowAsync() => UniTask.CompletedTask;
        protected virtual UniTask OnHideAsync() => UniTask.CompletedTask;
        protected virtual UniTask OnStateEnterAsync(string state) => UniTask.CompletedTask;
        protected virtual UniTask OnStateExitAsync(string state) => UniTask.CompletedTask;
        protected virtual void OnDispose() { }

        protected T Find<T>(string path) where T : Component
            => RootObject?.transform.Find(path)?.GetComponent<T>();

        protected GameObject FindGO(string path)
            => RootObject?.transform.Find(path)?.gameObject;
    }
}
