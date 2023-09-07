using BenStudios;
using BenStudios.Economy;
using BenStudios.ScreenManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutOfTimePopup : PopupBase
{
    [SerializeField] private Button m_adButton;
    [SerializeField] private TextMeshProUGUI m_coinsBuyText;
    [SerializeField] private TextMeshProUGUI m_timerTxt;
    [SerializeField] private GameObject m_loadingIcon;
    [SerializeField] private GameObject m_adIcon;
    [SerializeField] private int m_timeInSecondsToAdd = 60;
    private bool m_isAdAvailable = false;
    public override void OnEnable()
    {
        base.OnEnable();
        GlobalEventHandler.EventOnAdStateChanged += Callback_On_Ad_State_Changed;
        _Init();
    }
    public override void OnDisable()
    {
        base.OnDisable();
        GlobalEventHandler.EventOnAdStateChanged -= Callback_On_Ad_State_Changed;
    }


    public void BuyWithAd()
    {
        if (m_isAdAvailable)
        {
            GlobalEventHandler.RequestToShowRewardedAd();
        }
    }

    public void BuyWithCoins()
    {
        int coinCost = _GetCoinCost();
        if (coinCost > PlayerResourceManager.GetCoinsBalance())
        {
            ScreenManager.Instance.ChangeScreen(Window.StoreScreen, ScreenType.Additive, false);
            return;
        }
        Mathf.Clamp(++GlobalVariables.outOfTimeCounter, 1, 3);
        PlayerResourceManager.Take(PlayerResourceManager.COINS_ITEM_ID, coinCost);
        _AddTime();
    }
    private void _Init()
    {
        m_coinsBuyText.SetText(_GetCoinCost().ToString());
        m_timerTxt.SetText($"+{m_timeInSecondsToAdd}s");
        m_isAdAvailable = (bool)GlobalEventHandler.Request_Rewarded_Ad_Availability?.Invoke();
        m_adButton.interactable = m_isAdAvailable;
        m_adIcon.SetActive(m_isAdAvailable);
        m_loadingIcon.SetActive(!m_isAdAvailable);
    }

    public override void OnCloseClick()
    {
        ScreenManager.Instance.ChangeScreen(Window.ExitConfirmationPopup, ScreenType.Additive, false);
    }
    private void _AddTime()
    {
        ScreenManager.Instance.CloseLastAdditiveScreen(() =>
        {
            GlobalEventHandler.RequestToAddExtraLevelTime?.Invoke(60);
        });
    }
    private int _GetCoinCost()
    {
        switch (GlobalVariables.outOfTimeCounter)
        {
            case 1:
                return 60;
            case 2:
                return 90;
            case 3:
                return 120;
            default:
                return 120;
        }
    }

    private bool isAdWatchedCompletedly = false;
    private void Callback_On_Ad_State_Changed(AdEventData adEventData)
    {
        switch (adEventData.adState)
        {
            case AdState.REWARDED_REWARD_RECEIVED:
                isAdWatchedCompletedly = true;
                break;
            case AdState.REWARDED_DISMISSED:
                if (isAdWatchedCompletedly)
                    _AddTime();
                break;
        }
    }


}
