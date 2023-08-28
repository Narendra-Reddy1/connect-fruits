using BenStudios.ScreenManagement;
using DG.Tweening;
using System.Collections.Generic;
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
        [SerializeField] private GameObject m_goodMatchTxtPrefab;
        [SerializeField] private GameObject m_rowColClearedTxtPrefab;
        [SerializeField] private Transform m_praiseTxtsParent;

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
        [SerializeField] private Transform m_praiseTxtsStartPose;
        [SerializeField] private Transform m_praiseTxtsEndPose;

        [Header("Powerup Sprites")]
        [SerializeField] private Sprite m_fruitBombSprite;
        [SerializeField] private Sprite m_tripleBombSprite;
        [SerializeField] private Sprite m_fruitDumpSprite;

        private Vector3 m_powerupHolderInitialPose;
        private Vector3 m_topbarUiInitialPose;
        private Vector3 m_updateScoreInitialPose;
        private int m_score;
        private List<Color> m_matchTextColors = new List<Color>()
        {
            new Color(0,191,182),//CYAN
            new Color(191,158,0),//YELLOW
            new Color(191,7,79),//PURPLE
            new Color(0,191,2),//GREEN
        };
        private List<string> m_matchTextNames = new List<string>()
        {
            "PERFECT MATCH",
            "GREAT MATCH",
            "AWESOME",
            "EXCELLENT MATCH",
        };
        private const string COLUMN_CLEARED = "COLUMN CLEARED!";
        private const string ROW_CLEARED = "ROW CLEARED!";
        //object pool names
        private const string GOOD_MATCH_TEXT_POOL = "match_text_pool";
        private const string ROW_COL_CLEARED_TEXT_POOL = "row_col_cleared_text_pool";
        #endregion Variables

        #region Unity Methods
        private void OnEnable()
        {
            _Init();
            GlobalEventHandler.RequestToUpdateScore += Callback_On_Score_Update_Requested;
            GlobalEventHandler.RequestToActivatePowerUpMode += Callback_On_Activate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToDeactivatePowerUpMode += Callback_On_Deactivate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToShowGoodMatchText += Callback_On_Show_Match_Text_Requested;
            GlobalEventHandler.OnColumnCleared += Callback_On_Column_Cleared;
            GlobalEventHandler.On_Row_Cleared += Callback_On_Row_Cleared;
        }
        private void OnDisable()
        {
            GlobalEventHandler.RequestToUpdateScore -= Callback_On_Score_Update_Requested;
            GlobalEventHandler.RequestToActivatePowerUpMode -= Callback_On_Activate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToDeactivatePowerUpMode -= Callback_On_Deactivate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToShowGoodMatchText -= Callback_On_Show_Match_Text_Requested;
            GlobalEventHandler.OnColumnCleared -= Callback_On_Column_Cleared;
            GlobalEventHandler.On_Row_Cleared -= Callback_On_Row_Cleared;
        }
        #endregion Unity Methods



        #region Public Methods
        public void OnClickSettingsBtn()
        {
            ScreenManager.Instance.ChangeScreen(Window.SettingsPopup, ScreenType.Additive);
        }
        #endregion Public Methods

        #region Private Methods
        private void _Init()
        {
            m_updateScoreInitialPose = m_updateScoreEffectTxt.transform.position;
            m_powerupHolderInitialPose = m_powerupsHolderTransform.position;
            m_topbarUiInitialPose = m_topbarUiTransform.position;
            ObjectPoolManager.instance.InitializePool(GOOD_MATCH_TEXT_POOL, m_goodMatchTxtPrefab, 5, m_praiseTxtsParent);
            ObjectPoolManager.instance.InitializePool(ROW_COL_CLEARED_TEXT_POOL, m_rowColClearedTxtPrefab, 5, m_praiseTxtsParent);
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
        private Color _GetRandomColor()
        {
            int index = Random.Range(0, m_matchTextColors.Count);
            return m_matchTextColors[index];
        }
        private string _GetRandomMatchTxt()
        {
            int index = Random.Range(0, m_matchTextNames.Count);
            return m_matchTextNames[index];
        }
        private void _AnimateTxt(GameObject textObj, string text)
        {
            TextMeshProUGUI textComponent = textObj.GetComponent<TextMeshProUGUI>();
            CanvasGroup cg = textObj.GetComponent<CanvasGroup>();
            textComponent.color = _GetRandomColor();
            textComponent.SetText(text);
            textObj.transform.DOMove(m_praiseTxtsEndPose.position, 1).onComplete += () =>
            {
                textObj.transform.position = m_praiseTxtsStartPose.position;
                textObj.SetActive(false);
                cg.alpha = 1;
            };
            cg.DOFade(0, 1f);
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
        private void Callback_On_Show_Match_Text_Requested()
        {
            GameObject item = ObjectPoolManager.instance.GetObjectFromPool(GOOD_MATCH_TEXT_POOL);
            item.SetActive(true);
            _AnimateTxt(item, _GetRandomMatchTxt());
        }
        private void Callback_On_Row_Cleared()
        {
            GameObject item = ObjectPoolManager.instance.GetObjectFromPool(ROW_COL_CLEARED_TEXT_POOL);
            item.SetActive(true);
            _AnimateTxt(item, ROW_CLEARED);

        }
        private void Callback_On_Column_Cleared()
        {
            GameObject item = ObjectPoolManager.instance.GetObjectFromPool(ROW_COL_CLEARED_TEXT_POOL);
            item.SetActive(true);
            _AnimateTxt(item, COLUMN_CLEARED);
        }
        #endregion Callbacks



    }
}