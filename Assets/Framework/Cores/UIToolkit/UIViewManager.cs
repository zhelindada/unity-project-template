using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dada.Core.UI
{
    public class UIViewManager : IDisposable
    {
        private readonly Dictionary<UILayerType, VisualElement> _layers = new();
        private readonly Dictionary<UILayerType, Stack<UIView>> _modalStacks = new();
        private readonly List<UIView> _activeViews = new();
        private readonly Dictionary<Type, Func<UIView>> _viewFactories = new();
        private readonly VisualElement _rootElement;

        public event Action<UIView> ViewOpened;
        public event Action<UIView> ViewClosed;

        public UIViewManager(VisualElement rootElement)
        {
            _rootElement = rootElement;
            BuildLayers();
        }

        private void BuildLayers()
        {
            var layerDefs = new (UILayerType type, int order, bool isModal)[]
            {
                (UILayerType.Background,     0, false),
                (UILayerType.Screen,       100, false),
                (UILayerType.Panel,        200, true),
                (UILayerType.Popup,        300, true),
                (UILayerType.Notification, 400, false),
                (UILayerType.Tooltip,      500, false),
                (UILayerType.Overlay,      600, true),
            };

            foreach (var (type, _, isModal) in layerDefs)
            {
                var layer = new VisualElement
                {
                    name = $"{type}Layer",
                    style =
                    {
                        position = Position.Absolute,
                        top = 0, left = 0, right = 0, bottom = 0,
                        flexGrow = 1,
                    },
                    pickingMode = PickingMode.Ignore,
                };
                layer.style.display = DisplayStyle.Flex;

                _rootElement.Add(layer);
                _layers[type] = layer;

                if (isModal)
                    _modalStacks[type] = new Stack<UIView>();
            }
        }

        public void RegisterViewFactory<T>(Func<UIView> factory) where T : UIView
        {
            _viewFactories[typeof(T)] = factory;
        }

        public async UniTask<T> ShowAsync<T>(object data = null) where T : UIView
        {
            var view = CreateView<T>();
            if (view == null) return null;

            var layer = ResolveLayer<T>();
            view.Layer = layer;

            _layers[layer].Add(view.Root);
            _activeViews.Add(view);

            if (_modalStacks.TryGetValue(layer, out var stack))
                stack.Push(view);

            if (view is IUIViewDataReceiver receiver && data != null)
                receiver.ReceiveData(data);

            await view.ShowAsync();
            ViewOpened?.Invoke(view);
            return (T)view;
        }

        public async UniTask HideAsync(UIView view)
        {
            if (view == null || !view.IsVisible) return;

            RemoveFromModalStack(view);
            _activeViews.Remove(view);
            await view.HideAsync();
            view.Root.RemoveFromHierarchy();
            ViewClosed?.Invoke(view);
        }

        public void HideImmediate(UIView view)
        {
            if (view == null || !view.IsVisible) return;

            RemoveFromModalStack(view);
            _activeViews.Remove(view);
            view.HideImmediate();
            view.Root.RemoveFromHierarchy();
            ViewClosed?.Invoke(view);
        }

        public async UniTask HideAllAsync(UILayerType? layer = null)
        {
            var targets = layer.HasValue
                ? _activeViews.Where(v => v.Layer == layer.Value).ToList()
                : _activeViews.ToList();

            foreach (var view in targets)
                await HideAsync(view);
        }

        public void HideAllImmediate(UILayerType? layer = null)
        {
            var targets = layer.HasValue
                ? _activeViews.Where(v => v.Layer == layer.Value).ToList()
                : _activeViews.ToList();

            foreach (var view in targets)
                HideImmediate(view);
        }

        public bool IsVisible<T>() where T : UIView
            => _activeViews.Any(v => v is T);

        public T GetView<T>() where T : UIView
            => _activeViews.OfType<T>().FirstOrDefault();

        public IReadOnlyList<UIView> ActiveViews => _activeViews;

        public VisualElement GetLayer(UILayerType layer)
            => _layers.TryGetValue(layer, out var l) ? l : null;

        public bool HasModalOpen
            => _modalStacks.Any(kv => kv.Value.Count > 0);

        public bool CloseTop()
        {
            foreach (var kv in _modalStacks.OrderByDescending(k => (int)k.Key))
            {
                if (kv.Value.TryPeek(out var top) && top.IsVisible)
                {
                    HideAsync(top).Forget();
                    return true;
                }
            }
            return false;
        }

        private UIView CreateView<T>() where T : UIView
        {
            if (_viewFactories.TryGetValue(typeof(T), out var factory))
            {
                var view = factory();
                view.Create();
                return view;
            }

            var instance = (T)Activator.CreateInstance(typeof(T), nonPublic: true);
            instance.Create();
            return instance;
        }

        private UILayerType ResolveLayer<T>() where T : UIView
        {
            var t = typeof(T);
            if (t.Name.Contains("Popup") || t.Name.Contains("Dialog")) return UILayerType.Popup;
            if (t.Name.Contains("Notification") || t.Name.Contains("Toast")) return UILayerType.Notification;
            if (t.Name.Contains("Tooltip")) return UILayerType.Tooltip;
            if (t.Name.Contains("Overlay") || t.Name.Contains("Loading")) return UILayerType.Overlay;
            if (t.Name.Contains("Panel") || t.Name.Contains("Settings")) return UILayerType.Panel;
            return UILayerType.Screen;
        }

        private void RemoveFromModalStack(UIView view)
        {
            foreach (var kv in _modalStacks)
            {
                if (!kv.Value.Contains(view)) continue;
                var temp = new Stack<UIView>();
                while (kv.Value.TryPop(out var v))
                    if (v != view) temp.Push(v);
                while (temp.TryPop(out var v))
                    kv.Value.Push(v);
                break;
            }
        }

        public void Dispose()
        {
            HideAllImmediate();
            _activeViews.Clear();
            _modalStacks.Clear();
            _viewFactories.Clear();
            _layers.Clear();
        }
    }
}
