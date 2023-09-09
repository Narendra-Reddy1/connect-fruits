using BenStudios.Economy;
using BenStudios.ScreenManagement;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BenStudios
{
    public class FruitBomb : PowerUp
    {

        #region Variables
        private PowerupEntity m_myEntity;
        private int m_powerupBalance;
        #endregion Variables


        #region Unity Methods
        private void OnEnable()
        {
            Init();
            PlayerResourceManager.onStoreGiveCallback += OnResourcesUpdated;
            GlobalEventHandler.PerformOnClickFruitBomb += PerformPowerupAction;
        }
        private void OnDisable()
        {
            PlayerResourceManager.onStoreGiveCallback -= OnResourcesUpdated;
            GlobalEventHandler.PerformOnClickFruitBomb -= PerformPowerupAction;
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
                        GlobalVariables.isFruitBombInAction = true;
                        GlobalEventHandler.RequestToActivatePowerUpMode?.Invoke(PowerupType.FruitBomb);
                    }
                    else
                        ScreenManager.Instance.ChangeScreen(Window.PowerupPurchasePopup, ScreenType.Additive, false);
                    break;
                case GameplayType.ChallengeMode:
                    if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_fruit_bomb_tutorial_shown_ChallengeMode))
                        ScreenManager.Instance.ChangeScreen(Window.GenericPowerupTutorialPopup, ScreenType.Additive, onComplete: () =>
                        {
                            GenericPowerupInfoPopup.Init(GenericPowerupInfoPopup.PopupType.FruitBomb);
                        });
                    GlobalVariables.isFruitBombInAction = true;
                    GlobalEventHandler.RequestToActivatePowerUpMode?.Invoke(PowerupType.FruitBomb);
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
                    m_powerupBalance = PlayerResourceManager.GetBalance(PlayerResourceManager.FRUIT_BOMB_POWERUP_ITEM_ID);
                    if (m_powerupBalance > 0)
                    {
                        costPanel.SetActive(false);
                        powerupCountPanel.SetActive(true);
                        powerupHolderImage.sprite = powerupCountHolderSprite;
                        powerupCountTxt.SetText(m_powerupBalance.ToString());
                        MyUtils.Log($"FRUITBOMB IS SET...");
                    }
                    else
                    {
                        costPanel.SetActive(true);
                        coinPriceTxt.SetText(Konstants.FRUIT_BOMB_POWERUP_COST.ToString());
                        powerupHolderImage.sprite = plusIconSprite;
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
            if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_fruit_bomb_tutorial_shown))
            {
                GlobalEventHandler.RequestToPauseTimer?.Invoke(false);
                PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_fruit_bomb_tutorial_shown, true);
                return true;
            }
            if (PlayerResourceManager.GetBalance(PlayerResourceManager.FRUIT_BOMB_POWERUP_ITEM_ID) > 0)
            {
                PlayerResourceManager.Take(PlayerResourceManager.FRUIT_BOMB_POWERUP_ITEM_ID);
                deducted = true;
            }
            else if (PlayerResourceManager.GetCoinsBalance() >= Konstants.FRUIT_BOMB_POWERUP_COST)
            {
                PlayerResourceManager.Take(PlayerResourceManager.COINS_ITEM_ID, Konstants.FRUIT_BOMB_POWERUP_COST);
                deducted = true;
            }
            _HandlePowerupCount();
            return deducted;
        }

        #endregion Private Methods

        #region Callbacks
        private void OnResourcesUpdated()
        {
            MyUtils.Log($"@@@@@@@@@ Resources UPDATED::::");
            _HandlePowerupCount();
        }

        #endregion Callbacks
    }
}