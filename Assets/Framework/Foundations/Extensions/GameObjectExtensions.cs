using UnityEngine;

namespace Dada.Foundations;

public static class GameObjectExtensions{

    public static T GetOrAdd<T>(this GameObject obj) where T : Component{
        if(obj.TryGetComponent<T>(out T t))
            return t;
        return obj.AddComponent<T>();
    }
}
