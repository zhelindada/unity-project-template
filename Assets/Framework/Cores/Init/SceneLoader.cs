using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Dada.Cores;

public static class SceneLoader
{
    public static async UniTask LoadSceneAsync(
        string sceneName,
        LoadSceneMode mode = LoadSceneMode.Single)
    {
        await SceneManager.LoadSceneAsync(sceneName, mode).ToUniTask();
    }

    public static async UniTask UnloadSceneAsync(string sceneName)
    {
        await SceneManager.UnloadSceneAsync(sceneName).ToUniTask();
    }
}
