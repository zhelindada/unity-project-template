using System;
using Cysharp.Threading.Tasks;

namespace Dada.Cores
{
    /// <summary>
    /// 框架无关的字符串索引屏幕子状态机。
    /// UITK 和 uGUI 屏幕共享同一套状态逻辑。
    ///
    /// 使用:
    ///   var sm = new ScreenStateMachine(new[]{"Overview", "Details", "Relations"}, "Overview");
    ///   sm.OnStateChanged += (oldS, newS) => Debug.Log($"{oldS} → {newS}");
    ///   await sm.SetStateAsync("Details");
    /// </summary>
    public class ScreenStateMachine
    {
        private readonly string[] _validStates;

        public string CurrentState { get; private set; }
        public string[] ValidStates => _validStates;

        /// <summary>
        /// 状态切换事件 (oldState, newState)
        /// </summary>
        public event Action<string, string> OnStateChanged;

        /// <summary>
        /// 状态进入事件，在 OnStateChanged 之前触发
        /// </summary>
        public event Action<string> OnStateEnter;

        /// <summary>
        /// 状态退出事件，在 OnStateChanged 之前触发
        /// </summary>
        public event Action<string> OnStateExit;

        /// <summary>
        /// 异步状态进入钩子
        /// </summary>
        public Func<string, UniTask> OnStateEnterAsync;

        /// <summary>
        /// 异步状态退出钩子
        /// </summary>
        public Func<string, UniTask> OnStateExitAsync;

        public ScreenStateMachine(string[] validStates, string defaultState)
        {
            _validStates = validStates ?? Array.Empty<string>();
            CurrentState = defaultState;
        }

        /// <summary>
        /// 同步切换。需要异步动画时用 SetStateAsync。
        /// </summary>
        public void SetState(string newState)
        {
            SetStateAsync(newState).Forget();
        }

        /// <summary>
        /// 异步切换——等待 OnStateExitAsync 和 OnStateEnterAsync 完成。
        /// </summary>
        public async UniTask SetStateAsync(string newState)
        {
            if (newState == CurrentState) return;

            if (!IsValid(newState))
            {
                UnityEngine.Debug.LogWarning(
                    $"[StateMachine] 无效状态 '{newState}'，可用: {string.Join(", ", _validStates)}");
                return;
            }

            var oldState = CurrentState;

            // 退出旧状态
            if (oldState != null)
            {
                OnStateExit?.Invoke(oldState);
                if (OnStateExitAsync != null)
                    await OnStateExitAsync(oldState);
            }

            CurrentState = newState;

            // 进入新状态
            if (OnStateEnterAsync != null)
                await OnStateEnterAsync(newState);
            OnStateEnter?.Invoke(newState);
            OnStateChanged?.Invoke(oldState, newState);
        }

        public bool IsValid(string state)
        {
            foreach (var s in _validStates)
                if (s == state) return true;
            return false;
        }

        /// <summary>
        /// 重置到默认状态，不触发事件
        /// </summary>
        public void Reset(string defaultState)
        {
            CurrentState = defaultState;
        }
    }
}
