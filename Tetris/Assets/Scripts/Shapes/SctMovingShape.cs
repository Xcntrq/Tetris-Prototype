using UnityEngine;
using nsShapeProperties;
using nsGameBoard;

namespace nsMovingShape
{
    public class SctMovingShape : nsGhostShape.SctGhostShape
    {
        private SctShapeProperties m_sctShapeProperties;
        private nsGhostShape.SctGhostShape m_sctGhostShape;
        private SctGameBoard m_sctGameBoard;
        private Color m_ghostColor;
        private bool m_hasReceivedInput = false;

        public bool HasReceivedInput
        {
            get { return m_hasReceivedInput; }
            set { m_hasReceivedInput = value; }
        }

        public Color GhostColor
        {
            set
            {
                m_ghostColor = value;
            }
        }

        public nsGhostShape.SctGhostShape SctGhostShape
        {
            set
            {
                m_sctGhostShape = value;
                SpriteRenderer[] allSquares = m_sctGhostShape.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer square in allSquares)
                {
                    square.color = m_ghostColor;
                }
                UpdateGhostShape();
            }
        }

        private void UpdateGhostShape()
        {
            m_sctGhostShape.transform.SetPositionAndRotation(transform.position, transform.rotation);
            while (m_sctGameBoard.IsPositionValid(m_sctGhostShape))
            {
                m_sctGhostShape.MoveDown();
            }
            m_sctGhostShape.MoveUp();
        }

        private void Awake()
        {
            m_sctShapeProperties = GetComponent<SctShapeProperties>();
            m_sctGameBoard = FindObjectOfType<SctGameBoard>();
            m_sctGameBoard.OnRowClear += UpdateGhostShape;
            m_hasReceivedInput = false;
        }

        private void OnDestroy()
        {
            m_sctGameBoard.OnRowClear -= UpdateGhostShape;
            if (m_sctGhostShape != null) Destroy(m_sctGhostShape.gameObject);
        }

        public void MoveLeft()
        {
            transform.Translate(Vector3.left, Space.World);
            UpdateGhostShape();
        }

        public void MoveRight()
        {
            transform.Translate(Vector3.right, Space.World);
            UpdateGhostShape();
        }

        public void Rotate(RotationDirection rotationDirection)
        {
            if (!m_sctShapeProperties.IsRotatable) return;
            if (rotationDirection == RotationDirection.CW) transform.Rotate(0, 0, -90);
            if (rotationDirection == RotationDirection.CCW) transform.Rotate(0, 0, 90);
            UpdateGhostShape();
        }

        public void RotateOppositeDirection(RotationDirection rotationDirection)
        {
            if (!m_sctShapeProperties.IsRotatable) return;
            if (rotationDirection == RotationDirection.CW) transform.Rotate(0, 0, 90);
            if (rotationDirection == RotationDirection.CCW) transform.Rotate(0, 0, -90);
            UpdateGhostShape();
        }

        public SctShapeProperties ToSctShapeProperties()
        {
            var result = gameObject.GetComponent<SctShapeProperties>();
            //Destroys the GhostShape as well as the MovingShape itself, no need to call Destroy(m_sctGhostShape.gameObject);
            Destroy(this);
            //GameObject without the MovingShape component is returned
            return result;
        }
    }
}
