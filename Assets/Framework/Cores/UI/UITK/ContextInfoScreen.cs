using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Dada.Cores
{
    /// <summary>
    /// ContextInfoPanel 的具体屏幕——演示 IScreen + ScreenStateMachine 组合使用。
    ///
    /// 不依赖任何 VContainer 类型，构造函数可注入任意依赖。
    /// </summary>
    public class ContextInfoScreen : UIScreen
    {
        // UXML name 引用
        private Button _closeBtn;
        private Button _tabOverviewBtn;
        private Button _tabDetailsBtn;
        private Button _tabRelationsBtn;
        private VisualElement _contentContainer;
        private VisualElement _emptyState;
        private VisualElement _actionBar;

        private IContextData _contextData;

        public event Action<IContextData> OnContextChanged;
        public event Action<string, IContextData> OnActionClicked;

        protected override void OnInitialize()
        {
            _closeBtn = Q<Button>("close-btn");
            _tabOverviewBtn = Q<Button>("tab-overview-btn");
            _tabDetailsBtn = Q<Button>("tab-details-btn");
            _tabRelationsBtn = Q<Button>("tab-relations-btn");
            _contentContainer = Q<VisualElement>("content-container");
            _emptyState = Q<VisualElement>("empty-state");
            _actionBar = Q<VisualElement>("action-bar");

            _tabOverviewBtn?.RegisterCallback<ClickEvent>(_ => SetState("Overview"));
            _tabDetailsBtn?.RegisterCallback<ClickEvent>(_ => SetState("Details"));
            _tabRelationsBtn?.RegisterCallback<ClickEvent>(_ => SetState("Relations"));
        }

        public void SetCloseAction(Action onClose)
        {
            if (_closeBtn != null)
                _closeBtn.RegisterCallback<ClickEvent>(_ => onClose?.Invoke());
        }

        public void SetContext(IContextData data)
        {
            _contextData = data;
            OnContextChanged?.Invoke(data);
            UpdateTabAvailability(data);
            SetState(AvailableStates[0]);
        }

        public void SetActions(IEnumerable<ActionDef> actions)
        {
            _actionBar?.Clear();
            _actionBar.style.display = DisplayStyle.Flex;

            foreach (var action in actions)
            {
                var btn = new Button { text = action.label, name = $"{action.key}-btn" };
                btn.AddToClassList("action-btn");
                if (action.isDanger) btn.AddToClassList("danger");
                if (action.isSpecial) btn.AddToClassList("special");
                btn.clicked += () => OnActionClicked?.Invoke(action.key, _contextData);
                _actionBar.Add(btn);
            }
        }

        protected override async UniTask OnStateEnterAsync(string state)
        {
            if (_contextData == null) return;

            _tabOverviewBtn?.RemoveFromClassList("active");
            _tabDetailsBtn?.RemoveFromClassList("active");
            _tabRelationsBtn?.RemoveFromClassList("active");

            switch (state)
            {
                case "Overview":  _tabOverviewBtn?.AddToClassList("active");  break;
                case "Details":   _tabDetailsBtn?.AddToClassList("active");   break;
                case "Relations": _tabRelationsBtn?.AddToClassList("active"); break;
            }

            _emptyState?.RemoveFromClassList("visible");
            _contentContainer?.Clear();
            await UniTask.CompletedTask;
        }

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

        private void UpdateTabAvailability(IContextData data)
        {
            _tabOverviewBtn.style.display = data.HasOverview ? DisplayStyle.Flex : DisplayStyle.None;
            _tabDetailsBtn.style.display = data.HasDetails ? DisplayStyle.Flex : DisplayStyle.None;
            _tabRelationsBtn.style.display = data.HasRelations ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    public interface IContextData
    {
        bool HasOverview { get; }
        bool HasDetails { get; }
        bool HasRelations { get; }
        string ContextType { get; }
    }

    public struct ActionDef
    {
        public string key;
        public string label;
        public bool isDanger;
        public bool isSpecial;
    }
}
