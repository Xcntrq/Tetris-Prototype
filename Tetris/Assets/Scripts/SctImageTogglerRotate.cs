using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace nsImageTogglerRotate
{
    [RequireComponent(typeof(Image))]
    public class SctImageTogglerRotate : MonoBehaviour
    {
        public UnityEvent<RotationDirection> OnStart;

        [SerializeField] private Sprite m_imageWhenCW;
        [SerializeField] private Sprite m_imageWhenCCW;
        [SerializeField] private RotationDirection m_rotationDirection;

        Image m_image;

        private void Awake()
        {
            m_image = GetComponent<Image>();
        }

        private void Start()
        {
            OnStart?.Invoke(m_rotationDirection);
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (!m_image || !m_imageWhenCW || !m_imageWhenCCW) return;
            if (m_rotationDirection == RotationDirection.CW) m_image.sprite = m_imageWhenCW;
            if (m_rotationDirection == RotationDirection.CCW) m_image.sprite = m_imageWhenCCW;
        }

        public void ToggleImage()
        {
            m_rotationDirection = (m_rotationDirection == RotationDirection.CW) ? RotationDirection.CCW : RotationDirection.CW;
            UpdateImage();
        }
    }
}
