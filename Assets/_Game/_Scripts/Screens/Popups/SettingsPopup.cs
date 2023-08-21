using UnityEngine;
using BenStudios.ScreenManagement;
using UnityEngine.UI;
using BenStudios;

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
        GlobalEventHandler.RequestToPauseTimer?.Invoke(true);
        m_musicToggle.onValueChanged.AddListener(_ToggleMusic);
        m_soundToggle.onValueChanged.AddListener(_ToggleSound);
        _Init();
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
        GlobalEventHandler.RequestToPauseTimer?.Invoke(false);
        ScreenManager.Instance.CloseLastAdditiveScreen();
    }
    public void OnClickExit()
    {
        ScreenManager.Instance.ChangeScreen(Window.ExitConfirmationPopup, ScreenType.Additive);
    }
    #endregion Public Methods

    #region Private Methods
    private void _Init()
    {
        m_musicToggle.isOn = PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.music_toggle, true);
        m_soundToggle.isOn = PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.sound_toggle, true);
        // _ToggleMusic(m_musicToggle.isOn);
        //_ToggleSound(m_soundToggle.isOn);
    }
    private void _ToggleMusic(bool value)
    {
        MyUtils.Log($"_ToggleMusic:{value}");
        m_musicToggleOff.SetActive(!m_musicToggle.isOn);
        m_musicToggleOn.SetActive(m_musicToggle.isOn);
        PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.music_toggle, value);
        GlobalEventHandler.OnMusicToggled?.Invoke(value);
    }

    private void _ToggleSound(bool value)
    {
        m_soundToggleOff.SetActive(!m_soundToggle.isOn);
        m_soundToggleOn.SetActive(m_soundToggle.isOn);
        PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.sound_toggle, value);
        GlobalEventHandler.OnSoundToggled?.Invoke(value);
    }

    #endregion Private Methods 

    #region Callbacks

    #endregion Callbacks

}
