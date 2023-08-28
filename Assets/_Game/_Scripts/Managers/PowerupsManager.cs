using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BenStudios
{
    public class PowerupsManager : MonoBehaviour
    {
        [SerializeField] private List<PowerupEntity> m_powerupHolders;

        [SerializeField] private Image m_gainedPowerupIcon;
        [SerializeField] private Image m_powerupFillbar;
        [SerializeField] private GameObject m_powerupsFullTxt;

        [SerializeField] private GameObject m_fruitBombPowerup;
        [SerializeField] private GameObject m_tripleBombPowerup;
        [SerializeField] private GameObject m_fruitDumperPowerup;
        [SerializeField] private Sprite m_fruitBombSprite;
        [SerializeField] private Sprite m_tripleBombSprite;
        [SerializeField] private Sprite m_fruitDumpSprite;

        [SerializeField] private FruitCallManager m_fruitCallManager;
        private float m_fillAmount;
        private byte m_fillCounter;
        private Vector3 m_gainedPowerupIconDefaultPose;
        private const float GRACE_TIME_FOR_POWERUP_FILL_BAR = 1.5f;
        //TipleBomb
        private byte m_tripleBombCounter = 0;
        private void OnEnable()
        {
            m_gainedPowerupIconDefaultPose = m_gainedPowerupIcon.transform.position;
            GlobalEventHandler.OnFruitPairMatched += Callback_On_Fruit_Pari_Matched;
            GlobalEventHandler.RequestToPerformTripleBombPowerupAction += Callback_On_Triple_Bomb_Action_Requested;
            GlobalEventHandler.RequestToPerformFruitBombPowerupAction += Callback_On_Fruit_Bomb_Action_Requested;
            GlobalEventHandler.RequestToDeactivatePowerUpMode += Callback_On_Deactivate_Powerup_Mode;
        }
        private void OnDisable()
        {
            GlobalEventHandler.OnFruitPairMatched -= Callback_On_Fruit_Pari_Matched;
            GlobalEventHandler.RequestToPerformTripleBombPowerupAction -= Callback_On_Triple_Bomb_Action_Requested;
            GlobalEventHandler.RequestToPerformFruitBombPowerupAction -= Callback_On_Fruit_Bomb_Action_Requested;
            GlobalEventHandler.RequestToDeactivatePowerUpMode -= Callback_On_Deactivate_Powerup_Mode;
        }
        private void _GrantPowerup()
        {
            int powerup = 0;
            if (GlobalVariables.currentGameplayMode == GameplayType.LevelMode)
                powerup = Random.Range(0, 2);
            else if (!GlobalVariables.isBoardClearedNearToHalf)
                powerup = Random.Range(0, 3);

            switch (powerup)
            {
                case 0://FruitBomb
                    ShowPowerupGainAnimation(m_fruitBombSprite);

                    break;
                case 1://TripleBomb
                    ShowPowerupGainAnimation(m_tripleBombSprite);

                    break;
                case 2://FruitDumper
                    ShowPowerupGainAnimation(m_fruitDumpSprite);
                    break;
            }
            void ShowPowerupGainAnimation(Sprite powerupSprite)
            {
                m_gainedPowerupIcon.gameObject.SetActive(true);
                m_gainedPowerupIcon.sprite = powerupSprite;
                PowerupEntity vacantEntity = GetVacantEntity();
                if (vacantEntity != null)
                {
                    m_gainedPowerupIcon.transform.DOMove(vacantEntity.powerupPose.position, 0.5f).onComplete += () =>
                    {
                        vacantEntity.Init(powerup);
                        m_gainedPowerupIcon.gameObject.SetActive(false);
                        m_gainedPowerupIcon.transform.position = m_gainedPowerupIconDefaultPose;
                        if (_CheckIfAllPowerupHoldersAreFull())
                        {
                            m_powerupsFullTxt.gameObject.SetActive(true);
                            m_powerupFillbar.fillAmount = 0;
                        }
                    };
                }
            }
        }
        private PowerupEntity GetVacantEntity()
        {
            return m_powerupHolders.Find(x => !x.isOccupied);
        }
        private bool _CheckIfAllPowerupHoldersAreFull()
        {
            return m_powerupHolders.FindAll(x => x.isOccupied).Count == 3;
        }
        private void _FruitBombAction(FruitEntity entity1, FruitEntity entity2)
        {
            entity1.ShrinkAndDestroy();
            entity2.ShrinkAndDestroy(() =>
            {
                GlobalVariables.isFruitBombInAction = false;
                GlobalEventHandler.RequestToDeactivatePowerUpMode?.Invoke();
            });
            entity1.ShowFruitBombEffect();
            entity2.ShowFruitBombEffect();
        }

        private void _TipleBombAction(FruitEntity entity1, FruitEntity entity2)
        {
            m_tripleBombCounter++;
            entity1.ShrinkAndDestroy();
            entity2.ShrinkAndDestroy(() =>
            {
                if (m_tripleBombCounter >= 3)
                {
                    m_tripleBombCounter = 0;
                    GlobalVariables.isTripleBombInAction = false;
                    GlobalEventHandler.RequestToDeactivatePowerUpMode?.Invoke();

                }
            });
            entity1.ShowFruitBombEffect();
            entity2.ShowFruitBombEffect();
        }
        private void Callback_On_Fruit_Pari_Matched(int id)
        {
            if (GlobalVariables.currentGameplayMode == GameplayType.LevelMode) return;

            if (_CheckIfAllPowerupHoldersAreFull())
            {
                m_powerupsFullTxt.gameObject.SetActive(true);
                m_powerupFillbar.fillAmount = 0;
                return;
            }
            float remainingTime = m_fruitCallManager.GetActiveFruitCallRemainingTime() * GRACE_TIME_FOR_POWERUP_FILL_BAR;
            float currentFillAmount = m_powerupFillbar.fillAmount;
            float totalFillAmount = remainingTime + currentFillAmount;
            float extraFillAmount = 0;
            if (totalFillAmount > 1f)
                extraFillAmount = totalFillAmount - 1f;
            m_powerupFillbar.DOFillAmount(totalFillAmount - extraFillAmount, 0.3f).onComplete += () =>
            {
                if (extraFillAmount >= 0.1f || m_powerupFillbar.fillAmount >= 0.95)
                {
                    m_powerupFillbar.fillAmount = 0;
                    m_fillCounter = 0;
                    m_powerupFillbar.DOFillAmount(extraFillAmount, 0.3f);
                    _GrantPowerup();
                }
                else
                    m_fillCounter++;
            };

        }
        private void Callback_On_Triple_Bomb_Action_Requested(FruitEntity entity1, FruitEntity entity2)
        {
            _TipleBombAction(entity1, entity2);
        }
        private void Callback_On_Fruit_Bomb_Action_Requested(FruitEntity entity1, FruitEntity entity2)
        {
            _FruitBombAction(entity1, entity2);
        }
        private void Callback_On_Deactivate_Powerup_Mode()
        {
            m_powerupsFullTxt.gameObject.SetActive(_CheckIfAllPowerupHoldersAreFull());
        }

    }
}