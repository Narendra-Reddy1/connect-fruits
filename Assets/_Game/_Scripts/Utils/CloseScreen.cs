using BenStudios.ScreenManagement;
using UnityEngine;

public class CloseScreen : MonoBehaviour
{
    public void OnClickClose()
    {
        ScreenManager.Instance.CloseLastAdditiveScreen();
    }
}
