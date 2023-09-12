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
    private const string PURCHASE_INITIALIZING_TEXT = "Purchase Initializing...";
    private const string PURCHASE_SUCCESS_TEXT = "Purchase Success...";
    private const string PURCHASE_FAILED_TEXT = "Purchase Failed...";

    public override void OnEnable()
    {
        base.OnEnable();
        m_statusTxt.SetText(PURCHASE_INITIALIZING_TEXT);
        GlobalEventHandler.OnPurchaseSuccess += Callback_On_Purchase_Success;
        GlobalEventHandler.OnPurchaseFailed += Callback_On_Purchase_Failed;
        GlobalEventHandler.EventOnPurchaseRestoreFail += Callback_On_Purchsase_Restore_Failed;
        GlobalEventHandler.EventOnPurchaseRestoreSuccess += Callback_On_Purchsase_Restore_Successful;
        Invoke(nameof(CloseScreen), 6f);//To prevent stucking at purchase loading screen.
    }
    public override void OnDisable()
    {
        GlobalEventHandler.OnPurchaseSuccess -= Callback_On_Purchase_Success;
        GlobalEventHandler.OnPurchaseFailed -= Callback_On_Purchase_Failed;
        GlobalEventHandler.EventOnPurchaseRestoreFail -= Callback_On_Purchsase_Restore_Failed;
        GlobalEventHandler.EventOnPurchaseRestoreSuccess -= Callback_On_Purchsase_Restore_Successful;
    }

    private void CloseScreen() => ScreenManager.Instance.CloseLastAdditiveScreen();

    private void Callback_On_Purchase_Failed(PurchaseFailureDescription failureDescription)
    {
        m_statusTxt.SetText(PURCHASE_FAILED_TEXT);
        CancelInvoke(nameof(CloseScreen));
        Invoke(nameof(CloseScreen), 2f);
    }

    private void Callback_On_Purchase_Success(PurchaseData purchaseData)
    {
        MyUtils.Log($"Purchase Success Callback_PurchaseStatusScreen...");
        m_statusTxt.SetText(PURCHASE_SUCCESS_TEXT);
        CancelInvoke(nameof(CloseScreen));
        Invoke(nameof(CloseScreen), 2f);
    }
    public void Callback_On_Purchsase_Restore_Failed()
    {
        m_statusTxt.SetText($"Restore Success");
        CancelInvoke(nameof(CloseScreen));
        Invoke(nameof(CloseScreen), 2f);
    }
    public void Callback_On_Purchsase_Restore_Successful()
    {
        m_statusTxt.SetText($"Restore Failed");
        CancelInvoke(nameof(CloseScreen));
        Invoke(nameof(CloseScreen), 2f);
    }
}
