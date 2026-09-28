using System;
using System.Collections.Generic;

namespace Dada.Cores;
    public interface IEvent { }

    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _handlers = new();
        private static readonly object _lock = new();

        public static void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);
            lock (_lock)
            {
                if (!_handlers.ContainsKey(type))
                    _handlers[type] = new List<Delegate>();
                _handlers[type].Add(handler);
            }
        }

        public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            var type = typeof(T);
            lock (_lock)
            {
                if (_handlers.TryGetValue(type, out var list))
                    list.Remove(handler);
            }
        }

        public static void Publish<T>(T evt) where T : IEvent
        {
            var type = typeof(T);
            List<Delegate> handlers;
            lock (_lock)
            {
                if (!_handlers.TryGetValue(type, out handlers) || handlers.Count == 0)
                    return;
                handlers = new List<Delegate>(handlers);
            }

            foreach (var handler in handlers)
            {
                try
                {
                    ((Action<T>)handler)?.Invoke(evt);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogError($"[EventBus] Error handling {type.Name}: {ex}");
                }
            }
        }

        public static void Clear()
        {
            lock (_lock) { _handlers.Clear(); }
        }
    }
