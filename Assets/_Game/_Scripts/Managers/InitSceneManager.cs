using UnityEngine;
using UnityEngine.SceneManagement;
using BenStudios.ScreenManagement;
using BenStudios;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

public class InitSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject m_reporterPrefab;
    private const string PRODUCTION_ENVIRONMENT = "production";
    private const string DEVELOPMENT_ENVIRONMENT = "development";
    private void Start()
    {
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
                   SceneManager.UnloadSceneAsync(Konstants.INIT_SCENE);
               };
           }
       };


    }
}
