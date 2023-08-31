using BenStudios.IAP;
using System;
using UnityEngine;
using UnityEngine.Purchasing.Extension;

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
        public static Action RequestToShowGoodMatchText = default;
        public static Action OnColumnCleared = default;
        public static Action OnRowCleared = default;
        public static Action<short> EventOnStarParticleAttracted = default;
        public static Action OnPowerupTutorialCompleted = default;
        //POWERUP 
        public static Action HintPowerupActionRequested = default;
        public static Action RequestToCheckForPairIsAvailableForAutoMatch = default;
        public static Action EventOnPairIsAvailableForHintPowerup = default;
        public static Action EventOnNoPairIsAvailableForHintPowerup = default;

        //Chest
        public static Action EventOnStarChestClaimed = default;
        public static Action EventOnLevelChestClaimed = default;
        public static Action EventOnLevelChestOpenedButNotClaimed = default;
        public static Action EventOnStarChestOpenedButNotClaimed = default;

        //UI
        public static Action<int> RequestToUpdateScore = default;

        //Audio
        public static Action<AudioID> RequestToPlayBGM = default;
        public static Action<AudioID> RequestToPlaySFX = default;

        public static Action<bool> OnMusicToggled = default;
        public static Action<bool> OnSoundToggled = default;

        //IAP
        public static Action<string> RequestToInitializePurchase = default;
        public static Action<PurchaseData> OnPurchaseSuccess = default;
        public static Action<PurchaseFailureDescription> OnPurchaseFailed = default;















        //ReturnType Callbacks
        public static Func<int> RequestRemainingTimer = default;
        public static Func<int> RequestTotalMatchedFruits = default;
        public static Func<Vector2Int> RequestClearedRowAndColumnCount = default;
    }
}