using UnityEngine;
using UnityEngine.SceneManagement;
using BenStudios.ScreenManagement;
using BenStudios;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine.UI;
using DG.Tweening;
using GameAnalyticsSDK;

public class InitSceneManager : MonoBehaviour
{
    [SerializeField] private ProjectAssetManager m_projectAssetManager;

    [Header("Splash Screen")]
    [SerializeField] private Image m_fillbar;
    [SerializeField] private float m_fakeDuration = 2f;

    [Space(10)]
    [Space(10)]
    [Header("Debug")]
    [SerializeField] private GameObject m_reporterPrefab;


    private const string PRODUCTION_ENVIRONMENT = "production";
    private const string DEVELOPMENT_ENVIRONMENT = "development";
    private void Start()
    {
        Input.multiTouchEnabled = false;
        MaxSdk.SetSdkKey(m_projectAssetManager.projectSettingAssets.thirdPartySdkKeys.ApplovingSDK);
        MaxSdk.InitializeSdk();
        GameAnalytics.Initialize();
        GameAnalytics.onInitialize += (sender, args) =>
        {
            MyUtils.Log($"GameAnalytics Initialized:::{sender} :: {args}");
        };
        MaxSdkCallbacks.OnSdkInitializedEvent += (config) =>
        {
            MyUtils.Log($"Applovin SDK Initialized? {config.IsSuccessfullyInitialized}");
        };

        SceneManager.LoadSceneAsync(Konstants.PERSISTENT_MANAGERS, LoadSceneMode.Additive).completed += async (handle) =>
       {
           if (handle.isDone)
           {
               GlobalEventHandler.RequestToPlayBGM?.Invoke(AudioID.DashboardBGM);
               await UnityServices.InitializeAsync(new InitializationOptions()
#if UPLOAD_BUILD
                   .SetEnvironmentName(PRODUCTION_ENVIRONMENT)
#elif DEBUG_DEFINE || DEVELOPMENT_BUILD
                   .SetEnvironmentName(DEVELOPMENT_ENVIRONMENT)
#endif
                   );
               SceneManager.LoadSceneAsync(Konstants.HOME_SCENE, LoadSceneMode.Additive).completed += (handle1) =>
               {
#if DEVLOPMENT_BUILD || DEBUG_DEFINE
                   GameObject obj = Instantiate(m_reporterPrefab);
                   DontDestroyOnLoad(obj);
#else
                   m_reporterPrefab = null;
#endif
                   SceneManager.SetActiveScene(SceneManager.GetSceneByName(Konstants.HOME_SCENE));
                   ScreenManager.Instance.ChangeScreen(Window.Dashboard);
                   m_fillbar.DOFillAmount(.25f, m_fakeDuration / 4).onComplete += () =>
                   {
                       m_fillbar.DOFillAmount(0.5f, m_fakeDuration / 4).SetDelay(0.2f).onComplete += () =>
                       {
                           m_fillbar.DOFillAmount(1f, m_fakeDuration / 4).SetDelay(.1f).onComplete += () =>
                           {
                               SceneManager.UnloadSceneAsync(Konstants.INIT_SCENE);
                           };
                       };
                   };
               };
           }
       };


    }

    private void GameAnalytics_onInitialize(object sender, bool e)
    {
        throw new System.NotImplementedException();
    }
}
