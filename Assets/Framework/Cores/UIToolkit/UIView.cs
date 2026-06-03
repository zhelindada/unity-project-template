using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dada.Core.UI
{
    public abstract class UIView
    {
        public VisualElement Root { get; private set; }
        public UILayerType Layer { get; internal set; }
        public bool IsVisible { get; private set; }

        public event Action Opened;
        public event Action Closed;

        protected virtual string UxmlPath => null;

        public void Create()
        {
            if (!string.IsNullOrEmpty(UxmlPath))
            {
                var tree = Resources.Load<VisualTreeAsset>(UxmlPath);
                if (tree != null)
                {
                    Root = tree.CloneTree();
                    Root.style.flexGrow = 1;
                }
            }

            Root ??= BuildUI();
            if (Root == null)
                throw new InvalidOperationException($"{GetType().Name}: 必须通过 UxmlPath 或 BuildUI() 提供 Root VisualElement。");

            Root.visible = false;
            Root.style.display = DisplayStyle.None;
            OnCreate();
        }

        public async UniTask ShowAsync()
        {
            if (IsVisible) return;

            Root.style.display = DisplayStyle.Flex;
            Root.visible = true;
            Root.BringToFront();

            await PlayShowTransitionAsync();
            IsVisible = true;
            OnShow();
            Opened?.Invoke();
        }

        public async UniTask HideAsync()
        {
            if (!IsVisible) return;

            OnPreHide();
            IsVisible = false;

            await PlayHideTransitionAsync();

            Root.style.display = DisplayStyle.None;
            Root.visible = false;
            OnHide();
            Closed?.Invoke();
        }

        public void ShowImmediate()
        {
            if (IsVisible) return;
            Root.style.display = DisplayStyle.Flex;
            Root.visible = true;
            Root.BringToFront();
            IsVisible = true;
            OnShow();
            Opened?.Invoke();
        }

        public void HideImmediate()
        {
            if (!IsVisible) return;
            OnPreHide();
            IsVisible = false;
            Root.style.display = DisplayStyle.None;
            Root.visible = false;
            OnHide();
            Closed?.Invoke();
        }

        protected virtual VisualElement BuildUI() => null;

        protected virtual void OnCreate() { }
        protected virtual void OnShow() { }
        protected virtual void OnPreHide() { }
        protected virtual void OnHide() { }

        protected virtual UniTask PlayShowTransitionAsync()
        {
            return FadeTransitionAsync(Root, 0f, 1f, 0.2f);
        }

        protected virtual UniTask PlayHideTransitionAsync()
        {
            return FadeTransitionAsync(Root, 1f, 0f, 0.15f);
        }

        protected static async UniTask FadeTransitionAsync(VisualElement element, float from, float to, float duration)
        {
            element.style.opacity = from;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                element.style.opacity = Mathf.Lerp(from, to, elapsed / duration);
                await UniTask.Yield(PlayerLoopTiming.Update);
            }
            element.style.opacity = to;
        }

        public void Destroy()
        {
            if (IsVisible) HideImmediate();
            OnDestroy();
            Root?.RemoveFromHierarchy();
        }

        protected virtual void OnDestroy() { }
    }
}
