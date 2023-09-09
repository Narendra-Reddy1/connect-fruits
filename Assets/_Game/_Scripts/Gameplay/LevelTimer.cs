using BenStudios;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    #region Variables
    [SerializeField] private TextMeshProUGUI m_timerTxt;
    private int m_timerCounter = 0;
    private int m_totalTimeInSeconds;
    private bool m_isTimerCompleted;
    private bool m_isTimerRunning = false;
    #endregion Variables


    #region Unity Methods
    private void OnEnable()
    {
        GlobalEventHandler.RequestToAddExtraLevelTime += Callback_On_Add_Extra_Time_Requested;
    }
    private void OnDisable()
    {
        GlobalEventHandler.RequestToAddExtraLevelTime -= Callback_On_Add_Extra_Time_Requested;
    }
    #endregion Unity Methods


    #region Public Methods 
    public void InitTimer(int timeInSeconds)
    {
        m_totalTimeInSeconds = m_timerCounter = timeInSeconds;
        m_isTimerCompleted = false;
        m_isTimerRunning = false;
        if (m_timerTxt)
            m_timerTxt.text = MyUtils.GetFormattedSeconds(m_timerCounter);
    }
    public void InitTimerAndStartTimer(int timeInSeconds)
    {
        m_totalTimeInSeconds = m_timerCounter = timeInSeconds;
        m_isTimerCompleted = false;
        m_isTimerRunning = false;
        StartTimer();
    }
    public void StartTimer()
    {
        if (m_isTimerCompleted || m_isTimerRunning) return;
        m_isTimerRunning = true;
        m_timerTxt.transform.DOKill();
        MyUtils.Log($"Timer Started::");
        InvokeRepeating(nameof(_Tick), 1, 1);
    }
    public void StopTimer()
    {
        MyUtils.Log($"Timer Stopped::");
        m_isTimerRunning = false;
        CancelInvoke(nameof(_Tick));
    }
    public void RestartTimer()
    {
        StopTimer();
        InitTimerAndStartTimer(m_totalTimeInSeconds);
    }
    public void RestartTimer(int newTimeInSeconds)
    {
        m_totalTimeInSeconds = newTimeInSeconds;
        StopTimer();
        InitTimerAndStartTimer(m_totalTimeInSeconds);
    }
    public int GetRemaingTimeInSeconds()
    {
        return m_timerCounter;
    }
    public int GetElapsedTimeInSeconds() => m_totalTimeInSeconds - m_timerCounter;
    #endregion Public Methods 

    #region Private Methods 
    private void _Tick()
    {
        m_timerCounter--;
        if (m_timerCounter <= 0)
        {
            m_timerCounter = 0;
            m_isTimerCompleted = true;
            GlobalEventHandler.RequestToPlaySFX(AudioID.TimerCountdownEndSFX);
            StopTimer();
            GlobalEventHandler.OnLevelTimerIsCompleted?.Invoke();
        }
        if (m_timerCounter == 10)
            m_timerTxt.transform.DOPunchScale(Vector3.one * .2f, 1, 0).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        if (m_timerCounter <= 10)
            GlobalEventHandler.RequestToPlaySFX(AudioID.TimerCountdownSFX);
        if (m_timerTxt)
            m_timerTxt.text = MyUtils.GetFormattedSeconds(m_timerCounter);
    }
    #endregion Private Methods 

    #region Callbacks
    private void Callback_On_Add_Extra_Time_Requested(int timeInSeconds)
    {
        m_timerCounter += timeInSeconds;
        if (m_timerTxt) m_timerTxt.DOText(MyUtils.GetFormattedSeconds(m_timerCounter), .25f);
        m_isTimerCompleted = false;
        StartTimer();
    }
    #endregion Callbacks
}
