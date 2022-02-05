using nsGameStateManager;
using UnityEngine;

namespace nsOnPauseDisabler
{
    public class OnPauseDisabler : MonoBehaviour
    {
        private void OnEnable()
        {
            GameStateManager.Instance.OnPauseToggled += GameStateManager_OnPauseToggled;
        }

        private void OnDisable()
        {
            GameStateManager.Instance.OnPauseToggled -= GameStateManager_OnPauseToggled;
        }

        private void GameStateManager_OnPauseToggled(GameState gameState)
        {
            if (gameState == GameState.Paused) gameObject.SetActive(false);
        }
    }
}
