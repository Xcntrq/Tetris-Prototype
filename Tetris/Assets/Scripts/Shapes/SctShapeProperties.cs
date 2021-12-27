using UnityEngine;

namespace nsShapeProperties
{
    public class SctShapeProperties : MonoBehaviour
    {
        [SerializeField] private bool m_isRotatable;
        [SerializeField] private Vector3 m_centerOffset;

        public bool IsRotatable
        {
            get
            {
                return m_isRotatable;
            }
        }

        public Vector3 CenterOffset
        {
            get
            {
                return m_centerOffset;
            }
        }
    }
}
