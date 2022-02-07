using UnityEngine;

namespace nsApplicationQuitter
{
    public class ApplicationQuitter
    {
        public void QuitApplication()
        {
            Application.Quit();
            Debug.Log("I quit!");
        }
    }
}
