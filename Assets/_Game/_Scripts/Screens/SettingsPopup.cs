using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BenStudios.ScreenManagement;
using UnityEngine.UI;

public class SettingsPopup : PopupBase
{
    #region Variables
    [SerializeField] private Toggle m_musicToggle;
    [SerializeField] private Toggle m_soundToggle;

    [SerializeField] private GameObject m_musicToggleOff;
    [SerializeField] private GameObject m_musicToggleOn;
    [SerializeField] private GameObject m_soundToggleOff;
    [SerializeField] private GameObject m_soundToggleOn;
    #endregion Variables

    #region Unity Methods
    public override void OnEnable()
    {
        base.OnEnable();
        m_musicToggle.onValueChanged.AddListener(_ToggleMusic);
        m_soundToggle.onValueChanged.AddListener(_ToggleSound);
    }
    public override void OnDisable()
    {
        m_musicToggle.onValueChanged.RemoveListener(_ToggleMusic);
        m_soundToggle.onValueChanged.RemoveListener(_ToggleSound);
    }

    #endregion Unity Methods

    #region Public Methods

    public override void OnCloseClick()
    {
        ScreenManager.Instance.CloseLastAdditiveScreen();
    }

    #endregion Public Methods

    #region Private Methods
    private void _ToggleMusic(bool value)
    {
        m_musicToggleOff.SetActive(!m_musicToggle.isOn);
        m_musicToggleOn.SetActive(m_musicToggle.isOn);
    }
    private void _ToggleSound(bool value)
    {
        m_soundToggleOff.SetActive(!m_soundToggle.isOn);
        m_soundToggleOn.SetActive(m_soundToggle.isOn);
    }
    #endregion Private Methods 

    #region Callbacks

    #endregion Callbacks

}
