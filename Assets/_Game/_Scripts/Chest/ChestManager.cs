using System.Collections.Generic;
using UnityEngine;
using BenStudios.ScreenManagement;
using BenStudios.Economy;

namespace BenStudios
{
    public class ChestManager : MonoBehaviour
    {
        public static ChestManager Instance { get; private set; }
        [SerializeField] private RewardData rewardData;
        [SerializeField] private TextureDatabase m_textureDatabase;
        #region Unity Methods
        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            InitReward();
        }
        private void OnDisable()
        {
            Instance = null;
            Destroy(this.gameObject);
        }

        #endregion

        private void InitReward()
        {
            int lastRewardLevel = PlayerPrefsWrapper.GetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_level, GlobalVariables.lastLevelChestRewardDefinedAt);


            if (lastRewardLevel + GlobalVariables.levelChestRewardOccurence < GlobalVariables.highestUnlockedLevel)
            {
                int gapBetweenLevels = GlobalVariables.highestUnlockedLevel - lastRewardLevel;
                MyUtils.Log("level : gapBetweenLevels " + gapBetweenLevels, LogType.Error);
                int multiplerRequired = gapBetweenLevels / GlobalVariables.levelChestRewardOccurence;
                //Debug.LogError("level : multiplerRequired " + multiplerRequired);
                int remainingLevels = gapBetweenLevels % GlobalVariables.levelChestRewardOccurence;

                if (remainingLevels == 0)
                {
                    int expectedRewardLevel = (GlobalVariables.highestUnlockedLevel - GlobalVariables.levelChestRewardOccurence);
                    Debug.LogError("Setting last_level_reward_level" + expectedRewardLevel + " remainingLevels " + remainingLevels);
                    PlayerPrefsWrapper.SetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_level, expectedRewardLevel);
                }
                else
                {
                    int expectedLastLevel = lastRewardLevel + (GlobalVariables.levelChestRewardOccurence * multiplerRequired);
                    Debug.LogError("Setting last_level_reward_level" + expectedLastLevel + " remainingLevels " + remainingLevels);
                    PlayerPrefsWrapper.SetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_level, expectedLastLevel);
                }
            }

            Debug.Log("last_level_reward_level : " + PlayerPrefsWrapper.GetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_level));
        }

        public void LevelChestReward()
        {
            //if (GlobalVariables.currentSelectedLevel + 1 % 5 != 0) { return; }

            List<ChestAnimator.Reward> newRewardList = new List<ChestAnimator.Reward>();
            newRewardList.Clear();

            int getLastRewardIndex = PlayerPrefsWrapper.GetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_index, 0);
            Debug.Log("getLastRewardIndex " + getLastRewardIndex);
            getLastRewardIndex = getLastRewardIndex >= rewardData.LevelRewardDataCount ? 0 : getLastRewardIndex;

            List<Reward> chestRewards = rewardData.GetLevelChestData(getLastRewardIndex);
            //.levelReward[getLastRewardIndex].rewardData;
            Debug.Log("chestRewards " + chestRewards.Count);

            for (int i = 0; i < chestRewards.Count; i++)
            {
                newRewardList.Add(new ChestAnimator.Reward()
                {
                    rewardId = GetResourceIdBasedOnRewardType(chestRewards[i].resourceType),
                    rewardSprite = m_textureDatabase.GetSprite(chestRewards[i].resourceType),
                    rewardValue = chestRewards[i].rewardCount
                });
            }

            PlayerPrefsWrapper.SetPlayerPrefsBool(PlayerPrefKeys.is_level_chest_opened_but_not_claimed, true);

            ScreenManager.Instance.ChangeScreen(Window.ChestRewardScreen, ScreenType.Additive, true, onComplete: () =>
            {
                ChestRewardScreen.Instance.ShowChestReward(newRewardList, ChestType.LEVEL, () =>
                {

                    for (int i = 0; i < newRewardList.Count; i++)
                    {
                        PlayerResourceManager.Give(newRewardList[i].rewardId, newRewardList[i].rewardValue);
                    }

                    PlayerPrefsWrapper.SetPlayerPrefsInt(PlayerPrefKeys.last_level_reward_index, ++getLastRewardIndex);
                    GlobalEventHandler.EventOnLevelChestClaimed?.Invoke();
                }, () =>
                {
                    Debug.Log("Chest Not Claimed");
                    GlobalEventHandler.EventOnLevelChestOpenedButNotClaimed?.Invoke();
                    ScreenManager.Instance.CloseLastAdditiveScreen();
                });
            });
        }


        public void StarChestReward()
        {
            //if (GlobalVariables.currentSelectedLevel + 1 % 5 != 0) { return; }

            List<ChestAnimator.Reward> newRewardList = new List<ChestAnimator.Reward>();
            newRewardList.Clear();

            int getLastRewardIndex = PlayerPrefsWrapper.GetPlayerPrefsInt(PlayerPrefKeys.last_star_chest_reward_index, 0);
            getLastRewardIndex = getLastRewardIndex >= rewardData.StarRewardDataCount ? 0 : getLastRewardIndex;
            List<Reward> chestRewards = rewardData.GetStarRewardData(getLastRewardIndex);

            for (int i = 0; i < chestRewards.Count; i++)
            {
                newRewardList.Add(new ChestAnimator.Reward()
                {
                    rewardId = GetResourceIdBasedOnRewardType(chestRewards[i].resourceType),
                    rewardSprite = m_textureDatabase.GetSprite(chestRewards[i].resourceType),
                    rewardValue = chestRewards[i].rewardCount
                });
            }

            ScreenManager.Instance.ChangeScreen(Window.ChestRewardScreen, ScreenType.Additive, true, onComplete: () =>
            {
                ChestRewardScreen.Instance.ShowChestReward(newRewardList, ChestType.STAR, () =>
                 {
                     for (int i = 0; i < newRewardList.Count; i++)
                     {
                         PlayerResourceManager.Give(newRewardList[i].rewardId, newRewardList[i].rewardValue);
                     }

                     PlayerPrefsWrapper.SetPlayerPrefsInt(PlayerPrefKeys.last_star_chest_reward_index, ++getLastRewardIndex);
                     GlobalEventHandler.EventOnStarChestClaimed?.Invoke();
                     Debug.Log("Chest Claimed");
                 }, () =>
                 {
                     Debug.Log("Chest Not Claimed");
                     GlobalEventHandler.EventOnStarChestOpenedButNotClaimed?.Invoke();
                     ScreenManager.Instance.CloseLastAdditiveScreen();
                 });
            });

        }
        public string GetResourceIdBasedOnRewardType(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.Coin:
                    return PlayerResourceManager.COINS_ITEM_ID;
                case ResourceType.FruitBomb:
                    return PlayerResourceManager.FRUIT_BOMB_POWERUP_ITEM_ID;
                case ResourceType.TripleBomb:
                    return PlayerResourceManager.TRIPLE_BOMB_POWERUP_ITEM_ID;
                case ResourceType.HintPowerup:
                    return PlayerResourceManager.HINT_POWERUP_ITEM_ID;
                default:
                    return string.Empty;
            }
        }
    }
}