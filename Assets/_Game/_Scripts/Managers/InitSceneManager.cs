using UnityEngine;
using UnityEngine.SceneManagement;
using BenStudios.ScreenManagement;
using BenStudios;

public class InitSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject m_reporterPrefab;
    private void Start()
    {
        SceneManager.LoadSceneAsync(Konstants.PERSISTENT_MANAGERS, LoadSceneMode.Additive).completed += (handle) =>
        {
            if (handle.isDone)
            {
                GlobalEventHandler.RequestToPlayBGM?.Invoke(AudioID.DashboardBGM);
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
