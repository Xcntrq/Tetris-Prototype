using UnityEngine;
using nsGameStateManager;

namespace nsManagerCreator
{
    public class ManagerCreator : MonoBehaviour
    {
        private void Awake()
        {
            if (GameStateManager.Instance != null) GameStateManager.Instance.Initialize();
        }
    }
}
