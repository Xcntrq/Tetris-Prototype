using UnityEngine;

namespace nsGameStateManager
{
    public enum GameState
    {
        Playing,
        Paused,
        Over
    }

    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance { get; private set; } = null;

        public GameState GameState { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //GameStateManager doesn't need to be persistent - reloading the scene currently means starting a new game and should reset the state anyway.
                //DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            GameState = GameState.Playing;
        }
    }
}
