using BenStudios.ScreenManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BenStudios.IAP
{
    public class BasePack : MonoBehaviour
    {
        public string productID;

        public virtual void Buy()
        {
            ScreenManager.Instance.ChangeScreen(Window.PurchaseStatusScreen, ScreenType.Additive);
            GlobalEventHandler.RequestToInitializePurchase?.Invoke(productID);
        }
        public virtual void AssignProductID(BundleType bundleType)
        {
            switch (bundleType)
            {
                case BundleType.Starter:
                    productID = Konstants.STARTER_PACK;
                    break;
                case BundleType.Master:
                    productID = Konstants.MASTER_PACK;
                    break;
                case BundleType.CoinPack_1:
                    productID = Konstants.COIN_PACK_1;
                    break;
                case BundleType.CoinPack_2:
                    productID = Konstants.COIN_PACK_2;
                    break;
                case BundleType.CoinPack_3:
                    productID = Konstants.COIN_PACK_3;
                    break;
                case BundleType.CoinPack_4:
                    productID = Konstants.COIN_PACK_4;
                    break;
                case BundleType.CoinPack_5:
                    productID = Konstants.COIN_PACK_5;
                    break;
                case BundleType.FruitBombPack_1:
                    productID = Konstants.FRUIT_BOMB_PACK_1;
                    break;
                case BundleType.TripleBombPack_1:
                    productID = Konstants.TRIPLE_BOMB_PACK_1;
                    break;
                case BundleType.HindPowerupPack_1:
                    productID = Konstants.HINT_POWERUP_PACK_1;
                    break;
                case BundleType.No_Ads:
                    productID = Konstants.NO_ADS;
                    break;
                default:
                    MyUtils.Log($"Bundle type undefined:::AssignBundleID::BasePack:: {bundleType}", LogType.Exception);
                    break;
            }
        }
        public virtual void SetLocalizedPrice()
        {

        }
    }
}