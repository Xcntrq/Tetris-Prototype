using UnityEngine;

namespace nsSoundManager
{
    public class SctSoundManager : MonoBehaviour
    {
        [SerializeField] private bool m_isMusicEnabled;
        [SerializeField] private float m_musicVolume;
        [SerializeField] private AudioClip m_musicClip;
        [SerializeField] private AudioSource m_musicSource;

        private void Start()
        {
            PlayMusic();
        }

        public void PlayMusic()
        {
            if (!m_isMusicEnabled || !m_musicClip || !m_musicSource) return;
            m_musicSource.Stop();
            m_musicSource.clip = m_musicClip;
            m_musicSource.volume = m_musicVolume;
            m_musicSource.loop = true;
            m_musicSource.Play();
        }

        public void CheckIsMusicEnabled()
        {
            if (m_musicSource.isPlaying == m_isMusicEnabled) return;
            if (!m_isMusicEnabled) m_musicSource.Stop();
            if (m_isMusicEnabled) PlayMusic();
        }

        public void ToggleMusic()
        {
            m_isMusicEnabled = !m_isMusicEnabled;
            CheckIsMusicEnabled();
        }
    }
}
