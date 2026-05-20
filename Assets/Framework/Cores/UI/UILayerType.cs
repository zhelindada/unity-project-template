namespace Dada.Cores.UI
{
    /// <summary>
    /// UI层级，按渲染顺序从低到高排列。
    /// </summary>
    public enum UILayerType
    {
        /// <summary>背景层：极少使用，用于全屏背景图。</summary>
        Background,
        /// <summary>主界面层：常驻面板（侧栏、顶栏、底栏、HUD元素）。</summary>
        Main,
        /// <summary>面板层：叠加在主界面上方的可堆叠次级面板。</summary>
        Panel,
        /// <summary>弹窗层：模态对话框，确认框。</summary>
        Popup,
        /// <summary>通知层：Toast消息、飘字提示。</summary>
        Notification,
        /// <summary>工具提示层：悬停信息。</summary>
        Tooltip,
        /// <summary>覆盖层：加载画面、全屏过渡（最顶层）。</summary>
        Overlay,
    }
}
