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
            SceneManager.LoadScene("Load");
            var ao = SceneManager.LoadSceneAsync(sceneName);
        
            // while (!ao.isDone)
            // {
            //     //progressBar.value = ao.progress;
            // }
            SceneManager.UnloadSceneAsync("Load");
        }
    }
}

