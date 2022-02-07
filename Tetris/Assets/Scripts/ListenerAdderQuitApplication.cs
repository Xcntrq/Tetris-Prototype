using nsApplicationQuitter;
using UnityEngine;
using UnityEngine.UI;

namespace ns_ListenerAdderQuitApplication
{
    public class ListenerAdderQuitApplication : MonoBehaviour
    {
        private void Awake()
        {
            Button button = GetComponent<Button>();
            if (button != null)
            {
                GameManager gameManager = FindObjectOfType<GameManager>();
                ApplicationQuitter applicationQuitter = gameManager.ApplicationQuitter;
                button.onClick.AddListener(applicationQuitter.QuitApplication);
            }
            else
            {
                string errorMessage = string.Concat("No Button component on object: ", gameObject.name);
                Debug.Log(errorMessage);
            }
        }
    }
}
