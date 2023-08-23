using BenStudios;
using BenStudios.Economy;
using BenStudios.ScreenManagement;
using TMPro;
using UnityEngine;

public class DashboardScreen : ScreenBase
{
    #region Variables 
    [SerializeField] private TextMeshProUGUI m_levelTxt;
    [SerializeField] private TextMeshProUGUI m_coinsTxt;
    #endregion Variables


    #region Unity Methods
    private void OnEnable()
    {
        GlobalVariables.currentGameState = GameState.HomeScreen;
        _Init();
    }
    #endregion Unity Methods

    #region Public Methods

    public void OnClickSettingsBtn()
    {
        ScreenManager.Instance.ChangeScreen(Window.SettingsPopup, ScreenType.Additive);
    }
    public void OnClickStoreBtn()
    {
        ScreenManager.Instance.ChangeScreen(Window.StoreScreen, ScreenType.Additive);
    }
    public void OnClickPlayButton()
    {
        _StartGameplay();
    }
    #endregion Public Methods

    #region Private Methods
    private void _Init()
    {
        m_levelTxt.SetText($"LEVEL {GlobalVariables.highestUnlockedLevel}");
        m_coinsTxt.SetText(PlayerResourceManager.GetCoinsBalance().ToString());
    }
    private void _StartGameplay()
    {
        ScreenManager.Instance.ChangeScreen(Window.GameplayScreen, ScreenType.Replace, onComplete: () =>
        {
            GlobalEventHandler.RequestToPlayBGM?.Invoke(AudioID.GameplayBGM);
        });
    }
    #endregion Private Methods

    #region Callbacks

    #endregion Callbacks
}
