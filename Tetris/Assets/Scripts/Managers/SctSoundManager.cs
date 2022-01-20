using UnityEngine;
using nsInputManager;
using nsImageTogglerOnOff;
using System.Collections.Generic;

namespace nsSoundManager
{
    public class SctSoundManager : MonoBehaviour
    {
        [SerializeField] private float m_voicesVolume;
        [SerializeField] private AudioClip m_voiceGameOver;
        [SerializeField] private AudioClip m_voiceLevelUp;
        [SerializeField] private List<AudioClip> m_voiceBonus;

        [Space]
        [Space]
        [Space]
        [Space]

        [SerializeField] private AudioSource m_musicSource;

        [HideInInspector] public bool m_isMusicEnabled;
        [HideInInspector] public SctImageTogglerOnOff m_imageTogglerMusic;
        [HideInInspector] public AudioClip m_musicClip;
        [HideInInspector] public float m_musicVolume;
        [HideInInspector] public float m_musicVolumeWhenPaused;

        [HideInInspector] public bool m_isSoundEnabled;
        [HideInInspector] public SctImageTogglerOnOff m_imageTogglerSound;
        [HideInInspector] public float m_soundVolume;

        [HideInInspector] public AudioClip m_audioGameOverClip;
        [HideInInspector] public float m_audioGameOverVolume;
        [HideInInspector] public AudioClip m_audioLevelUpClip;
        [HideInInspector] public float m_audioLevelUpVolume;
        [HideInInspector] public AudioClip m_audioRowClearClip;
        [HideInInspector] public float m_audioRowClearVolume;
        [HideInInspector] public AudioClip m_audioShapeDropClip;
        [HideInInspector] public float m_audioShapeDropVolume;
        [HideInInspector] public AudioClip m_audioShapeHoldClip;
        [HideInInspector] public float m_audioShapeHoldVolume;
        [HideInInspector] public AudioClip m_audioShapeMoveErrorClip;
        [HideInInspector] public float m_audioShapeMoveErrorVolume;
        [HideInInspector] public AudioClip m_audioShapeMoveSuccessClip;
        [HideInInspector] public float m_audioShapeMoveSuccessVolume;

        private Vector3 m_cameraPosition;

        private void Awake()
        {
            m_cameraPosition = Camera.main.transform.position;
            GameManager sctGameManager = FindObjectOfType<GameManager>();
            SctInputManager sctInputManager = FindObjectOfType<SctInputManager>();

            sctGameManager.OnGameOver += GameManager_OnGameOver;
            sctGameManager.OnShapeDrop += GameManager_OnShapeDrop;
            sctGameManager.OnShapeHold += () => { PlayClip(m_audioShapeHoldClip, m_audioShapeHoldVolume); };
            sctInputManager.OnShapeMoveError += () => { PlayClip(m_audioShapeMoveErrorClip, m_audioShapeMoveErrorVolume); };
            sctInputManager.OnShapeMoveSuccess += () => { PlayClip(m_audioShapeMoveSuccessClip, m_audioShapeMoveSuccessVolume); };
            sctGameManager.OnPauseToggled += GameManager_OnPauseToggled;
        }

        private void Start()
        {
            if (m_imageTogglerSound != null) m_imageTogglerSound.SetImage(m_isSoundEnabled);
            if (m_imageTogglerMusic != null) m_imageTogglerMusic.SetImage(m_isMusicEnabled);
            m_musicSource.volume = m_musicVolume;
            m_musicSource.loop = true;
            CheckIfMusicShouldPlay();
        }

        private void PlayMusic()
        {
            if (!m_musicClip || !m_musicSource) return;
            m_musicSource.Stop();
            m_musicSource.clip = m_musicClip;
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
            AudioSource.PlayClipAtPoint(clip, m_cameraPosition, Mathf.Clamp(m_soundVolume * volume, 0.05f, 1f));
        }

        private void GameManager_OnGameOver()
        {
            if (m_isMusicEnabled) ToggleMusic();
            PlayClip(m_audioGameOverClip, m_audioGameOverVolume);
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
                    PlayClip(m_audioShapeDropClip, m_audioShapeDropVolume);
                }
                else if (rowsCleared == 1)
                {
                    PlayClip(m_audioRowClearClip, m_audioRowClearVolume);
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
            m_musicSource.volume = isGamePaused ? m_musicVolumeWhenPaused : m_musicVolume;
        }
    }
}
