using System;
using UnityEngine;

namespace Dada.Cores;

/// <summary>
/// 输入控制器接口 — 观察者模式。
///
/// 持续状态（Move、鼠标位置、Held）保持属性轮询；
/// 瞬时输入（按键按下/释放）通过事件通知。
/// </summary>
public interface IInputController
{
    // ── 持续状态（属性） ──────────────────────────

    Vector2 Move { get; }
    Vector2 Look { get; }
    float Zoom { get; }
    Vector2 MouseScreenPos { get; }

    bool SprintHeld { get; }
    bool InteractHeld { get; }
    bool LeftMouseHeld { get; }
    bool RightMouseHeld { get; }
    bool MiddleMouseHeld { get; }

    // ── 瞬时事件 ──────────────────────────────────

    event Action OnInteractPressed;
    event Action OnInteractReleased;
    event Action OnCancelPressed;
    event Action OnMenuPressed;
    event Action OnPausePressed;

    event Action OnLeftMousePressed;
    event Action OnRightMousePressed;
    event Action OnMiddleMousePressed;

    /// <summary>1/2/3 对应三个速度档位</summary>
    event Action<int> OnTimeSpeedPressed;

    event Action OnAction1Pressed;
    event Action OnAction2Pressed;
    event Action OnAction3Pressed;
    event Action OnAction4Pressed;
    event Action OnAction5Pressed;
    event Action OnAction6Pressed;
    event Action OnAction7Pressed;
}
