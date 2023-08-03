using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BenStudios
{
    public class InGameUIManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private EntityDatabase m_entityDatabase;
        [SerializeField] private TextMeshProUGUI m_scoreTxt;
        [SerializeField] private TextMeshProUGUI m_updateScoreEffectTxt;
        [SerializeField] private Transform m_updateScoreReferencePose;
        [SerializeField] private CanvasGroup m_scoreEffectTxtCanvasGroup;

        //PowerupMode
        [SerializeField] private Transform m_topbarUiTransform;
        [SerializeField] private Transform m_powerupsHolderTransform;
        [SerializeField] private Transform m_topbarUiEndPose;
        [SerializeField] private Transform m_powerupsHolderEndPose;
        [SerializeField] private Transform m_activePowerupIconTransform;
        [SerializeField] private Image m_activePowerupImage;
        [SerializeField] private Transform m_powerupInfoStrip;
        [SerializeField] private Transform m_powerupStripEndPose;
        [SerializeField] private TextMeshProUGUI m_powerupTitleTxt;
        [SerializeField] private TextMeshProUGUI m_powerupInfoTxt;
        [SerializeField] private CanvasGroup m_powerupStripCanvasGroup;
        [SerializeField] private CanvasGroup m_fruitCallPanelCanvasGroup;
        [SerializeField] private Image m_fruitToDumpImage;
        [SerializeField] private GameObject m_fruitbombInfoStripPanel;
        [SerializeField] private GameObject m_fruitDumperInfoStripPanel;


        [Header("Powerup Sprites")]
        [SerializeField] private Sprite m_fruitBombSprite;
        [SerializeField] private Sprite m_tripleBombSprite;
        [SerializeField] private Sprite m_fruitDumpSprite;

        private Vector3 m_powerupHolderInitialPose;
        private Vector3 m_topbarUiInitialPose;
        private Vector3 m_updateScoreInitialPose;
        private int m_score;
        #endregion Variables

        #region Unity Methods
        private void OnEnable()
        {
            _Init();
            GlobalEventHandler.RequestToUpdateScore += Callback_On_Score_Update_Requested;
            GlobalEventHandler.RequestToActivatePowerUpMode += Callback_On_Activate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToDeactivatePowerUpMode += Callback_On_Deactivate_Powerup_Mode_Requested;
        }
        private void OnDisable()
        {
            GlobalEventHandler.RequestToUpdateScore -= Callback_On_Score_Update_Requested;
            GlobalEventHandler.RequestToActivatePowerUpMode -= Callback_On_Activate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToDeactivatePowerUpMode -= Callback_On_Deactivate_Powerup_Mode_Requested;

        }
        #endregion Unity Methods



        #region Public Methods
        public void OnClickSettingsBtn()
        {

        }
        #endregion Public Methods

        #region Private Methods
        private void _Init()
        {
            m_updateScoreInitialPose = m_updateScoreEffectTxt.transform.position;
            m_powerupHolderInitialPose = m_powerupsHolderTransform.position;
            m_topbarUiInitialPose = m_topbarUiTransform.position;
        }


        private void _ShowScoreUpdateEffect(int score)
        {
            m_updateScoreEffectTxt.text = $"+{score}";
            m_updateScoreEffectTxt.gameObject.SetActive(true);
            m_updateScoreEffectTxt.transform.DOMoveY(m_updateScoreReferencePose.position.y, 1.25f).onComplete += () =>
            {
                m_scoreEffectTxtCanvasGroup.alpha = 1;
                m_updateScoreEffectTxt.transform.position = m_updateScoreInitialPose;
                m_updateScoreEffectTxt.gameObject.SetActive(false);
            };
            m_scoreEffectTxtCanvasGroup.DOFade(0, .75f).SetDelay(0.35f);
            _UpdateScore(score);
        }
        private void _UpdateScore(int scoreToAdd)
        {
            m_score += scoreToAdd;
            m_scoreTxt.DOText(m_score.ToString(), .75f, scrambleMode: ScrambleMode.Numerals); ;
        }

        private void _ActivatePowerupMode(PowerupType powerupType)
        {

            switch (powerupType)
            {
                case PowerupType.FruitBomb:
                    ActivateFruitBombPowerupUIMode();
                    m_powerupTitleTxt.text = $"Fruit Bomb";
                    m_powerupInfoTxt.text = $"Choose any matching pairs of fruits";
                    m_activePowerupImage.sprite = m_fruitBombSprite;
                    break;
                case PowerupType.TripleBomb:
                    ActivateFruitBombPowerupUIMode();
                    m_powerupTitleTxt.text = $"Triple Bomb";
                    m_powerupInfoTxt.text = $"Choose any matching pairs of fruits";
                    m_activePowerupImage.sprite = m_tripleBombSprite;
                    break;
                case PowerupType.FruitDumper:
                    if (GlobalVariables.dumpedFruitIndex == -1) break;
                    m_fruitbombInfoStripPanel.SetActive(false);
                    m_fruitDumperInfoStripPanel.SetActive(true);
                    ShowFruitDumpUI();
                    break;
            }
            void ActivateFruitBombPowerupUIMode()
            {
                m_fruitbombInfoStripPanel.SetActive(true);
                m_fruitDumperInfoStripPanel.SetActive(false);
                m_fruitCallPanelCanvasGroup.DOFade(0, .45f);
                m_topbarUiTransform.DOMove(m_topbarUiEndPose.position, .5f);
                m_powerupsHolderTransform.DOMove(m_powerupsHolderEndPose.position, .5f);
                m_activePowerupIconTransform.DOScale(Vector3.one, .5f);
                m_powerupStripCanvasGroup.alpha = 1;
                m_powerupInfoStrip.DOMove(m_powerupStripEndPose.position, 0.45f);
            }
            void ShowFruitDumpUI()
            {
                m_fruitToDumpImage.sprite = m_entityDatabase.fruitSprites[GlobalVariables.dumpedFruitIndex];
                m_fruitCallPanelCanvasGroup.DOFade(0, .2f);
                m_powerupStripCanvasGroup.alpha = 1;
                m_powerupInfoStrip.DOMove(m_powerupStripEndPose.position, 0.2f);
            }
        }
        private void _DeactivatePowerupMode()
        {
            m_powerupStripCanvasGroup.DOFade(0, .2f).onComplete += () =>
            {
                m_powerupInfoStrip.position = m_topbarUiEndPose.position;
            };
            m_activePowerupIconTransform.DOScale(Vector3.zero, 0.1f);
            m_fruitCallPanelCanvasGroup.DOFade(1, .1f);
            m_topbarUiTransform.DOMove(m_topbarUiInitialPose, .3f);
            m_powerupsHolderTransform.DOMove(m_powerupHolderInitialPose, .3f);
        }

        #endregion Private Methods

        #region Callbacks
        private void Callback_On_Score_Update_Requested(int score)
        {
            _ShowScoreUpdateEffect(score);
        }

        private void Callback_On_Activate_Powerup_Mode_Requested(PowerupType powerupType)
        {
            _ActivatePowerupMode(powerupType);
        }
        private void Callback_On_Deactivate_Powerup_Mode_Requested()
        {
            _DeactivatePowerupMode();
        }

        #endregion Callbacks



    }
}