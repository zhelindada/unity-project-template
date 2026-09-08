using System.Collections.Generic;
using UnityEngine;

namespace Dada.Cores
{
    /// <summary>
    /// UI模块对象池。缓存已关闭的模块实例，避免频繁Instantiate/Destroy。
    /// 按类型分组管理，支持预热和容量限制。
    /// </summary>
    public class UIPool
    {
        private readonly Dictionary<System.Type, Stack<UIModule>> _pool = new();
        private readonly Transform _poolRoot;
        private readonly int _defaultMaxSize;

        public UIPool(Transform root, int defaultMaxSize = 10)
        {
            _poolRoot = root;
            _defaultMaxSize = defaultMaxSize;
        }

        /// <summary>从池中获取一个实例，池空则返回null。</summary>
        public T Get<T>() where T : UIModule
        {
            var type = typeof(T);
            if (!_pool.TryGetValue(type, out var stack) || stack.Count == 0)
                return null;

            var instance = stack.Pop() as T;
            if (instance != null)
                instance.gameObject.SetActive(true);
            return instance;
        }

        /// <summary>将模块回收至池中。</summary>
        public void Return(UIModule module)
        {
            if (module == null) return;

            var type = module.GetType();
            if (!_pool.TryGetValue(type, out var stack))
            {
                stack = new Stack<UIModule>();
                _pool[type] = stack;
            }

            if (stack.Count >= _defaultMaxSize)
            {
                Object.Destroy(module.gameObject);
                return;
            }

            module.gameObject.SetActive(false);
            module.transform.SetParent(_poolRoot, false);
            stack.Push(module);
        }

        /// <summary>预热指定数量的实例。</summary>
        public void Warmup<T>(T prefab, int count) where T : UIModule
        {
            for (int i = 0; i < count; i++)
            {
                var instance = Object.Instantiate(prefab, _poolRoot);
                instance.gameObject.SetActive(false);
                Return(instance);
            }
        }

        /// <summary>清空池。</summary>
        public void Clear()
        {
            foreach (var stack in _pool.Values)
            {
                while (stack.Count > 0)
                {
                    var module = stack.Pop();
                    if (module != null)
                        Object.Destroy(module.gameObject);
                }
            }
            _pool.Clear();
        }

        public int Count<T>() where T : UIModule
        {
            return _pool.TryGetValue(typeof(T), out var stack) ? stack.Count : 0;
        }
    }
}
