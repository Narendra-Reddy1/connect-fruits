using BenStudios;
using BenStudios.ScreenManagement;
using UnityEngine;

public class DashboardScreen : ScreenBase
{
    #region Variables 

    #endregion Variables


    #region Unity Methods
    #endregion Unity Methods

    #region Public Methods

    public void OnClickSettingsBtn()
    {
        ScreenManager.Instance.ChangeScreen(Window.SettingsPopup, ScreenType.Additive);
    }
    public void OnClickPlayButton()
    {
        _StartGameplay();
    }
    #endregion Public Methods

    #region Private Methods
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
