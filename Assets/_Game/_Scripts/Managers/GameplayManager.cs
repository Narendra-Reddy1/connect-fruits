using BenStudios.ScreenManagement;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
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
        [SerializeField] private Transform m_blastEffect_1;
        [SerializeField] private Transform m_blastEffect_2;
        [SerializeField] private ParticleSystem m_blastEffectParticleSystem_1;
        [SerializeField] private ParticleSystem m_blastEffectParticleSystem_2;
        private FruitEntity[,] m_fruitEntityArray;
        [Tooltip("This list contains the fruit entities that are not destroyed only")]
        private List<FruitEntity> m_fruitEnityList = new List<FruitEntity>();
        private Stack<FruitEntity> m_selectedEntityStack = new Stack<FruitEntity>();
        private WaitForEndOfFrame _waitForEndOfTheFrame = new WaitForEndOfFrame();
        private WaitForSeconds delayToShowEntireBoardClearedEffect = new WaitForSeconds(1.5f);
        #endregion Variables

        #region Unity Methods
        private void OnEnable()
        {
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

        }
        private void OnDisable()
        {
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
        }
        private IEnumerator Start()
        {
            yield return _waitForEndOfTheFrame;
            GlobalVariables.currentGameState = GameState.Gameplay;
            _InitLevel();
            m_levelTimer.InitTimer(m_timerData.GetTimerData(TimerType.LevelTimer).timeInSeconds);
        }

        /* 
         //Debug Board Clear Effect....
          private void Update()
         {
             if (Input.GetKeyUp(KeyCode.L))
             {
                 for (int i = 0; i < Konstants.ROW_SIZE; i++)
                 {
                     for (int j = 0; j < Konstants.COLUMN_SIZE; j++)
                     {
                         m_fruitEntityArray[i, j].ShrinkAndDestroy();
                     }
                 }
             }
             if (Input.GetKeyUp(KeyCode.O))
             {
                 for (int i = 0; i < Konstants.ROW_SIZE; i++)
                 {
                     for (int j = 0; j < Konstants.COLUMN_SIZE; j++)
                     {
                         m_fruitEntityArray[i, j].ShowRowOrColumnGotClearedEffect();
                     }
                 }
             }
             if (Input.GetKeyUp(KeyCode.P))
             {
                 for (int i = 0; i < Konstants.ROW_SIZE; i++)
                 {
                     for (int j = 0; j < Konstants.COLUMN_SIZE; j++)
                     {
                         m_fruitEntityArray[i, j].ShowEntireBoardClearedEffect();
                     }
                 }
             }
         }*/

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
        private void _InitLevel()//MUST HAVE FRUIT PAIR CHECK
        {
            GlobalVariables.isBoardClearedNearToHalf = false;
            m_fruitEntityArray = null;
            m_fruitEntityArray = new FruitEntity[Konstants.ROW_SIZE, Konstants.COLUMN_SIZE];
            for (int i = 0; i < Konstants.ROW_SIZE; i++)
                for (int j = 0; j < Konstants.COLUMN_SIZE; j++)
                {
                    FruitEntity item = Instantiate(m_fruitEntityTemplate, m_gridParent);
                    int index = IsInvisibleCell(i, j) ? 0 : LevelData.leveldata[i - 1, j - 1];
                    item.Init(m_entityDatabase.fruitSprites[index], m_entityDatabase.outlineFruitSprites[index], index, i, j, IsInvisibleCell(i, j));
                    item.name = $"Fruit_{i}_{j}";
                    m_fruitEntityArray[i, j] = item;
                    if (!IsInvisibleCell(i, j))
                        m_fruitEnityList.Add(item);
                }
            foreach (FruitEntity entity in m_fruitEntityArray)
            {
                entity.SetupNeighbours(m_fruitEntityArray);
            }
            Debug.Log($"{m_fruitEntityArray.Length}");
        }
        private bool IsInvisibleCell(int row, int column)
        {
            return (row == 0 || column == 0 || row == Konstants.ROW_SIZE - 1 || column == Konstants.COLUMN_SIZE - 1);
        }
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
            if ((entity1.ID != entity2.ID) ||
                (entity1.ID != GlobalVariables.currentFruitCallId && entity2.ID != GlobalVariables.currentFruitCallId))
            {
                //Same Objects only matched
                entity1.ResetEntity();
                entity2.ResetEntity();
                if (GlobalVariables.isFruitBombInAction || GlobalVariables.isTripleBombInAction)
                {
                    _HighlightThePossibleFruitsForFruitBomb(entity1, false);
                    _HighlightThePossibleFruitsForFruitBomb(entity2, false);
                }
                _ShowFruitPairCantMatchAnimation(entity1, entity2, MatchFailedCause.FruitIDsAreNotSame);
                if (m_selectedEntityStack.Count > 0)
                    m_selectedEntityStack.Clear();
                return;
            }
            if (entity1.neighbours.Values.Contains(entity2))//Fruits are side by side
            {
                MyUtils.Log($"Neighbour FRUITS....");
                pathFound = true;
                optimizedPath.Add(new Vector2Int(entity1.Row, entity1.Column));
                optimizedPath.Add(new Vector2Int(entity2.Row, entity2.Column));
                // _SetupLinesToLinerender(0, entity1.RectTransform.localPosition);
                //_SetupLinesToLinerender(1, entity2.RectTransform.localPosition);
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
            {
                _OnMatchFoundWithPath(optimizedPath, entity1, entity2);
            }
            else
            {
                entity1.ResetEntity();
                entity2.ResetEntity();
                _ShowFruitPairCantMatchAnimation(entity1, entity2, MatchFailedCause.PathIsMoreThan3StraightLines);
                m_selectedEntityStack.Clear();
            }
            if (m_selectedEntityStack.Count > 0)
                m_selectedEntityStack.Clear();
        }

        private void _OnMatchFoundWithPath(List<Vector2Int> optimizedPath, FruitEntity entity1, FruitEntity entity2)
        {
            GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.MatchSuccessSFX);
            _AddAndUpdatePairMatchScore();
            GlobalEventHandler.OnFruitPairMatched?.Invoke(entity1.ID);
            _ShowMatchEffect(optimizedPath, entity1, entity2, () =>
            {
                ResetLineRenderer();
            });
        }

        private void _AddAndUpdatePairMatchScore()
        {
            int score = 0;
            score += m_fruitCallManager.GetQuickMatchBonusScore();
            score += Konstants.PAIR_MATCH_SCORE;
            GlobalEventHandler.RequestToUpdateScore?.Invoke(score);
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
            m_uiLineRenderer.Points = new Vector2[optimizedPath.Count];
            for (int i = 0, count = optimizedPath.Count; i < count; i++)
            {
                // m_fruitEntityArray[optimizedPath[i].x, optimizedPath[i].y].CanShowSelectedEffect(true);
                _SetupLinesToLinerender(i, m_fruitEntityArray[optimizedPath[i].x, optimizedPath[i].y].RectTransform.localPosition);
                m_uiLineRenderer.SetAllDirty();
            }
            m_blastEffect_1.position = item1.transform.position;
            m_blastEffect_2.position = item2.transform.position;
            m_blastEffectParticleSystem_1.gameObject.SetActive(true);
            m_blastEffectParticleSystem_2.gameObject.SetActive(true);
            m_blastEffectParticleSystem_1.Play(true);
            m_blastEffectParticleSystem_2.Play(true);
            item1.ShowMatchEffect(onComplete: () =>
            {
                m_fruitEnityList.Remove(item1);
                _CheckIfEntireRowOrColumnIsCleared(item1);
                ResetLineRenderer();
                onComplete?.Invoke();
            });
            item2.ShowMatchEffect(onComplete: () =>
            {
                m_fruitEnityList.Remove(item2);
                _CheckIfEntireRowOrColumnIsCleared(item2);
            });
            if (m_fruitEnityList.Count <= LevelData.leveldata.Length / 3)
                GlobalVariables.isBoardClearedNearToHalf = true;
        }
        private bool _CheckIfEntitesAreOnEdgeRowOrColumn(FruitEntity entity1, FruitEntity entity2)
        {
            return (entity1.Column == 0 && entity2.Column == 0) || (entity1.Row == 0 && entity2.Row == 0) ||
            (entity1.Column == Konstants.COLUMN_SIZE - 1 && entity2.Column == Konstants.COLUMN_SIZE - 1) || (entity1.Row == Konstants.ROW_SIZE - 1 && entity2.Row == Konstants.ROW_SIZE - 1);
        }

        private List<int> clearedRows = new List<int>();
        private List<int> clearedColumns = new List<int>();
        private void _CheckIfEntireRowOrColumnIsCleared(FruitEntity entity)
        {
            List<FruitEntity> rowList = _GetRowEntites(entity);
            List<FruitEntity> columnList = _GetColumnEntities(entity);
            byte delayCounter = 0;
            if (rowList.FindAll(x => x.IsDestroyed).Count == rowList.Count)
            {
                if (clearedRows.Contains(rowList[0].Row)) return;
                delayCounter = 0;
                ShowEffectOnEntity(rowList);
                GlobalEventHandler.RequestToUpdateScore(Konstants.ROW_CLEAR_BONUS);
                clearedRows.Add(rowList[0].Row);
                MyUtils.Log($"Row:: Clear Row effect:: {delayCounter}");
            }
            if (columnList.FindAll(x => x.IsDestroyed).Count == columnList.Count)
            {
                if (clearedColumns.Contains(rowList[0].Column)) return;
                delayCounter = 0;
                ShowEffectOnEntity(columnList);
                GlobalEventHandler.RequestToUpdateScore(Konstants.COLUMN_CLEAR_BONUS);
                clearedColumns.Add(columnList[0].Column);
                MyUtils.Log($"Column:: Clear Column effect:: {delayCounter}");
            }
            if (m_fruitEnityList.Count <= 0)
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
        private IEnumerator _ShowEntireBoardClearedEffect()
        {
            GlobalVariables.highestUnlockedLevel++;//next level
            GlobalEventHandler.RequestToDeactivatePowerUpMode?.Invoke();
            m_levelTimer.StopTimer();
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
            });
        }
        private IEnumerator _ShowTimeUpEffect()
        {
            GlobalEventHandler.RequestToScreenBlocker?.Invoke(true);
            m_fruitCallManager.ActiveFruitCall.PauseTimer();
            yield return delayToShowEntireBoardClearedEffect;
            ScreenManager.Instance.ChangeScreen(Window.ScoreBoardScreen, ScreenType.Additive, onComplete: () =>
            {
                GlobalEventHandler.RequestToScreenBlocker?.Invoke(false);
                ScoreBoardScreen._Init(ScoreBoardScreen.PopupType.GameOver);
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

        private void _HighlightThePossibleFruitsForFruitBomb(FruitEntity fruitEntity, bool canHighlight)
        {
            foreach (FruitEntity entity in m_fruitEnityList)
            {
                if (entity == fruitEntity) continue;
                if (entity.ID == fruitEntity.ID)
                    entity.HighlightForFruitBombSelection(canHighlight);
            }
        }
        private void _HighlightThePossibleFruitsForFruitBomb(FruitEntity fruitEntity, FruitEntity exceptionalEntity, bool canHighlight)
        {
            foreach (FruitEntity entity in m_fruitEnityList)
            {
                if (entity == fruitEntity) continue;
                if (entity.ID == fruitEntity.ID)
                    entity.HighlightForFruitBombSelection(canHighlight);
            }
        }
        private void _CheckForPossibleAutoMatch()
        {
            List<FruitEntity> sameIdEntities = m_fruitEnityList.FindAll(x => x.ID == GlobalVariables.currentFruitCallId);
            Vector2Int startCell = new Vector2Int();
            Vector2Int endCell = new Vector2Int();
            //FruitEntity possibleMatchEntity1 = null;
            //FruitEntity possibleMatchEntity2 = null;
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
                        //possibleMatchEntity1 = sameIdEntities[i];
                        //possibleMatchEntity2 = sameIdEntities[j];
                        _OnMatchFoundWithPath(optimizedPath, sameIdEntities[i], sameIdEntities[j]);
                        goto ENDOFTHEMETHOD;
                    }
                }
            }
        ENDOFTHEMETHOD:
            MyUtils.Log(string.Empty);

        }

        private int _GetRemainingSecondsInLevelTimer()
        {
            return m_levelTimer.GetRemaingTimeInSeconds();
        }

        private int _GetClearedFruitCount()
        {
            return ((Konstants.REAL_ROW_SIZE * Konstants.REAL_COLUMN_SIZE) - m_fruitEnityList.Count);
        }
        private Vector2Int m_clearedRowAndColumnCount;
        private Vector2Int GetClearedRowAndColumnCount()
        {
            m_clearedRowAndColumnCount.x = clearedRows.Count;
            m_clearedRowAndColumnCount.y = clearedColumns.Count;
            return m_clearedRowAndColumnCount;
        }
        #endregion Private Methods


        #region Callbacks

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
        private void Callback_On_Fruit_Bomb_Action_Requested(FruitEntity entity1, FruitEntity entity2)
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


        private void Callback_On_LevelStartup_Timer_Completed()
        {
            m_levelTimer.StartTimer();
        }
        private void Callback_On_Activate_Powerup_Mode_Requested(PowerupType powerupType)
        {
            m_levelTimer.StopTimer();
        }
        private void Callback_On_Deactivate_Powerup_Mode_Requested()
        {
            m_levelTimer.StartTimer();
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
        private void Callback_On_Level_Timer_Completed()
        {
            StartCoroutine(_ShowTimeUpEffect());
        }
        private void Callback_On_TimePauseRequested(bool canPause)
        {
            if (canPause)
            {
                m_levelTimer.StopTimer();
                m_fruitCallManager.ActiveFruitCall.PauseTimer();
            }
            else
            {
                m_levelTimer.StartTimer();
                m_fruitCallManager.ActiveFruitCall.ResumeTimer();
            }
        }
        #endregion Callbacks

        #region Deug PathFinding LinearAlgo
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
}