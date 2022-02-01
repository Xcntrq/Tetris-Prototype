using UnityEngine;

namespace nsGameStateManager
{
    public enum GameState
    {
        Playing,
        Paused,
        Over
    }

    public class GameStateManager
    {
        private static readonly GameStateManager instance = new GameStateManager();

        public static GameStateManager Instance { get => instance; }

        public GameState GameState { get; private set; }

        private GameStateManager()
        {
            GameState = GameState.Playing;
        }

        public void TogglePause()
        {
            switch (GameState)
            {
                case GameState.Playing:
                    GameState = GameState.Paused;
                    break;
                case GameState.Paused:
                    GameState = GameState.Playing;
                    break;
            }
            //Debug.Log("CRAAAAAAP");
        }

        public void TriggerGameOver()
        {
            GameState = GameState.Over;
        }
    }
}
