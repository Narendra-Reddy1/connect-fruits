using BenStudios.ScreenManagement;
using UnityEngine;

namespace BenStudios
{
    public class ExitConfirmationPopup : PopupBase
    {
        public void OnClickExit()
        {
            switch (GlobalVariables.currentGameState)
            {
                case GameState.HomeScreen:
                    Application.Quit();
                    break;
                case GameState.Gameplay:
                    // ScreenManager.Instance.ChangeScreen(Window.Dashboard);
                    ScreenManager.Instance.ChangeScreen(Window.ScoreBoardScreen, ScreenType.Additive, onComplete: () =>
                    {
                        ScoreBoardScreen._Init(ScoreBoardScreen.PopupType.GameOver);
                    });
                    break;
            }
        }
        public void OnClickContinue()
        {
            ScreenManager.Instance.CloseLastAdditiveScreen();
        }


    }
}
