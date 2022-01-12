using UnityEngine;
using nsMovingShape;
using nsShape;

namespace nsShapeHolder
{
    public class SctShapeHolder : MonoBehaviour
    {
        [SerializeField] private Transform m_shapeHoldSlot;
        [SerializeField] private float m_shapeScaleFactor;

        private SctMovingShape m_currentlyHeldShape;
        private Transform m_parent;
        private Quaternion m_localRotation;
        private Vector3 m_localScale;
        private nsGameBoard.SctGameBoard m_sctGameBoard;

        private void Awake()
        {
            m_currentlyHeldShape = null;
            m_sctGameBoard = FindObjectOfType<nsGameBoard.SctGameBoard>();
        }

        public SctMovingShape HoldShape(SctMovingShape shapeToHold)
        {
            SctMovingShape shapeToReturn = m_currentlyHeldShape;
            if (shapeToReturn != null)
            {
                shapeToReturn.transform.parent = m_parent;
                shapeToReturn.transform.position = shapeToHold.transform.position;
                shapeToReturn.transform.localRotation = m_localRotation;
                shapeToReturn.transform.localScale = m_localScale;
                while (m_sctGameBoard.IsPositionValid(shapeToReturn) == false)
                {
                    shapeToReturn.transform.Translate(Vector3.up, Space.World);
                }
                shapeToReturn.EnableGhost();
            }

            shapeToHold.DisableGhost();
            m_currentlyHeldShape = shapeToHold;
            m_parent = shapeToHold.transform.parent;
            m_localRotation = shapeToHold.transform.localRotation;
            m_localScale = shapeToHold.transform.localScale;

            m_currentlyHeldShape.transform.parent = m_shapeHoldSlot;
            m_currentlyHeldShape.transform.rotation = Quaternion.identity;
            m_currentlyHeldShape.transform.localPosition = Vector3.zero;

            Vector3 centerOffset = m_currentlyHeldShape.GetComponent<nsShapeProperties.SctShapeProperties>().CenterOffset;

            centerOffset = Vector3.zero - centerOffset * m_shapeScaleFactor;
            Vector3 rotatedCenterOffset = Quaternion.AngleAxis(m_localRotation.eulerAngles.z, Vector3.forward) * centerOffset;

            m_currentlyHeldShape.transform.rotation = m_localRotation;
            m_currentlyHeldShape.transform.localPosition = rotatedCenterOffset;
            m_currentlyHeldShape.transform.localScale = new Vector3(m_shapeScaleFactor, m_shapeScaleFactor, 1);

            return shapeToReturn;
        }
    }
}
