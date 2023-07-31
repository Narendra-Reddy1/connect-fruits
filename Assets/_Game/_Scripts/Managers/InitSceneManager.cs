using UnityEngine;
using UnityEngine.SceneManagement;

public class InitSceneManager : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadSceneAsync(Konstants.PERSISTENT_MANAGERS, LoadSceneMode.Additive).completed += (handle) =>
        {
            if (handle.isDone)
            {
                SceneManager.LoadSceneAsync(Konstants.HOME_SCENE, LoadSceneMode.Additive).completed += (handle1) =>
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(Konstants.HOME_SCENE));
                    SceneManager.UnloadSceneAsync(Konstants.INIT_SCENE);
                };
            }
        };


    }
}
