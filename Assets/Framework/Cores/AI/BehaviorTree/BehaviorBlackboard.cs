using System;
using System.Collections.Generic;

namespace Dada.Cores.AI
{
    public sealed class BehaviorBlackboard
    {
        private readonly Dictionary<string, object> _values = new(StringComparer.Ordinal);

        public int Count => _values.Count;
        public IReadOnlyCollection<string> Keys => _values.Keys;

        public void Set<T>(string key, T value)
        {
            ValidateKey(key);
            _values[key] = value;
        }

        public T Get<T>(string key)
        {
            ValidateKey(key);
            if (!_values.TryGetValue(key, out object rawValue))
                throw new KeyNotFoundException($"行为树黑板中不存在键 '{key}'。");

            if (TryConvert(rawValue, out T value))
                return value;

            string actualType = rawValue?.GetType().FullName ?? "null";
            throw new InvalidCastException(
                $"行为树黑板键 '{key}' 的值类型是 {actualType}，不能读取为 {typeof(T).FullName}。");
        }

        public T Get<T>(string key, T defaultValue)
        {
            return TryGet(key, out T value) ? value : defaultValue;
        }

        public bool TryGet<T>(string key, out T value)
        {
            ValidateKey(key);
            if (_values.TryGetValue(key, out object rawValue) && TryConvert(rawValue, out value))
                return true;

            value = default;
            return false;
        }

        public bool Contains(string key)
        {
            ValidateKey(key);
            return _values.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            ValidateKey(key);
            return _values.Remove(key);
        }

        public void Clear()
        {
            _values.Clear();
        }

        private static bool TryConvert<T>(object rawValue, out T value)
        {
            if (rawValue is T typedValue)
            {
                value = typedValue;
                return true;
            }

            if (rawValue == null && default(T) is null)
            {
                value = default;
                return true;
            }

            value = default;
            return false;
        }

        private static void ValidateKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("行为树黑板键不能为空。", nameof(key));
        }
    }
}
