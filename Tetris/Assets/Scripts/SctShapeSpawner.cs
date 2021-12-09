using UnityEngine;
using nsShape;

namespace nsShapeSpawner
{
    public class SctShapeSpawner : MonoBehaviour
    {
        [SerializeField] private SctShape[] m_allShapes;

        private SctShape GetRandomShape()
        {
            int i = Random.Range(0, m_allShapes.Length);
            if (m_allShapes[i] == null) Debug.Log("ERROR! Null shape at index " + i + "!");
            return m_allShapes[i];
        }

        public SctShape SpawnRandomShape()
        {
            SctShape newShape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity, transform);
            if (newShape == null) Debug.Log("ERROR! Null shape in SpawnRandomShape()!");
            return newShape;
        }
    }
}
