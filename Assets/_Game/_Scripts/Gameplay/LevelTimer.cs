using BenStudios;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    #region Variables
    [SerializeField] private TextMeshProUGUI m_timerTxt;
    private int m_timerCounter = 0;
    #endregion Variables


    #region Unity Methods
 
    #endregion Unity Methods


    #region Public Methods 
    public void InitTimer(int timeInSeconds)
    {
        m_timerCounter = timeInSeconds;
        if (m_timerTxt)
            m_timerTxt.text = MyUtils.GetFormattedSeconds(m_timerCounter);
    }
    public void InitTimerAndStartTimer(int timeInSeconds)
    {
        m_timerCounter = timeInSeconds;
        StartTimer();
    }
    public void StartTimer()
    {
        InvokeRepeating(nameof(_Tick), 1, 1);
    }
    public void StopTimer()
    {
        CancelInvoke(nameof(_Tick));
    }
    #endregion Public Methods 

    #region Private Methods 
    private void _Tick()
    {
        m_timerCounter--;
        if (m_timerCounter <= 0)
        {
            m_timerCounter = 0;
            StopTimer();
            GlobalEventHandler.OnLevelTimerIsCompleted?.Invoke();
        }
        if (m_timerTxt)
            m_timerTxt.text = MyUtils.GetFormattedSeconds(m_timerCounter);
    }
    #endregion Private Methods 

    #region Callbacks
    
    #endregion Callbacks
}
