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


        private void OnEnable()
        {
            OnPopupInitalized += _CalculateScores;
        }
        private void OnDisable()
        {
            OnPopupInitalized -= _CalculateScores;
        }


        public static void _Init(PopupType popupType)
        {
            m_popupType = popupType;
            OnPopupInitalized?.Invoke();
        }
        public void OnClickSubmit()
        {
            ScreenManager.Instance.ChangeScreen(Window.Dashboard);
        }


        private void _CalculateScores()
        {
            int totalMatchedFruitsCount = (int)(GlobalEventHandler.RequestTotalMatchedFruits?.Invoke());
            m_multiClearScore = totalMatchedFruitsCount * Konstants.PAIR_MATCH_SCORE / 2;
            Vector2Int rowAndColumnCount = (Vector2Int)GlobalEventHandler.RequestClearedRowAndColumnCount?.Invoke();
            m_rowColumnClearScore += rowAndColumnCount.x * Konstants.ROW_CLEAR_BONUS;
            m_rowColumnClearScore += rowAndColumnCount.y * Konstants.COLUMN_CLEAR_BONUS;
            m_timeBonus = (m_popupType != PopupType.LevelCompleted) ? 0 : (int)GlobalEventHandler.RequestRemainingTimer?.Invoke();

            m_allClearBonus = (totalMatchedFruitsCount == (Konstants.REAL_ROW_SIZE * Konstants.REAL_COLUMN_SIZE)) ? Konstants.ENTIRE_BOARD_CLEAR_BONUS : 0;
            m_finalScore = m_multiClearScore + m_rowColumnClearScore + m_allClearBonus + m_timeBonus;
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
        }
    }

}