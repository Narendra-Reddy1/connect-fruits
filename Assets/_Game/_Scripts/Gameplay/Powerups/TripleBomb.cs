using BenStudios;
using BenStudios.ScreenManagement;

public class TripleBomb : PowerUp
{
    #region Variables
    private PowerupEntity m_myEntity;
    #endregion Variables


    #region Unity Methods

    private void OnEnable()
    {
        m_myEntity = GetComponentInParent<PowerupEntity>();
    }
    #endregion Unity Methods


    #region Public Methods
    public override void Init()
    {
    }

    public override void PerformPowerupAction()
    {
        if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_triple_bomb_tutorial_shown))
            ScreenManager.Instance.ChangeScreen(Window.GenericPowerupTutorialPopup, ScreenType.Additive, onComplete: () =>
            {
                GenericPowerupInfoPopup.Init(GenericPowerupInfoPopup.PopupType.TripleBomb);
            });
        GlobalVariables.isTripleBombInAction = true;
        GlobalEventHandler.RequestToActivatePowerUpMode?.Invoke(PowerupType.TripleBomb);
        if (m_myEntity != null)
        {
            m_myEntity.isOccupied = false;
        }
        gameObject.SetActive(false);
    }
    #endregion Public Methods

    #region Private Methods



    #endregion Private Methods

    #region Callbacks


    #endregion Callbacks
}
