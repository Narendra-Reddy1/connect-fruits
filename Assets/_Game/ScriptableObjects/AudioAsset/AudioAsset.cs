using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace BenStudios
{
    [CreateAssetMenu(fileName = "newAudioAsset", menuName = "ScriptableObjects/AudioAsset", order = 1)]
    public class AudioAsset : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<AudioID, AudioClip> m_audioAsset;
        public AudioClip GetAudioClipByID(AudioID id)
        {
            AudioClip clip = null;
            m_audioAsset.TryGetValue(id, out clip);
            return clip;
        }
    }
    public enum AudioID
    {
        DashboardBGM,
        GameplayBGM,
        ButtonClickSFX,
        TimerCountdownSFX,
        TimerCountdownEndSFX,
        LevelCompleteSFX,
        MatchFailedSFX,
        MatchSuccessSFX,
        RewardSparklesSFX,
        PowerupSelectionSFX,
    }
}