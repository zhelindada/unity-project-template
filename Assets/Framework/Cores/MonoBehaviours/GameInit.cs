using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dada.Cores
{
    public class GameInit : MonoBehaviour
    {
        [SerializeField] private string _targetScene;

        private async void Start()
        {
            var initScene = SceneManager.GetActiveScene().name;

            await SceneLoader.LoadSceneAsync(_targetScene, LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_targetScene));
            await SceneLoader.UnloadSceneAsync(initScene);
        }
    }
}

