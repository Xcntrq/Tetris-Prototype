using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace nsImageTogglerOnOff
{
    [RequireComponent(typeof(Image))]
    public class SctImageTogglerOnOff : MonoBehaviour
    {
        public UnityEvent<bool> OnStart;

        [SerializeField] private Sprite m_imageWhenOn;
        [SerializeField] private Sprite m_imageWhenOff;
        [SerializeField] private bool m_isOn;

        Image m_image;

        private void Awake()
        {
            m_image = GetComponent<Image>();
        }

        private void Start()
        {
            OnStart?.Invoke(m_isOn);
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (!m_image || !m_imageWhenOn || !m_imageWhenOff) return;
            m_image.sprite = m_isOn ? m_imageWhenOn : m_imageWhenOff;
        }

        public void ToggleImage()
        {
            m_isOn = !m_isOn;
            UpdateImage();
        }
    }
}
