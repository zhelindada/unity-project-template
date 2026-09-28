using System.Collections.Generic;
using UnityEngine;

namespace Dada.Cores
{
    /// <summary>
    /// 常驻面板基类。用于侧栏、顶栏、底栏等持久显示的UI面板。
    /// 与Popup不同，Panel默认不响应ESC关闭，可在初始化时注册子面板进行栈管理。
    /// </summary>
    public abstract class PanelUI : UIModule
    {
        [Header("Panel")]
        [SerializeField] protected RectTransform _contentArea;

        /// <summary>面板内子模块的栈，用于面板内部的面板切换。</summary>
        public Stack<UIModule> SubStack { get; protected set; } = new();

        /// <summary>内容区域Transform，子面板实例化于此。</summary>
        public RectTransform ContentArea => _contentArea;

        protected override void OnPreShow()
        {
            _closeOnEscape = false;
        }

        /// <summary>在面板内容区域打开一个子模块。</summary>
        public T OpenSubPanel<T>(T prefab, object data = null) where T : UIModule
        {
            if (_contentArea == null)
            {
                Debug.LogError($"[{GetType().Name}] ContentArea未设置，无法打开子面板");
                return null;
            }

            var instance = UIManager.HasInstance
                ? UIManager.Instance.CreateAndOpen(prefab, _contentArea, UILayerType.Panel, data)
                : null;

            if (instance != null)
                SubStack.Push(instance);

            return instance;
        }

        /// <summary>关闭面板内最顶层的子模块。</summary>
        public void CloseTopSubPanel()
        {
            if (!SubStack.TryPop(out var top)) return;
            top.Close();
        }

        /// <summary>关闭面板内所有子模块。</summary>
        public void CloseAllSubPanels()
        {
            while (SubStack.TryPop(out var module))
                module.Close();
        }
    }
}
