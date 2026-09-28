using System;

namespace Dada.Cores
{
    /// <summary>
    /// 屏幕公共接口——UITK 和 uGUI 的 UIScreen / UGUIScreen 都实现此接口。
    /// 纯状态 + 身份，不含任何 UI 框架类型。
    /// </summary>
    public interface IScreen
    {
        string ScreenId { get; }
        string CurrentState { get; }
        bool IsVisible { get; }
        string[] AvailableStates { get; }
        ScreenStateMachine StateMachine { get; }

        /// <summary>
        /// 状态切换事件 (oldState, newState)
        /// </summary>
        event Action<string, string> OnStateChanged;
    }
}
