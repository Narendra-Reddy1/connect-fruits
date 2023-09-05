using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BenStudios.ScreenManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConsoleButton : MonoBehaviour
{
#if (DEBUG_DEFINE || DEVLOPMENT_BUILD)
    [SerializeField] private Button m_button;
    private void Awake()
    {

        m_button = GetComponent<Button>();
        m_button.interactable = false;
        SceneManager.sceneLoaded += (scene, loadMode) =>
        {
            if (scene == SceneManager.GetSceneByName(Konstants.HOME_SCENE))
                m_button.interactable = true;
        };
    }

#endif
    public void OnClickConsoleBtn()
    {
        ScreenManager.Instance.ChangeScreen(Window.Console, ScreenType.Additive, false);
    }
}
