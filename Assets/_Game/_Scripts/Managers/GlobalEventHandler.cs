using System;
using UnityEngine;

namespace BenStudios
{
    public static class GlobalEventHandler
    {
        //Gameplay
        public static Action<FruitEntity> OnFruitEntitySelected = default;
        public static Action<FruitEntity> OnFruitEntityUnSelected = default;
        public static Action<int> OnFruitPairMatched = default;
        public static Action<MatchFailedCause> OnFruitPairMatchFailed = default;

        public static Action<PowerupType> RequestToActivatePowerUpMode = default;
        public static Action RequestToDeactivatePowerUpMode = default;
        public static Action<FruitEntity, FruitEntity> RequestToPerformFruitBombPowerupAction = default;
        public static Action<FruitEntity, FruitEntity> RequestToPerformTripleBombPowerupAction = default;
        public static Action RequestToFruitDumperPowerupAction = default;

        public static Action<bool> RequestToScreenBlocker = default;
        public static Action OnFruitCallIsCompleted = default;
        public static Action RequestToStartFruitCall = default;
        public static Action OnLevelStartupTimerIsCompleted = default;
        public static Action OnLevelTimerIsCompleted = default;
        public static Action<bool> RequestToPauseTimer = default;//true will pause false will unpause.

        //UI
        public static Action<int> RequestToUpdateScore = default;

        //Audio
        public static Action<AudioID> RequestToPlayBGM = default;
        public static Action<AudioID> RequestToPlaySFX = default;

        public static Action<bool> OnMusicToggled = default;
        public static Action<bool> OnSoundToggled = default;

        //ReturnType Callbacks
        public static Func<int> RequestRemainingTimer = default;
        public static Func<int> RequestTotalMatchedFruits = default;
        public static Func<Vector2Int> RequestClearedRowAndColumnCount = default;
    }
}