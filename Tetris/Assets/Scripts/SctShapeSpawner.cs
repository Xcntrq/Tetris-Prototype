using UnityEngine;
using nsGhostShape;
using nsMovingShape;
using nsShapeProperties;

namespace nsShapeSpawner
{
    public class SctShapeSpawner : MonoBehaviour
    {
        [SerializeField] private SctShapeProperties[] m_allShapes;
        [SerializeField] private Color m_ghostColor;
        [SerializeField] private Transform[] m_shapeQueueSlots;
        [SerializeField] private Vector3 m_shapeQueueScaler;
        private SctShapeProperties[] m_shapeQueue = new SctShapeProperties[3];

        private void Start()
        {
            bool isQueueFull = true;
            for (int i = 0; i < m_shapeQueue.Length; i++)
            {
                if (m_shapeQueue[i] == null) isQueueFull = false;
            }
            if (isQueueFull == false) InitializeQueue();
        }

        private SctShapeProperties GetRandomShape()
        {
            int i = Random.Range(0, m_allShapes.Length);
            if (m_allShapes[i] == null) Debug.Log("ERROR! Null shape at index " + i + "!");
            return m_allShapes[i];
        }

        public SctMovingShape GetShape()
        {
            bool isQueueFull = true;
            for (int i = 0; i < m_shapeQueue.Length; i++)
            {
                if (m_shapeQueue[i] == null) isQueueFull = false;
            }
            if (isQueueFull == false) InitializeQueue();

            SctShapeProperties movingShapeProperties = m_shapeQueue[0];
            for (int i = 0; i < m_shapeQueue.Length - 1; i++)
            {
                m_shapeQueue[i] = m_shapeQueue[i + 1];
                m_shapeQueue[i].transform.parent = m_shapeQueueSlots[i].transform;
                m_shapeQueue[i].transform.localPosition = Vector3.zero;
            }
            InitializeQueueSlot(2);

            movingShapeProperties.transform.parent = transform;
            movingShapeProperties.transform.localPosition = Vector3.zero;
            movingShapeProperties.transform.localScale = Vector3.one;

            SctShapeProperties ghostShapeProperties = Instantiate(movingShapeProperties);
            SctGhostShape newGhostShape = ghostShapeProperties.gameObject.AddComponent<SctGhostShape>();
            SctMovingShape newMovingShape = movingShapeProperties.gameObject.AddComponent<SctMovingShape>();
            newMovingShape.GhostColor = m_ghostColor;
            newMovingShape.SctGhostShape = newGhostShape;
            return newMovingShape;
        }

        private void InitializeQueue()
        {
            for (int i = 0; i < m_shapeQueue.Length; i++)
            {
                if (m_shapeQueue[i] == null)
                {
                    InitializeQueueSlot(i);
                }
            }
        }

        private void InitializeQueueSlot(int i)
        {
            m_shapeQueue[i] = Instantiate(GetRandomShape(), m_shapeQueueSlots[i].transform);
            m_shapeQueue[i].transform.localPosition = Vector3.zero;
            m_shapeQueue[i].transform.rotation = Quaternion.identity;
            m_shapeQueue[i].transform.localScale = m_shapeQueueScaler;
        }
    }
}
