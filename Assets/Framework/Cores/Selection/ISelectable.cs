using UnityEngine;

namespace Dada.Cores
{
    /// <summary>
    /// 可选实体接口 — 由需要被选择系统发现的 MonoBehaviour 实现。
    /// 框架层定义契约，游戏层实现具体行为。
    /// </summary>
    public interface ISelectable
    {
        GameObject GameObject { get; }
        string DisplayName { get; }
        bool CanBeSelected { get; }

        void OnSelected();
        void OnDeselected();
    }
}
