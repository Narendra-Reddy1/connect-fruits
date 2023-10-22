using UnityEngine;
using BenStudios.ScreenManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using GameAnalyticsSDK;

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
        [SerializeField] private Image m_separator;
        [SerializeField] private Image m_separator_2;

        [SerializeField] private GameObject m_timeBonusParent;
        [SerializeField] private GameObject m_rowColScorePanel;

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
            GlobalEventHandler.EventOnAdStateChanged += Callback_On_Ad_State_Changed;
        }
        private void OnDisable()
        {
            OnPopupInitalized -= _CalculateScores;
            OnPopupInitalized -= _SetupPopup;
            GlobalEventHandler.EventOnAdStateChanged -= Callback_On_Ad_State_Changed;
        }


        public static void _Init(PopupType popupType)
        {
            m_popupType = popupType;
            OnPopupInitalized?.Invoke();
        }
        public void OnClickSubmit()
        {
            if (CanShowAd())
                _SubmitAction();
            else
                GlobalEventHandler.RequestToShowInterstitial?.Invoke();

            bool CanShowAd()
            {
                return !(bool)GlobalEventHandler.Request_Interstitial_Availability?.Invoke() || GlobalVariables.highestUnlockedLevel < Konstants.MIN_LEVEL_TO_SHOW_INTERSTITIAL || GlobalVariables.highestUnlockedLevel % 3 != 0;
            }
        }
        private void _SubmitAction()
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
            int timer = (int)GlobalEventHandler.RequestRemainingTimer?.Invoke();
            switch (m_popupType)
            {
                case PopupType.LevelCompleted:
                    m_headerTxt.SetText(LEVEL_COMPLETED_TEXT);
                    if (GlobalVariables.currentGameplayMode == GameplayType.LevelMode)
                    {
                        GlobalVariables.CollectedStars += GameplayManager.CollectedStars;
                        if (PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_support_dev_popup_shown))
                            PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_support_dev_popup_shown, false);
                        PlayerDataManager.instance.SaveData();
                    }
                    #region Analytics

                    //GlobalEventHandler.RequestRecordEvent(AnalyticsEvent.LevelComplete, new ParameterBlock()
                    //{
                    //    parameters = new System.Collections.Generic.Dictionary<string, object>()
                    //    {
                    //        {"status",LevelCompleteStatus.Success },
                    //        {"level",GlobalVariables.highestUnlockedLevel },
                    //        {"remainingTime",timer},
                    //        //{"fruitBombsUsed",?? }
                    //        //{"tripleBombsUsed",?? }
                    //        //{"hintPowerupsUsed",?? }
                    //    }
                    //});

                    AnalyticsManager.instance.RecordLevelComplete(new ParameterBlock()
                    {
                        parameters = new System.Collections.Generic.Dictionary<string, object>()
                        {
                            {"status",LevelCompleteStatus.Success },
                            {"level",GlobalVariables.highestUnlockedLevel },
                            {"remainingTime",timer},
                            //{"fruitBombsUsed",?? }
                            //{"tripleBombsUsed",?? }
                            //{"hintPowerupsUsed",?? }
                        }
                    });


                    #endregion Analytics
                    break;
                case PopupType.GameOver:
                    m_headerTxt.SetText(GAME_OVER_TEXT);
                    #region Analytics
                    //GlobalEventHandler.RequestRecordEvent(AnalyticsEvent.LevelComplete, new ParameterBlock()
                    //{
                    //    parameters = new System.Collections.Generic.Dictionary<string, object>()
                    //    {
                    //        {"status",LevelCompleteStatus.Exited},
                    //        {"level",GlobalVariables.highestUnlockedLevel},
                    //        {"remainingTime",timer},
                    //    }
                    //});
                    AnalyticsManager.instance.RecordLevelComplete(new ParameterBlock()
                    {
                        parameters = new System.Collections.Generic.Dictionary<string, object>()
                        {
                            {"status",LevelCompleteStatus.Exited},
                            {"level",GlobalVariables.highestUnlockedLevel},
                            {"remainingTime",timer},
                        }
                    });
                    #endregion Analytics
                    break;
                case PopupType.TimeUp:
                    m_headerTxt.SetText(TIME_UP_TEXT);
                    #region Analytics

                    //GlobalEventHandler.RequestRecordEvent(AnalyticsEvent.LevelComplete, new ParameterBlock()
                    //{
                    //    parameters = new System.Collections.Generic.Dictionary<string, object>()
                    //    {
                    //        {"status",LevelCompleteStatus.TimeUp},
                    //        {"level",GlobalVariables.highestUnlockedLevel},
                    //        {"remainingTime",timer},
                    //    }
                    //});

                    AnalyticsManager.instance.RecordLevelComplete(new ParameterBlock()
                    {
                        parameters = new System.Collections.Generic.Dictionary<string, object>()
                        {
                            {"status",LevelCompleteStatus.TimeUp},
                            {"level",GlobalVariables.highestUnlockedLevel},
                            {"remainingTime",timer},
                        }
                    });
                    #endregion Analytics
                    break;
            }

            m_timeBonusParent.SetActive(CanShowThisScore());
            m_rowColScorePanel.SetActive(CanShowThisScore());
        }
        private bool CanShowThisScore()
        {
            return (GlobalVariables.highestUnlockedLevel >= Konstants.MIN_LEVEL_FOR_TIMER) || GlobalVariables.currentGameplayMode == GameplayType.ChallengeMode;
        }
        private void _CalculateScores()
        {
            if (m_popupType == PopupType.LevelCompleted)
            {
                int totalFruitsInTheLevel = (int)GlobalEventHandler.RequestTotalMatchedFruits?.Invoke();
                m_multiClearScore = totalFruitsInTheLevel * (Konstants.PAIR_MATCH_SCORE / 2);

                m_rowColumnClearScore += Konstants.REAL_ROW_SIZE * Konstants.ROW_CLEAR_BONUS;
                m_rowColumnClearScore += Konstants.REAL_COLUMN_SIZE * Konstants.COLUMN_CLEAR_BONUS;

                m_timeBonus = (int)GlobalEventHandler.RequestRemainingTimer?.Invoke();
                m_timeBonus *= Konstants.REMAINING_TIMER_PER_SECOND_BONUS;
                if ((GlobalVariables.highestUnlockedLevel <= Konstants.MIN_LEVEL_FOR_TIMER) && GlobalVariables.currentGameplayMode == GameplayType.LevelMode)
                    m_timeBonus = 0;
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
                            m_separator.DOFillAmount(1, m_scoreCalculatingTime);
                            m_separator_2.DOFillAmount(1, m_scoreCalculatingTime);
                            GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.FinalScoreCountSFX);
                            m_finalScoreTxt.DOText(m_finalScore.ToString(), m_scoreCalculatingTime).onComplete += () =>
                            {
                                m_submitBtn.transform.DOScale(1, .3f);
                                m_submitBtn.interactable = true;
                            };
                        };
                    };
                };
            };
        }

        private void Callback_On_Ad_State_Changed(AdEventData adEventData)
        {
            switch (adEventData.adState)
            {
                case AdState.INTERSTITIAL_DISMISSED:
                case AdState.INTERSTITIAL_FAILED_TO_DISPLAY:
                    _SubmitAction();
                    break;
            }
        }


        public enum PopupType
        {
            LevelCompleted,//all the level is cleared.
            GameOver,//exit or timeup.
            TimeUp,
        }
    }

}