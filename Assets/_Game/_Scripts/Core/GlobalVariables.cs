
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

        public static void ResetGameplayVariables()
        {
            isBoardClearedNearToHalf = false;
            isFruitBombInAction = false;
            isTripleBombInAction = false;
            dumpedFruitIndex = -1;
            currentFruitCallId = -1;
        }
    }
}