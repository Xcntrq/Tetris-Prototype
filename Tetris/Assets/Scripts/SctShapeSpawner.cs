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

        private SctShapeProperties GetRandomShape()
        {
            int i = Random.Range(0, m_allShapes.Length);
            if (m_allShapes[i] == null) Debug.Log("ERROR! Null shape at index " + i + "!");
            return m_allShapes[i];
        }

        public SctMovingShape SpawnRandomShape()
        {
            SctShapeProperties randomShapeProperties = GetRandomShape();
            if (randomShapeProperties == null) Debug.Log("ERROR! Null shape in SpawnRandomShape()!");
            SctShapeProperties ghostShapeProperties = Instantiate(randomShapeProperties, transform.position, Quaternion.identity, transform);
            SctShapeProperties movingShapeProperties = Instantiate(randomShapeProperties, transform.position, Quaternion.identity, transform);
            SctGhostShape newGhostShape = ghostShapeProperties.gameObject.AddComponent<SctGhostShape>();
            SctMovingShape newMovingShape = movingShapeProperties.gameObject.AddComponent<SctMovingShape>();
            newMovingShape.GhostColor = m_ghostColor;
            newMovingShape.SctGhostShape = newGhostShape;
            return newMovingShape;
        }
    }
}
