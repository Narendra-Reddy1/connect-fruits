using System.Collections.Generic;
using UnityEngine;
using System;
using BenStudios;
using BenStudios.Economy;

namespace BenStudios
{
    public class PlayerDataManager : MonoBehaviour
    {
        #region SINGLETON
        public static PlayerDataManager instance { get; private set; }

        #endregion

        #region Variables

        private PlayerData m_playerData;
        public PlayerData PlayerData => m_playerData;

        private static bool m_isPlayerDataLoaded = false;
        //public string levelKey { get; private set; }

        public Func<bool> OnDataInitialized;
        #endregion

        #region Unity Built-In Methods

        private void Awake()
        {
            if (instance != this && instance != null)
            {
                Destroy(gameObject);
            }
            else if (instance == null)
            {
                instance = this;
            }
            MyUtils.Log($"Path: {Application.persistentDataPath}");
        }

        public void Start()
        {
            if (SessionManager.sessionCounter == 1)
            {
                //First time game is being opened
                m_playerData = new PlayerData();
                _AddFirstStartData();
            }
            else
            {
                //Load Data
                LoadData();
            }
            m_isPlayerDataLoaded = true;
            OnDataInitialized?.Invoke();
            //Save Data
            SaveData();
        }

        private void OnEnable()
        {
            OnDataInitialized += IsPlayerDataLoaded;
        }
        private void OnDisable()
        {
            OnDataInitialized -= IsPlayerDataLoaded;
        }

        #endregion

        #region Custom Methods
        public static bool IsPlayerDataLoaded()
        {
            return m_isPlayerDataLoaded;
        }

        /// <summary>
        /// This method is responsible for saving the player data.
        /// </summary>
        public void SaveData()
        {
            m_playerData.storeInventory = PlayerResourceManager.StoreInventory;
            m_playerData.levelIndex = GlobalVariables.highestUnlockedLevel;
            m_playerData.collectedStars = GlobalVariables.CollectedStars;
            DataSerializer.Save("playerData.dat", m_playerData);
        }

        /// <summary>
        /// This method is responsible for loading the player data.
        /// </summary>
        public void LoadData()
        {
            m_playerData = DataSerializer.Load<PlayerData>("playerData.dat");

            if (m_playerData == null)
            {
                Debug.Log(" Error with Loading..");
                m_playerData = new PlayerData();
                _AddFirstStartData();
                SaveData();
                return;
            }
            PlayerResourceManager.StoreInventory = m_playerData.storeInventory;
            GlobalVariables.highestUnlockedLevel = m_playerData.levelIndex;
            GlobalVariables.CollectedStars = m_playerData.collectedStars;
            Debug.Log("Done with Loading");
        }


        //public void UpdateHighestUnlockedLevel()
        //{
        //    if (GlobalVariables.CurrentSelectedLevelIndex >= GlobalVariables.HighestUnlockedLevel)
        //    {
        //        GlobalVariables.HighestUnlockedLevel = GlobalVariables.CurrentSelectedLevelIndex + 1;
        //        GlobalVariables.HighestUnlockedLevelKey = GlobalVariables.HighestUnlockedLevel + "_0";
        //        Debug.Log($"HighestUnlockedLevel: {GlobalVariables.HighestUnlockedLevel}, HighestUnlockedLevelKey{GlobalVariables.HighestUnlockedLevelKey}");
        //        SaveData();
        //    }
        //}



        private void _AddFirstStartData()
        {
            if (PlayerResourceManager.Instance == null)
            {
                MyUtils.Log("Store Manager not found", LogType.Error);
                Debug.Break(); // IF THIS BREAKS, FIGURE OUT A SOLUTION ON PRIORITY
            }
            PlayerResourceManager.Instance.InitializeStoreItems();
            PlayerResourceManager.Give(PlayerResourceManager.COINS_ITEM_ID, Konstants.DEFAULT_COIN_BALANCE);
            PlayerResourceManager.UpdateToPlayerData(PlayerResourceManager.COINS_ITEM_ID);

            PlayerResourceManager.Give(PlayerResourceManager.FRUIT_BOMB_POWERUP_ITEM_ID, Konstants.DEFAULT_FRUIT_BOMB_POWERUP_BALANCE);
            PlayerResourceManager.UpdateToPlayerData(PlayerResourceManager.FRUIT_BOMB_POWERUP_ITEM_ID);

            PlayerResourceManager.Give(PlayerResourceManager.TRIPLE_BOMB_POWERUP_ITEM_ID, Konstants.DEFAULT_TRIPLE_BOMB_POWERUP_BALANCE);
            PlayerResourceManager.UpdateToPlayerData(PlayerResourceManager.TRIPLE_BOMB_POWERUP_ITEM_ID);

            PlayerResourceManager.Give(PlayerResourceManager.HINT_POWERUP_ITEM_ID, Konstants.DEFAULT_HINT_POWERUP_BALANCE);
            PlayerResourceManager.UpdateToPlayerData(PlayerResourceManager.HINT_POWERUP_ITEM_ID);
            m_playerData.levelIndex = 0;
        }

        #endregion
    }

    /// <summary>
    /// This is persistant class to store the player data.
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        public string levelKey;
        public int levelIndex;
        public short collectedStars;
        public Dictionary<string, VirtualItem> storeInventory;


        public PlayerData()
        {
            this.storeInventory = new Dictionary<string, VirtualItem>();
            //this.levelKey = levelKey;
        }

    }
}