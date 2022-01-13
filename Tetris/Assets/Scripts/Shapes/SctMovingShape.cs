using UnityEngine;
using nsShapeProperties;
using nsGameBoard;

namespace nsMovingShape
{
    public class SctMovingShape : nsShape.SctShape
    {
        private SctShapeProperties m_sctShapeProperties;
        private nsShape.SctShape m_ghost;
        private SctGameBoard m_sctGameBoard;
        private Color m_ghostColor;
        private bool m_hasReceivedInput = false;

        public bool HasReceivedInput
        {
            get => m_hasReceivedInput;
            set => m_hasReceivedInput = value;
        }

        public Color GhostColor
        {
            set => m_ghostColor = value;
        }

        public nsShape.SctShape Ghost
        {
            set
            {
                m_ghost = value;
                SpriteRenderer[] allSquares = m_ghost.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer square in allSquares)
                {
                    square.color = m_ghostColor;
                }
                UpdateShapeGhost();
            }
        }

        private void UpdateShapeGhost()
        {
            m_ghost.transform.SetPositionAndRotation(transform.position, transform.rotation);
            while (m_sctGameBoard.IsPositionValid(m_ghost))
            {
                m_ghost.MoveDown();
            }
            m_ghost.MoveUp();
        }

        private void Awake()
        {
            m_sctShapeProperties = GetComponent<SctShapeProperties>();
            m_sctGameBoard = FindObjectOfType<SctGameBoard>();
            m_sctGameBoard.OnRowClear += UpdateShapeGhost;
            m_hasReceivedInput = false;
        }

        private void OnDestroy()
        {
            m_sctGameBoard.OnRowClear -= UpdateShapeGhost;
            if (m_ghost != null) Destroy(m_ghost.gameObject);
        }

        public void MoveLeft()
        {
            transform.Translate(Vector3.left, Space.World);
            m_hasReceivedInput = true;
            UpdateShapeGhost();
        }

        public void MoveRight()
        {
            transform.Translate(Vector3.right, Space.World);
            m_hasReceivedInput = true;
            UpdateShapeGhost();
        }

        public void MoveDown(bool isReceivingInput)
        {
            transform.Translate(Vector3.down, Space.World);
            m_hasReceivedInput = m_hasReceivedInput || isReceivingInput;
        }

        public void Rotate(RotationDirection rotationDirection)
        {
            if (!m_sctShapeProperties.IsRotatable) return;
            if (rotationDirection == RotationDirection.CW) transform.Rotate(0, 0, -90);
            if (rotationDirection == RotationDirection.CCW) transform.Rotate(0, 0, 90);
            m_hasReceivedInput = true;
            UpdateShapeGhost();
        }

        public void RotateOppositeDirection(RotationDirection rotationDirection)
        {
            if (!m_sctShapeProperties.IsRotatable) return;
            if (rotationDirection == RotationDirection.CW) transform.Rotate(0, 0, 90);
            if (rotationDirection == RotationDirection.CCW) transform.Rotate(0, 0, -90);
            m_hasReceivedInput = true;
            UpdateShapeGhost();
        }

        public void EnableGhost()
        {
            m_ghost.gameObject.SetActive(true);
            UpdateShapeGhost();
        }

        public void DisableGhost()
        {
            m_ghost.gameObject.SetActive(false);
        }
    }
}
