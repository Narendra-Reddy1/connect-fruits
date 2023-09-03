using BenStudios;
using BenStudios.IAP;
using BenStudios.ScreenManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportDevAskScreen : PopupBase
{
    public override void OnCloseClick()
    {
        ScreenManager.Instance.CloseLastAdditiveScreen();
    }

}
