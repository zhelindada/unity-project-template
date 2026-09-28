using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dada.Cores;

public class SerializationService : ISerializationService
{
    public void Save<T>(string key, T value)
    {
        if (value == null)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.DeleteKey(TypeTagKey(key));
            return;
        }

        var type = typeof(T);
        object payload;
        string typeTag;

        if (IsList(type))
        {
            typeTag = "list";
            var elemType = type.GetGenericArguments()[0];
            payload = WrapList(value, elemType);
        }
        else if (IsDictionary(type))
        {
            typeTag = "dict";
            var args = type.GetGenericArguments();
            payload = WrapDict(value, args[0], args[1]);
        }
        else
        {
            typeTag = "direct";
            payload = value;
        }

        PlayerPrefs.SetString(TypeTagKey(key), typeTag);
        PlayerPrefs.SetString(key, JsonUtility.ToJson(payload));
        PlayerPrefs.Save();
    }

    public T Load<T>(string key, T defaultValue = default)
    {
        if (!PlayerPrefs.HasKey(key))
            return defaultValue;

        var type = typeof(T);
        var typeTag = PlayerPrefs.GetString(TypeTagKey(key), "direct");
        var json = PlayerPrefs.GetString(key);

        try
        {
            return typeTag switch
            {
                "list" => (T)UnwrapList(json, type),
                "dict" => (T)UnwrapDict(json, type),
                _ => JsonUtility.FromJson<T>(json)
            };
        }
        catch
        {
            return defaultValue;
        }
    }

    public bool HasKey(string key) => PlayerPrefs.HasKey(key);

    public void Delete(string key)
    {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.DeleteKey(TypeTagKey(key));
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Flush()
    {
        PlayerPrefs.Save();
    }

    // ── Helpers ────────────────────────────────────

    private static string TypeTagKey(string key) => key + "#type";

    private static bool IsList(Type t) =>
        t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>);

    private static bool IsDictionary(Type t) =>
        t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>);

    // ── Wrap ───────────────────────────────────────

    private static object WrapList(object value, Type elemType)
    {
        var wrapperType = typeof(WrapperList<>).MakeGenericType(elemType);
        return Activator.CreateInstance(wrapperType, value);
    }

    private static object WrapDict(object value, Type keyType, Type valType)
    {
        var wrapperType = typeof(WrapperDict<,>).MakeGenericType(keyType, valType);
        return Activator.CreateInstance(wrapperType, value);
    }

    // ── Unwrap ─────────────────────────────────────

    private static object UnwrapList(string json, Type listType)
    {
        var elemType = listType.GetGenericArguments()[0];
        var wrapperType = typeof(WrapperList<>).MakeGenericType(elemType);
        var wrapper = JsonUtility.FromJson(json, wrapperType);
        return wrapperType.GetProperty("Items")!.GetValue(wrapper);
    }

    private static object UnwrapDict(string json, Type dictType)
    {
        var args = dictType.GetGenericArguments();
        var wrapperType = typeof(WrapperDict<,>).MakeGenericType(args[0], args[1]);
        var wrapper = JsonUtility.FromJson(json, wrapperType);
        return wrapperType.GetProperty("Items")!.GetValue(wrapper);
    }

    // ── Wrapper types ──────────────────────────────

    [Serializable]
    internal class WrapperList<T>
    {
        public T[] array;

        public WrapperList() { }

        public WrapperList(List<T> list)
        {
            array = list.ToArray();
        }

        public List<T> Items => new(array ?? Array.Empty<T>());
    }

    [Serializable]
    internal class WrapperDict<K, V>
    {
        public K[] keys;
        public V[] values;

        public WrapperDict() { }

        public WrapperDict(Dictionary<K, V> dict)
        {
            keys = dict.Keys.ToArray();
            values = dict.Values.ToArray();
        }

        public Dictionary<K, V> Items
        {
            get
            {
                var dict = new Dictionary<K, V>();
                if (keys == null || values == null) return dict;
                var count = Mathf.Min(keys.Length, values.Length);
                for (int i = 0; i < count; i++)
                    dict[keys[i]] = values[i];
                return dict;
            }
        }
    }
}
