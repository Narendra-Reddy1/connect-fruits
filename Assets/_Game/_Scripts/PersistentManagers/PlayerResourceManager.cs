using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BenStudios.Economy
{
    [System.Serializable]
    public class PlayerResourceManager : MonoBehaviour
    {
        public static PlayerResourceManager Instance;
        public static bool debugMode = true;

        //bool isStoreManagerInitialized = false;

        private void Awake()
        {
            // if the singleton hasn't been initialized yet
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            Instance = this;
            //DontDestroyOnLoad (this.gameObject);
        }

        void Start()
        {
            InitializeStoreItems();
            updateValueStoreItems();
        }

        public static string COINS_ITEM_ID = "COIN_CURRENCY";
        public static string FRUIT_BOMB_POWERUP_ITEM_ID = "UNDO_POWERUP";
        public static string TRIPLE_BOMB_POWERUP_ITEM_ID = "RETUR3_POWERUP";
        public static string FRUIT_DUMPER_POWERUP_ITEM_ID = "SHUFFLE_POWERUP";


        [SerializeField]
        public static Dictionary<string, VirtualItem> StoreInventory;

        [Header("Currencies")]
        public Currency coins;
        public PowerUp fruitBombPowerup;
        public PowerUp tripleBombPowerUp;
        public PowerUp fruitDumperPowerUp;

        public void updateValueStoreItems()
        {

        }

        /// <summary>
        /// Initializes the store items.
        /// Call this Method the very first time the store is used
        /// </summary>
        public void InitializeStoreItems()
        {
            if (StoreInventory == null)
            {
                StoreInventory = new Dictionary<string, VirtualItem>();
            }
            //isStoreManagerInitialized = true;

            //Currency define - Start everything with zero balance..aditional items can be given later
            coins = new Currency(COINS_ITEM_ID);
            fruitBombPowerup = new PowerUp(FRUIT_BOMB_POWERUP_ITEM_ID, COINS_ITEM_ID, Konstants.FRUIT_BOMB_POWERUP_COST);
            tripleBombPowerUp = new PowerUp(TRIPLE_BOMB_POWERUP_ITEM_ID, COINS_ITEM_ID, Konstants.TRIPLE_BOMB_POWERUP_COST);
            fruitDumperPowerUp = new PowerUp(FRUIT_DUMPER_POWERUP_ITEM_ID, COINS_ITEM_ID, Konstants.FRUIT_DUMPER_POWERUP_COST);
            //Add the item to the inventory.
            addItemToStoreInventory(coins);
            addItemToStoreInventory(fruitBombPowerup);
            addItemToStoreInventory(tripleBombPowerUp);
            addItemToStoreInventory(fruitDumperPowerUp);
        }

        void addItemToStoreInventory(VirtualItem item)
        {
            if (!StoreInventory.ContainsKey(item.itemId))
                StoreInventory.Add(item.itemId, item);
        }

        public void LoadDataForInspector()
        {
            //Currency
            coins = (Currency)StoreInventory[COINS_ITEM_ID];
        }

        /// <summary>
        /// Gets the item balance -
        /// Returns Balance for Currency, Consumable, Equipable
        /// Returns Current Level for Upgradable
        /// </summary>
        /// <returns>The item balance.</returns>
        public static int GetCoinsBalance() => StoreInventory[COINS_ITEM_ID].GetBalance();
        public static int GetBalance(string itemId)
        {
            switch (StoreInventory[itemId].virtualItemType)
            {

                case VirtualItemType.CURRENCY:
                    return ((Currency)StoreInventory[itemId]).GetBalance();

                case VirtualItemType.CONSUMABLE:
                    return ((Consumable)StoreInventory[itemId]).GetBalance();

                //case VirtualItemType.EQUIPABLE:
                //  return ((Equipable)StoreInventory[itemId]).GetBalance();

                // case VirtualItemType.UPGRADABLE:
                // return ((Upgradable)StoreInventory[itemId]).GetBalance();

                case VirtualItemType.POWERUP:
                    return ((PowerUp)StoreInventory[itemId]).GetBalance();

                default:
                    Debug.Log("Invalid Item ID");

                    return -1;
            }



            //if (StoreInventory[itemId].virtualItemType == VirtualItemType.CURRENCY)
            //{
            //    return ((Currency)StoreInventory[itemId]).GetBalance();
            //}
            //else if (StoreInventory[itemId].virtualItemType == VirtualItemType.CONSUMABLE)
            //{
            //    return ((Consumable)StoreInventory[itemId]).GetBalance();
            //}
            //else if (StoreInventory[itemId].virtualItemType == VirtualItemType.EQUIPABLE)
            //{
            //    return ((Equipable)StoreInventory[itemId]).GetBalance();
            //}
            //else
            //{
            //    //UPGRADABLE
            //    return ((Upgradable)StoreInventory[itemId]).GetBalance();
            //}
        }

        public static int GetCollectedBalance(string itemId)
        {
            switch (StoreInventory[itemId].virtualItemType)
            {

                case VirtualItemType.CURRENCY:
                    return ((Currency)StoreInventory[itemId]).GetCollectedBalance();
                case VirtualItemType.POWERUP:
                    return ((PowerUp)StoreInventory[itemId]).GetCollectedBalance();

                default:
                    Debug.Log("Invalid Item ID");

                    return -1;
            }
        }

        public static int GetNonCollectedBalance(string itemId)
        {
            switch (StoreInventory[itemId].virtualItemType)
            {

                case VirtualItemType.CURRENCY:
                    return ((Currency)StoreInventory[itemId]).GetNonCollectedBalance();
                case VirtualItemType.POWERUP:
                    return ((PowerUp)StoreInventory[itemId]).GetNonCollectedBalance();

                default:
                    Debug.Log("Invalid Item ID");

                    return -1;
            }
        }



        /// <summary>
        /// This method gives the coin balance .
        /// </summary>
        /// <returns>Coin Balance</returns>
        public int GetCoinBalance()
        {
            return GetBalance(COINS_ITEM_ID);
        }

        /// <summary>
        /// This Method Adds the given amount of coins .
        /// </summary>
        /// <param name="quantity"></param>
        public void AddCoins(int quantity)
        {
            Give(COINS_ITEM_ID, quantity);
            PlayerDataManager.instance.SaveData();
        }


        /// <summary>
        /// This method removes the given amount of coins.
        /// If the  balance is less than 0 it will resets to 0.
        /// </summary>
        /// <param name="quantity"></param>
        public void DeductCoins(int quantity)
        {
            Take(COINS_ITEM_ID, quantity);
            PlayerDataManager.instance.SaveData();
        }


        public static void ResetBalance(string itemId)
        {
            if (StoreInventory[itemId].virtualItemType == VirtualItemType.CURRENCY)
            {
                ((Currency)StoreInventory[itemId]).ResetBalance();
            }
            PlayerDataManager.instance.SaveData();
            //else if (StoreInventory[itemId].virtualItemType == VirtualItemType.CONSUMABLE)
            //{
            //	 ((Consumable)StoreInventory[itemId]).ResetBalance();
            //}
            //else if (StoreInventory[itemId].virtualItemType == VirtualItemType.EQUIPABLE)
            //{
            //	return ((Equipable)StoreInventory[itemId]).ResetBalance();
            //}
            //else
            //{
            //	//UPGRADABLE
            // ((Currency)StoreInventory[itemId]).GetBalance();
            //}
        }
        public static void Take(string itemId)
        {
            Take(itemId, 1);
        }

        public static void Take(string itemId, int quantity)
        {
            switch (StoreInventory[itemId].virtualItemType)
            {
                case VirtualItemType.CURRENCY:
                    ((Currency)StoreInventory[itemId]).Take(quantity);
                    break;
                case VirtualItemType.CONSUMABLE:
                    ((Consumable)StoreInventory[itemId]).Take(quantity);
                    break;
                // case VirtualItemType.EQUIPABLE:
                // ((Equipable)StoreInventory[itemId]).Take(quantity);
                // break;
                //case VirtualItemType.UPGRADABLE:
                //   break;
                case VirtualItemType.POWERUP:
                    ((PowerUp)StoreInventory[itemId]).Take(quantity);
                    break;
            }
            PlayerDataManager.instance.SaveData();
            invokeStoreGiveCallback();
        }

        public static void Give(string itemId)
        {
            Give(itemId, 1);
        }

        public static void Give(string itemId, int quantity)
        {
            if (debugMode)
            {
                Debug.Log("BEfore Give - " + itemId + " - " + quantity + "\nCurrent Balance - " + PlayerResourceManager.GetBalance(itemId));
            }

            switch (StoreInventory[itemId].virtualItemType)
            {
                case VirtualItemType.CURRENCY:
                    ((Currency)StoreInventory[itemId]).Give(quantity);
                    break;

                case VirtualItemType.CONSUMABLE:
                    ((Consumable)StoreInventory[itemId]).Give(quantity);
                    break;

                // case VirtualItemType.EQUIPABLE:
                //  ((Equipable)StoreInventory[itemId]).Give(quantity);
                //  break;

                case VirtualItemType.POWERUP:
                    ((PowerUp)StoreInventory[itemId]).Give(quantity);
                    break;

                default:
                    Debug.Log("Invalid Item ID");
                    break;
            }

            PlayerDataManager.instance.SaveData();
            invokeStoreGiveCallback();
        }

        public static void UpdateToPlayerData(string itemId)
        {

            switch (StoreInventory[itemId].virtualItemType)
            {
                case VirtualItemType.CURRENCY:
                    ((Currency)StoreInventory[itemId]).UpdateBalance();
                    break;
                case VirtualItemType.POWERUP:
                    ((PowerUp)StoreInventory[itemId]).UpdateBalance();
                    break;
            }

            PlayerDataManager.instance.SaveData();
            invokeStoreGiveCallback();
        }

        public static bool Buy(string itemId)
        {
            switch (StoreInventory[itemId].virtualItemType)
            {
                case VirtualItemType.CURRENCY:
                    return ((Currency)StoreInventory[itemId]).Buy();
                case VirtualItemType.CONSUMABLE:
                    return ((Consumable)StoreInventory[itemId]).Buy();
                //case VirtualItemType.EQUIPABLE:
                //return ((Equipable)StoreInventory[itemId]).Buy();
                // case VirtualItemType.UPGRADABLE:
                //   return ((Upgradable)StoreInventory[itemId]).Buy();
                //case VirtualItemType.POWERUP:
                // break;
                default:
                    MyUtils.Log($"PlayerResousrces::BUY:: DefaultCase::{itemId}");
                    return false;
            }
        }

        public static string GetPurchaseCurrency(string itemId)
        {
            switch (StoreInventory[itemId].virtualItemType)
            {
                case VirtualItemType.CURRENCY:
                    return ((Currency)StoreInventory[itemId]).GetPurchaseCurrency();
                case VirtualItemType.CONSUMABLE:
                    return ((Consumable)StoreInventory[itemId]).GetPurchaseCurrency();
                //case VirtualItemType.EQUIPABLE:
                // return ((Equipable)StoreInventory[itemId]).GetPurchaseCurrency();
                // case VirtualItemType.UPGRADABLE:
                // return ((Upgradable)StoreInventory[itemId]).GetPurchaseCurrency();
                //   case VirtualItemType.POWERUP:
                default:
                    MyUtils.Log($"PlayerResourcesManager::GetPurchaseCurrency::DefaultCase::{itemId}");
                    return COINS_ITEM_ID;
            }
        }

        public static int GetPurchaseValue(string itemId)
        {
            switch (StoreInventory[itemId].virtualItemType)
            {
                case VirtualItemType.CURRENCY:
                    return ((Currency)StoreInventory[itemId]).GetPurchaseValue();
                case VirtualItemType.CONSUMABLE:
                    return ((Consumable)StoreInventory[itemId]).GetPurchaseValue();
                // case VirtualItemType.EQUIPABLE:
                // return ((Equipable)StoreInventory[itemId]).GetPurchaseValue();
                //case VirtualItemType.UPGRADABLE:
                // return ((Upgradable)StoreInventory[itemId]).GetPurchaseValue();
                //case VirtualItemType.POWERUP:
                default:
                    MyUtils.Log($"PlayerResourcesManager::GetPurchaseValue::DefaultCase::{itemId}");
                    return 0;

            }
        }

        public static int GetMaxBalance(string itemId)
        {
            if (StoreInventory[itemId].virtualItemType == VirtualItemType.CURRENCY)
            {
                return ((Currency)StoreInventory[itemId]).MaxBalance;
            }
            else if (StoreInventory[itemId].virtualItemType == VirtualItemType.CONSUMABLE)
            {
                return 999999;
            }
            else if (StoreInventory[itemId].virtualItemType == VirtualItemType.EQUIPABLE)
            {
                return 1;
            }
            else
            {
                //UPGRADABLE
                return 5;
            }
        }


        public delegate void StoreGiveCallback();
        public static event StoreGiveCallback onStoreGiveCallback;
        public static void invokeStoreGiveCallback()
        {
            if (onStoreGiveCallback != null)
            {
                onStoreGiveCallback();
            }

        }

    }
}