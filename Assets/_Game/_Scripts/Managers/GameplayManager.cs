using BenStudios.ScreenManagement;
using Coffee.UIExtensions;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace BenStudios
{
    public class GameplayManager : MonoBehaviour
    {


        #region Variables


        [SerializeField] private Transform m_gridParent;
        [SerializeField] private EntityDatabase m_entityDatabase;
        [SerializeField] private FruitEntity m_fruitEntityTemplate;
        [SerializeField] private UILineRenderer m_uiLineRenderer;
        [SerializeField] private LevelTimer m_levelTimer;
        [SerializeField] private TimerData m_timerData;
        [SerializeField] private FruitCallManager m_fruitCallManager;
        //[SerializeField] private Transform m_blastEffect_1;
        //[SerializeField] private Transform m_blastEffect_2;
        //[SerializeField] private ParticleSystem m_blastEffectParticleSystem_1;
        //[SerializeField] private ParticleSystem m_blastEffectParticleSystem_2;
        [SerializeField] private List<ParticleSystem> m_blastParticleSystemList;
        [Space(15)]
        [Header("Streak")]
        [SerializeField] private Image m_streakFillbar;
        [SerializeField] private UIParticleAttractor m_particleAttractor;
        [SerializeField] private TextMeshProUGUI m_streakCounterTxt;
        [SerializeField] private StarsParticleSystemManager m_starsParticleSystemManager;
        [Space(15)]
        [SerializeField] private TutorialHandler m_tutorialHandler;
        private FruitEntity[,] m_fruitEntityArray;
        [Tooltip("This list contains the fruit entities that are not destroyed only")]
        private List<FruitEntity> m_fruitEnityList = new List<FruitEntity>();
        private Stack<FruitEntity> m_selectedEntityStack = new Stack<FruitEntity>();
        private WaitForEndOfFrame _waitForEndOfTheFrame = new WaitForEndOfFrame();
        private WaitForSeconds delayToShowEntireBoardClearedEffect = new WaitForSeconds(1.5f);
        private float m_streakCounterTxtFontSize = 45;
        private byte _streakCounter = 0;
        private static short m_collectedStars = 0;
        private Vector2Int m_clearedRowAndColumnCount;
        private int m_totalFruitsInTheLevel = 0;
        public static short CollectedStars => m_collectedStars;

        #endregion Variables

        #region Unity Methods
        private void Awake()
        {
            _Init();
        }
        private void OnEnable()
        {
            m_particleAttractor.AddListnerToOnPartilceAttracted(Callback_On_Star_Particle_Attracted);
            ScreenManager.OnScreenChange += Callback_On_Screen_Changed;
            GlobalEventHandler.OnFruitEntitySelected += Callback_On_Fruit_Entity_Selected;
            GlobalEventHandler.OnFruitEntityUnSelected += Callback_On_Unselected_Fruit_Entity;
            GlobalEventHandler.OnLevelStartupTimerIsCompleted += Callback_On_LevelStartup_Timer_Completed;
            GlobalEventHandler.RequestToActivatePowerUpMode += Callback_On_Activate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToDeactivatePowerUpMode += Callback_On_Deactivate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToPerformFruitBombPowerupAction += Callback_On_Fruit_Bomb_Action_Requested;
            GlobalEventHandler.RequestToPerformTripleBombPowerupAction += Callback_On_Triple_Bomb_Action_Requested;
            GlobalEventHandler.RequestToFruitDumperPowerupAction += Callback_On_Fruit_Dumper_Action_Requested;
            GlobalEventHandler.RequestRemainingTimer += _GetRemainingSecondsInLevelTimer;
            GlobalEventHandler.RequestTotalMatchedFruits += _GetClearedFruitCount;
            GlobalEventHandler.OnLevelTimerIsCompleted += Callback_On_Level_Timer_Completed;
            GlobalEventHandler.RequestToPauseTimer += Callback_On_TimePauseRequested;
            GlobalEventHandler.RequestClearedRowAndColumnCount += GetClearedRowAndColumnCount;
            GlobalEventHandler.HintPowerupActionRequested += Callback_On_Hint_Action_Requested;
            GlobalEventHandler.OnPowerupTutorialCompleted += Callback_On_Powerup_Tutorial_Completed;
            GlobalEventHandler.RequestToCheckForPairIsAvailableForAutoMatch += Callback_On_Check_For_Auto_Match_Pair_Availability_Requested;

        }
        private void OnDisable()
        {
            m_particleAttractor.RemoveListenerToOnParticleAttracted(Callback_On_Star_Particle_Attracted);
            ScreenManager.OnScreenChange -= Callback_On_Screen_Changed;
            GlobalEventHandler.OnFruitEntitySelected -= Callback_On_Fruit_Entity_Selected;
            GlobalEventHandler.OnFruitEntityUnSelected -= Callback_On_Unselected_Fruit_Entity;
            GlobalEventHandler.OnLevelStartupTimerIsCompleted -= Callback_On_LevelStartup_Timer_Completed;
            GlobalEventHandler.RequestToActivatePowerUpMode -= Callback_On_Activate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToDeactivatePowerUpMode -= Callback_On_Deactivate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToPerformFruitBombPowerupAction -= Callback_On_Fruit_Bomb_Action_Requested;
            GlobalEventHandler.RequestToPerformTripleBombPowerupAction -= Callback_On_Triple_Bomb_Action_Requested;
            GlobalEventHandler.RequestToFruitDumperPowerupAction -= Callback_On_Fruit_Dumper_Action_Requested;
            GlobalEventHandler.RequestRemainingTimer -= _GetRemainingSecondsInLevelTimer;
            GlobalEventHandler.RequestTotalMatchedFruits -= _GetClearedFruitCount;
            GlobalEventHandler.RequestClearedRowAndColumnCount -= GetClearedRowAndColumnCount;
            GlobalEventHandler.OnLevelTimerIsCompleted -= Callback_On_Level_Timer_Completed;
            GlobalEventHandler.RequestToPauseTimer -= Callback_On_TimePauseRequested;
            GlobalEventHandler.HintPowerupActionRequested -= Callback_On_Hint_Action_Requested;
            GlobalEventHandler.OnPowerupTutorialCompleted -= Callback_On_Powerup_Tutorial_Completed;
            GlobalEventHandler.RequestToCheckForPairIsAvailableForAutoMatch -= Callback_On_Check_For_Auto_Match_Pair_Availability_Requested;
        }
        private IEnumerator Start()
        {
            yield return _waitForEndOfTheFrame;
            GlobalVariables.currentGameState = GameState.Gameplay;
            GlobalVariables.isLevelCompletedSuccessfully = false;
            _InitLevel();
            m_levelTimer.InitTimer(m_timerData.GetTimerData(TimerType.LevelTimer).timeInSeconds);
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.O))
            {
                StartCoroutine(_ShowEntireBoardClearedEffect());
            }

        }
#endif

        #endregion Unity Methods

        #region Public Methods
        public FruitEntity[,] GetFruitEntitiesOnTheBoard() => m_fruitEntityArray;
        public void ResetLineRenderer()
        {
            m_uiLineRenderer.Points = new Vector2[1] { Vector2.zero };
            m_uiLineRenderer.SetAllDirty();
        }
        #endregion Public Methods

        #region Private Methods

        #region Initializing

        private void _Init()
        {
            if (GlobalVariables.currentGameplayMode != GameplayType.LevelMode) return;
            m_levelTimer.gameObject.SetActive(GlobalVariables.highestUnlockedLevel >= Konstants.MIN_LEVEL_FOR_TIMER);
            m_collectedStars = 0;
            _streakCounter = 0;
            m_fruitCallManager.gameObject.SetActive(false);
            m_streakCounterTxt.gameObject.SetActive(true);
            m_streakCounterTxt.fontSizeMax = m_streakCounterTxtFontSize;
            m_streakCounterTxt.SetText(GlobalVariables.highestUnlockedLevel < Konstants.MIN_LEVEL_FOR_STREAK ? $"Unlocks at Level {Konstants.MIN_LEVEL_FOR_STREAK}" : $"X{_streakCounter + 1}");
        }
        int[,] m_currentLevelData;
        private void _InitLevel()//MUST HAVE FRUIT PAIR CHECK
        {
            GlobalVariables.isBoardClearedNearToHalf = false;
            m_fruitEntityArray = null;
            m_fruitEntityArray = new FruitEntity[Konstants.ROW_SIZE, Konstants.COLUMN_SIZE];
            m_currentLevelData = LevelData.GetLevelDataByIndex(GlobalVariables.highestUnlockedLevel);
            for (int i = 0; i < Konstants.ROW_SIZE; i++)
                for (int j = 0; j < Konstants.COLUMN_SIZE; j++)
                {
                    FruitEntity item = Instantiate(m_fruitEntityTemplate, m_gridParent);
                    int index = IsInvisibleCell(i, j) ? -1 : m_currentLevelData[i - 1, j - 1];
                    item.Init(m_entityDatabase.GetFruitSprite(index), m_entityDatabase.GetFruitOutlineSprite(index), index, i, j, IsInvisibleCell(i, j));
                    item.name = $"Fruit_{i}_{j}";
                    m_fruitEntityArray[i, j] = item;
                    if (index != -1)
                        m_fruitEnityList.Add(item);
                }
            foreach (FruitEntity entity in m_fruitEntityArray)
            {
                entity.SetupNeighbours(m_fruitEntityArray);
            }
            m_totalFruitsInTheLevel = m_fruitEnityList.Count;
            Debug.Log($"{m_fruitEntityArray.Length}");
        }

        #endregion Initializing

        #region StartLevel And Board Generation

        private void _StartLevelTimer()
        {
            if (GlobalVariables.highestUnlockedLevel <= Konstants.MIN_LEVEL_FOR_TIMER) return;
            m_levelTimer.StartTimer();
        }
        private bool IsInvisibleCell(int row, int column)
        {
            return (row == 0 || column == 0 || row == Konstants.ROW_SIZE - 1 || column == Konstants.COLUMN_SIZE - 1);
        }

        #endregion StartLevel And Board Generation
        #region OnBoarding
        private void _OnBoardPlayerIfPlayingFirstTime()
        {
            if (GlobalVariables.highestUnlockedLevel > 1) return;
            //AutoMatchData matchData = _CheckForPairToMatchAvailabilityAndReturnIfAvailable();
            // m_tutorialHandler.ShowLevelOnBoardingTutorial();


        }
        #endregion OnBoarding

        #region Pair Matching Logic

        private void _CheckForMatch()
        {
            /*
             check for id match
            if id is matched then check for path is within 3 straight lines
            **if path is within bounds then perform match operation.
            else show warning of more than three lines or not matched fruits.
             */

            ResetLineRenderer();
            bool pathFound = false;
            List<Vector2Int> optimizedPath = new List<Vector2Int>();
            FruitEntity entity2 = m_selectedEntityStack.Pop();
            FruitEntity entity1 = m_selectedEntityStack.Pop();
            if (entity1.ID == entity2.ID)
                if (GlobalVariables.isFruitBombInAction)
                {
                    GlobalEventHandler.RequestToPerformFruitBombPowerupAction?.Invoke(entity1, entity2);
                    return;
                }
                else if (GlobalVariables.isTripleBombInAction)
                {
                    GlobalEventHandler.RequestToPerformTripleBombPowerupAction.Invoke(entity1, entity2);
                    return;
                }

            bool areSelectedEntitesAreSame = (entity1.ID == entity2.ID);
            switch (GlobalVariables.currentGameplayMode)
            {
                case GameplayType.LevelMode:
                    if (!areSelectedEntitesAreSame)
                    {
                        ResetUnMatchedEntites(MatchFailedCause.FruitIDsAreNotSame);
                        return;
                    }
                    break;
                case GameplayType.ChallengeMode:
                    if (!areSelectedEntitesAreSame ||
               (entity1.ID != GlobalVariables.currentFruitCallId && entity2.ID != GlobalVariables.currentFruitCallId))
                    {
                        //Same Objects only matched
                        ResetUnMatchedEntites(MatchFailedCause.FruitIDsAreNotSame);
                        return;
                    }
                    break;
            }
            if (entity1.neighbours.Values.Contains(entity2))//Fruits are side by side
            {
                MyUtils.Log($"Neighbour FRUITS....");
                pathFound = true;
                optimizedPath.Add(new Vector2Int(entity1.Row, entity1.Column));
                optimizedPath.Add(new Vector2Int(entity2.Row, entity2.Column));
            }
            else ///ADD 3 Straight Line Constraint HERE
            {

                MyUtils.Log($"PathFinding Condition");
                Vector2Int startCell = new Vector2Int(entity1.Row, entity1.Column);
                Vector2Int endCell = new Vector2Int(entity2.Row, entity2.Column);
                optimizedPath = _GetValidAndEfficientPath(PathFinding.FindPossiblePathsLinearAlgo(startCell, endCell), startCell, endCell);
                if (optimizedPath != null)
                    pathFound = true;
            }
            if (pathFound)
                _OnMatchFoundWithPath(optimizedPath, entity1, entity2);
            else
                ResetUnMatchedEntites(MatchFailedCause.PathIsMoreThan3StraightLines);

            ClearSelectedStack();

            void ResetUnMatchedEntites(MatchFailedCause failedCause)
            {
                if (GlobalVariables.isFruitBombInAction || GlobalVariables.isTripleBombInAction)
                {
                    _HighlightThePossibleFruitsForFruitBomb(entity1, false);
                    _HighlightThePossibleFruitsForFruitBomb(entity2, false);
                }
                entity1.ResetEntity();
                entity2.ResetEntity();
                _ShowFruitPairCantMatchAnimation(entity1, entity2, MatchFailedCause.PathIsMoreThan3StraightLines);
                ClearSelectedStack();
            }
            void ClearSelectedStack()
            {
                if (m_selectedEntityStack.Count > 0)
                    m_selectedEntityStack.Clear();
            }
        }
        private void _OnMatchFoundWithPath(List<Vector2Int> optimizedPath, FruitEntity entity1, FruitEntity entity2)
        {
            GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.MatchSuccessSFX);
            _AddAndUpdatePairMatchScore();
            GlobalEventHandler.OnFruitPairMatched?.Invoke(entity1.ID);
            if (GlobalVariables.currentGameplayMode == GameplayType.LevelMode)
                _OnNewMatchMade();//to show star multiplier fill effect;
            _ShowMatchEffect(optimizedPath, entity1, entity2);
        }
        private void _ShowFruitPairCantMatchAnimation(FruitEntity entity1, FruitEntity entity2, MatchFailedCause matchFailedCause)
        {
            GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.MatchFailedSFX);
            entity1.transform.DOShakeRotation(0.3f, 15f);
            entity2.transform.DOShakeRotation(0.3f, 15f);
            GlobalEventHandler.OnFruitPairMatchFailed?.Invoke(matchFailedCause);
        }
        private void _ShowMatchEffect(List<Vector2Int> optimizedPath, FruitEntity item1, FruitEntity item2, System.Action onComplete = null)
        {
            _DrawLinedPath(optimizedPath);
            _ShowBlastParticleEffect(item1, item2);

            //Removing items
            m_fruitEnityList.Remove(item1);
            m_fruitEnityList.Remove(item2);

            item1.ShowMatchEffect(onComplete: () =>
            {
                _CheckIfEntireRowOrColumnIsCleared(item1);
                ResetLineRenderer();
            });
            item2.ShowMatchEffect(onComplete: () =>
            {
                _CheckIfEntireRowOrColumnIsCleared(item2);
                onComplete?.Invoke();
            });
            GlobalEventHandler.RequestToCheckForPairIsAvailableForAutoMatch?.Invoke();
            if (m_fruitEnityList.Count <= (Konstants.REAL_ROW_SIZE * Konstants.REAL_COLUMN_SIZE) / 3)
                GlobalVariables.isBoardClearedNearToHalf = true;
        }
        private void _ShowBlastParticleEffect(FruitEntity item1, FruitEntity item2)
        {
            _PlayBlastEffect(item1);
            _PlayBlastEffect(item2);

            //ParticleSystem blastEffect_1 = _GetIdleBlastParticleSystem();
            //blastEffect_1.transform.parent.position = item1.transform.position;
            //blastEffect_1.Play(true);
            //ParticleSystem blastEffect_2 = _GetIdleBlastParticleSystem();
            //blastEffect_2.transform.parent.position = item2.transform.position;
            //blastEffect_2.Play(true);

            // m_blastEffectParticleSystem_1.gameObject.SetActive(true);
            // m_blastEffectParticleSystem_2.gameObject.SetActive(true);
        }
        private void _PlayBlastEffect(FruitEntity item)
        {
            ParticleSystem blastEffect = _GetIdleBlastParticleSystem();
            if (blastEffect == null) blastEffect = _GetRandomBlastEffect();
            blastEffect.transform.parent.position = item.transform.position;
            blastEffect.Play(true);
        }
        private ParticleSystem _GetRandomBlastEffect()
        {
            return m_blastParticleSystemList[Random.Range(0, m_blastParticleSystemList.Count)];
        }
        private ParticleSystem _GetIdleBlastParticleSystem()
        {
            return m_blastParticleSystemList.Find(x => !x.IsAlive());
        }
        private void _DrawLinedPath(List<Vector2Int> pathData)
        {
            m_uiLineRenderer.Points = new Vector2[pathData.Count];
            for (int i = 0, count = pathData.Count; i < count; i++)
            {
                // m_fruitEntityArray[optimizedPath[i].x, optimizedPath[i].y].CanShowSelectedEffect(true);
                _SetupLinesToLinerender(i, m_fruitEntityArray[pathData[i].x, pathData[i].y].transform.localPosition);
                m_uiLineRenderer.SetAllDirty();
            }
        }
        private List<Vector2Int> _GetValidAndEfficientPath(List<List<Vector2Int>> paths, Vector2Int startCell, Vector2Int endCell)
        {
            byte counter = 0;
            List<List<Vector2Int>> eligiblePath = new List<List<Vector2Int>>();
            foreach (List<Vector2Int> path in paths)//Loop to find valid Paths.
            {
                counter = 2;
                foreach (Vector2Int point in path)
                {
                    if (point == startCell || point == endCell) continue;
                    if (!m_fruitEntityArray[point.x, point.y].IsDestroyed)
                    {
                        counter = 0;
                        break;
                    }
                    counter++;
                    if (counter == path.Count) eligiblePath.Add(path);
                }
            }
            if (eligiblePath.Count > 0)
                return eligiblePath[0];
            else
                return null;
        }
        private void _SetupLinesToLinerender(int index, Vector2 position)
        {
            m_uiLineRenderer.Points[index] = new Vector2(position.x, position.y);
        }

        private void _CheckIfEntireRowOrColumnIsCleared(FruitEntity entity)
        {
            List<FruitEntity> rowList = _GetRowEntites(entity);
            List<FruitEntity> columnList = _GetColumnEntities(entity);
            byte delayCounter = 0;
            if (rowList.FindAll(x => x.IsDestroyed).Count == rowList.Count)
            {
                if (clearedRows.Contains(rowList[0].Row)) return;
                delayCounter = 0;
                clearedRows.Add(rowList[0].Row);
                GlobalEventHandler.OnRowCleared?.Invoke();
                GlobalEventHandler.RequestToUpdateScore(Konstants.ROW_CLEAR_BONUS);
                ShowEffectOnEntity(rowList);
            }
            if (columnList.FindAll(x => x.IsDestroyed).Count == columnList.Count)
            {
                if (clearedColumns.Contains(rowList[0].Column)) return;
                delayCounter = 0;
                GlobalEventHandler.RequestToUpdateScore(Konstants.COLUMN_CLEAR_BONUS);
                clearedColumns.Add(columnList[0].Column);
                GlobalEventHandler.OnColumnCleared?.Invoke();
                ShowEffectOnEntity(columnList);
            }
            MyUtils.Log($"## Total Fruit count On the board::{m_fruitEnityList.Count}:: ClearedColumns:{clearedColumns.Count} ==> ClearedRowCount::{clearedRows.Count}");
            if ((m_fruitEnityList.Count <= 0) /*|| (clearedRows.Count == Konstants.REAL_ROW_SIZE && clearedColumns.Count == Konstants.REAL_COLUMN_SIZE)*/)
            {
                StartCoroutine(_ShowEntireBoardClearedEffect());
            }
            void ShowEffectOnEntity(List<FruitEntity> entityList)
            {
                entityList.ForEach(x =>
                {
                    delayCounter++;
                    x.ShowRowOrColumnGotClearedEffect(delayCounter * 0.2f); ;
                });
            }
        }


        #endregion Pair Matching Logic

        #region Challenge Mode
        private IEnumerator _ShowTimeUpEffect()
        {
            GlobalEventHandler.RequestToScreenBlocker?.Invoke(true);
            if (GlobalVariables.currentGameplayMode == GameplayType.ChallengeMode)
                m_fruitCallManager.ActiveFruitCall.PauseTimer();
            yield return delayToShowEntireBoardClearedEffect;
            ScreenManager.Instance.ChangeScreen(Window.OutOfTimePopup, ScreenType.Additive, false,
                () =>
                {
                    GlobalEventHandler.RequestToScreenBlocker?.Invoke(false);
                });
            //ScreenManager.Instance.ChangeScreen(Window.ScoreBoardScreen, ScreenType.Additive, onComplete: () =>
            //{
            //    GlobalEventHandler.RequestToScreenBlocker?.Invoke(false);
            //    ScoreBoardScreen._Init(ScoreBoardScreen.PopupType.TimeUp);
            //});
        }
        #endregion Challenge Mode

        #region LevelComplete

        private IEnumerator _ShowEntireBoardClearedEffect()
        {
            if (GlobalVariables.isLevelCompletedSuccessfully) yield break;
            GlobalEventHandler.RequestToScreenBlocker?.Invoke(true);
            GlobalVariables.isLevelCompletedSuccessfully = true;
            GlobalVariables.highestUnlockedLevel++;//next level
            GlobalEventHandler.RequestToDeactivatePowerUpMode?.Invoke();
            m_levelTimer.StopTimer();
            if (GlobalVariables.currentGameplayMode == GameplayType.ChallengeMode)
                m_fruitCallManager.ActiveFruitCall.PauseTimer();
            yield return delayToShowEntireBoardClearedEffect;
            for (int j = 0; j < Konstants.COLUMN_SIZE; j++)
                for (int i = 0; i < Konstants.ROW_SIZE; i++)
                {
                    m_fruitEntityArray[i, j].ShowEntireBoardClearedEffect(0.1f * j);
                }
            yield return delayToShowEntireBoardClearedEffect;
            yield return delayToShowEntireBoardClearedEffect;
            ScreenManager.Instance.ChangeScreen(Window.ScoreBoardScreen, ScreenType.Additive, onComplete: () =>
            {
                ScoreBoardScreen._Init(ScoreBoardScreen.PopupType.LevelCompleted);
                GlobalEventHandler.RequestToScreenBlocker?.Invoke(false);
            });
        }

        private List<FruitEntity> _GetRowEntites(FruitEntity entity)
        {
            List<FruitEntity> rowElements = new List<FruitEntity>();
            for (int i = 0, count = Konstants.COLUMN_SIZE; i < count; i++)
                rowElements.Add(m_fruitEntityArray[entity.Row, i]);
            return rowElements;
        }

        private List<FruitEntity> _GetColumnEntities(FruitEntity entity)
        {
            List<FruitEntity> columnElements = new List<FruitEntity>();
            for (int i = 0, count = Konstants.ROW_SIZE; i < count; i++)
                columnElements.Add(m_fruitEntityArray[i, entity.Column]);
            return columnElements;

        }


        #endregion LevelComplete

        #region Powerups
        private void _CheckForPowerupUnlockAndShowTutorial()
        {
            if ((GlobalVariables.highestUnlockedLevel >= Konstants.HINT_POWERUP_UNLOCK_LEVEL) && !PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_hint_powerup_tutorial_shown)) m_tutorialHandler.ShowPowerupTutorial(PowerupType.Hint);
            if ((GlobalVariables.highestUnlockedLevel >= Konstants.FRUIT_BOMB_UNLOCK_LEVEL) && !PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_fruit_bomb_tutorial_shown))
                m_tutorialHandler.ShowPowerupTutorial(PowerupType.FruitBomb);
            if ((GlobalVariables.highestUnlockedLevel >= Konstants.TRIPLE_BOMB_UNLOCK_LEVEL) && !PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_triple_bomb_tutorial_shown))
                m_tutorialHandler.ShowPowerupTutorial(PowerupType.TripleBomb);
        }

        private void _HighlightThePossibleFruitsForFruitBomb(FruitEntity fruitEntity, bool canHighlight)
        {
            foreach (FruitEntity entity in m_fruitEnityList)
            {
                if (entity == fruitEntity) continue;
                if (entity.ID == fruitEntity.ID)
                    entity.HighlightFruitntity(canHighlight);
            }
        }

        private void _CheckForPossibleAutoMatch()
        {
            if (GlobalVariables.currentGameplayMode == GameplayType.LevelMode) return;
            AutoMatchData autoMatchData = _GetAutoMatchDataForID(GlobalVariables.currentFruitCallId);
            if (!autoMatchData.Equals(default(AutoMatchData)))
                _OnMatchFoundWithPath(autoMatchData.path, autoMatchData.startCell, autoMatchData.endCell);
        }


        private void _PerformHintPowerupAction()
        {
            MyUtils.Log($"Hint Powerup Action From gameplayMAnager");
            //OnComplete.....
            GlobalEventHandler.RequestToScreenBlocker?.Invoke(false);
            AutoMatchData autoMatchData = _CheckForPairToMatchAvailabilityAndReturnIfAvailable();
            if (!autoMatchData.Equals(default(AutoMatchData)))
                _OnMatchFoundWithPath(autoMatchData.path, autoMatchData.startCell, autoMatchData.endCell);
        }

        #endregion Powerups

        #region Streak Logic
        private void _StartFillingDown()
        {
            DOTween.Kill(m_streakFillbar);
            float remainingFillbar = m_streakFillbar.fillAmount * 100;
            if (remainingFillbar >= 65)
                GlobalEventHandler.RequestToShowGoodMatchText?.Invoke();
            m_streakFillbar.DOFillAmount(1, 0.15f).onComplete += () =>
            {
                m_streakCounterTxt.transform.DOPunchScale(Vector3.one * .2f, .2f, 1).SetEase(Ease.Linear);
                m_streakFillbar.DOFillAmount(0, _GetStreakTimer(_streakCounter)).SetUpdate(true).onComplete += () =>
                {
                    _ResetMatchMultiplier();
                };
            };
            m_streakCounterTxt.SetText($"X{_streakCounter + 1}");
        }
        //Hardcoded values for faster shipping 
        //Replace it with appropriate formula later!!
        private float _GetStreakTimer(byte streakCounter)
        {
            if (streakCounter <= 5) return Konstants.DEFAULT_STAR_MULTIPLIER_TIMER_IN_SECONDS;
            if (streakCounter <= 10) return Konstants.DEFAULT_STAR_MULTIPLIER_TIMER_IN_SECONDS - Konstants.STAR_MULTIPLIER_DECAY_RATE;
            else
                return Konstants.STAR_MULTIPLIER_LOW_CAP_TIMER;
        }

        private void _ResetMatchMultiplier()
        {
            _streakCounter = 0;
            DOTween.Kill(m_streakFillbar);
            m_streakFillbar.fillAmount = 0;
            m_streakCounterTxt.SetText($"X{_streakCounter + 1}");
        }
        private void _OnNewMatchMade()
        {
            _RestartTimerForPairHint();
            m_tutorialHandler.ClosePlayerStuckMessage();
            if (GlobalVariables.highestUnlockedLevel < Konstants.MIN_LEVEL_FOR_STREAK) return;
            _streakCounter++;
            m_starsParticleSystemManager.SetupAndEmitParticles(_streakCounter);
            _StartFillingDown();
        }

        #endregion Streak Logic

        #region Available Match Hint Logic 
        AutoMatchData _availablePairMatchHintData = default;
        private int _pairHintTimerCounter = 0;
        private bool _isPairHintAnimationActive = false;
        private void _ShowAvailablePairToMatch()
        {
            if (GlobalVariables.isLevelCompletedSuccessfully || GlobalVariables.isTripleBombInAction || GlobalVariables.isFruitBombInAction) return;
            _availablePairMatchHintData = _CheckForPairToMatchAvailabilityAndReturnIfAvailable();

            if (!_availablePairMatchHintData.Equals(default(AutoMatchData)) && (GlobalVariables.highestUnlockedLevel < Konstants.MAX_LEVEL_TO_SHOW_PAIR_HINT))
            {
                _isPairHintAnimationActive = true;
                _availablePairMatchHintData.startCell.HighlightFruitntity(true);
                _availablePairMatchHintData.endCell.HighlightFruitntity(true);
                _DrawLinedPath(_availablePairMatchHintData.path);
            }
            else
            {
                if (GlobalVariables.highestUnlockedLevel > Konstants.FRUIT_BOMB_UNLOCK_LEVEL)
                    m_tutorialHandler.ShowPlayerStuckMessageToUsePowerup(PowerupType.FruitBomb);
            }
        }
        private void _RestartTimerForPairHint()
        {
            CancelInvoke(nameof(_Tick));
            _DisableHighlightedFruitForPairHint();
            _pairHintTimerCounter = Konstants.TIME_TO_WAIT_TO_SHOW_AVAILABLE_PAIR_TO_MATCH;
            InvokeRepeating(nameof(_Tick), 1, 1);
        }
        private void _StopTimerForPairHint()
        {
            CancelInvoke(nameof(_Tick));
            _DisableHighlightedFruitForPairHint();
        }
        private void _Tick()
        {
            _pairHintTimerCounter--;
            if (_pairHintTimerCounter <= 0)
            {
                _pairHintTimerCounter = 0;
                CancelInvoke(nameof(_Tick));
                _ShowAvailablePairToMatch();
            }
        }
        private void _DisableHighlightedFruitForPairHint()
        {
            if (_isPairHintAnimationActive)
            {
                _isPairHintAnimationActive = false;
                _availablePairMatchHintData.startCell.HighlightFruitntity(false);
                _availablePairMatchHintData.endCell.HighlightFruitntity(false);
            }
        }


        #endregion Available Match Hint Logic

        private void _AddAndUpdatePairMatchScore()
        {
            int score = 0;
            if (GlobalVariables.currentGameplayMode == GameplayType.ChallengeMode)
                score += m_fruitCallManager.GetQuickMatchBonusScore();
            score += Konstants.PAIR_MATCH_SCORE;
            GlobalEventHandler.RequestToUpdateScore?.Invoke(score);
        }

        private bool _CheckIfEntitesAreOnEdgeRowOrColumn(FruitEntity entity1, FruitEntity entity2)
        {
            return (entity1.Column == 0 && entity2.Column == 0) || (entity1.Row == 0 && entity2.Row == 0) ||
            (entity1.Column == Konstants.COLUMN_SIZE - 1 && entity2.Column == Konstants.COLUMN_SIZE - 1) || (entity1.Row == Konstants.ROW_SIZE - 1 && entity2.Row == Konstants.ROW_SIZE - 1);
        }

        private List<int> clearedRows = new List<int>();
        private List<int> clearedColumns = new List<int>();

        private void _HighlightThePossibleFruitsForFruitBomb(FruitEntity fruitEntity, FruitEntity exceptionalEntity, bool canHighlight)
        {
            foreach (FruitEntity entity in m_fruitEnityList)
            {
                if (entity == fruitEntity) continue;
                if (entity.ID == fruitEntity.ID)
                    entity.HighlightFruitntity(canHighlight);
            }
        }

        private int _GetRemainingSecondsInLevelTimer()
        {
            return m_levelTimer.GetRemaingTimeInSeconds();
        }

        private int _GetClearedFruitCount()
        {
            return m_totalFruitsInTheLevel - m_fruitEnityList.Count;
        }
        private Vector2Int GetClearedRowAndColumnCount()
        {
            m_clearedRowAndColumnCount.x = clearedRows.Count;
            m_clearedRowAndColumnCount.y = clearedColumns.Count;
            return m_clearedRowAndColumnCount;
        }


        private AutoMatchData _CheckForPairToMatchAvailabilityAndReturnIfAvailable()
        {
            int id;
            AutoMatchData autoMatchData = default;
            for (int i = 0, count = m_fruitEnityList.Count; i < count; i++)
            {
                id = m_fruitEnityList[i].ID;
                autoMatchData = _GetAutoMatchDataForID(id);
                if (!autoMatchData.Equals(default(AutoMatchData)))
                    break;
            }
            return autoMatchData;
        }
        private AutoMatchData _GetAutoMatchDataForID(int id)
        {
            AutoMatchData autoMatchData = default;
            List<FruitEntity> sameIdEntities = m_fruitEnityList.FindAll(x => x.ID == id);
            Vector2Int startCell = new Vector2Int();
            Vector2Int endCell = new Vector2Int();
            for (int i = 0, count = sameIdEntities.Count; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (i == j) continue;
                    startCell = new Vector2Int(sameIdEntities[i].Row, sameIdEntities[i].Column);
                    endCell = new Vector2Int(sameIdEntities[j].Row, sameIdEntities[j].Column);

                    List<Vector2Int> optimizedPath = _GetValidAndEfficientPath(PathFinding.FindPossiblePathsLinearAlgo(startCell, endCell), startCell, endCell);
                    if (optimizedPath != null)
                    {
                        autoMatchData.path = optimizedPath;
                        autoMatchData.startCell = sameIdEntities[i];
                        autoMatchData.endCell = sameIdEntities[j];
                        GlobalEventHandler.EventOnPairIsAvailableForHintPowerup?.Invoke();
                        goto ENDOFTHEMETHOD;
                    }
                }
            }
            MyUtils.Log($"NO PAIR IS AVAILABLE FOR AUTO MATCH");
            GlobalEventHandler.EventOnNoPairIsAvailableForHintPowerup?.Invoke();
        ENDOFTHEMETHOD:
            return autoMatchData;
        }
        public struct AutoMatchData
        {
            public List<Vector2Int> path;
            public FruitEntity startCell;
            public FruitEntity endCell;
        }


        #endregion Private Methods


        #region Callbacks


        #region Gameplay
        private void Callback_On_Fruit_Entity_Selected(FruitEntity selectedEntity)
        {
            m_selectedEntityStack.Push(selectedEntity);
            if (m_selectedEntityStack.Count == 1 && (GlobalVariables.isFruitBombInAction || GlobalVariables.isTripleBombInAction))
            {
                _HighlightThePossibleFruitsForFruitBomb(selectedEntity, true);
            }
            if (m_selectedEntityStack?.Count >= Konstants.MIN_FRUITS_TO_MATCH)
                _CheckForMatch();
        }
        private void Callback_On_Unselected_Fruit_Entity(FruitEntity unSelectedEntity)
        {
            if (GlobalVariables.isFruitBombInAction || GlobalVariables.isTripleBombInAction)
            {
                _HighlightThePossibleFruitsForFruitBomb(unSelectedEntity, false);
            }
            if (m_selectedEntityStack.Count > 0)
                m_selectedEntityStack?.Pop();
        }
        private void Callback_On_LevelStartup_Timer_Completed()
        {
            _StartLevelTimer();
            _OnBoardPlayerIfPlayingFirstTime();
            _CheckForPowerupUnlockAndShowTutorial();
            _RestartTimerForPairHint();
        }

        private void Callback_On_Level_Timer_Completed()
        {
            StartCoroutine(_ShowTimeUpEffect());
        }
        private void Callback_On_TimePauseRequested(bool canPause)
        {
            if (canPause)
            {
                m_levelTimer.StopTimer();
                if (GlobalVariables.currentGameplayMode == GameplayType.ChallengeMode)
                    m_fruitCallManager.ActiveFruitCall.PauseTimer();
            }
            else
            {
                _StartLevelTimer();
                if (GlobalVariables.currentGameplayMode == GameplayType.ChallengeMode)
                    m_fruitCallManager.ActiveFruitCall.ResumeTimer();
            }
        }

        private void Callback_On_Check_For_Auto_Match_Pair_Availability_Requested()
        {
            _CheckForPairToMatchAvailabilityAndReturnIfAvailable();
        }

        #endregion Gameplay

        #region Powerups
        private void Callback_On_Powerup_Tutorial_Completed()
        {
            _StartLevelTimer();
        }

        private void Callback_On_Hint_Action_Requested()
        {
            _PerformHintPowerupAction();
        }
        private void Callback_On_Fruit_Dumper_Action_Requested()
        {
            List<FruitEntity> list = new List<FruitEntity>();
            int indexToRemove = -1;
            foreach (int index in m_fruitCallManager.fruitCallIndices)
            {
                list = m_fruitEnityList.FindAll(x => x.ID == index);
                if (list.Count <= 0)
                    indexToRemove = index;
            }
            if (indexToRemove == -1)
            {
                //Nothing to remvoe////
                MyUtils.Log($"NOTHING TO DUMP::::");
            }
            else
            {
                MyUtils.Log($"{indexToRemove} will be dumped");
                m_fruitCallManager.fruitCallIndices.Remove(indexToRemove);
            }
            GlobalVariables.dumpedFruitIndex = indexToRemove;
            MyUtils.DelayedCallback(1.5f, () =>
            {
                GlobalEventHandler.RequestToDeactivatePowerUpMode?.Invoke();
                GlobalEventHandler.RequestToScreenBlocker?.Invoke(false);
            });
        }
        private void Callback_On_Fruit_Bomb_Action_Requested(FruitEntity entity1, FruitEntity entity2)
        {
            m_tutorialHandler.ClosePlayerStuckMessage();
            m_fruitEnityList.Remove(entity1);
            m_fruitEnityList.Remove(entity2);
            _HighlightThePossibleFruitsForFruitBomb(entity1, false);
            _CheckForPossibleAutoMatch();
            MyUtils.DelayedCallback(1f, () =>
            {
                GlobalEventHandler.RequestToUpdateScore?.Invoke(Konstants.PAIR_MATCH_SCORE);
                _CheckIfEntireRowOrColumnIsCleared(entity1);
                _CheckIfEntireRowOrColumnIsCleared(entity2);
            });
        }
        private void Callback_On_Triple_Bomb_Action_Requested(FruitEntity entity1, FruitEntity entity2)
        {
            m_fruitEnityList.Remove(entity1);
            m_fruitEnityList.Remove(entity2);
            _HighlightThePossibleFruitsForFruitBomb(entity1, false);
            _CheckForPossibleAutoMatch();
            MyUtils.DelayedCallback(1f, () =>
            {
                GlobalEventHandler.RequestToUpdateScore?.Invoke(Konstants.PAIR_MATCH_SCORE);
                _CheckIfEntireRowOrColumnIsCleared(entity1);
                _CheckIfEntireRowOrColumnIsCleared(entity2);
            });
        }
        private void Callback_On_Activate_Powerup_Mode_Requested(PowerupType powerupType)
        {
            // m_tutorialHandler.ClosePowerUpTutorial();
            m_levelTimer.StopTimer();
        }
        private void Callback_On_Deactivate_Powerup_Mode_Requested()
        {
            _StartLevelTimer();
        }


        #endregion Powerups

        #region Streak

        private void Callback_On_Star_Particle_Attracted()
        {
            m_collectedStars++;
            GlobalEventHandler.EventOnStarParticleAttracted?.Invoke(m_collectedStars);
        }

        #endregion Streak

        private void Callback_On_Screen_Changed(Window window)
        {
            if (window == Window.StoreScreen) m_tutorialHandler.ClosePlayerStuckMessage();
        }
        #endregion Callbacks



        #region Deug PathFinding LinearAlgo

#if DEVLOPMENT_BUILD || DEBUG_DEFINE

        [DebugButton("DebugTimeUp")]
        public void TimeUP()
        {
            GlobalEventHandler.RequestToPauseTimer?.Invoke(true);
            ScreenManager.Instance.ChangeScreen(Window.OutOfTimePopup, ScreenType.Additive, false);
        }
#endif

        //[Space(100)]
        //public Vector2Int startCell;
        //public Vector2Int endCell;

        //[ContextMenu("Test Paths")]
        //public void TestPathFinding()
        //{
        //    List<List<Vector2Int>> paths = TESTSCRIPT.FindPossiblePathsLinearAlgo(startCell, endCell);
        //    for (int i = 0, count = paths.Count; i < count; i++)
        //    {
        //        MyUtils.Log($"________________________________________________________________________________________________________________________");
        //        for (int j = 0, max = paths[i].Count; j < max; j++)
        //        {
        //            MyUtils.Log($"{paths[i][j]}");
        //        }
        //    }
        //}
        #endregion Deug PathFinding




    }



    public enum MatchFailedCause
    {
        FruitIDsAreNotSame,
        PathIsMoreThan3StraightLines,
    }
    public enum GameState
    {
        HomeScreen,
        Gameplay,
    }
    public enum GameplayType
    {
        LevelMode,
        ChallengeMode
    }

}