using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitFrenzy
{
    public class ScreenBlocker : MonoBehaviour
    {
        [SerializeField] private GameObject m_screenBlocker;


        private void OnEnable()
        {
            GlobalEventHandler.RequestToScreenBlocker += _BlockScreen;
        }
        private void OnDisable()
        {
            GlobalEventHandler.RequestToScreenBlocker -= _BlockScreen;
        }

        private void _BlockScreen(bool canBlock)
        {
            m_screenBlocker.SetActive(canBlock);
        }
    }
}