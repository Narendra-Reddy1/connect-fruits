using System;

namespace FruitFrenzy
{
    public static class GlobalEventHandler
    {
        public static Action<FruitEntity> OnFruitEntitySelected = default;
        public static Action<FruitEntity> OnFruitEntityUnSelected = default;
        public static Action<int> OnFruitPairMatched = default;
        public static Action<MatchFailedCause> OnFruitPairMatchFailed = default;
        public static Action<int> RequestToUpdateScore = default;

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

    }
}