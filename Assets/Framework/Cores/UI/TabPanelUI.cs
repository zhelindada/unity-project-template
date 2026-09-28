using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dada.Cores
{
    /// <summary>
    /// 标签页面板容器。管理一组Tab页签，点击切换显示对应的内容面板。
    /// 用于角色面板的多Tab详情（属性/技能/关系等）。
    /// </summary>
    public class TabPanelUI : PanelUI
    {
        [Header("Tabs")]
        [SerializeField] private Transform _tabBar;
        [SerializeField] private TabButton _tabButtonPrefab;

        /// <summary>Tab定义。</summary>
        [Serializable]
        public struct TabDefinition
        {
            public string Label;
            public UIModule ContentPrefab;
        }

        private List<TabDefinition> _tabs = new();
        private List<TabButton> _tabButtons = new();
        private UIModule _currentContent;
        private int _activeTabIndex = -1;

        // ==================== 配置 ====================

        public void SetTabs(List<TabDefinition> tabs)
        {
            _tabs = tabs;
            BuildTabs();
        }

        /// <summary>在指定位置插入新Tab。</summary>
        public void InsertTab(int index, string label, UIModule contentPrefab)
        {
            _tabs.Insert(index, new TabDefinition { Label = label, ContentPrefab = contentPrefab });
            BuildTabs();
            if (_activeTabIndex >= index) _activeTabIndex++;
        }

        // ==================== 内部实现 ====================

        private void BuildTabs()
        {
            ClearTabs();

            for (int i = 0; i < _tabs.Count; i++)
            {
                var index = i;
                var tab = Instantiate(_tabButtonPrefab, _tabBar);
                tab.SetLabel(_tabs[i].Label);
                tab.OnClick += () => SwitchTab(index);
                _tabButtons.Add(tab);
            }

            if (_tabs.Count > 0)
                SwitchTab(0);
        }

        private void SwitchTab(int index)
        {
            if (index < 0 || index >= _tabs.Count || index == _activeTabIndex) return;

            // 关闭当前内容
            if (_currentContent != null)
                _currentContent.Close();

            // 更新Tab高亮
            for (int i = 0; i < _tabButtons.Count; i++)
                _tabButtons[i].SetActive(i == index);

            // 打开新内容
            _activeTabIndex = index;
            var def = _tabs[index];
            if (def.ContentPrefab != null && ContentArea != null)
            {
                _currentContent = UIManager.HasInstance
                    ? UIManager.Instance.CreateAndOpen(def.ContentPrefab, ContentArea, UILayerType.Panel)
                    : null;
            }
        }

        private void ClearTabs()
        {
            foreach (var btn in _tabButtons)
                Destroy(btn.gameObject);
            _tabButtons.Clear();

            if (_currentContent != null)
            {
                _currentContent.Close();
                _currentContent = null;
            }

            _activeTabIndex = -1;
        }

        protected override void OnHide()
        {
            ClearTabs();
        }
    }

    /// <summary>
    /// Tab按钮组件。TabPanelUI内部使用。
    /// </summary>
    public class TabButton : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _label;
        [SerializeField] private UnityEngine.UI.Image _background;
        [SerializeField] private Color _activeColor = Color.white;
        [SerializeField] private Color _inactiveColor = Color.gray;

        public event Action OnClick;

        public void SetLabel(string text)
        {
            if (_label != null) _label.text = text;
        }

        public void SetActive(bool active)
        {
            if (_background != null)
                _background.color = active ? _activeColor : _inactiveColor;
        }

        // 由UnityEvent或代码调用
        public void Click() => OnClick?.Invoke();
    }
}
