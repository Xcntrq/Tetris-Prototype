using UnityEngine;
using nsShapeProperties;
using nsGameBoard;
using nsShape;

namespace nsMovingShape
{
    public class SctMovingShape : SctShape
    {
        private SctShapeProperties m_sctShapeProperties;
        private SctShape m_ghostShape;
        private SctGameBoard m_sctGameBoard;

        public bool HasReceivedInput { get; set; }

        public void AttachGhostShape(SctShape ghostShape, Color ghostColor)
        {
            m_ghostShape = ghostShape;
            m_ghostShape.Color = ghostColor;
            UpdateGhostShape();
        }

        private void UpdateGhostShape()
        {
            Vector3 ghostPosition = transform.position;
            ghostPosition.z = m_ghostShape.transform.position.z;
            m_ghostShape.transform.SetPositionAndRotation(ghostPosition, transform.rotation);
            while (m_sctGameBoard.IsPositionValid(m_ghostShape))
            {
                m_ghostShape.MoveDown();
            }
            m_ghostShape.MoveUp();
        }

        private void Awake()
        {
            m_sctShapeProperties = GetComponent<SctShapeProperties>();
            m_sctGameBoard = FindObjectOfType<SctGameBoard>();
            m_sctGameBoard.OnRowClear += UpdateGhostShape;
            HasReceivedInput = false;
        }

        private void OnDestroy()
        {
            m_sctGameBoard.OnRowClear -= UpdateGhostShape;
            if (m_ghostShape != null) Destroy(m_ghostShape.gameObject);
        }

        public void MoveLeft()
        {
            transform.Translate(Vector3.left, Space.World);
            HasReceivedInput = true;
            UpdateGhostShape();
        }

        public void MoveRight()
        {
            transform.Translate(Vector3.right, Space.World);
            HasReceivedInput = true;
            UpdateGhostShape();
        }

        public void MoveDown(bool isReceivingInput)
        {
            transform.Translate(Vector3.down, Space.World);
            if (isReceivingInput) HasReceivedInput = true;
        }

        public void Rotate(RotationDirection rotationDirection)
        {
            if (!m_sctShapeProperties.IsRotatable) return;
            if (rotationDirection == RotationDirection.CW) transform.Rotate(0, 0, -90);
            if (rotationDirection == RotationDirection.CCW) transform.Rotate(0, 0, 90);
            HasReceivedInput = true;
            UpdateGhostShape();
        }

        public void RotateOppositeDirection(RotationDirection rotationDirection)
        {
            if (!m_sctShapeProperties.IsRotatable) return;
            if (rotationDirection == RotationDirection.CW) transform.Rotate(0, 0, 90);
            if (rotationDirection == RotationDirection.CCW) transform.Rotate(0, 0, -90);
            HasReceivedInput = true;
            UpdateGhostShape();
        }

        public void EnableGhost()
        {
            m_ghostShape.gameObject.SetActive(true);
            UpdateGhostShape();
        }

        public void DisableGhost()
        {
            m_ghostShape.gameObject.SetActive(false);
        }
    }
}
