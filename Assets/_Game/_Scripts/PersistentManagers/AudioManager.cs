using Unity.VisualScripting;
using UnityEngine;

namespace BenStudios
{

    public class AudioManager : MonoBehaviour
    {
        #region Variables

        [SerializeField] private AudioAsset m_audioAsset;
        [SerializeField] private AudioSource m_bgmAudioSource;
        [SerializeField] private AudioSource m_sfxAudioSource;

        #endregion Variables

        #region Unity Methods
        private void Awake()
        {
            _Init();
        }
        private void OnEnable()
        {
            GlobalEventHandler.RequestToPlayBGM += _PlayBGM;
            GlobalEventHandler.RequestToPlaySFX += _PlaySFX;
            GlobalEventHandler.OnMusicToggled += _ToggleMusic;
            GlobalEventHandler.OnSoundToggled += _ToggleSound;
        }
        private void OnDisable()
        {
            GlobalEventHandler.RequestToPlayBGM -= _PlayBGM;
            GlobalEventHandler.RequestToPlaySFX -= _PlaySFX;
            GlobalEventHandler.OnMusicToggled -= _ToggleMusic;
            GlobalEventHandler.OnSoundToggled -= _ToggleSound;
        }
        #endregion Unity Methods

        #region Private Methods

        private void _Init()
        {
            m_bgmAudioSource = this.AddComponent<AudioSource>();
            m_sfxAudioSource = this.AddComponent<AudioSource>();
            m_bgmAudioSource.playOnAwake = false;
            m_sfxAudioSource.playOnAwake = false;
            m_sfxAudioSource.loop = false;
            m_bgmAudioSource.loop = true;
            m_bgmAudioSource.volume = .7f;
            m_bgmAudioSource.mute = !PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.music_toggle, true);
            m_sfxAudioSource.mute = !PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.sound_toggle, true);
        }
        private void _PlaySFX(AudioID audioID)
        {
            AudioClip clip = m_audioAsset.GetAudioClipByID(audioID);
            if (clip)
                m_sfxAudioSource.PlayOneShot(clip);
            else
                MyUtils.Log($"_PLAY SFX Null::", LogType.Error);
        }
        private void _PlayBGM(AudioID audioID)
        {
            AudioClip clip = m_audioAsset.GetAudioClipByID(audioID);
            if (clip)
            {
                m_bgmAudioSource.clip = m_audioAsset.GetAudioClipByID(audioID);
                m_bgmAudioSource.Play();
            }
            else
                MyUtils.Log($"_PLAY BGM Null::", LogType.Error);
        }
        private void _ToggleMusic(bool value)
        {
            m_bgmAudioSource.mute = !value;
        }
        private void _ToggleSound(bool value)
        {
            m_sfxAudioSource.mute = !value;

        }
        #endregion Private Methods

    }
}