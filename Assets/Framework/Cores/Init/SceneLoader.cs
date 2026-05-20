using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dada.Cores
{
    public static class SceneLoader
    {
        public static IEnumerator LoadSceneAsync(
            string sceneName,
            Action<float> onProgress = null,
            LoadSceneMode mode = LoadSceneMode.Single)
        {
            var op = SceneManager.LoadSceneAsync(sceneName, mode);
            if (op == null) yield break;

            op.allowSceneActivation = true;

            while (!op.isDone)
            {
                onProgress?.Invoke(op.progress);
                yield return null;
            }
        }

        public static IEnumerator UnloadSceneAsync(string sceneName)
        {
            var op = SceneManager.UnloadSceneAsync(sceneName);
            if (op == null) yield break;

            while (!op.isDone)
                yield return null;
        }
    }
}
