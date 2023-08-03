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
            GlobalVariables.isFruitBombInAction = true;
            GlobalEventHandler.RequestToActivatePowerUpMode?.Invoke(PowerupType.FruitBomb);
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
}