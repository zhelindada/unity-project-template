using System;
using System.Collections.Generic;

namespace Dada.Cores.EventBus
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _handlers = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_handlers.ContainsKey(type))
                _handlers[type] = Delegate.Combine(_handlers[type], handler);
            else
                _handlers[type] = handler;
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type)) return;
            _handlers[type] = Delegate.Remove(_handlers[type], handler);
            if (_handlers[type] == null)
                _handlers.Remove(type);
        }

        public static void Publish<T>(T eventData)
        {
            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var del)) return;
            (del as Action<T>)?.Invoke(eventData);
        }

        public static void Clear()
        {
            _handlers.Clear();
        }
    }
}
