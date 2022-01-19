using UnityEngine;
using nsInputManager;
using nsImageTogglerOnOff;
using System.Collections.Generic;

namespace nsSoundManager
{
    public class SctSoundManager : MonoBehaviour
    {
        [System.Serializable]
        public class AudioClipAndVolume : Object
        {
            public AudioClip AudioClip { get; set; }
            public float Volume { get; set; }
        }

        [SerializeField] private float m_voicesVolume;
        [SerializeField] private AudioClip m_voiceGameOver;
        [SerializeField] private AudioClip m_voiceLevelUp;
        [SerializeField] private List<AudioClip> m_voiceClips;

        [Space]
        [Space]
        [Space]
        [Space]

        [SerializeField] private AudioSource m_musicSource;

        public bool IsMusicEnabled { get; set; }
        public SctImageTogglerOnOff ImageTogglerMusic { get; set; }
        public AudioClip MusicClip { get; set; }
        public float MusicVolume { get; set; }
        public float MusicVolumeWhenPaused { get; set; }

        public bool IsSoundEnabled { get; set; }
        public SctImageTogglerOnOff ImageTogglerSound { get; set; }
        public float SoundVolume { get; set; }

        public AudioClipAndVolume AudioGameOver { get; set; }
        public AudioClipAndVolume AudioLevelUp { get; set; }
        public AudioClipAndVolume AudioRowClear { get; set; }
        public AudioClipAndVolume AudioShapeDrop { get; set; }
        public AudioClipAndVolume AudioShapeHold { get; set; }
        public AudioClipAndVolume AudioShapeMoveError { get; set; }
        public AudioClipAndVolume AudioShapeMoveSuccess { get; set; }

        private Vector3 m_cameraPosition;

        private void Awake()
        {
            m_cameraPosition = Camera.main.transform.position;
            GameManager sctGameManager = FindObjectOfType<GameManager>();
            SctInputManager sctInputManager = FindObjectOfType<SctInputManager>();

            sctGameManager.OnGameOver += GameManager_OnGameOver;
            sctGameManager.OnShapeDrop += GameManager_OnShapeDrop;
            sctGameManager.OnShapeHold += () => { PlayClip(AudioShapeHold.AudioClip, AudioShapeHold.Volume); };
            sctInputManager.OnShapeMoveError += () => { PlayClip(AudioShapeMoveError.AudioClip, AudioShapeMoveError.Volume); };
            sctInputManager.OnShapeMoveSuccess += () => { PlayClip(AudioShapeMoveSuccess.AudioClip, AudioShapeMoveSuccess.Volume); };
            sctGameManager.OnPauseToggled += GameManager_OnPauseToggled;
        }

        private void Start()
        {
            ImageTogglerSound.SetImage(IsSoundEnabled);
            ImageTogglerMusic.SetImage(IsMusicEnabled);
            m_musicSource.volume = MusicVolume;
            m_musicSource.loop = true;
            CheckIfMusicShouldPlay();
        }

        private void PlayMusic()
        {
            if (!MusicClip || !m_musicSource) return;
            m_musicSource.Stop();
            m_musicSource.clip = MusicClip;
            m_musicSource.Play();
        }

        private void CheckIfMusicShouldPlay()
        {
            if (m_musicSource.isPlaying == IsMusicEnabled) return;
            if (!IsMusicEnabled) m_musicSource.Stop();
            if (IsMusicEnabled) PlayMusic();
        }

        public void ToggleMusic()
        {
            IsMusicEnabled = !IsMusicEnabled;
            CheckIfMusicShouldPlay();
            ImageTogglerMusic.SetImage(IsMusicEnabled);
        }

        public void ToggleSound()
        {
            IsSoundEnabled = !IsSoundEnabled;
            ImageTogglerSound.SetImage(IsSoundEnabled);
        }

        private void PlayClip(AudioClip clip, float volume)
        {
            if (!IsSoundEnabled || (clip == null)) return;
            AudioSource.PlayClipAtPoint(clip, m_cameraPosition, Mathf.Clamp(SoundVolume * volume, 0.05f, 1f));
        }

        private void GameManager_OnGameOver()
        {
            if (IsMusicEnabled) ToggleMusic();
            PlayClip(AudioGameOver.AudioClip, AudioGameOver.Volume);
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
                    PlayClip(AudioShapeDrop.AudioClip, AudioShapeDrop.Volume);
                }
                else if (rowsCleared == 1)
                {
                    PlayClip(AudioRowClear.AudioClip, AudioRowClear.Volume);
                }
                else if (rowsCleared > 1)
                {
                    int i = Random.Range(0, m_voiceClips.Count);
                    PlayClip(m_voiceClips[i], m_voicesVolume);
                }
            }
        }

        private void GameManager_OnPauseToggled(bool isGamePaused)
        {
            m_musicSource.volume = isGamePaused ? MusicVolumeWhenPaused : MusicVolume;
        }
    }
}
