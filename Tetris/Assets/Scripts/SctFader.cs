using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace nsFader
{
    [RequireComponent(typeof(MaskableGraphic))]
    public class SctFader : MonoBehaviour
    {
        [SerializeField] private float m_startAlpha;
        [SerializeField] private float m_targetAlpha;
        [SerializeField] private float m_targetAlphaThreshold;
        [SerializeField] private float m_startDelay;
        [SerializeField] private float m_timeToFade;

        private MaskableGraphic m_maskableGraphic;
        private Color m_currentColor;
        private float m_currentAlpha;
        private float m_r;
        private float m_g;
        private float m_b;

        private void Awake()
        {
            m_maskableGraphic = GetComponent<MaskableGraphic>();
        }

        private void Start()
        {
            Color m_originalColor = m_maskableGraphic.color;
            m_r = m_originalColor.r;
            m_g = m_originalColor.g;
            m_b = m_originalColor.b;
            m_currentAlpha = m_startAlpha;
            m_currentColor = new Color(m_r, m_g, m_b, m_currentAlpha);
            m_maskableGraphic.color = m_currentColor;
            StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            yield return new WaitForSeconds(m_startDelay);
            while (Mathf.Abs(m_targetAlpha - m_currentAlpha) > m_targetAlphaThreshold)
            {
                yield return new WaitForEndOfFrame();
                m_currentAlpha += (m_targetAlpha - m_startAlpha) / m_timeToFade * Time.deltaTime;
                m_currentColor = new Color(m_r, m_g, m_b, m_currentAlpha);
                m_maskableGraphic.color = m_currentColor;
            }
        }
    }
}
