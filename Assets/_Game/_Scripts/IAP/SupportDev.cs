using BenStudios.IAP;
using BenStudios.ScreenManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BenStudios
{
    public class SupportDev : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_itemPriceTxt;


        private void OnEnable()
        {
            GlobalEventHandler.OnPurchaseSuccess += Callback_On_Purchase_Success;
            transform.localScale = Vector3.one;
            if (m_itemPriceTxt != null)
                m_itemPriceTxt.SetText(InAppPurchasingManager.instance.GetLocalizedPrice(Konstants.SUPPORT_DEV_PACK));
        }
        private void OnDisable()
        {
            GlobalEventHandler.OnPurchaseSuccess -= Callback_On_Purchase_Success;
        }
        public void SupportDeveloper()
        {
            GlobalEventHandler.RequestToInitializePurchase?.Invoke(Konstants.SUPPORT_DEV_PACK);
        }

        private void Callback_On_Purchase_Success(PurchaseData purchaseData)
        {
            if (purchaseData.productID == Konstants.SUPPORT_DEV_PACK)
                ScreenManager.Instance.ChangeScreen(Window.SupportDevSuccessScreen, ScreenType.Additive, false);
        }
    }
}