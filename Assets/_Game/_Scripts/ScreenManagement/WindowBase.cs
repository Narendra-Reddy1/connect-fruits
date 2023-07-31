using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deftouch
{
    public abstract class WindowBase : MonoBehaviour, IWindowBase
    {
        public abstract void EndAnimation(Action OnAnimationComplete);
        public abstract void StartAnimation();
        public abstract void OnCloseClick();
    }
}
