using UnityEngine;
using UnityEngine.UI;

namespace nsImageTogglerOnOff
{
    [RequireComponent(typeof(Image))]
    public class SctImageTogglerOnOff : MonoBehaviour
    {
        [SerializeField] private Sprite m_imageWhenOn;
        [SerializeField] private Sprite m_imageWhenOff;

        Image m_image;

        private void Awake()
        {
            m_image = GetComponent<Image>();
        }

        public void SetImage(bool isOn)
        {
            if (!m_image || !m_imageWhenOn || !m_imageWhenOff) return;
            m_image.sprite = isOn ? m_imageWhenOn : m_imageWhenOff;
        }
    }
}
