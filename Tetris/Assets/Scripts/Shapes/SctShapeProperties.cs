using UnityEngine;

namespace nsShapeProperties
{
    public class SctShapeProperties : MonoBehaviour
    {
        [SerializeField] private bool m_isRotatable;

        public bool IsRotatable
        {
            get
            {
                return m_isRotatable;
            }
        }
    }
}
