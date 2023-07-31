using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deftouch
{
    public interface IWindowBase
    {
        public void StartAnimation();
        public void EndAnimation(Action OnAnimationComplete);
        public void OnCloseClick();
    }
}
