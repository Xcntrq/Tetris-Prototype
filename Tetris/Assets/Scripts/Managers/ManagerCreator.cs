using UnityEngine;
using nsGameStateManager;

namespace nsManagerCreator
{
    public class ManagerCreator : MonoBehaviour
    {
        private void Awake()
        {
            if (GameStateManager.Instance == null)
            {
                GameObject gameObject = new GameObject("GameStateManager");
                //GameStateManager doesn't need to be persistent because re-launching the scene means a new game and resets the state of the game anyways.
                //So into this object as a child it goes.
                gameObject.transform.parent = transform;
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.AddComponent<GameStateManager>();
            }
        }
    }
}
