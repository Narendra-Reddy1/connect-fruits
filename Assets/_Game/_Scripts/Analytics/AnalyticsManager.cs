using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
using Facebook.Unity;

namespace BenStudios
{
    public enum AnalyticsEvent
    {
        SessionStart,
        SessionEnd,
        LevelStart,
        LevelComplete,
        //Purchase events
        PurchaseConversion,
        PurchaseImpression,
        //Ad events
        AdRevenueReceived,
        AdDisplayed,
        AdClosed,
        AdClicked,
        //resources

    }
    public enum LevelCompleteStatus
    {
        Success,
        TimeUp,
        Exited,
    }

    public class ParameterBlock
    {
        public Dictionary<string, object> parameters;
        public ParameterBlock()
        {
            parameters = new Dictionary<string, object>();
        }
    }

    public class AnalyticsManager : MonoBehaviour
    {
        #region SINGLETON
        public static AnalyticsManager instance { get; private set; }
        #endregion SINGLETON


        #region Varibales

        #endregion Varibales

        #region Unity Methods
        private void Awake()
        {
            instance = this;
        }
        private void OnEnable()
        {
            //  GlobalEventHandler.RequestRecordEvent += RecordEvent;
            GlobalEventHandler.EventOnAdStateChanged += Callback_On_Ad_State_Changed;
        }
        private void OnDisable()
        {
            //GlobalEventHandler.RequestRecordEvent -= RecordEvent;
            GlobalEventHandler.EventOnAdStateChanged -= Callback_On_Ad_State_Changed;
        }

        #region Facebook App Events
        // Unity will call OnApplicationPause(false) when an app is resumed
        // from the background
        void OnApplicationPause(bool pauseStatus)
        {
            // Check the pauseStatus to see if we are in the foreground
            // or background
            if (!pauseStatus)
            {
                //app resume
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
                }
                else
                {
                    //Handle FB.Init
                    FB.Init(() =>
                    {
                        FB.ActivateApp();
                    });
                }
            }
        }
        #endregion Facebook App Events


        #endregion Unity Methods

        #region Public Methods

        public void RecordLevelStart(ParameterBlock parameterBlock)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level", parameterBlock.parameters);
        }

        public void RecordLevelComplete(ParameterBlock parameterBlock)
        {
            LevelCompleteStatus status = (LevelCompleteStatus)parameterBlock.parameters["status"];
            switch (status)
            {
                case LevelCompleteStatus.Success:
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level", parameterBlock.parameters);
                    break;
                case LevelCompleteStatus.TimeUp:
                case LevelCompleteStatus.Exited:
                    GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level", parameterBlock.parameters);
                    break;
            }
        }



        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #region Callbacks
        private void Callback_On_Ad_State_Changed(AdEventData adEventData)
        {
            ParameterBlock parameterBlock = new ParameterBlock();
            switch (adEventData.adState)
            {
                case AdState.BANNER_AD_REVENUE_PAID:
                case AdState.INTERSTITIAL_REVENUE_PAID:
                case AdState.REWARDED_REVENUE_PAID:
                    //Conversion^^^^
                    parameterBlock.parameters.Add("state", adEventData.adState.ToString().ToLower());
                    parameterBlock.parameters.Add("adType", adEventData.adFormat);
                    parameterBlock.parameters.Add("revenu", adEventData.revenue);
                    parameterBlock.parameters.Add("network", adEventData.networkName);
                    parameterBlock.parameters.Add("revenuPrecision", !string.IsNullOrEmpty(adEventData.adRevenuePrecision) ? adEventData.adRevenuePrecision : "0");


                    GameAnalytics.NewDesignEvent(nameof(AnalyticsEvent.AdRevenueReceived), parameterBlock.parameters);
                    break;

                case AdState.INTERSTITIAL_DISPLAYED:
                case AdState.REWARDED_DISPLAYED:
                case AdState.BANNER_AD_LOADED:
                    //Impression^^^
                    parameterBlock.parameters.Add("state", adEventData.adState.ToString().ToLower());
                    parameterBlock.parameters.Add("adType", adEventData.adFormat);
                    parameterBlock.parameters.Add("revenu", adEventData.revenue);
                    parameterBlock.parameters.Add("network", adEventData.networkName);
                    parameterBlock.parameters.Add("revenuPrecision", adEventData.adRevenuePrecision);

                    GameAnalytics.NewDesignEvent(nameof(AnalyticsEvent.AdDisplayed), parameterBlock.parameters);
                    break;

                    //case AdState.INTERSTITIAL_DISMISSED:
                    //case AdState.REWARDED_DISMISSED:
                    //    parameterBlock.parameters.Add("state", adEventData.adState.ToString().ToLower());
                    //    parameterBlock.parameters.Add("adType", adEventData.adFormat);
                    //    parameterBlock.parameters.Add("revenu", adEventData.revenue);
                    //    parameterBlock.parameters.Add("revenuPrecision", adEventData.adRevenuePrecision);
                    //    parameterBlock.parameters.Add("network", adEventData.networkName;
                    //    GameAnalytics.NewDesignEvent(nameof(AnalyticsEvent.AdRevenueReceived), parameterBlock.parameters);
                    //    break;

                    //case AdState.BANNER_AD_FAILED_TO_DISPLAY:
                    //case AdState.INTERSTITIAL_FAILED_TO_DISPLAY:
                    //case AdState.REWARDED_FAILED_TO_DISPLAY:
                    //case AdState.BANNER_AD_FAILED_TO_LOAD:
                    //case AdState.INTERSTITIAL_FAILED_TO_LOAD:
                    //case AdState.REWARDED_FAILED_TO_LOAD:
                    //    parameterBlock.parameters.Add("state", adEventData.adState.ToString().ToLower());
                    //    parameterBlock.parameters.Add("adType", adEventData.adFormat);
                    //    parameterBlock.parameters.Add("revenu", adEventData.revenue);
                    //    parameterBlock.parameters.Add("revenuPrecision", adEventData.adRevenuePrecision);
                    //    parameterBlock.parameters.Add("network", adEventData.networkName);
                    //    parameterBlock.parameters.Add("errorInfo", MyUtils.ObjectToJson<MaxSdk.ErrorInfo>(adEventData.errorInfo));
                    //    GameAnalytics.NewDesignEvent(nameof(AnalyticsEvent.AdRevenueReceived), parameterBlock.parameters);
                    //    break;

            }
            MyUtils.Log($"PARAMETERS:::: {MyUtils.ObjectToJson(parameterBlock.parameters)}");
        }
        #endregion Callbacks
    }
}