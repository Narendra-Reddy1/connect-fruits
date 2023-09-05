using BenStudios;
using UnityEngine;

public class GenericAdsManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private AdUnitIds adUnitIds;
    private IAds AdsManager;

    #endregion Variables

    #region Unity Methods
    private void Awake()
    {
        Init();

    }
    private void OnEnable()
    {
        GlobalEventHandler.RequestToShowBannerAd += Callback_On_ShowBannerAd_Requested;
        GlobalEventHandler.RequestToHideBannerAd += Callback_On_HideBannerAd_Requested;
        GlobalEventHandler.RequestToShowInterstitial += Callback_On_ShowInterstitialAd_Requested;
        GlobalEventHandler.RequestToShowRewardedAd += Callback_On_ShowRewardedAd_Requested;

        GlobalEventHandler.Request_Interstitial_Availability += Callback_On_InterstitialAd_Availability_Requested;
        GlobalEventHandler.Request_Rewarded_Ad_Availability += Callback_On_RewardedAd_Availability_Requested;
        //GlobalEventHandler.AddListener(EventID.EVENT_ON_LOAD_MREC_AD_REQUESTED, Callback_On_Load_MREC_Ad_Requested);
        //GlobalEventHandler.AddListener(EventID.EVENT_ON_SHOW_MREC_AD_REQUESTED, Callback_On_ShowMRECAd_Requested);
        //GlobalEventHandler.AddListener(EventID.EVENT_ON_HIDE_MREC_AD_REQUESTED, Callback_On_HideMRECAd_Requested);
        //GlobalEventHandler.AddListener(EventID.EVENT_ON_LOAD_APP_OPEN_AD_REQUESTED, Callback_On_Load_App_Open_Ad_Requested);
        //GlobalEventHandler.AddListener(EventID.EVENT_ON_SHOW_APP_OPEN_AD_REQUESTED, Callback_On_Show_AppOpenAd_Requested);
        //GlobalEventHandler.AddListener(EventID.EVENT_ON_APP_OPEN_AD_AVAILABILITTY_REQUESTED, Callback_On_AppOpenAd_Availability_Requested);

    }
    private void OnDisable()
    {
        GlobalEventHandler.RequestToShowBannerAd -= Callback_On_ShowBannerAd_Requested;
        GlobalEventHandler.RequestToHideBannerAd -= Callback_On_HideBannerAd_Requested;
        GlobalEventHandler.RequestToShowInterstitial -= Callback_On_ShowInterstitialAd_Requested;
        GlobalEventHandler.RequestToShowRewardedAd -= Callback_On_ShowRewardedAd_Requested;
       
        GlobalEventHandler.Request_Interstitial_Availability -= Callback_On_InterstitialAd_Availability_Requested;
        GlobalEventHandler.Request_Rewarded_Ad_Availability -= Callback_On_RewardedAd_Availability_Requested;

        //GlobalEventHandler.RemoveListener(EventID.EVENT_ON_LOAD_MREC_AD_REQUESTED, Callback_On_Load_MREC_Ad_Requested);
        //GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SHOW_MREC_AD_REQUESTED, Callback_On_ShowMRECAd_Requested);
        //GlobalEventHandler.RemoveListener(EventID.EVENT_ON_HIDE_MREC_AD_REQUESTED, Callback_On_HideMRECAd_Requested);
        //GlobalEventHandler.RemoveListener(EventID.EVENT_ON_LOAD_APP_OPEN_AD_REQUESTED, Callback_On_Load_App_Open_Ad_Requested);
        //GlobalEventHandler.RemoveListener(EventID.EVENT_ON_SHOW_APP_OPEN_AD_REQUESTED, Callback_On_Show_AppOpenAd_Requested);
        //GlobalEventHandler.RemoveListener(EventID.EVENT_ON_APP_OPEN_AD_AVAILABILITTY_REQUESTED, Callback_On_AppOpenAd_Availability_Requested);
    }

    #endregion Unity Methods

    public void Init()
    {
        AdsManager = new ApplovinManager(adUnitIds);
    }

    #region Banner Ads
    private void Callback_On_ShowBannerAd_Requested()
    {
        AdsManager.ShowBannerAd();
    }
    private void Callback_On_HideBannerAd_Requested()
    {
        AdsManager.HideBannerAd();
    }
    #endregion Banner Ads

    #region Interstitial Ads
    private void Callback_On_ShowInterstitialAd_Requested()
    {
        AdsManager.ShowInterstitialAd();
    }
    private bool Callback_On_InterstitialAd_Availability_Requested()
    {
        return AdsManager.IsInterstitialAdAvailable();
    }
    #endregion Interstitial Ads

    #region Rewarded Ads
    private void Callback_On_ShowRewardedAd_Requested()
    {
        AdsManager.ShowRewardedAd();
    }
    private bool Callback_On_RewardedAd_Availability_Requested()
    {
        return AdsManager.IsRewardedAdAvailable();
    }
    #endregion Rewarded Ads

    #region MREC Ads
    //private void Callback_On_Load_MREC_Ad_Requested()
    //{
    //    AdsManager.LoadMRECAd();
    //}
    //private void Callback_On_ShowMRECAd_Requested()
    //{
    //    AdsManager.ShowMRECAd();
    //}
    //private void Callback_On_HideMRECAd_Requested()
    //{
    //    AdsManager.HideMRECAd();
    //}
    #endregion MREC Ads

    #region App Open Ads
    //private void Callback_On_Load_App_Open_Ad_Requested()
    //{
    //    AdsManager.LoadAppOpenAd();
    //}
    //private void Callback_On_Show_AppOpenAd_Requested()
    //{
    //    AdsManager.ShowAppOpenAd();
    //}
    //private object Callback_On_AppOpenAd_Availability_Requested()
    //{
    //    return AdsManager.IsAppOpenAdAvailable();
    //}
    #endregion App Open Ads

#if DEVLOPMENT_BUILD ||DEBUG_DEFINE

    #region Debug 
    [DebugButton("ShowBanner")]
    public void DebugShowBannerAd()
    {
        Callback_On_ShowBannerAd_Requested();
    }

    [DebugButton("HideBanner")]
    public void DebugHideBannerAd()
    {
        Callback_On_HideBannerAd_Requested();
    }


    [DebugButton("ShowInter")]
    public void DebugShowInter()
    {
        Callback_On_ShowInterstitialAd_Requested();
    }

    [DebugButton("ShowRewarded")]
    public void DebugShowRewarded()
    {
        Callback_On_ShowRewardedAd_Requested();
    }

    //[DebugButton("LoadAppOpen")]
    //public void DebugLoadAppOpenAd()
    //{
    //    Callback_On_Load_App_Open_Ad_Requested();
    //}
    //[DebugButton("ShowAppOpen")]
    //public void DebugShowAppOpenAd()
    //{
    //    Callback_On_Show_AppOpenAd_Requested();
    //}
    //[DebugButton("ShowMRec")]
    //public void DebugShowMrec()
    //{
    //    Callback_On_ShowMRECAd_Requested();
    //}

    //[DebugButton("HideMRec")]
    //public void DebugHideMRec()
    //{
    //    Callback_On_HideMRECAd_Requested();
    //}


    [DebugButton("MediationDebugger")]
    public void ShowMediationDebugger()
    {
        MaxSdk.ShowMediationDebugger();
    }
    #endregion Debug

#endif
}