using nsGameStateManager;
using UnityEngine;
using UnityEngine.UI;

namespace nsListenerAdderTogglePause
{
    public class ListenerAdderTogglePause : MonoBehaviour
    {
        private void Awake()
        {
            Button button = GetComponent<Button>();
            if (button != null) button.onClick.AddListener(GameStateManager.Instance.TogglePause);
        }
    }
}
