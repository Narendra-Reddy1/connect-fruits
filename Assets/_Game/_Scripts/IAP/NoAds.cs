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
        gameObject.SetActive(!PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_no_ads_purchased));
        AssignProductID(BundleType.No_Ads);
        SetLocalizedPrice();
    }
    public void OnClickNoAdsBtn()
    {
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
}
