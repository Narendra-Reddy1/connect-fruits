using BenStudios;
using BenStudios.Economy;
using BenStudios.IAP;
using BenStudios.ScreenManagement;
using TMPro;
using UnityEngine;

public class SupportDevSuccessScreen : PopupBase
{
    [SerializeField] private TextMeshProUGUI m_coinCountTxt;
    [SerializeField] private TextMeshProUGUI m_fruitBombCountTxt;
    [SerializeField] private TextMeshProUGUI m_tripleBombCountTxt;
    [SerializeField] private TextMeshProUGUI m_hintPowerupCountTxt;
    public override void OnEnable()
    {
        base.OnEnable();
        _Init();
    }

    public override void OnCloseClick()
    {
        ScreenManager.Instance.CloseLastAdditiveScreen();
    }
    private void _Init()
    {
        BundlePackData bundleData = InAppPurchasingManager.instance.GetBundlePackData(BundleType.Support_Dev);
        m_coinCountTxt.SetText($"X{bundleData.GetResourceCountIfHave(ResourceType.Coin)}");
        m_fruitBombCountTxt.SetText($"X{bundleData.GetResourceCountIfHave(ResourceType.FruitBomb)}");
        m_tripleBombCountTxt.SetText($"X{bundleData.GetResourceCountIfHave(ResourceType.TripleBomb)}");
        m_hintPowerupCountTxt.SetText($"X{bundleData.GetResourceCountIfHave(ResourceType.HintPowerup)}");
        PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_player_supported_dev, true);
    }


}
