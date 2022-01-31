using UnityEngine;
using nsInputManager;
using nsImageTogglerOnOff;
using System.Collections.Generic;

namespace nsSoundManager
{
    [System.Serializable]
    public class AudioClipAndVolume
    {
        public AudioClip m_audioClip;
        public float m_volume;
    }

    public class SctSoundManager : MonoBehaviour
    {
        [SerializeField] private float m_voicesVolume;
        [SerializeField] private AudioClip m_voiceGameOver;
        [SerializeField] private AudioClip m_voiceLevelUp;
        [SerializeField] private List<AudioClip> m_voiceBonus;
        [SerializeField] private AudioSource m_soundSource;

        [Space]
        [Space]
        [Space]
        [Space]

        [SerializeField] private AudioSource m_musicSource;

        [HideInInspector] public bool m_isMusicEnabled;
        [HideInInspector] public SctImageTogglerOnOff m_imageTogglerMusic;
        [HideInInspector] public AudioClipAndVolume m_music;
        [HideInInspector] public float m_musicVolumeWhenPaused;

        [HideInInspector] public bool m_isSoundEnabled;
        [HideInInspector] public SctImageTogglerOnOff m_imageTogglerSound;
        [HideInInspector] public float m_soundVolume;

        [HideInInspector] public AudioClipAndVolume m_audioGameOver;
        [HideInInspector] public AudioClipAndVolume m_audioLevelUp;
        [HideInInspector] public AudioClipAndVolume m_audioRowClear;
        [HideInInspector] public AudioClipAndVolume m_audioShapeDrop;
        [HideInInspector] public AudioClipAndVolume m_audioShapeHold;
        [HideInInspector] public AudioClipAndVolume m_audioShapeMoveError;
        [HideInInspector] public AudioClipAndVolume m_audioShapeMoveSuccess;

        private Vector3 m_cameraPosition;

        private void Awake()
        {
            m_cameraPosition = Camera.main.transform.position;
            GameManager sctGameManager = FindObjectOfType<GameManager>();
            SctInputManager sctInputManager = FindObjectOfType<SctInputManager>();

            sctGameManager.OnGameOver += GameManager_OnGameOver;
            sctGameManager.OnShapeDrop += GameManager_OnShapeDrop;
            sctGameManager.OnShapeHold += () => { PlayClip(m_audioShapeHold.m_audioClip, m_audioShapeHold.m_volume); };
            sctInputManager.OnShapeMoveError += () => { PlayClip(m_audioShapeMoveError.m_audioClip, m_audioShapeMoveError.m_volume); };
            sctInputManager.OnShapeMoveSuccess += () => { PlayClip(m_audioShapeMoveSuccess.m_audioClip, m_audioShapeMoveSuccess.m_volume); };
            sctGameManager.OnPauseToggled += GameManager_OnPauseToggled;
        }

        private void Start()
        {
            if (m_imageTogglerSound != null) m_imageTogglerSound.SetImage(m_isSoundEnabled);
            if (m_imageTogglerMusic != null) m_imageTogglerMusic.SetImage(m_isMusicEnabled);
            m_soundSource.transform.position = m_cameraPosition;
            m_soundSource.volume = m_soundVolume;
            m_soundSource.loop = false;
            m_musicSource.transform.position = m_cameraPosition;
            m_musicSource.volume = m_music.m_volume;
            m_musicSource.loop = true;
            CheckIfMusicShouldPlay();
        }

        private void PlayMusic()
        {
            if (!m_music.m_audioClip || !m_musicSource) return;
            m_musicSource.Stop();
            m_musicSource.clip = m_music.m_audioClip;
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
            float volumeClamped = Mathf.Clamp(m_soundVolume * volume, 0.05f, 1f);
            AudioSource.PlayClipAtPoint(clip, m_cameraPosition, volumeClamped);
        }

        private void GameManager_OnGameOver()
        {
            if (m_isMusicEnabled) ToggleMusic();
            PlayClip(m_audioGameOver.m_audioClip, m_audioGameOver.m_volume);
            PlayClip(m_voiceGameOver, m_voicesVolume);
        }

        private void GameManager_OnShapeDrop(int rowsCleared, bool hasLeveledUp)
        {
            if (hasLeveledUp)
            {
                PlayClip(m_voiceLevelUp, m_voicesVolume);
            }
            else
            {
                if (rowsCleared == 0)
                {
                    PlayClip(m_audioShapeDrop.m_audioClip, m_audioShapeDrop.m_volume);
                }
                else if (rowsCleared == 1)
                {
                    PlayClip(m_audioRowClear.m_audioClip, m_audioRowClear.m_volume);
                }
                else if (rowsCleared > 1)
                {
                    int i = Random.Range(0, m_voiceBonus.Count);
                    PlayClip(m_voiceBonus[i], m_voicesVolume);
                }
            }
        }

        private void GameManager_OnPauseToggled(bool isGamePaused)
        {
            m_musicSource.volume = isGamePaused ? m_musicVolumeWhenPaused : m_music.m_volume;
        }
    }
}
