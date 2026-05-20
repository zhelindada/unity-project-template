using System.Collections;
using TMPro;
using UnityEngine;

namespace Dada.Cores.UI
{
    /// <summary>
    /// 工具提示。跟随鼠标或锚定在指定位置，显示简短信息后自动隐藏。
    /// </summary>
    public class TooltipUI : UIModule
    {
        [Header("Tooltip")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private RectTransform _background;
        [SerializeField] private Vector2 _offset = new(15f, -10f);

        private bool _followMouse = true;
        private Vector2 _anchoredPosition;

        // ==================== 配置 ====================

        public TooltipUI SetText(string text)
        {
            if (_text != null) _text.text = text;
            return this;
        }

        /// <summary>设置锚定位置（屏幕坐标），同时禁用鼠标跟随。</summary>
        public TooltipUI SetPosition(Vector2 screenPosition)
        {
            _anchoredPosition = screenPosition;
            _followMouse = false;
            return this;
        }

        /// <summary>启用鼠标跟随模式。</summary>
        public TooltipUI SetFollowMouse(bool follow = true)
        {
            _followMouse = follow;
            return this;
        }

        // ==================== 生命周期 ====================

        protected override void OnPreShow()
        {
            _closeOnEscape = false;
        }

        protected override void OnShow()
        {
            StartCoroutine(UpdatePositionRoutine());
        }

        private System.Collections.IEnumerator UpdatePositionRoutine()
        {
            while (IsOpen)
            {
                UpdatePosition();
                yield return null;
            }
        }

        private void UpdatePosition()
        {
            var targetPos = _followMouse
                ? (Vector2)Input.mousePosition + _offset
                : _anchoredPosition;

            var rt = transform as RectTransform;
            if (rt != null)
            {
                // 保持在屏幕内
                var pivotOffset = new Vector2(
                    targetPos.x + rt.rect.width > Screen.width ? -rt.rect.width : 0,
                    targetPos.y - rt.rect.height < 0 ? rt.rect.height : 0
                );
                rt.position = targetPos + pivotOffset;
            }
        }

        public override bool Cancel()
        {
            Close();
            return true;
        }
    }
}
