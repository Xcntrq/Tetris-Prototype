using UnityEngine;

namespace nsShapeProperties
{
    public class SctShapeProperties : MonoBehaviour
    {
        [SerializeField] private bool m_isRotatable;
        [SerializeField] private Vector3 m_centerOffset;

        public bool IsRotatable { get => m_isRotatable; }
        public Vector3 CenterOffset { get => m_centerOffset; }
    }
}
