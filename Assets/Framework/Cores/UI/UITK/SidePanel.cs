using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Dada.Cores
{
    /// <summary>
    /// 通用右侧滑入面板基类。
    /// 提供 Tab 栏、关闭按钮、ScrollView 内容区、操作按钮栏的统一骨架。
    ///
    /// 子类只需覆写 OnTabEnter(state) 填充各 Tab 的内容。
    ///
    /// UXML 约定（name 属性）：
    ///   title-label       — 面板标题
    ///   close-btn         — 关闭按钮
    ///   tab-overview-btn  — 概览 Tab（可选）
    ///   tab-details-btn   — 详情 Tab（可选）
    ///   tab-relations-btn — 关系 Tab（可选）
    ///   content-container — 内容容器（ScrollView 内）
    ///   empty-state       — 空状态占位
    ///   action-bar        — 操作按钮栏
    /// </summary>
    public abstract class SidePanel : UIScreen
    {
        // ──── UXML 控件 ────

        protected Label TitleLabel;
        protected Button CloseBtn;
        protected Button TabOverviewBtn;
        protected Button TabDetailsBtn;
        protected Button TabRelationsBtn;
        protected VisualElement ContentContainer;
        protected VisualElement EmptyState;
        protected VisualElement ActionBar;

        /// <summary>
        /// 关闭时回调。外部订阅以处理清理逻辑。
        /// </summary>
        public event Action OnClose;

        // ──── 初始化 ────

        protected override void OnInitialize()
        {
            TitleLabel = Q<Label>("title-label");
            CloseBtn = Q<Button>("close-btn");
            TabOverviewBtn = Q<Button>("tab-overview-btn");
            TabDetailsBtn = Q<Button>("tab-details-btn");
            TabRelationsBtn = Q<Button>("tab-relations-btn");
            ContentContainer = Q<VisualElement>("content-container");
            EmptyState = Q<VisualElement>("empty-state");
            ActionBar = Q<VisualElement>("action-bar");

            CloseBtn?.RegisterCallback<ClickEvent>(_ => Hide());
            TabOverviewBtn?.RegisterCallback<ClickEvent>(_ => SetState("Overview"));
            TabDetailsBtn?.RegisterCallback<ClickEvent>(_ => SetState("Details"));
            TabRelationsBtn?.RegisterCallback<ClickEvent>(_ => SetState("Relations"));

            ActionBar.SetDisplay(false);
        }

        // ──── 对外 API ────

        /// <summary>
        /// 设置面板标题。
        /// </summary>
        public void SetTitle(string title)
        {
            if (TitleLabel != null) TitleLabel.text = title;
        }

        /// <summary>
        /// 设置操作按钮列表。
        /// </summary>
        public void SetActions(params ActionDef[] actions) => SetActions((IEnumerable<ActionDef>)actions);

        /// <summary>
        /// 设置操作按钮列表。
        /// </summary>
        public void SetActions(IEnumerable<ActionDef> actions)
        {
            ActionBar?.Clear();

            var hasAny = false;
            foreach (var action in actions)
            {
                hasAny = true;
                var btn = new Button { text = action.label, name = $"action-{action.key}" };
                btn.AddToClassList("action-btn");
                if (action.isDanger) btn.AddToClassList("danger");
                if (action.isSpecial) btn.AddToClassList("special");
                var key = action.key;
                btn.clicked += () => OnActionClicked?.Invoke(key);
                ActionBar.Add(btn);
            }

            ActionBar.SetDisplay(hasAny);
        }

        /// <summary>
        /// 操作按钮点击事件。
        /// </summary>
        public event Action<string> OnActionClicked;

        /// <summary>
        /// 关闭面板——播放退场动画后销毁。
        /// </summary>
        public async void Hide()
        {
            OnClose?.Invoke();
            await HideAsync();
        }

        /// <summary>
        /// 清空内容区并显示。
        /// </summary>
        public void ClearContent() => ContentContainer?.Clear();

        /// <summary>
        /// 隐藏空状态占位。
        /// </summary>
        public void HideEmptyState() => EmptyState?.RemoveFromClassList("visible");

        /// <summary>
        /// 更新 Tab 可见性（按需打开/关闭某些 Tab）。
        /// </summary>
        public void SetTabs(bool overview, bool details, bool relations)
        {
            TabOverviewBtn.SetDisplay(overview);
            TabDetailsBtn.SetDisplay(details);
            TabRelationsBtn.SetDisplay(relations);
        }

        // ──── 状态切换 ────

        protected override async UniTask OnStateEnterAsync(string state)
        {
            // 更新 Tab 高亮
            TabOverviewBtn?.RemoveFromClassList("active");
            TabDetailsBtn?.RemoveFromClassList("active");
            TabRelationsBtn?.RemoveFromClassList("active");

            switch (state)
            {
                case "Overview":  TabOverviewBtn?.AddToClassList("active");  break;
                case "Details":   TabDetailsBtn?.AddToClassList("active");   break;
                case "Relations": TabRelationsBtn?.AddToClassList("active"); break;
            }

            ContentContainer?.Clear();
            HideEmptyState();
            await OnTabEnter(state);
        }

        /// <summary>
        /// Tab 进入时调用——子类在此填充内容。
        /// </summary>
        protected abstract UniTask OnTabEnter(string state);

        // ──── 入场/退场动画 ────

        protected override async UniTask OnShowAsync()
        {
            RootElement.style.translate = new StyleTranslate(new Translate(480, 0));
            await UniTask.DelayFrame(1);
            RootElement.style.translate = new StyleTranslate(new Translate(0, 0));
        }

        protected override async UniTask OnHideAsync()
        {
            RootElement.style.translate = new StyleTranslate(new Translate(480, 0));
            await UniTask.Delay(250);
        }
    }

    /// <summary>
    /// VisualElement 扩展辅助。
    /// </summary>
    public static class VisualElementExtensions
    {
        public static void SetDisplay(this VisualElement el, bool visible)
        {
            if (el != null) el.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
