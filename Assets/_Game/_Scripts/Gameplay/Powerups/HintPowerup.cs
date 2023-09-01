using BenStudios.Economy;
using BenStudios.ScreenManagement;
using Coffee.UIEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BenStudios
{
    public class HintPowerup : PowerUp
    {

        #region Variables
        [SerializeField] private List<UIEffect> m_grayScaleEffects;
        #endregion Variables


        #region Unity Methods

        private void OnEnable()
        {
            Init();
            GlobalEventHandler.EventOnNoPairIsAvailableForHintPowerup += Callback_On_Auto_Match_Is_Not_Available;
        }
        private void OnDisable()
        {
            GlobalEventHandler.EventOnNoPairIsAvailableForHintPowerup -= Callback_On_Auto_Match_Is_Not_Available;
        }
        #endregion Unity Methods


        #region Public Methods
        public override void Init()
        {
            _HandlePowerupCount();
        }

        public override void PerformPowerupAction()
        {
            if (_DeductPowerup())
            {
                GlobalEventHandler.RequestToScreenBlocker?.Invoke(true);
                GlobalEventHandler.HintPowerupActionRequested?.Invoke();
            }
            else
            {
                //Out Of Powerups
                //Show Store.....
            }
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
            if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_hint_powerup_tutorial_shown))
            {
                PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_hint_powerup_tutorial_shown, true);
                return true;
            }
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
        private void Callback_On_Auto_Match_Is_Not_Available()
        {
            for (int i = 0, count = m_grayScaleEffects.Count; i < count; i++)
            {
                m_grayScaleEffects[i].effectMode = EffectMode.Grayscale;
            }
        }

        #endregion Callbacks
    }
}