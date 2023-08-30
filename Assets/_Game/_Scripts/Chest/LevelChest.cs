using BenStudios;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelChest : MonoBehaviour
{
    [SerializeField]
    private Image ChestImageHolder;
    [SerializeField]
    private Image ChestBgImageHolder;
    [SerializeField]
    private Sprite[] Open_Close_Chest;
    [SerializeField] private GameObject m_unlockedBG;
    [SerializeField]
    private GameObject[] object_To_Disable_Enable;
    [SerializeField]
    public Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField]
    private Button ChestButton;

    private void OnEnable()
    {
        GlobalEventHandler.EventOnLevelChestClaimed += Callback_On_Level_Chest_Claimed;
        GlobalEventHandler.EventOnLevelChestOpenedButNotClaimed += Callback_On_Level_Chest_Opened_But_Not_Claimed;
        Init();
    }

    private void OnDisable()
    {
        GlobalEventHandler.EventOnLevelChestClaimed -= Callback_On_Level_Chest_Claimed;
        GlobalEventHandler.EventOnLevelChestOpenedButNotClaimed -= Callback_On_Level_Chest_Opened_But_Not_Claimed;
    }

    private void Init()
    {
        UpdateUI();
    }

    private bool CheckChestNotClaimed()
    {
        return PlayerPrefsWrapper.GetPlayerPrefsBool(PlayerPrefKeys.is_level_chest_opened_but_not_claimed);
    }


    private void UpdateUI()
    {
#if DEBUG_DEFINE
        GlobalVariables.CurrentSelectedLevel = PlayerPrefsWrapper.GetPlayerPrefsInt(PlayerPrefKeys.debug_current_level, GlobalVariables.CurrentSelectedLevel);
#endif
        int last_level = PlayerPrefsWrapper.GetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_level, 0);
        int expectedRewardLevel = PlayerPrefsWrapper.GetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_level, 0) + GlobalVariables.levelChestRewardOccurence;
        expectedRewardLevel = expectedRewardLevel % 5 == 0 ? expectedRewardLevel : expectedRewardLevel + 1;
        int currentLevel = GlobalVariables.highestUnlockedLevel;

        progressBar.minValue = last_level == 0 ? last_level : last_level + 1;
        progressBar.maxValue = expectedRewardLevel;
        progressBar.value = currentLevel;
        progressText.text = $"{currentLevel}/{expectedRewardLevel}";
        if (currentLevel < expectedRewardLevel)
        {
            ChestImageHolder.sprite = Open_Close_Chest[0];
            m_unlockedBG.SetActive(false);
            ObjectActiveIfGoalNotReached(true);
            ChestButton.interactable = false;
        }
        else
        {
            ChestImageHolder.sprite = Open_Close_Chest[0];
            m_unlockedBG.SetActive(true);
            ObjectActiveIfGoalNotReached(false);
            ChestButton.interactable = true;
        }
    }

    private void ObjectActiveIfGoalNotReached(bool isActive)
    {
        object_To_Disable_Enable[0].SetActive(isActive);
        object_To_Disable_Enable[1].SetActive(isActive);
        object_To_Disable_Enable[2].SetActive(!isActive);
    }

    private int GetUnClaimedChest()
    {
        //last claimed
        int last_claimed = PlayerPrefsWrapper.GetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_level, 0);
        int temp_level_claimed = -1;

        if (last_claimed == 0)
        {
            //Debug.Log("New Chest Unlocked at " + GlobalVariables.lastLevelChestRewardDefinedAt);
            return GlobalVariables.lastLevelChestRewardDefinedAt;
        }

        if ((last_claimed + GlobalVariables.levelChestRewardOccurence) <= GlobalVariables.highestUnlockedLevel)
        {
            temp_level_claimed = last_claimed + GlobalVariables.levelChestRewardOccurence;
        }

        //Debug.Log("New Chest Unlocked at " + temp_level_claimed);
        return temp_level_claimed;
    }

    private void Callback_On_Level_Chest_Claimed()
    {
        PlayerPrefsWrapper.SetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_level, GetUnClaimedChest());
        PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_level_chest_opened_but_not_claimed, false);
        Init();
    }

    private void Callback_On_Level_Chest_Opened_But_Not_Claimed()
    {
        ChestImageHolder.sprite = Open_Close_Chest[1];
    }
}