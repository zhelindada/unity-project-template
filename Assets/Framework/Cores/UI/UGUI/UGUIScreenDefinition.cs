using System;
using UnityEngine;

namespace Dada.Cores
{
    /// <summary>
    /// uGUI 屏幕注册定义——与 UITK 的 ScreenDefinition 平行，
    /// 使用 GameObject Prefab 而非 UXML。
    /// </summary>
    [Serializable]
    public class UGUIScreenDefinition
    {
        /// <summary>
        /// 屏幕唯一 ID
        /// </summary>
        public string screenId;

        /// <summary>
        /// Prefab 引用
        /// </summary>
        public GameObject prefab;

        /// <summary>
        /// 该屏幕支持的所有子状态 ID
        /// </summary>
        public string[] states;

        /// <summary>
        /// 默认状态
        /// </summary>
        public string defaultState;

        /// <summary>
        /// 显示优先级
        /// </summary>
        public int priority;

        /// <summary>
        /// 是否为模态
        /// </summary>
        public bool isModal;

        /// <summary>
        /// 父容器路径（如 "Canvas/PanelLayer"），空则挂到根 Canvas
        /// </summary>
        public string parentPath;

        /// <summary>
        /// VContainer 注册用的类型名
        /// </summary>
        public string screenType;
    }
}
