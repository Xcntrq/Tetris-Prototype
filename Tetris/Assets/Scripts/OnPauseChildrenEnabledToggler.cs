using UnityEngine;
using nsGameStateManager;

namespace nsOnPauseChildrenEnabledToggler
{
    public class OnPauseChildrenEnabledToggler : MonoBehaviour
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
            switch (gameState)
            {
                case GameState.Playing:
                    SetChildrenActive(false);
                    break;
                case GameState.Paused:
                    SetChildrenActive(true);
                    break;
            }
        }

        private void SetChildrenActive(bool value)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(value);
            }
        }
    }
}
