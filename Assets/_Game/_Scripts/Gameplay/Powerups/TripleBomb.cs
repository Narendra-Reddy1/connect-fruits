using BenStudios;
using BenStudios.Economy;
using BenStudios.ScreenManagement;

namespace BenStudios
{
    public class TripleBomb : PowerUp
    {
        #region Variables
        private PowerupEntity m_myEntity;
        #endregion Variables


        #region Unity Methods

        private void OnEnable()
        {
            Init();
        }
        #endregion Unity Methods


        #region Public Methods
        public override void Init()
        {
            m_myEntity = GetComponentInParent<PowerupEntity>();
            _HandlePowerupCount();
        }

        public override void PerformPowerupAction()
        {
            switch (GlobalVariables.currentGameplayMode)
            {
                case GameplayType.LevelMode:
                    if (_DeductPowerup())
                    {

                        GlobalVariables.isTripleBombInAction = true;
                        GlobalEventHandler.RequestToActivatePowerUpMode?.Invoke(PowerupType.TripleBomb);
                    }
                    else
                    {
                        //Out of powerups...
                        //Show Shop Screen...
                    }
                    break;
                case GameplayType.ChallengeMode:

                    if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_triple_bomb_tutorial_shown_ChallengeMode))
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
                    break;
            }
        }
        #endregion Public Methods

        #region Private Methods

        private void _HandlePowerupCount()
        {
            switch (GlobalVariables.currentGameplayMode)
            {
                case GameplayType.LevelMode:
                    int m_powerupBalance = PlayerResourceManager.GetBalance(PlayerResourceManager.TRIPLE_BOMB_POWERUP_ITEM_ID);
                    if (m_powerupBalance > 0)
                    {
                        powerupHolderImage.sprite = powerupCountHolderSprite;
                        powerupCountTxt.SetText(m_powerupBalance.ToString());
                    }
                    else
                    {
                        powerupHolderImage.sprite = plusIconSprite;
                        powerupCountTxt.gameObject.SetActive(false);
                    }
                    break;
                case GameplayType.ChallengeMode:
                    powerupHolderImage.gameObject.SetActive(false);
                    break;
            }
        }
        private bool _DeductPowerup()
        {
            bool deducted = false;
            if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_triple_bomb_tutorial_shown))
            {
                GlobalEventHandler.RequestToPauseTimer?.Invoke(false);
                PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_triple_bomb_tutorial_shown, true);
                return true;
            }
            if (PlayerResourceManager.GetBalance(PlayerResourceManager.TRIPLE_BOMB_POWERUP_ITEM_ID) > 0)
            {
                PlayerResourceManager.Take(PlayerResourceManager.TRIPLE_BOMB_POWERUP_ITEM_ID);
                deducted = true;
            }
            _HandlePowerupCount();
            return deducted;
        }

        #endregion Private Methods

        #region Callbacks


        #endregion Callbacks
    }
}