using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BenStudios
{
    [CreateAssetMenu(fileName = "newStoreCatalogue", menuName = "ScriptableObjects/StoreCatalogue", order = 10)]
    public class StoreCatalogue : ScriptableObject
    {
        public List<BundlePackData> bundlePacks;
        public List<SinglePackData> singlePacks;
    }

    [System.Serializable]
    public class BundlePackData
    {
        public string packID;
        public BundleType bundleType;
        public bool isOfferRunning;
        public SerializedDictionary<ResourceType, int> resourcesDictionary;
    }
    [System.Serializable]
    public class SinglePackData
    {
        public BundleType bundleType;
        public ResourceType resourceType;
        public int itemCount;
    }
    public enum BundleType
    {
        Starter,
        Master,
        Monster,

        //coin Packs
        CoinPack_1,
        CoinPack_2,
        CoinPack_3,
        CoinPack_4,
        CoinPack_5,
        FruitBombPack_1,
        TripleBombPack_1,
        FruitDumperPack_1,
    }
}