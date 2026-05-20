namespace Dada.Foundations;

public class Singleton<T> where T : Singleton<T>, new()
{
    private static T _instance;

    public static T Instance {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    public void Initialize()
    {
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {
        
    }
}
