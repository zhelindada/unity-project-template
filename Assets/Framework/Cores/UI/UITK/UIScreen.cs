using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Dada.Cores
{
    /// <summary>
    /// UITK 屏幕基类——实现 IScreen，组合 ScreenStateMachine + VisualElement。
    /// 子类构造函数可注入 VContainer 管理的依赖。
    /// </summary>
    public abstract class UIScreen : IScreen, IDisposable
    {
        public string ScreenId => _definition?.screenId;
        public VisualElement RootElement { get; private set; }
        public bool IsVisible { get; private set; }

        public ScreenStateMachine StateMachine { get; private set; }
        public string CurrentState => StateMachine?.CurrentState;
        public string[] AvailableStates => StateMachine?.ValidStates;

        protected ScreenDefinition _definition;
        private VisualElement _parentContainer;

        public event Action<string, string> OnStateChanged;

        // ──── 初始化（由 UITKManager 调用） ────

        internal void Initialize(ScreenDefinition definition, VisualElement parentContainer)
        {
            _definition = definition;
            _parentContainer = parentContainer;

            StateMachine = new ScreenStateMachine(definition.states, definition.defaultState);
            StateMachine.OnStateChanged += (oldS, newS) => OnStateChanged?.Invoke(oldS, newS);
            StateMachine.OnStateEnterAsync = OnStateEnterAsync;
            StateMachine.OnStateExitAsync = OnStateExitAsync;

            RootElement = definition.uxml.Instantiate();
            RootElement.name = definition.screenId;

            OnInitialize();
        }

        // ──── 状态切换 ────

        public void SetState(string newState) => StateMachine.SetState(newState);
        public UniTask SetStateAsync(string newState) => StateMachine.SetStateAsync(newState);

        // ──── 显示/隐藏（无参，parent 由 Manager 初始化时注入） ────

        public async UniTask ShowAsync()
        {
            RootElement.style.display = DisplayStyle.Flex;
            if (RootElement.parent != _parentContainer)
            {
                RootElement.RemoveFromHierarchy();
                _parentContainer.Add(RootElement);
            }

            IsVisible = true;
            await OnShowAsync();

            var targetState = CurrentState ?? _definition.defaultState;
            await StateMachine.SetStateAsync(targetState);
        }

        public async UniTask HideAsync()
        {
            await OnHideAsync();
            RootElement.style.display = DisplayStyle.None;
            IsVisible = false;
        }

        public virtual void Dispose()
        {
            RootElement?.RemoveFromHierarchy();
            RootElement = null;
            OnDispose();
        }

        // ──── 子类覆写点 ────

        protected virtual void OnInitialize() { }
        protected virtual UniTask OnShowAsync() => UniTask.CompletedTask;
        protected virtual UniTask OnHideAsync() => UniTask.CompletedTask;
        protected virtual UniTask OnStateEnterAsync(string state) => UniTask.CompletedTask;
        protected virtual UniTask OnStateExitAsync(string state) => UniTask.CompletedTask;
        protected virtual void OnDispose() { }

        protected T Q<T>(string name) where T : VisualElement
            => RootElement?.Q<T>(name);
    }
}
