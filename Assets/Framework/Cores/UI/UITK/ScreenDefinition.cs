using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dada.Cores
{
    /// <summary>
    /// 屏幕注册定义——在 UITKConfig 中配置，描述一个屏幕的静态元数据。
    /// screenType 字段让 ScreenFactory 知道用哪个 VContainer 注册类型来实例化。
    /// </summary>
    [Serializable]
    public class ScreenDefinition
    {
        /// <summary>
        /// 屏幕唯一 ID，如 "DetailPanel"、"HUD"
        /// </summary>
        public string screenId;

        /// <summary>
        /// UXML 资源
        /// </summary>
        public VisualTreeAsset uxml;

        /// <summary>
        /// 该屏幕支持的所有子状态 ID
        /// </summary>
        public string[] states;

        /// <summary>
        /// 默认状态，必须包含在 states 中
        /// </summary>
        public string defaultState;

        /// <summary>
        /// 显示优先级，数值越大越在上层（预留）
        /// </summary>
        public int priority;

        /// <summary>
        /// 是否为模态
        /// </summary>
        public bool isModal;

        /// <summary>
        /// VContainer 注册用的 key——与 builder.Register{T}(...) 的 T 类型名一致。
        /// 例如 "ContextInfoScreen"。留空则走泛型 GetOrCreateScreen{T} 路径。
        /// </summary>
        public string screenType;
    }
}
