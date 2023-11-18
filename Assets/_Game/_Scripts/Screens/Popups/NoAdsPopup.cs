using BenStudios;
using BenStudios.IAP;
using BenStudios.ScreenManagement;
using TMPro;
using UnityEngine;

public class NoAdsPopup : PopupBase
{
    [Space(10)]
    [SerializeField] private TextMeshProUGUI _priceTxt;
    [SerializeField] private CustomButton _buyBtn;

    public override void OnEnable()
    {
        base.OnEnable();
        GlobalEventHandler.OnPurchaseSuccess += Callback_On_Purchase_Success;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        GlobalEventHandler.OnPurchaseSuccess -= Callback_On_Purchase_Success;
    }
    public override void OnCloseClick()
    {
        ScreenManager.Instance.CloseLastAdditiveScreen();
    }

    private void Callback_On_Purchase_Success(PurchaseData purchaseData)
    {
        if (!purchaseData.isNoAds) return;
        _priceTxt.SetText($"Done");
    }

}
