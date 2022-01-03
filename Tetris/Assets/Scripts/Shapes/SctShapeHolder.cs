using UnityEngine;
using nsShapeProperties;
using nsMovingShape;
using nsGhostShape;

namespace nsShapeHolder
{
    public class SctShapeHolder : MonoBehaviour
    {
        [SerializeField] private Transform m_shapeHoldSlot;
        [SerializeField] private float m_shapeScaleFactor;
        [SerializeField] private Color m_ghostColor;

        private SctShapeProperties m_sctShapeProperties;
        private Transform m_parent;
        private Quaternion m_localRotation;
        private Vector3 m_localScale;
        private nsGameBoard.SctGameBoard m_sctGameBoard;

        private void Awake()
        {
            m_sctGameBoard = FindObjectOfType<nsGameBoard.SctGameBoard>();
        }

        public SctMovingShape HoldShape(SctShapeProperties shapePropertiesToStore)
        {
            SctMovingShape currentlyHeldShape = null;
            SctShapeProperties currentlyHeldShapeProperties = m_sctShapeProperties;
            if (currentlyHeldShapeProperties != null)
            {
                currentlyHeldShapeProperties.transform.parent = m_parent;
                currentlyHeldShapeProperties.transform.position = shapePropertiesToStore.transform.position;
                currentlyHeldShapeProperties.transform.localRotation = m_localRotation;
                currentlyHeldShapeProperties.transform.localScale = m_localScale;
                SctShapeProperties ghostShapeProperties = Instantiate(currentlyHeldShapeProperties);
                ghostShapeProperties.transform.parent = m_parent;
                ghostShapeProperties.transform.position = shapePropertiesToStore.transform.position;
                ghostShapeProperties.transform.localRotation = m_localRotation;
                ghostShapeProperties.transform.localScale = m_localScale;
                SctGhostShape newGhostShape = ghostShapeProperties.gameObject.AddComponent<SctGhostShape>();
                currentlyHeldShape = currentlyHeldShapeProperties.gameObject.AddComponent<SctMovingShape>();
                while (m_sctGameBoard.IsPositionValid(currentlyHeldShape) == false)
                {
                    currentlyHeldShapeProperties.transform.Translate(Vector3.up, Space.World);
                }
                currentlyHeldShape.GhostColor = m_ghostColor;
                currentlyHeldShape.SctGhostShape = newGhostShape;
            }

            m_sctShapeProperties = shapePropertiesToStore;
            m_parent = shapePropertiesToStore.transform.parent;
            m_localRotation = shapePropertiesToStore.transform.localRotation;
            m_localScale = shapePropertiesToStore.transform.localScale;

            m_sctShapeProperties.transform.parent = m_shapeHoldSlot;
            m_sctShapeProperties.transform.rotation = Quaternion.identity;
            m_sctShapeProperties.transform.localPosition = Vector3.zero;

            Vector3 centerOffset = Vector3.zero - m_sctShapeProperties.CenterOffset * m_shapeScaleFactor;
            Vector3 rotatedCenterOffset = Quaternion.AngleAxis(m_localRotation.eulerAngles.z, Vector3.forward) * centerOffset;

            m_sctShapeProperties.transform.rotation = m_localRotation;
            m_sctShapeProperties.transform.localPosition = rotatedCenterOffset;
            m_sctShapeProperties.transform.localScale = new Vector3(m_shapeScaleFactor, m_shapeScaleFactor, 1);

            return currentlyHeldShape;
        }
    }
}
