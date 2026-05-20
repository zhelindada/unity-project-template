using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dada.Cores.UI
{
    /// <summary>
    /// CK3/City of Gangsters风格的叙事事件弹窗。
    /// 包含标题、描述文本、角色肖像（可选）、多个选项按钮。
    /// 每个选项可附带效果描述和回调。
    /// </summary>
    public class EventDialogUI : UIModule
    {
        [Header("Event")]
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TextMeshProUGUI _bodyText;
        [SerializeField] private Image _portraitImage;
        [SerializeField] private Transform _choicesContainer;
        [SerializeField] private Button _choiceButtonPrefab;

        /// <summary>选项定义。</summary>
        public struct Choice
        {
            public string Label;
            public string Tooltip;
            public Action OnSelected;
            public bool IsEnabled;
        }

        private List<Choice> _choices;
        private List<GameObject> _choiceInstances = new();

        // ==================== Builder模式 ====================

        public EventDialogUI SetTitle(string title)
        {
            if (_titleText != null) _titleText.text = title;
            return this;
        }

        public EventDialogUI SetBody(string body)
        {
            if (_bodyText != null) _bodyText.text = body;
            return this;
        }

        public EventDialogUI SetPortrait(Sprite sprite)
        {
            if (_portraitImage != null)
            {
                _portraitImage.sprite = sprite;
                _portraitImage.gameObject.SetActive(sprite != null);
            }
            return this;
        }

        public EventDialogUI SetChoices(List<Choice> choices)
        {
            _choices = choices;
            return this;
        }

        // ==================== 生命周期 ====================

        protected override void OnPreShow()
        {
            BuildChoices();
        }

        protected override void OnHide()
        {
            ClearChoices();
        }

        private void BuildChoices()
        {
            ClearChoices();

            if (_choices == null || _choicesContainer == null || _choiceButtonPrefab == null)
                return;

            foreach (var choice in _choices)
            {
                var btnGo = Instantiate(_choiceButtonPrefab, _choicesContainer);
                var btn = btnGo.GetComponent<Button>();
                var label = btnGo.GetComponentInChildren<TextMeshProUGUI>();

                if (label != null) label.text = choice.Label;
                if (btn != null)
                {
                    btn.interactable = choice.IsEnabled;
                    btn.onClick.AddListener(() =>
                    {
                        choice.OnSelected?.Invoke();
                        Close();
                    });
                }

                _choiceInstances.Add(btnGo.gameObject);
            }
        }

        private void ClearChoices()
        {
            foreach (var go in _choiceInstances)
                Destroy(go);
            _choiceInstances.Clear();
        }

        public override bool Cancel()
        {
            if (!IsOpen || IsAnimating || _choices == null) return false;

            // ESC选择第一个可用选项
            foreach (var choice in _choices)
            {
                if (choice.IsEnabled)
                {
                    choice.OnSelected?.Invoke();
                    break;
                }
            }
            Close();
            return true;
        }
    }
}
