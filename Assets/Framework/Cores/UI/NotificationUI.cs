using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Dada.Cores
{
    /// <summary>
    /// Toast通知消息。从屏幕上方/下方滑入，短暂停留后自动消失。
    /// 支持Info、Warning、Error三种视觉样式。
    /// </summary>
    public class NotificationUI : UIModule
    {
        public enum NotificationType { Info, Warning, Error }

        [Header("Notification")]
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private float _displayDuration = 3f;
        [SerializeField] private Color _infoColor = new(0.2f, 0.2f, 0.2f, 0.9f);
        [SerializeField] private Color _warningColor = new(0.8f, 0.6f, 0.1f, 0.9f);
        [SerializeField] private Color _errorColor = new(0.8f, 0.15f, 0.15f, 0.9f);

        private Coroutine _autoCloseRoutine;
        private float _elapsed;

        public NotificationType Type { get; private set; }
        public string Message { get; private set; }

        /// <summary>由UIManager调用，设置通知内容。</summary>
        public void Configure(string message, NotificationType type = NotificationType.Info, float duration = 3f)
        {
            Message = message;
            Type = type;
            _displayDuration = duration;

            if (_messageText != null)
                _messageText.text = message;

            ApplyStyle(type);
        }

        private void ApplyStyle(NotificationType type)
        {
            var bg = GetComponent<UnityEngine.UI.Image>();
            if (bg == null) return;

            bg.color = type switch
            {
                NotificationType.Warning => _warningColor,
                NotificationType.Error => _errorColor,
                _ => _infoColor,
            };
        }

        protected override void OnShow()
        {
            _elapsed = 0f;
            _autoCloseRoutine = StartCoroutine(AutoCloseRoutine());
        }

        protected override void OnHide()
        {
            if (_autoCloseRoutine != null)
            {
                StopCoroutine(_autoCloseRoutine);
                _autoCloseRoutine = null;
            }
        }

        private IEnumerator AutoCloseRoutine()
        {
            while (_elapsed < _displayDuration)
            {
                _elapsed += Time.unscaledDeltaTime;
                yield return null;
            }
            Close();
        }

        /// <summary>通知不响应ESC（由通知层自行管理）。</summary>
        public override bool Cancel() => false;
    }
}
