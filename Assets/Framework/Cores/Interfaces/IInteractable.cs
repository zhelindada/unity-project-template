using UnityEngine;

namespace Dada.Cores;

/// <summary>
/// 可交互对象接口 — 由对象实现来响应玩家的"使用/交互"操作。
/// 框架层定义契约，游戏层实现具体行为。
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// 交互时调用，返回 true 表示已处理（阻止移动回退）
    /// </summary>
    bool Interact(GameObject user);

    /// <summary>
    /// 悬停提示文本，null 表示无提示
    /// </summary>
    string GetTooltip();
}
