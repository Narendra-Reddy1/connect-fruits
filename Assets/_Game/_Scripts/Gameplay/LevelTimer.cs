using BenStudios;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    #region Variables
    [SerializeField] private TextMeshProUGUI m_timerTxt;
    private int m_timerCounter = 0;
    private int m_timeInSeconds;
    #endregion Variables


    #region Unity Methods

    #endregion Unity Methods


    #region Public Methods 
    public void InitTimer(int timeInSeconds)
    {
        m_timeInSeconds = m_timerCounter = timeInSeconds;
        if (m_timerTxt)
            m_timerTxt.text = MyUtils.GetFormattedSeconds(m_timerCounter);
    }
    public void InitTimerAndStartTimer(int timeInSeconds)
    {
        m_timeInSeconds = m_timerCounter = timeInSeconds;
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
    public int GetRemaingTimeInSeconds()
    {
        return (m_timeInSeconds - m_timerCounter);
    }
    #endregion Public Methods 

    #region Private Methods 
    private void _Tick()
    {
        m_timerCounter--;
        if (m_timerCounter <= 0)
        {
            m_timerCounter = 0;
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

    #endregion Callbacks
}
