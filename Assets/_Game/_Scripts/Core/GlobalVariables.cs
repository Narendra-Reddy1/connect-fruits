
namespace BenStudios
{
    public static class GlobalVariables
    {

        public static int currentFruitCallId = -1;
        public static bool isFruitBombInAction = false;
        public static bool isTripleBombInAction = false;
        public static int dumpedFruitIndex = -1;
        public static bool isBoardClearedNearToHalf = false;
        public static GameState currentGameState = GameState.HomeScreen;
        public static int highestUnlockedLevel = 1;
        public static GameplayType currentGameplayMode = GameplayType.LevelMode;
        //public static int currentSelectedLevel;
        public static int levelChestRewardOccurence = 5;
        public static int lastLevelChestRewardDefinedAt = 5;
        public static short CollectedStars;
        internal static bool isLevelCompletedSuccessfully;
        public static byte outOfTimeCounter = 1;

        public static void ResetGameplayVariables()
        {
            isBoardClearedNearToHalf = false;
            isFruitBombInAction = false;
            isTripleBombInAction = false;
            dumpedFruitIndex = -1;
            currentFruitCallId = -1;
            outOfTimeCounter = 1;
        }
    }
}