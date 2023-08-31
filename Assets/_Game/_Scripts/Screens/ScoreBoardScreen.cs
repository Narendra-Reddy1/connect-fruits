using UnityEngine;
using BenStudios.ScreenManagement;
using TMPro;
using DG.Tweening;
using UnityEditor;
using UnityEngine.UI;

namespace BenStudios
{
    public class ScoreBoardScreen : ScreenBase
    {
        [SerializeField] private TextMeshProUGUI m_headerTxt;
        [SerializeField] private TextMeshProUGUI m_multiClearScoreTxt;
        [SerializeField] private TextMeshProUGUI m_rowColumnClearScoreTxt;
        [SerializeField] private TextMeshProUGUI m_allClearBonusTxt;
        [SerializeField] private TextMeshProUGUI m_timeBonusTxt;
        [SerializeField] private TextMeshProUGUI m_finalScoreTxt;
        [SerializeField] private float m_scoreCalculatingTime = .7f;
        [SerializeField] private Button m_submitBtn;


        private static PopupType m_popupType;
        private int m_multiClearScore = 0;
        private int m_rowColumnClearScore = 0;
        private int m_timeBonus = 0;
        private int m_allClearBonus = 0;
        private int m_finalScore = 0;
        private static System.Action OnPopupInitalized = default;

        private const string LEVEL_COMPLETED_TEXT = "LEVEL COMPLETED";
        private const string GAME_OVER_TEXT = "GAME OVER";
        private const string TIME_UP_TEXT = "TIME'S UP";

        private void OnEnable()
        {
            OnPopupInitalized += _SetupPopup;
            OnPopupInitalized += _CalculateScores;
        }
        private void OnDisable()
        {
            OnPopupInitalized -= _CalculateScores;
            OnPopupInitalized -= _SetupPopup;
        }


        public static void _Init(PopupType popupType)
        {
            m_popupType = popupType;
            OnPopupInitalized?.Invoke();
        }
        public void OnClickSubmit()
        {
            if (m_popupType == PopupType.LevelCompleted && GlobalVariables.currentGameplayMode == GameplayType.LevelMode)
            {
                ScreenManager.Instance.ChangeScreen(Window.LevelCompleteScreeen, ScreenType.Replace);
                return;
            }
            GlobalEventHandler.RequestToPlayBGM?.Invoke(AudioID.DashboardBGM);
            ScreenManager.Instance.ChangeScreen(Window.Dashboard);
        }

        private void _SetupPopup()
        {
            switch (m_popupType)
            {
                case PopupType.LevelCompleted:
                    m_headerTxt.SetText(LEVEL_COMPLETED_TEXT);
                    if (GlobalVariables.currentGameplayMode == GameplayType.LevelMode)
                    {
                        GlobalVariables.CollectedStars += GameplayManager.CollectedStars;
                        PlayerDataManager.instance.SaveData();
                    }
                    break;
                case PopupType.GameOver:
                    m_headerTxt.SetText(GAME_OVER_TEXT);
                    break;
                case PopupType.TimeUp:
                    m_headerTxt.SetText(TIME_UP_TEXT);
                    break;
            }
        }
        private void _CalculateScores()
        {
            if (m_popupType == PopupType.LevelCompleted)
            {
                int totalMatchedFruitsCount = (int)(GlobalEventHandler.RequestTotalMatchedFruits?.Invoke());

                m_multiClearScore = (Konstants.REAL_ROW_SIZE * Konstants.REAL_COLUMN_SIZE) * Konstants.PAIR_MATCH_SCORE / 2;

                m_rowColumnClearScore += Konstants.REAL_ROW_SIZE * Konstants.ROW_CLEAR_BONUS;
                m_rowColumnClearScore += Konstants.REAL_COLUMN_SIZE * Konstants.COLUMN_CLEAR_BONUS;

                m_timeBonus = (int)GlobalEventHandler.RequestRemainingTimer?.Invoke();
                m_timeBonus *= Konstants.REMAINING_TIMER_PER_SECOND_BONUS;

                m_allClearBonus = Konstants.ENTIRE_BOARD_CLEAR_BONUS;

                m_finalScore = m_multiClearScore + m_rowColumnClearScore + m_allClearBonus + m_timeBonus;
            }
            _UpdateScoresToUI();
        }
        private void _UpdateScoresToUI()
        {
            GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.ScoreCountSFX);
            m_multiClearScoreTxt.DOText(m_multiClearScore.ToString(), m_scoreCalculatingTime).onComplete += () =>
            {
                GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.ScoreCountSFX);
                m_rowColumnClearScoreTxt.DOText(m_rowColumnClearScore.ToString(), m_scoreCalculatingTime).onComplete += () =>
                {
                    GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.ScoreCountSFX);
                    m_allClearBonusTxt.DOText(m_allClearBonus.ToString(), m_scoreCalculatingTime).onComplete += () =>
                    {
                        GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.ScoreCountSFX);
                        m_timeBonusTxt.DOText(m_timeBonus.ToString(), m_scoreCalculatingTime).onComplete += () =>
                        {

                            GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.FinalScoreCountSFX);
                            m_finalScoreTxt.DOText(m_finalScore.ToString(), m_scoreCalculatingTime).onComplete += () =>
                            {
                                m_submitBtn.transform.DOScale(1, .35f);
                                m_submitBtn.interactable = true;
                            };
                        };
                    };
                };
            };
        }

        public enum PopupType
        {
            LevelCompleted,//all the level is cleared.
            GameOver,//exit or timeup.
            TimeUp,
        }
    }

}