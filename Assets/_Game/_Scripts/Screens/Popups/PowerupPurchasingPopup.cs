using BenStudios;
using BenStudios.Economy;
using BenStudios.IAP;
using BenStudios.ScreenManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupPurchasingPopup : PopupBase
{
    [SerializeField] private Button m_adButton;
    [SerializeField] private TextMeshProUGUI m_powerupPriceTxt;
    [SerializeField] private TextMeshProUGUI m_powerupCountTxt;
    private string m_productID;

    public override void OnEnable()
    {
        base.OnEnable();
        _Init();
        GlobalEventHandler.OnPurchaseSuccess += Callback_On_Purchase_Success;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        GlobalEventHandler.OnPurchaseSuccess -= Callback_On_Purchase_Success;
    }

    private void _Init()
    {
        //m_adButton.interactable=IsAdAvailable();
        m_powerupCountTxt.SetText($"X{InAppPurchasingManager.instance.GetSinglePackData(BundleType.FruitBomb_Nano_Pack)}");
        m_productID = Konstants.MINI_STORE_FRUIT_BOMB_NANO_PACK;
        m_powerupPriceTxt.SetText(InAppPurchasingManager.instance.GetLocalizedPrice(m_productID));
    }
    public override void OnCloseClick()
    {
        ScreenManager.Instance.CloseLastAdditiveScreen();
    }

    public void OnClickPowerupAdBtn()
    {
        //show ad
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
        PlayerResourceManager.Give(PlayerResourceManager.FRUIT_BOMB_POWERUP_ITEM_ID, purchaseData.fruitBombs);
        if (ScreenManager.Instance.GetCurrentScreen() == Window.PowerupPurchasePopup)
            ScreenManager.Instance.CloseLastAdditiveScreen();
    }
}
