using UnityEngine;
using nsShape;
using nsMovingShape;
using nsShapeProperties;

namespace nsShapeSpawner
{
    public class SctShapeSpawner : MonoBehaviour
    {
        [SerializeField] private SctShapeProperties[] m_allShapes;
        [SerializeField] private Color m_ghostColor;
        [SerializeField] private Transform[] m_shapeQueueSlots;
        [SerializeField] private float m_shapeScaleFactor;

        private Vector3 m_shapeScaleVector;
        private readonly SctShapeProperties[] m_shapeQueue = new SctShapeProperties[3];

        private void Start()
        {
            if (IsShapeQueueFull() == false) FillShapeQueue();
        }

        private SctShapeProperties GetRandomShape()
        {
            int i = Random.Range(0, m_allShapes.Length);
            if (m_allShapes[i] == null) Debug.Log($"ERROR! Null shape at index {i}!");
            return m_allShapes[i];
        }

        public SctMovingShape GetNextShape()
        {
            if (IsShapeQueueFull() == false) FillShapeQueue();
            SctShapeProperties movingShapeProperties = Dequeue();
            movingShapeProperties.transform.parent = transform;
            movingShapeProperties.transform.localPosition = Vector3.zero;
            movingShapeProperties.transform.localScale = Vector3.one;
            SctShapeProperties ghostShapeProperties = Instantiate(movingShapeProperties);
            ghostShapeProperties.transform.parent = transform;
            ghostShapeProperties.transform.localPosition = Vector3.zero;
            ghostShapeProperties.transform.localScale = Vector3.one;
            SctShape newGhostShape = ghostShapeProperties.gameObject.AddComponent<SctShape>();
            SctMovingShape newMovingShape = movingShapeProperties.gameObject.AddComponent<SctMovingShape>();
            newMovingShape.GhostColor = m_ghostColor;
            newMovingShape.SctShapeGhost = newGhostShape;
            return newMovingShape;
        }

        private bool IsShapeQueueFull()
        {
            bool isShapeQueueFull = true;
            for (int i = 0; i < m_shapeQueue.Length; i++)
            {
                if (m_shapeQueue[i] == null) isShapeQueueFull = false;
            }
            return isShapeQueueFull;
        }

        private void FillShapeQueue()
        {
            m_shapeScaleVector = new Vector3(m_shapeScaleFactor, m_shapeScaleFactor, 1);
            for (int i = 0; i < m_shapeQueue.Length; i++)
            {
                if (m_shapeQueue[i] == null) FillShapeQueueSlot(i);
            }
        }

        private void FillShapeQueueSlot(int i)
        {
            m_shapeQueue[i] = Instantiate(GetRandomShape());
            m_shapeQueue[i].transform.parent = m_shapeQueueSlots[i].transform;
            m_shapeQueue[i].transform.localPosition = Vector3.zero - m_shapeQueue[i].CenterOffset * m_shapeScaleFactor;
            m_shapeQueue[i].transform.rotation = Quaternion.identity;
            m_shapeQueue[i].transform.localScale = m_shapeScaleVector;
        }

        private SctShapeProperties Dequeue()
        {
            SctShapeProperties dequeue = m_shapeQueue[0];
            for (int i = 0; i < m_shapeQueue.Length - 1; i++)
            {
                m_shapeQueue[i] = m_shapeQueue[i + 1];
                m_shapeQueue[i].transform.parent = m_shapeQueueSlots[i].transform;
                m_shapeQueue[i].transform.localPosition = Vector3.zero - m_shapeQueue[i].CenterOffset * m_shapeScaleFactor;
            }
            FillShapeQueueSlot(m_shapeQueue.Length - 1);
            return dequeue;
        }
    }
}
