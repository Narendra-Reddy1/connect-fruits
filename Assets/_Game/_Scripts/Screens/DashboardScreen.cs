using BenStudios;
using BenStudios.Economy;
using BenStudios.ScreenManagement;
using System.Collections;
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
        PlayerResourceManager.onStoreGiveCallback += Callback_On_ResourcesUpdated;
        GlobalVariables.currentGameState = GameState.HomeScreen;
        _Init();
    }
    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.P))
    //    {
    //        GlobalVariables.CollectedStars += 100;
    //    }
    //}
    private void OnDisable()
    {
        PlayerResourceManager.onStoreGiveCallback -= Callback_On_ResourcesUpdated;
    }
    #endregion Unity Methods

    #region Public Methods

    public void OnClickSettingsBtn()
    {
        ScreenManager.Instance.ChangeScreen(Window.SettingsPopup, ScreenType.Additive, false);
    }
    public void OnClickStoreBtn()
    {
        ScreenManager.Instance.ChangeScreen(Window.StoreScreen, ScreenType.Additive, false);
    }
    public void OnClickPlayButton()
    {
        _StartGameplay();
    }

    public void OnClickStarChest()
    {
        ChestManager.Instance.StarChestReward();
    }
    public void OnClickLevelChest()
    {
        ChestManager.Instance.LevelChestReward();
    }
    #endregion Public Methods

    #region Private Methods
    private void _Init()
    {
        m_levelTxt.SetText($"LEVEL {GlobalVariables.highestUnlockedLevel}");
        _SetCoinsText();
        StartCoroutine(_ShowSupportPopup());
    }
    private void _SetCoinsText() => m_coinsTxt.SetText(PlayerResourceManager.GetCoinsBalance().ToString());
    private void _StartGameplay()
    {
        GlobalVariables.currentGameplayMode = GameplayType.LevelMode;
        ScreenManager.Instance.ChangeScreen(Window.GameplayScreen, ScreenType.Replace, false, onComplete: () =>
        {
            GlobalEventHandler.RequestToPlayBGM?.Invoke(AudioID.GameplayBGM);
        });
    }
    private WaitForSeconds _waitForSecondsToShowPopup = new WaitForSeconds(.65f);
    private IEnumerator _ShowSupportPopup()
    {
        if (GlobalVariables.highestUnlockedLevel < Konstants.MIN_LEVEL_TO_ASK_SUPPORT || PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_player_supported_dev)) yield break;

        if (GlobalVariables.highestUnlockedLevel % 50 != 0 && !PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_support_dev_popup_shown)) yield break;

        yield return _waitForSecondsToShowPopup;
        PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_support_dev_popup_shown, true);
        ScreenManager.Instance.ChangeScreen(Window.SupportDevAskScreen, ScreenType.Additive, false);
    }
    #endregion Private Methods

    #region Callbacks
    private void Callback_On_ResourcesUpdated()
    {
        _SetCoinsText();
    }
    #endregion Callbacks
}
