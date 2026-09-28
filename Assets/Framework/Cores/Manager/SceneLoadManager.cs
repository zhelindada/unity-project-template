using Dada.Foundations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Dada.Cores
{
    public class SceneLoadManager : MonoSingleton<SceneLoadManager>
    {
        [SerializeField] private Slider progressBar;

        public void LoadSceneAsync(string sceneName)
        {
            var ao = SceneManager.LoadSceneAsync(sceneName);
        }
    }
}

