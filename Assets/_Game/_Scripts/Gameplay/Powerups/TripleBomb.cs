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
            PlayerResourceManager.onStoreGiveCallback += OnResourcesUpdated;
            Init();
        }
        private void OnDisable()
        {
            PlayerResourceManager.onStoreGiveCallback -= OnResourcesUpdated;
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
                        GlobalEventHandler.RequestToPauseTimer?.Invoke(true);
                        ScreenManager.Instance.ChangeScreen(Window.StoreScreen, ScreenType.Additive);
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
                        costPanel.SetActive(false);
                        powerupCountPanel.SetActive(true);
                        powerupHolderImage.sprite = powerupCountHolderSprite;
                        powerupCountTxt.SetText(m_powerupBalance.ToString());
                    }
                    else
                    {
                        costPanel.SetActive(true);
                        coinPriceTxt.SetText(Konstants.TRIPLE_BOMB_POWERUP_COST.ToString());
                        powerupCountPanel.SetActive(false);
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
            else if (PlayerResourceManager.GetCoinsBalance() >= Konstants.TRIPLE_BOMB_POWERUP_COST)
            {
                PlayerResourceManager.Take(PlayerResourceManager.COINS_ITEM_ID, Konstants.TRIPLE_BOMB_POWERUP_COST);
                deducted = true;
            }
            _HandlePowerupCount();
            return deducted;
        }

        #endregion Private Methods

        #region Callbacks

        private void OnResourcesUpdated()
        {
            _HandlePowerupCount();
        }
        #endregion Callbacks
    }
}