using UnityEngine;

namespace Dada.Cores
{
    [DefaultExecutionOrder(-1000)]
    public class GameInitiator : MonoBehaviour
    {
        [SerializeField] private string _firstScene = "Main";

        private void Awake()
        {
            var gm = GameManager.Instance;
            gm.SetState(GameState.Init);
        }

        private void Start()
        {
            GameManager.Instance.StartGame(_firstScene);
        }
    }
}
