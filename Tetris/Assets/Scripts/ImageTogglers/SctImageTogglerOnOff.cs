using UnityEngine;
using UnityEngine.UI;

namespace nsImageTogglerOnOff
{
    [RequireComponent(typeof(Image))]
    public class SctImageTogglerOnOff : MonoBehaviour
    {
        [SerializeField] private Sprite m_imageWhenOn;
        [SerializeField] private Sprite m_imageWhenOff;
        [SerializeField] private Color m_colorWhenOn;
        [SerializeField] private Color m_colorWhenOff;

        Image m_image = null;

        public void SetImage(bool isOn)
        {
            if (m_image == null) m_image = GetComponent<Image>();
            if (!m_image || !m_imageWhenOn || !m_imageWhenOff) return;
            m_image.sprite = isOn ? m_imageWhenOn : m_imageWhenOff;
            m_image.color = isOn ? m_colorWhenOn : m_colorWhenOff;
        }
    }
}
