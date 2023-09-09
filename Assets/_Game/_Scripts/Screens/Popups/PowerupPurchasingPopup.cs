using BenStudios;
using BenStudios.Economy;
using BenStudios.IAP;
using BenStudios.ScreenManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupPurchasingPopup : PopupBase
{
    [SerializeField] private Button m_adButton;
    [SerializeField] private TextMeshProUGUI m_powerupPriceTxt;
    [SerializeField] private TextMeshProUGUI m_powerupCountTxt;
    [SerializeField] private GameObject m_adIcon;
    [SerializeField] private GameObject m_loadingIcon;
    private string m_productID;
    bool m_isConnectedToInternet;
    bool m_isAdAvailable;

    public override void OnEnable()
    {
        base.OnEnable();
        _Init();
        GlobalEventHandler.RequestToPauseTimer?.Invoke(true);
        GlobalEventHandler.OnPurchaseSuccess += Callback_On_Purchase_Success;
        GlobalEventHandler.EventOnAdStateChanged += Callback_On_Ad_State_Changed;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        GlobalEventHandler.RequestToPauseTimer?.Invoke(false);
        GlobalEventHandler.OnPurchaseSuccess -= Callback_On_Purchase_Success;
        GlobalEventHandler.EventOnAdStateChanged -= Callback_On_Ad_State_Changed;
    }

    private void _Init()
    {
        m_powerupCountTxt.SetText($"X{InAppPurchasingManager.instance.GetSinglePackData(BundleType.FruitBomb_Nano_Pack).itemCount}");
        m_productID = Konstants.MINI_STORE_FRUIT_BOMB_NANO_PACK;
        m_powerupPriceTxt.SetText(InAppPurchasingManager.instance.GetLocalizedPrice(m_productID));
        _UpdateAdButtonOnAdAvailability();
        _UpdateAdButtonOnInternetConnectionChange();
    }
    private void _UpdateAdButtonOnAdAvailability()
    {
        m_isAdAvailable = (bool)GlobalEventHandler.Request_Rewarded_Ad_Availability?.Invoke();
    }
    private void _UpdateAdButtonOnInternetConnectionChange()
    {
        m_isConnectedToInternet = MyUtils.IsApplicationConnectedToInternet();
        m_adIcon.SetActive(m_isConnectedToInternet);
        m_loadingIcon.SetActive(!m_isConnectedToInternet);
    }
    public override void OnCloseClick()
    {
        ScreenManager.Instance.CloseLastAdditiveScreen();
    }

    public void OnClickPowerupAdBtn()
    {
        if (!m_isConnectedToInternet)
        {
            _UpdateAdButtonOnInternetConnectionChange();
            return;
        }
        else if (!m_isAdAvailable)
        {
            _UpdateAdButtonOnAdAvailability();
            return;
        }
        GlobalEventHandler.RequestToShowRewardedAd?.Invoke();
    }
    public void OnClickPowerupBuyBtn()
    {
        ScreenManager.Instance.ChangeScreen(Window.PurchaseStatusScreen, ScreenType.Additive, false);
        GlobalEventHandler.RequestToInitializePurchase?.Invoke(m_productID);
    }
    public void OnClickStoreBtn()
    {
        ScreenManager.Instance.ChangeScreen(Window.StoreScreen, ScreenType.Additive, false);
    }

    private void Callback_On_Purchase_Success(PurchaseData purchaseData)
    {
        if (purchaseData.productID != m_productID) return;
        if (ScreenManager.Instance.GetCurrentScreen() == Window.PowerupPurchasePopup)
            ScreenManager.Instance.CloseLastAdditiveScreen();
    }
    private bool isRewardedAdWatchedCompletely = false;
    private void Callback_On_Ad_State_Changed(AdEventData adEventData)
    {
        switch (adEventData.adState)
        {
            case AdState.REWARDED_REWARD_RECEIVED:
                isRewardedAdWatchedCompletely = true;
                break;
            case AdState.REWARDED_DISMISSED:
                if (isRewardedAdWatchedCompletely)
                {
                    isRewardedAdWatchedCompletely = false;
                    PlayerResourceManager.Give(PlayerResourceManager.FRUIT_BOMB_POWERUP_ITEM_ID);
                    ScreenManager.Instance.CloseLastAdditiveScreen(() =>
                    {
                        GlobalEventHandler.PerformOnClickFruitBomb?.Invoke();
                    });
                }
                break;
        }
    }
}
