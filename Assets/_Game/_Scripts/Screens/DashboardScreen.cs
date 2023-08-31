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
    }
    private void _SetCoinsText() => m_coinsTxt.SetText(PlayerResourceManager.GetCoinsBalance().ToString());
    private void _StartGameplay()
    {
        GlobalVariables.currentGameplayMode = GameplayType.LevelMode;
        ScreenManager.Instance.ChangeScreen(Window.GameplayScreen, ScreenType.Replace, onComplete: () =>
        {
            GlobalEventHandler.RequestToPlayBGM?.Invoke(AudioID.GameplayBGM);
        });
    }
    #endregion Private Methods

    #region Callbacks
    private void Callback_On_ResourcesUpdated()
    {
        _SetCoinsText();
    }
    #endregion Callbacks
}
