using BenStudios;
using BenStudios.ScreenManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugLevelsPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_levelInputField;
    [SerializeField] private Button m_nextLevelBtn;
    [SerializeField] private Button m_prevLevelBtn;
    private int currentLevel = 1;

    private void OnEnable()
    {
        m_levelInputField.onValueChanged.AddListener(OnLevelFieldValueChanged);
    }
    private void OnDisable()
    {
        m_levelInputField.onValueChanged.RemoveListener(OnLevelFieldValueChanged);

    }
    private void Start()
    {
        currentLevel = GlobalVariables.highestUnlockedLevel;
        m_levelInputField.SetTextWithoutNotify(currentLevel.ToString());

    }
    private void OnLevelFieldValueChanged(string str)
    {
        currentLevel = int.Parse(str);
        GlobalVariables.highestUnlockedLevel = currentLevel;
    }
    public void OnClickNextLevelBtn()
    {
        currentLevel++;
        GlobalVariables.highestUnlockedLevel = currentLevel;
        m_levelInputField.SetTextWithoutNotify(currentLevel.ToString());
    }
    public void OnClickPrevLevelBtn()
    {
        currentLevel--;
        if (currentLevel <= 0)
            currentLevel = 1;
        GlobalVariables.highestUnlockedLevel = currentLevel;
        m_levelInputField.SetTextWithoutNotify(currentLevel.ToString());
    }
    public void OnClickStart()
    {
        ScreenManager.Instance.RemoveAllScreens(() =>
        {
            ScreenManager.Instance.ChangeScreen(Window.GameplayScreen);

        }); 
    }
}
