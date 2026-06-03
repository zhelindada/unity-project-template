using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine.UIElements;

namespace Dada.Core.UI
{
    public static class UIViewExtensions
    {
        // ==================== R3 → UI Toolkit Bindings ====================

        public static IDisposable BindTextTo(this Label label, Observable<string> source)
        {
            return source.Subscribe(text =>
            {
                if (label != null) label.text = text ?? string.Empty;
            });
        }

        public static IDisposable BindTextTo<T>(this Label label, Observable<T> source)
        {
            return source.Subscribe(value =>
            {
                if (label != null) label.text = value?.ToString() ?? string.Empty;
            });
        }

        public static IDisposable BindValueTo(this TextField field, Observable<string> source)
        {
            return source.Subscribe(text =>
            {
                if (field != null && field.value != text) field.value = text ?? string.Empty;
            });
        }

        public static IDisposable BindValueTo(this Toggle toggle, Observable<bool> source)
        {
            return source.Subscribe(value =>
            {
                if (toggle != null && toggle.value != value) toggle.value = value;
            });
        }

        public static IDisposable BindValueTo<T>(this Slider slider, Observable<T> source) where T : struct
        {
            return source.Subscribe(value =>
            {
                if (slider != null) slider.value = Convert.ToSingle(value);
            });
        }

        public static IDisposable BindValueTo<T>(this SliderInt slider, Observable<T> source) where T : struct
        {
            return source.Subscribe(value =>
            {
                if (slider != null) slider.value = Convert.ToInt32(value);
            });
        }

        public static IDisposable BindEnabledTo(this VisualElement element, Observable<bool> source)
        {
            return source.Subscribe(enabled =>
            {
                if (element != null) element.SetEnabled(enabled);
            });
        }

        public static IDisposable BindVisibleTo(this VisualElement element, Observable<bool> source)
        {
            return source.Subscribe(visible =>
            {
                if (element != null)
                    element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            });
        }

        public static IDisposable BindClassTo(this VisualElement element, Observable<bool> source, string className)
        {
            return source.Subscribe(active =>
            {
                if (element != null)
                {
                    if (active) element.AddToClassList(className);
                    else element.RemoveFromClassList(className);
                }
            });
        }

        // ==================== UI Toolkit → R3 ====================

        public static Observable<string> OnValueChangedAsObservable(this TextField field)
        {
            return Observable.FromEvent<string>(
                h => field.RegisterValueChangedCallback(evt => h(evt.newValue)),
                h => field.UnregisterValueChangedCallback(evt => h(evt.newValue)));
        }

        public static Observable<bool> OnValueChangedAsObservable(this Toggle toggle)
        {
            return Observable.FromEvent<bool>(
                h => toggle.RegisterValueChangedCallback(evt => h(evt.newValue)),
                h => toggle.UnregisterValueChangedCallback(evt => h(evt.newValue)));
        }

        public static Observable<float> OnValueChangedAsObservable(this Slider slider)
        {
            return Observable.FromEvent<float>(
                h => slider.RegisterValueChangedCallback(evt => h(evt.newValue)),
                h => slider.UnregisterValueChangedCallback(evt => h(evt.newValue)));
        }

        public static Observable<int> OnValueChangedAsObservable(this SliderInt slider)
        {
            return Observable.FromEvent<int>(
                h => slider.RegisterValueChangedCallback(evt => h(evt.newValue)),
                h => slider.UnregisterValueChangedCallback(evt => h(evt.newValue)));
        }

        public static Observable<ClickEvent> OnClickAsObservable(this VisualElement element)
        {
            return Observable.FromEvent<ClickEvent>(
                h => element.RegisterCallback<ClickEvent>(h),
                h => element.UnregisterCallback<ClickEvent>(h));
        }

        public static Observable<ChangeEvent<T>> OnValueChangedAsObservable<T>(this VisualElement element)
        {
            return Observable.FromEvent<ChangeEvent<T>>(
                h => element.RegisterCallback<ChangeEvent<T>>(h),
                h => element.UnregisterCallback<ChangeEvent<T>>(h));
        }

        // ==================== VisualElement Helpers ====================

        public static T Q<T>(this VisualElement element, string name = null, string className = null) where T : VisualElement
        {
            return element?.Q<T>(name, className);
        }

        public static CancellationToken GetCancellationTokenOnDestroy(this UIView view)
        {
            var cts = new CancellationTokenSource();
            void OnClosed() { cts.Cancel(); cts.Dispose(); }
            view.Closed += OnClosed;
            return cts.Token;
        }

        public static IDisposable AddTo(this IDisposable disposable, UIView view)
        {
            void OnClosed() { disposable.Dispose(); }
            view.Closed += OnClosed;
            return disposable;
        }
    }
}
