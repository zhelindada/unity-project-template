using System;
using UnityEngine;

namespace Dada.Foundations;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;
    protected string typeName;

    public static bool HasInstance => _instance != null;

    public static T Instance {
        get
        {
            if (_instance != null) return _instance;
            var find = FindObjectsOfType(typeof(T));
            _instance = find.Length > 0 ? find[0] as T : Instantiate();
            _instance!.Initialize();
            return _instance;
        }
    }

    public virtual void Initialize()
    {
        
    }

    public static T Instantiate()
    {
        if (_instance != null) return _instance;
        
        GameObject go = new GameObject(typeof(T).Name);
        go.AddComponent<T>();
        _instance = go.GetComponent<T>();
        _instance.Initialize();
        DontDestroyOnLoad(go);
        return go.GetComponent<T>();
    }

    private void Awake()
    {
        typeName = GetType().Name;
        Debug.Log($"[{typeName}::Awake] On Awake");
        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }
}
