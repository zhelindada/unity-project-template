using System;
using System.Collections;
using UnityEngine;

namespace Dada.Cores.UI
{
    /// <summary>
    /// CanvasGroup淡入淡出组件。挂载到任意GameObject上即可使用。
    /// 支持即时切换和动画过渡。
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIFader : MonoBehaviour
    {
        [SerializeField] private float _fadeInDuration = 0.2f;
        [SerializeField] private float _fadeOutDuration = 0.15f;

        private CanvasGroup _canvasGroup;
        private Coroutine _currentRoutine;

        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        public float FadeInDuration => _fadeInDuration;
        public float FadeOutDuration => _fadeOutDuration;

        public void SetAlpha(float alpha)
        {
            StopCurrentRoutine();
            CanvasGroup.alpha = alpha;
        }

        public void FadeIn(Action onComplete = null)
        {
            StopCurrentRoutine();
            _currentRoutine = StartCoroutine(FadeRoutine(1f, _fadeInDuration, onComplete));
        }

        public void FadeOut(Action onComplete = null)
        {
            StopCurrentRoutine();
            _currentRoutine = StartCoroutine(FadeRoutine(0f, _fadeOutDuration, onComplete));
        }

        public void ShowImmediate()
        {
            StopCurrentRoutine();
            CanvasGroup.alpha = 1f;
            SetInteractable(true);
        }

        public void HideImmediate()
        {
            StopCurrentRoutine();
            CanvasGroup.alpha = 0f;
            SetInteractable(false);
        }

        private IEnumerator FadeRoutine(float target, float duration, Action onComplete)
        {
            SetInteractable(target > 0.5f);

            float start = CanvasGroup.alpha;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                CanvasGroup.alpha = Mathf.Lerp(start, target, elapsed / duration);
                yield return null;
            }

            CanvasGroup.alpha = target;
            onComplete?.Invoke();
        }

        private void SetInteractable(bool value)
        {
            CanvasGroup.interactable = value;
            CanvasGroup.blocksRaycasts = value;
        }

        private void StopCurrentRoutine()
        {
            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);
                _currentRoutine = null;
            }
        }
    }
}
