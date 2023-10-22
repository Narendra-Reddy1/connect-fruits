using BenStudios;
using BenStudios.ScreenManagement;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StarChest : MonoBehaviour
{
    [SerializeField]
    private Image ChestImageHolder;
    [SerializeField]
    private Image ChestBgImageHolder;
    [SerializeField]
    private Sprite[] Open_Close_Chest;
    [SerializeField]
    private GameObject[] object_To_Disable_Enable;
    [SerializeField]
    public Slider progressBar;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField]
    private Button ChestButton;
    [SerializeField] private RewardCollectionAnimator rewardCollectionAnimator;
    [SerializeField] private Transform chestPosition;

    private void OnEnable()
    {
        GlobalEventHandler.EventOnStarChestClaimed += Callback_On_Star_Chest_Claimed;
        GlobalEventHandler.EventOnStarChestOpenedButNotClaimed += Callback_On_Star_Chest_Opened_But_Not_Claimed;

        Init();
    }
    private void OnDisable()
    {

        GlobalEventHandler.EventOnStarChestClaimed -= Callback_On_Star_Chest_Claimed;
        GlobalEventHandler.EventOnStarChestOpenedButNotClaimed -= Callback_On_Star_Chest_Opened_But_Not_Claimed;

    }

    private void Init()
    {
        if (!GlobalVariables.isLevelCompletedSuccessfully)
            UpdateUI();
        if ((GlobalVariables.isLevelCompletedSuccessfully && GlobalVariables.CollectedStars < Konstants.MAX_STARS_REQUIRED_FOR_STAR_CHEST) && GlobalVariables.highestUnlockedLevel >= Konstants.MIN_LEVEL_FOR_STREAK)
        {
            StartCoroutine(WaitForStarAnimation());
            GlobalVariables.isLevelCompletedSuccessfully = false;
        }
        else
            UpdateUI();
        if (CheckChestNotClaimed())
        {
            ChestImageHolder.sprite = Open_Close_Chest[0];
            ObjectActiveIfGoalNotReached(false);
            return;
        }
        Debug.Log("INIT  " + CheckChestNotClaimed());
    }

    private bool CheckChestNotClaimed()
    {
        int count = PlayerPrefsWrapper.GetPlayerPrefsInt(PlayerPrefKeys.is_star_chest_opened_but_not_claimed);
        if (count > 0) { return true; }


        return false;
    }

    private void UpdateUI()
    {
        progressBar.minValue = 0;
        progressBar.maxValue = Konstants.MAX_STARS_REQUIRED_FOR_STAR_CHEST;
        progressBar.value = GlobalVariables.CollectedStars;
        progressText.DOText($"{GlobalVariables.CollectedStars}/{Konstants.MAX_STARS_REQUIRED_FOR_STAR_CHEST}", .2f, scrambleMode: ScrambleMode.Numerals);


        if (GlobalVariables.CollectedStars < Konstants.MAX_STARS_REQUIRED_FOR_STAR_CHEST)
        {
            ChestImageHolder.sprite = Open_Close_Chest[0];

            ChestButton.interactable = false;
            ObjectActiveIfGoalNotReached(true);
        }
        else
        {
            ChestImageHolder.sprite = Open_Close_Chest[0];
            ChestButton.interactable = true;
            ObjectActiveIfGoalNotReached(false);
        }
    }

    private void ObjectActiveIfGoalNotReached(bool isActive)
    {
        object_To_Disable_Enable[0].SetActive(isActive);
        object_To_Disable_Enable[1].SetActive(isActive);
        object_To_Disable_Enable[2].SetActive(!isActive);
    }

    IEnumerator WaitForStarAnimation()
    {
        GlobalEventHandler.RequestToScreenBlocker?.Invoke(true);
        yield return new WaitForSeconds(1f);
        MoveStarsToHUD(() =>
        {
            GlobalEventHandler.RequestToScreenBlocker?.Invoke(false);
        });
        yield return new WaitForSeconds(1f);
        UpdateUI();
    }
    [ContextMenu("DebugStarAnimation")]
    public void DebugStarAnimation()
    {
        MoveStarsToHUD(null);
    }

    public void MoveStarsToHUD(Action onActionComplete)
    {
        rewardCollectionAnimator.ShowRewardCollectionAnimation(() =>
        {
            chestPosition.transform.DOScale(1.05f, 0.1f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                GlobalEventHandler.RequestToScreenBlocker?.Invoke(false);
                onActionComplete?.Invoke();
            });
        });
    }


    private void Callback_On_Star_Chest_Claimed()
    {
        MyUtils.Log($"STAR CHEST CLAIMED......");
        PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_star_chest_opened_but_not_claimed, false);
        PlayerPrefsWrapper.SetPlayerPrefsInt(PlayerPrefKeys.collected_stars, 0);
        GlobalVariables.CollectedStars = 0;
        UpdateUI();
    }

    private void Callback_On_Star_Chest_Opened_But_Not_Claimed()
    {
        ChestImageHolder.sprite = Open_Close_Chest[1];
        PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_star_chest_opened_but_not_claimed, true);
    }

}
