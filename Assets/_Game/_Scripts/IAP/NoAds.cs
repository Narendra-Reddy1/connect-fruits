using BenStudios;
using BenStudios.IAP;
using BenStudios.ScreenManagement;
using TMPro;
using UnityEngine;

public class NoAds : BasePack
{
    [SerializeField] private TextMeshProUGUI m_priceTxt; 

    private void Awake()
    {
        if (PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_no_ads_purchased))
        {
            gameObject.SetActive(false);
            return;
        }
        AssignProductID(BundleType.No_Ads);
        SetLocalizedPrice();
    }
    private void OnEnable()
    {
        GlobalEventHandler.OnPurchaseSuccess += Callback_On_Purchase_Success;
    }
    private void OnDisable()
    {
        GlobalEventHandler.OnPurchaseSuccess -= Callback_On_Purchase_Success;
    }
    public void ShowNoAdsPopup()
    {
        ScreenManager.Instance.ChangeScreen(Window.NoAdsPopup, ScreenType.Additive, false);
    }
    public void OnClickBuy()
    {
        if (!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_no_ads_purchased))
            ScreenManager.Instance.ChangeScreen(Window.PurchaseStatusScreen, ScreenType.Additive, onComplete: () =>
            {
                GlobalEventHandler.RequestToInitializePurchase?.Invoke(productID);
            });
    }
    public override void SetLocalizedPrice()
    {
        if (m_priceTxt != null)
            m_priceTxt.SetText(InAppPurchasingManager.instance.GetLocalizedPrice(productID));
    }
    private void Callback_On_Purchase_Success(PurchaseData purchaseData)
    {
        if (!purchaseData.isNoAds) return;
        GlobalEventHandler.RequestToHideBannerAd?.Invoke();
        gameObject.SetActive(false);
    }
}
