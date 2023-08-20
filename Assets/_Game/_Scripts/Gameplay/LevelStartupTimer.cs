using DG.Tweening;
using BenStudios;
using TMPro;
using UnityEngine;

public class LevelStartupTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerTxt;

    private int timerCounter = 0;
    #region Unity Methods

    private void Awake()
    {
        InitTimer();
        InvokeRepeating(nameof(_Tick), 0, 1);
    }
    private void Start()
    {
        GlobalEventHandler.RequestToScreenBlocker?.Invoke(true);
    }
    #endregion Unity Methods


    #region Public Methods


    #endregion Public Methods


    #region Private Methods

    private void InitTimer()
    {
        timerCounter = Konstants.LEVEL_STARTUP_TIMER_IN_SECONDS;
    }
    private void _Tick()
    {
        timerCounter--;
        if (timerCounter < 0)
        {
            CancelInvoke(nameof(_Tick));
            m_timerTxt.transform.DOScale(0, .65f).onComplete += () =>
            {
                gameObject.SetActive(false);
                GlobalEventHandler.RequestToScreenBlocker?.Invoke(false);
                GlobalEventHandler.OnLevelStartupTimerIsCompleted?.Invoke();
            };
            return;
            //Timer completed;
        }
        m_timerTxt.transform.DOScale(0, .65f).onComplete += () =>
        {
            m_timerTxt.text = timerCounter.ToString();
            if (timerCounter == 0)
            {
                GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.TimerCountdownEndSFX);
                m_timerTxt.text = $"GO";
            }
            else
                GlobalEventHandler.RequestToPlaySFX?.Invoke(AudioID.TimerCountdownSFX);
            m_timerTxt.transform.DOScale(1, .35f);
        };
    }

    #endregion Private Methods

}
