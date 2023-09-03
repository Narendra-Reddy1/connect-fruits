using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BenStudios
{
    [CreateAssetMenu(fileName = "newStoreCatalogue", menuName = "ScriptableObjects/StoreCatalogue", order = 10)]
    public class StoreCatalogue : ScriptableObject
    {
        [Space(5)]
        public List<BundlePackData> bundlePacks;
        [Space(10)]
        public List<SinglePackData> singlePacks;
    }

    [System.Serializable]
    public class BundlePackData : IAPPackData
    {
        public string bundleTitle;
        public bool isOfferRunning;
        public SerializedDictionary<ResourceType, int> resourcesDictionary;

        public int GetResourceCountIfHave(ResourceType resourceType)
        {
            int resourceCount = 0;
            resourcesDictionary.TryGetValue(resourceType, out resourceCount);
            return resourceCount;
        }
    }
    [System.Serializable]
    public class SinglePackData : IAPPackData
    {
        public ResourceType resourceType;
        public int itemCount;
    }
    [System.Serializable]
    public class IAPPackData
    {
        public BundleType bundleType;
        public bool dontListInStore;
    }
    public enum BundleType
    {
        //bundles
        Starter,
        Master,
        Monster,

        //coin Packs
        CoinPack_1,
        CoinPack_2,
        CoinPack_3,
        CoinPack_4,
        CoinPack_5,
        //powerup packs
        FruitBombPack_1,
        TripleBombPack_1,
        HindPowerupPack_1,
        FruitBomb_Nano_Pack,
        No_Ads,
        //misc
        Support_Dev,

    }
}