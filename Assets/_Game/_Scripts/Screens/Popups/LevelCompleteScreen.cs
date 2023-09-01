using BenStudios.ScreenManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BenStudios
{
    public class LevelCompleteScreen : PopupBase
    {
        [SerializeField] private GameObject m_continueBtn;
        [SerializeField] private GameObject m_rewardsWaitingTxt;

        public override void OnEnable()
        {
            base.OnEnable();
            Init();
        }
        public void Init()
        {
            m_continueBtn.SetActive(CanShowContinueBtn());
            m_rewardsWaitingTxt.SetActive(!CanShowContinueBtn());
        }
        public void OnClickContinue()
        {
            GlobalVariables.currentGameplayMode = GameplayType.LevelMode;
            ScreenManager.Instance.ChangeScreen(Window.GameplayScreen, ScreenType.Replace, onComplete: () =>
            {
                GlobalEventHandler.RequestToPlayBGM?.Invoke(AudioID.GameplayBGM);
            });
        }
        public void OnClickExit()
        {
            GlobalEventHandler.RequestToPlayBGM?.Invoke(AudioID.DashboardBGM);
            ScreenManager.Instance.ChangeScreen(Window.Dashboard);
        }

        private bool CanShowContinueBtn()
        {
            int expectedRewardLevel = PlayerPrefsWrapper.GetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_level, 0) + GlobalVariables.levelChestRewardOccurence;
            expectedRewardLevel = expectedRewardLevel % 5 == 0 ? expectedRewardLevel : expectedRewardLevel + 1;

            return !((GlobalVariables.CollectedStars >= Konstants.MAX_STARS_REQUIRED_FOR_STAR_CHEST) || (GlobalVariables.highestUnlockedLevel >= expectedRewardLevel));
        }
    }
}