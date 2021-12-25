using UnityEngine;

namespace nsSoundManager
{
    public class SctSoundManager : MonoBehaviour
    {
        [SerializeField] private bool m_isSoundEnabled;
        [SerializeField] private nsImageTogglerOnOff.SctImageTogglerOnOff m_imageTogglerSound;
        [SerializeField] private bool m_isMusicEnabled;
        [SerializeField] private nsImageTogglerOnOff.SctImageTogglerOnOff m_imageTogglerMusic;

        [Space]

        [SerializeField] private float m_SoundVolume;
        [SerializeField] private float m_musicVolume;
        [SerializeField] private float m_musicVolumeMultiplierWhenPaused;
        [SerializeField] private AudioClip m_musicClip;
        [SerializeField] private AudioClip m_gameOverClip;
        [SerializeField] private AudioClip m_rowClearClip;
        [SerializeField] private AudioClip m_shapeDropClip;
        [SerializeField] private AudioClip m_shapeMoveErrorClip;
        [SerializeField] private AudioClip m_shapeMoveSuccessClip;
        [SerializeField] private AudioClip m_gameOverVoiceClip;
        [SerializeField] private AudioClip m_LevelUpVoiceClip;
        [SerializeField] private AudioClip[] m_voiceClips;
        [SerializeField] private AudioSource m_musicSource;

        private float m_musicVolumeMultiplier;
        private Vector3 m_cameraPosition;

        private void Awake()
        {
            m_cameraPosition = Camera.main.transform.position;
            GameManager sctGameManager = FindObjectOfType<GameManager>();
            sctGameManager.OnGameOver += GameManager_OnGameOver;
            sctGameManager.OnRowClear += GameManager_OnRowClear;
            sctGameManager.OnShapeDrop += () => { PlayClip(m_shapeDropClip, 1); };
            sctGameManager.OnShapeMoveError += () => { PlayClip(m_shapeMoveErrorClip, 1); };
            sctGameManager.OnShapeMoveSuccess += () => { PlayClip(m_shapeMoveSuccessClip, 1); };
            sctGameManager.OnPauseToggled += GameManager_OnPauseToggled;
            m_musicVolumeMultiplier = 1;
        }

        private void Start()
        {
            m_imageTogglerSound.SetImage(m_isSoundEnabled);
            m_imageTogglerMusic.SetImage(m_isMusicEnabled);
        }

        private void PlayMusic()
        {
            if (!m_musicClip || !m_musicSource) return;
            m_musicSource.Stop();
            m_musicSource.clip = m_musicClip;
            m_musicSource.volume = m_musicVolume * m_musicVolumeMultiplier;
            m_musicSource.loop = true;
            m_musicSource.Play();
        }

        private void CheckIfMusicShouldPlay()
        {
            if (m_musicSource.isPlaying == m_isMusicEnabled) return;
            if (!m_isMusicEnabled) m_musicSource.Stop();
            if (m_isMusicEnabled) PlayMusic();
        }

        public void ToggleMusic()
        {
            m_isMusicEnabled = !m_isMusicEnabled;
            CheckIfMusicShouldPlay();
            m_imageTogglerMusic.SetImage(m_isMusicEnabled);
        }

        public void ToggleSound()
        {
            m_isSoundEnabled = !m_isSoundEnabled;
            m_imageTogglerSound.SetImage(m_isSoundEnabled);
        }

        private void PlayClip(AudioClip clip, float volume)
        {
            if (!m_isSoundEnabled || (clip == null)) return;
            AudioSource.PlayClipAtPoint(clip, m_cameraPosition, Mathf.Clamp(m_SoundVolume * volume, 0.05f, 1f));
        }

        private void GameManager_OnGameOver()
        {
            if (m_isMusicEnabled) ToggleMusic();
            PlayClip(m_gameOverClip, 1);
            PlayClip(m_gameOverVoiceClip, 1);
        }

        private void GameManager_OnRowClear(int rowsCleared, bool hasLeveledUp)
        {
            if (hasLeveledUp)
            {
                PlayClip(m_LevelUpVoiceClip, 1);
            }
            else if (rowsCleared > 1)
            {
                int i = Random.Range(0, m_voiceClips.Length);
                PlayClip(m_voiceClips[i], 1);
            }
            PlayClip(m_rowClearClip, 1);
        }

        private void GameManager_OnPauseToggled(bool isGamePaused)
        {
            m_musicVolumeMultiplier = isGamePaused ? m_musicVolumeMultiplierWhenPaused : 1;
            m_musicSource.volume = m_musicVolume * m_musicVolumeMultiplier;
        }
    }
}
