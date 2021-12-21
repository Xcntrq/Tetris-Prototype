using UnityEngine;

namespace nsShape
{
    public class SctShape : MonoBehaviour
    {
        [SerializeField] private bool m_isRotatable;

        public void MoveLeft()
        {
            transform.Translate(Vector3.left, Space.World);
        }

        public void MoveRight()
        {
            transform.Translate(Vector3.right, Space.World);
        }

        public void MoveUp()
        {
            transform.Translate(Vector3.up, Space.World);
        }

        public void MoveDown()
        {
            transform.Translate(Vector3.down, Space.World);
        }

        private void RotateCW()
        {
            if (!m_isRotatable) return;
            transform.Rotate(0, 0, -90);
        }

        private void RotateCCW()
        {
            if (!m_isRotatable) return;
            transform.Rotate(0, 0, 90);
        }

        public void Rotate(RotationDirection rotationDirection)
        {
            if (rotationDirection == RotationDirection.CW) RotateCW();
            if (rotationDirection == RotationDirection.CCW) RotateCCW();
        }

        public void RotateOppositeDirection(RotationDirection rotationDirection)
        {
            if (rotationDirection == RotationDirection.CW) RotateCCW();
            if (rotationDirection == RotationDirection.CCW) RotateCW();
        }
    }
}
