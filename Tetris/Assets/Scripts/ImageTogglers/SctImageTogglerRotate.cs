using UnityEngine;
using UnityEngine.UI;

namespace nsImageTogglerRotate
{
    [RequireComponent(typeof(Image))]
    public class SctImageTogglerRotate : MonoBehaviour
    {
        [SerializeField] private Sprite m_imageWhenCW;
        [SerializeField] private Sprite m_imageWhenCCW;

        Image m_image = null;

        public void SetImage(RotationDirection rotationDirection)
        {
            if (m_image == null) m_image = GetComponent<Image>();
            if (!m_image || !m_imageWhenCW || !m_imageWhenCCW) return;
            if (rotationDirection == RotationDirection.CW) m_image.sprite = m_imageWhenCW;
            if (rotationDirection == RotationDirection.CCW) m_image.sprite = m_imageWhenCCW;
        }
    }
}
