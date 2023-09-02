using BenStudios.ScreenManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BenStudios
{
    public class ExitConfirmationPopup : PopupBase
    {
        [SerializeField] private GameObject m_starsPanel;
        [SerializeField] private GameObject m_normalConfrimationPanel;
        [SerializeField] private TextMeshProUGUI m_starsCountTxt;
        [SerializeField] private Button m_exitBtn, m_continueBtn;
        public override void OnEnable()
        {
            base.OnEnable();
            _Init();
        }
        private void _Init()
        {
            if (GlobalVariables.currentGameState == GameState.HomeScreen) return;
            if (GameplayManager.CollectedStars <= 0) return;
            m_normalConfrimationPanel.SetActive(false);
            m_starsPanel.SetActive(true);
            m_starsCountTxt.SetText(GameplayManager.CollectedStars.ToString());
        }

        public void OnClickExit()
        {
            m_continueBtn.interactable = false;
            switch (GlobalVariables.currentGameState)
            {
                case GameState.HomeScreen:
                    Application.Quit();
                    break;
                case GameState.Gameplay:
                    ScreenManager.Instance.ChangeScreen(Window.ScoreBoardScreen, ScreenType.Additive, onComplete: () =>
                    {
                        ScoreBoardScreen._Init(ScoreBoardScreen.PopupType.GameOver);
                    });
                    break;
            }
        }
        public void OnClickContinue()
        {
            m_exitBtn.interactable = false;
            ScreenManager.Instance.CloseLastAdditiveScreen();
        }


    }
}
