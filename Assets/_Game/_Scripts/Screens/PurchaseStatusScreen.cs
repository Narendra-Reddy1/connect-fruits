using BenStudios;
using BenStudios.IAP;
using BenStudios.ScreenManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing.Extension;

public class PurchaseStatusScreen : PopupBase
{
    [Header("Script Contents")]
    [SerializeField] private TextMeshProUGUI m_statusTxt;


    public override void OnEnable()
    {
        base.OnEnable();
        m_statusTxt.SetText(Konstants.PURCHASE_INITIALIZING_TEXT);
        GlobalEventHandler.OnPurchaseSuccess += Callback_On_Purchase_Success;
        GlobalEventHandler.OnPurchaseFailed += Callback_On_Purchase_Failed;
        Invoke(nameof(CloseScreen), 4f);
    }
    public override void OnDisable()
    {
        GlobalEventHandler.OnPurchaseSuccess -= Callback_On_Purchase_Success;
        GlobalEventHandler.OnPurchaseFailed -= Callback_On_Purchase_Failed;
    }

    private void CloseScreen() => ScreenManager.Instance.CloseLastAdditiveScreen();

    private void Callback_On_Purchase_Failed(PurchaseFailureDescription failureDescription)
    {
        m_statusTxt.SetText(Konstants.PURCHASE_FAILED_TEXT);
        CancelInvoke(nameof(CloseScreen));
        Invoke(nameof(CloseScreen), 2f);
    }
    private void Callback_On_Purchase_Success(PurchaseData purchaseData)
    {
        m_statusTxt.SetText(Konstants.PURCHASE_SUCCESS_TEXT);
        CancelInvoke(nameof(CloseScreen));
        Invoke(nameof(CloseScreen), 2f);
    }
}
