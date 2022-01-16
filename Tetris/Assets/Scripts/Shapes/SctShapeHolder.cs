using UnityEngine;
using nsMovingShape;

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
                CorrectPosition(shapeToReturn);
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

        private void CorrectPosition(SctMovingShape sctMovingShape)
        {
            int gridWidth;
            int gridHeight;
            (gridWidth, gridHeight) = m_sctGameBoard.GridSize;
            int GridMinX = 0;
            int GridMaxX = gridWidth - 1;

            int ChildSquaresMinX = int.MaxValue;
            int ChildSquaresMaxX = int.MinValue;
            foreach (Transform childSquare in sctMovingShape.transform)
            {
                Vector3Int childPosition = nsVectorf.Vectorf.RoundToInt(childSquare.position);
                int ChildSquareX = childPosition.x;
                if (ChildSquareX < ChildSquaresMinX) ChildSquaresMinX = ChildSquareX;
                if (ChildSquareX > ChildSquaresMaxX) ChildSquaresMaxX = ChildSquareX;
            }

            if ((ChildSquaresMinX < GridMinX) || (ChildSquaresMaxX > GridMaxX))
            {
                int xDelta = 0;
                Vector3Int shapePosition = nsVectorf.Vectorf.RoundToInt(sctMovingShape.transform.position);
                if (ChildSquaresMinX < GridMinX) xDelta = GridMinX - ChildSquaresMinX;
                if (ChildSquaresMaxX > GridMaxX) xDelta = GridMaxX - ChildSquaresMaxX;
                int x = shapePosition.x + xDelta;
                shapePosition.x = x;
                sctMovingShape.transform.position = shapePosition;
            }

            while ((m_sctGameBoard.IsPositionValid(sctMovingShape) == false) && (sctMovingShape.transform.position.y < gridHeight))
            {
                sctMovingShape.MoveUp();
            }
        }
    }
}
