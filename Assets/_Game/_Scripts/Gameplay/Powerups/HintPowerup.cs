using BenStudios.Economy;
using BenStudios.ScreenManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BenStudios
{
    public class HintPowerup : PowerUp
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
            GlobalEventHandler.RequestToScreenBlocker?.Invoke(true);
            GlobalEventHandler.HintPowerupActionRequested?.Invoke();
        }
        #endregion Public Methods

        #region Private Methods

        private void _HandlePowerupCount()
        {
            switch (GlobalVariables.currentGameplayMode)
            {
                case GameplayType.LevelMode:
                    int m_powerupBalance = PlayerResourceManager.GetBalance(PlayerResourceManager.HINT_POWERUP_ITEM_ID);
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
                    gameObject.SetActive(false);
                    break;
            }
        }
        private bool _DeductPowerup()
        {
            bool deducted = false;
            if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_triple_bomb_tutorial_shown)) return true;
            if (PlayerResourceManager.GetBalance(PlayerResourceManager.HINT_POWERUP_ITEM_ID) > 0)
            {
                PlayerResourceManager.Take(PlayerResourceManager.HINT_POWERUP_ITEM_ID);
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