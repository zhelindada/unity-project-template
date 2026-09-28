using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dada.Cores
{
    /// <summary>
    /// 模态弹窗。包含标题、正文、确认/取消按钮。
    /// 仿CK3/City of Gangsters风格的通用确认对话框。
    /// </summary>
    public class PopupUI : UIModule
    {
        [Header("Popup")]
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _bodyText;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private TextMeshProUGUI _confirmLabel;
        [SerializeField] private TextMeshProUGUI _cancelLabel;

        private Action _onConfirm;
        private Action _onCancel;

        // ==================== Builder模式 ====================

        public PopupUI SetTitle(string title)
        {
            if (_titleText != null) _titleText.text = title;
            return this;
        }

        public PopupUI SetBody(string body)
        {
            if (_bodyText != null) _bodyText.text = body;
            return this;
        }

        public PopupUI SetConfirm(string label, Action onConfirm)
        {
            _onConfirm = onConfirm;
            if (_confirmLabel != null) _confirmLabel.text = label;
            if (_confirmButton != null)
            {
                _confirmButton.onClick.RemoveAllListeners();
                _confirmButton.onClick.AddListener(OnConfirmClicked);
            }
            return this;
        }

        public PopupUI SetCancel(string label, Action onCancel = null)
        {
            _onCancel = onCancel;
            if (_cancelLabel != null) _cancelLabel.text = label;
            if (_cancelButton != null)
            {
                _cancelButton.onClick.RemoveAllListeners();
                _cancelButton.onClick.AddListener(OnCancelClicked);
            }
            return this;
        }

        public PopupUI HideCancel()
        {
            if (_cancelButton != null) _cancelButton.gameObject.SetActive(false);
            return this;
        }

        // ==================== 生命周期 ====================

        protected override void OnShow()
        {
            if (_confirmButton != null) _confirmButton.onClick.AddListener(OnConfirmClicked);
            if (_cancelButton != null) _cancelButton.onClick.AddListener(OnCancelClicked);
        }

        protected override void OnHide()
        {
            if (_confirmButton != null) _confirmButton.onClick.RemoveListener(OnConfirmClicked);
            if (_cancelButton != null) _cancelButton.onClick.RemoveListener(OnCancelClicked);
        }

        private void OnConfirmClicked()
        {
            _onConfirm?.Invoke();
            Close();
        }

        private void OnCancelClicked()
        {
            _onCancel?.Invoke();
            Close();
        }

        public override bool Cancel()
        {
            if (!IsOpen || IsAnimating) return false;
            _onCancel?.Invoke();
            Close();
            return true;
        }
    }
}
