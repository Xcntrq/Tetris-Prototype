using UnityEngine;

namespace nsGhostShape
{
    public class SctGhostShape : MonoBehaviour
    {
        public void MoveUp()
        {
            transform.Translate(Vector3.up, Space.World);
        }

        public void MoveDown()
        {
            transform.Translate(Vector3.down, Space.World);
        }
    }
}
