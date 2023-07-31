using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public enum Window
{
    None,
    DashboardScreen,
    Gameplay,
    CommonInfoPopup,
    GenericOutOfScreen,
    LevelCompleteScreen,
    ExitConfirmationPopup,
    SettingPopup,
    CoinBankPopup,
    StorePopup,
    OutOfLivesPopup,
    OutOfTimePopup,
    ZenPassScreen,
    GoldenTileRewardScreen,
    PurchaseFailPopup,
    ShopScreen,
    ChestRewardScreen,
    PurchaseSuccessPopup,
    PurchaseLoadingScreen,
    Console,
    HowToPlayPopup,
    NoAdsPopup,
    ComingSoonPopup,
    MiniStore,
    TimedEventPopup,
    DebugPanel,
    ProfileEdit,
    LevelFailedPopup,
    BoosterSelectionPopUp,
    MiniStorePopup,

    LimitedTimeDeal,

    CoinRewardScreen,
    MainNavigation,
    PausePopup,
    FashOffersView,

    TutorialUI,
    Tutorial3dObjects,
    PurchaseStatus,
    NewObjectsUnlocked,
    DebugLevelDataScreen,
    PiggyBankCoinAnimation,
    RewardMultiplier,
    NewObjectsProgressStrip,
    GameAccountDelete,
    ConfirmDelete,
    MissionPassInfoScreen,
    MissionPassActivatePopup,
    MissionPassScreen,
    MissionPassPopup,
    MissionPassRewardClaimPopup,
}

[System.Serializable]
public class WindowObject
{
    [SerializeField]
    private GameObject gameObjectRef;
    private GameObject gameObject;

    #region Public Methods
    public ref GameObject GetObj()
    {
        return ref gameObject;
    }
    public GameObject Instantiate(Transform parent, bool instantiateInWorldSpace)
    {
        gameObject = GameObject.Instantiate(gameObjectRef, parent, instantiateInWorldSpace);
        return gameObject;
    }
    public void Release()
    {
        GameObject.Destroy(gameObject);
    }
    #endregion
}

[System.Serializable]
public class WindowAddressableObject
{
    [SerializeField]
    private AssetReferenceGameObject assetRefObj;
    private AsyncOperationHandle<GameObject> asyncObj;
    [HideInInspector]
    public GameObject gameObject;
    [HideInInspector]
    public bool IsObjectExist = false;
    private bool IsObjectinstantiating = false;
    #region Public Methods
    public ref GameObject GetObj()
    {
        return ref gameObject;
    }
    public void Instantiate(Transform parent, bool instantiateInWorldSpace, Action<bool> OnComplete)
    {
        if (IsObjectinstantiating)
        {
            OnComplete.Invoke(false);
        }
        IsObjectinstantiating = true;
        if (IsObjectExist)
        {
            OnComplete.Invoke(true);
            return;
        }


        Debug.Log("asset ref obj " + assetRefObj);
        Debug.Log("parent " + parent);
        Debug.Log("instantiateInWorldSpace " + instantiateInWorldSpace);
        Debug.Log("AddressableAssetLoader.Instance. " + AddressableAssetLoader.Instance);

        AddressableAssetLoader.Instance.Instantiate(assetRefObj, parent, instantiateInWorldSpace,
            (status, _asynobj) =>
            {
                IsObjectinstantiating = false;
                if (status)
                {
                    asyncObj = _asynobj;
                    gameObject = asyncObj.Result;
                    IsObjectExist = true;
                    OnComplete.Invoke(true);
                }
                else
                {
                    OnComplete.Invoke(false);
                    IsObjectExist = false;
                }
            });
    }
    public void Release()
    {
        IsObjectinstantiating = false;
        if (IsObjectExist)
        {
            AddressableAssetLoader.Instance.Release(asyncObj);
            // assetRefObj.ReleaseAsset();

        }
        IsObjectExist = false;
        gameObject = null;
    }
    #endregion
}
