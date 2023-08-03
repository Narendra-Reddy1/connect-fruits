using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BenStudios
{
    [CreateAssetMenu(fileName = "newTimerData", menuName = "ScriptableObjects/TimerData", order = 1)]
    public class TimerData : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<TimerType, TimerDataSet> timerData;

        public TimerDataSet GetTimerData(TimerType timerType)
        {
            if (timerData.ContainsKey(timerType))
            {
                return timerData[timerType];
            }
            return null;
        }
    }
    [System.Serializable]
    public class TimerDataSet
    {
        public int timeInSeconds;
        public bool canCache;
    }
    public enum TimerType
    {
        LevelTimer,
    }
}