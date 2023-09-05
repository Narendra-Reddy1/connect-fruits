using UnityEngine;

[CreateAssetMenu(fileName = "newAdUnitIds", menuName = "ScriptableObjects/AdUnits")]
public class AdUnitIds : ScriptableObject
{
    [Space(10)]
    [SerializeField] private string bannerAdId;
    [SerializeField] private string interstitialAdId;
    [SerializeField] private string rewardedAdId;
    [SerializeField] private string mrecAdId;
    [SerializeField] private string appOpenAdId;

    public string BannerAdId => bannerAdId;
    public string InterstitialAdId => interstitialAdId;
    public string RewardedAdId => rewardedAdId;
    public string MRECAdId => mrecAdId;
    public string AppOpenId => appOpenAdId;

}


