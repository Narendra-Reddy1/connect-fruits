using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "newRewardData", menuName = "ScriptableObjects/Reward")]
public class RewardData : ScriptableObject
{
    [SerializeField] private List<LevelRewardData> levelReward = new List<LevelRewardData>();
    [SerializeField] private List<StarRewardData> starReward = new List<StarRewardData>();
    // public List<ObjectsUnlockRewardData> objectsUnlockRewardData = new List<ObjectsUnlockRewardData>();

    public int LevelRewardDataCount => levelReward.Count;
    public int StarRewardDataCount => starReward.Count;
    public List<Reward> GetLevelChestData(int index)
    {
        return levelReward[index].rewardData;
    }
    public List<Reward> GetStarRewardData(int index)
    {
        return starReward[index].rewardData;
    }

    #region DataStructs
    
    [Serializable]
    public class LevelRewardData
    {
        public List<Reward> rewardData = new List<Reward>();
    }

    [Serializable]
    public class StarRewardData
    {
        public List<Reward> rewardData = new List<Reward>();
    }

    //[Serializable]
    //public class ObjectsUnlockRewardData
    //{
    //    public List<Reward> rewardData = new List<Reward>();
    //}
    #endregion DataStructs
}


[System.Serializable]
public class Reward
{
    public ResourceType resourceType;
    public int rewardCount;
}