using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace BenStudios
{
    public class FruitCallManager : MonoBehaviour
    {

        #region Variables

        [SerializeField] private GameObject m_fruitCallTemplate;

        [SerializeField] private Transform m_activeFruitCallHolder;
        [SerializeField] private Transform m_previousFruitCallHolder;
        [SerializeField] private Transform m_ancientFruitCallHolder;

        [SerializeField] private EntityDatabase m_entityDatabase;
        [SerializeField] private FruitCallData m_fruitCallData;
        public List<int> fruitCallIndices;

        private const byte MINIMUM_POOL_OBJECTS_COUNT = 4;
        private const string POOL_NAME = "fruit_call_pool";
        private FruitCall m_activeFruitCall;
        private FruitCall m_previousFruitCall;
        private FruitCall m_ancientFruitCall;
        private Vector3 m_completedFruitCallScale = Vector3.one * 0.75f;
        public FruitCall ActiveFruitCall => m_activeFruitCall;
        #endregion Variables

        #region    Unity Methods

        private void OnEnable()
        {
            GlobalEventHandler.OnFruitCallIsCompleted += Callback_On_Fruit_Timer_Completed;
            GlobalEventHandler.OnLevelStartupTimerIsCompleted += Callback_On_Level_Startup_Timer_Completed;
            GlobalEventHandler.OnFruitPairMatched += Callback_On_Fruit_Pair_Matched;
            GlobalEventHandler.RequestToActivatePowerUpMode += Callback_On_Activate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToDeactivatePowerUpMode += Callback_On_Deactivate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToFruitDumperPowerupAction += Callback_OnFruit_Dump_Action_Requested;
        }

        private void OnDisable()
        {
            ObjectPoolManager.instance.RemovePool(POOL_NAME);
            GlobalEventHandler.OnFruitCallIsCompleted -= Callback_On_Fruit_Timer_Completed;
            GlobalEventHandler.OnLevelStartupTimerIsCompleted -= Callback_On_Level_Startup_Timer_Completed;
            GlobalEventHandler.OnFruitPairMatched -= Callback_On_Fruit_Pair_Matched;
            GlobalEventHandler.RequestToActivatePowerUpMode -= Callback_On_Activate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToDeactivatePowerUpMode -= Callback_On_Deactivate_Powerup_Mode_Requested;
            GlobalEventHandler.RequestToFruitDumperPowerupAction -= Callback_OnFruit_Dump_Action_Requested;
        }

        private void Start()
        {
            _Init();
        }
        #endregion Unity Methods

        #region Public Methods
        public int GetQuickMatchBonusScore()
        {
            return m_activeFruitCall.GetQuickTimeBonusScore();
        }
        public float GetActiveFruitCallRemainingTime()
        {
            return m_activeFruitCall.GetRemainingTime();
        }
        #endregion Public Methods

        #region Private Methods

        private void _Init()
        {
            foreach (int index in LevelData.leveldata)
            {
                if (!fruitCallIndices.Contains(index))
                    fruitCallIndices.Add(index);
            }
            ObjectPoolManager.instance.InitializePool(POOL_NAME, m_fruitCallTemplate, MINIMUM_POOL_OBJECTS_COUNT, transform);
        }
        /// <summary>
        /// This method will return a random index for fruit calls.
        /// It take care of returning same index from previous index
        /// </summary>
        /// <returns></returns>
        private int _GetRandomFruitCallIndex()
        {
            int index = Random.Range(0, fruitCallIndices.Count);
            int previousIndex = -1;
            if (m_previousFruitCall)
                previousIndex = fruitCallIndices.IndexOf(m_previousFruitCall.ID);
            ///***Uncomment the below to to avoid showing the ancient fruit call again.***
            // int ancientIndex = -1;
            //int previousAncientIndex = -1;
            //if (m_ancientFruitCall)
            //  ancientIndex = fruitCallIndices.IndexOf(m_ancientFruitCall.ID);
            // previousAncientIndex = ancientIndex;

            while (previousIndex == index /*|| ancientIndex == index || previousAncientIndex == index*/)
                index = Random.Range(0, fruitCallIndices.Count);
            return fruitCallIndices[index];
        }

        private void _ActivateNewFruitCall()
        {
            int index = _GetRandomFruitCallIndex();
            GlobalVariables.currentFruitCallId = index;
            GameObject go = ObjectPoolManager.instance.GetObjectFromPool(POOL_NAME);
            if (go != null)
            {
                go.SetActive(true);
                go.transform.SetParent(m_activeFruitCallHolder);
                go.transform.position = m_activeFruitCallHolder.position;
                go.transform.DOScale(1, .25f);
                //go.transform.DOMove(m_activeFruitCallHolder.position, .25f);
                FruitCall call = go.GetComponent<FruitCall>();
                call.Init(index, m_entityDatabase.fruitSprites[index], m_entityDatabase.outlineFruitSprites[index]);
                m_activeFruitCall = call;
                call.ActivateEntity();
            }
        }
        #endregion Private Methods

        #region Callbacks


        private void Callback_On_Fruit_Timer_Completed()
        {

            //references
            m_ancientFruitCall?.DeactivateEntity();
            m_ancientFruitCall = m_previousFruitCall;
            m_previousFruitCall = m_activeFruitCall;
            //Parents
            if (m_ancientFruitCall)
            {
                _ShiftFruitCall(m_ancientFruitCall.transform, m_ancientFruitCallHolder);
                //m_ancientFruitCall.transform.SetParent(m_ancientFruitCallHolder);
                //m_ancientFruitCall.transform.DOScale(m_completedFruitCallScale, .25f);
                //m_ancientFruitCall.transform.DOMove(m_ancientFruitCallHolder.position, .25f);
            }
            if (m_previousFruitCall)
            {
                _ShiftFruitCall(m_previousFruitCall.transform, m_previousFruitCallHolder);
                //m_previousFruitCall.transform.SetParent(m_previousFruitCallHolder);
                //m_previousFruitCall.transform.DOScale(m_completedFruitCallScale, .25f);
                //m_previousFruitCall.transform.DOMove(m_previousFruitCallHolder.position, .25f);
            }

            _ActivateNewFruitCall();
        }
        private void _ShiftFruitCall(Transform call, Transform shiftToTransform)
        {
            call.SetParent(shiftToTransform);
            call.DOScale(m_completedFruitCallScale, 0.25f);
            call.DOMoveX(shiftToTransform.position.x, 0.25f);
        }
        private void Callback_On_Level_Startup_Timer_Completed()
        {
            _ActivateNewFruitCall();
        }
        private void Callback_On_Fruit_Pair_Matched(int id)
        {
            if (m_activeFruitCall.ID == id)
            {
                m_activeFruitCall.ShowMatchDoneEffect();
            }
        }


        private void Callback_On_Activate_Powerup_Mode_Requested(PowerupType powerupType)
        {
            m_activeFruitCall.PauseTimer();
        }

        private void Callback_On_Deactivate_Powerup_Mode_Requested()
        {
            m_activeFruitCall.ResumeTimer();
            if (m_ancientFruitCall)
            {
                _ShiftFruitCall(m_ancientFruitCall.transform, m_ancientFruitCallHolder);
            }
            if (m_previousFruitCall)
            {
                _ShiftFruitCall(m_previousFruitCall.transform, m_previousFruitCallHolder);
            }
        }

        private void Callback_OnFruit_Dump_Action_Requested()
        {

        }
        #endregion Callbacks



    }
}