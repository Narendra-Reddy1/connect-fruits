using BenStudios.Economy;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

namespace BenStudios.IAP
{
    public class InAppPurchasingManager : MonoBehaviour, IDetailedStoreListener
    {
        #region SINGLETON
        public static InAppPurchasingManager instance { get; private set; }
        #endregion SINGLETON
        #region Variables
        [SerializeField] private StoreCatalogue m_storeCatalogue;
        private IStoreController m_storeController;
        private IExtensionProvider m_extensionProvider;
        private Dictionary<string, BundlePackData> m_bundlePacksDict;
        private Dictionary<string, SinglePackData> m_singlePacksDict;
        private List<Product> m_products;
        #endregion Variables

        #region Unity Methods
        private void OnEnable()
        {
            GlobalEventHandler.RequestToInitializePurchase += Callback_On_Purchase_Initialize_Requested;
        }
        private void OnDisable()
        {
            GlobalEventHandler.RequestToInitializePurchase -= Callback_On_Purchase_Initialize_Requested;
        }

        private void Start()
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct(Konstants.STARTER_PACK, ProductType.Consumable);
            builder.AddProduct(Konstants.MASTER_PACK, ProductType.Consumable);
            builder.AddProduct(Konstants.COIN_PACK_1, ProductType.Consumable);
            builder.AddProduct(Konstants.COIN_PACK_2, ProductType.Consumable);
            builder.AddProduct(Konstants.COIN_PACK_3, ProductType.Consumable);
            builder.AddProduct(Konstants.COIN_PACK_4, ProductType.Consumable);
            builder.AddProduct(Konstants.COIN_PACK_5, ProductType.Consumable);
            builder.AddProduct(Konstants.FRUIT_BOMB_PACK_1, ProductType.Consumable);
            builder.AddProduct(Konstants.TRIPLE_BOMB_PACK_1, ProductType.Consumable);
            builder.AddProduct(Konstants.HINT_POWERUP_PACK_1, ProductType.Consumable);
            builder.AddProduct(Konstants.NO_ADS, ProductType.NonConsumable);

            builder.Configure<IGooglePlayConfiguration>().SetServiceDisconnectAtInitializeListener(_OnGooglePlayServiceDisconnected);
            _InitDictioanries();
            UnityPurchasing.Initialize(this, builder);
        }


        #endregion Unity Methods

        #region Public Methods

        public string GetLocalizedPrice(string productID)
        {
            return m_products.Find(x => x.definition.id == productID).metadata.localizedPriceString;
        }

        #region IDetailedStoreListner Callbacks
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            m_storeController = controller;
            m_extensionProvider = extensions;
            m_products = controller.products.all.ToList();
            instance = this;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {

        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {

        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {

        }

        //Triggered on purchase is done.
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            string productID = purchaseEvent.purchasedProduct.definition.id;
            bool isValidPurchase = false;
#if (UNITY_ANDROID || UNITY_IOS )&&!UNITY_EDITOR
            //This validates the purchase 
            CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
            try
            {
                IPurchaseReceipt[] results = validator.Validate(purchaseEvent.purchasedProduct.receipt);
                foreach (IPurchaseReceipt receipt in results)
                {
                    if (receipt.productID == purchaseEvent.purchasedProduct.definition.id)
                    {
                        isValidPurchase = true;
                        break;
                    }
                    else
                        isValidPurchase = false;
                }
            }
            catch (System.Exception e)
            {
                MyUtils.Log("Invalid receipt, not unlocking the content...");
                isValidPurchase = false;
            }

            if (!isValidPurchase)
            {
                GlobalEventHandler.OnPurchaseFailed?.Invoke(new PurchaseFailureDescription(purchaseEvent.purchasedProduct.definition.id, PurchaseFailureReason.Unknown, "Invalid Purchase"));
                return PurchaseProcessingResult.Complete;
            }
#endif
            GlobalEventHandler.OnPurchaseSuccess?.Invoke(_GetPurchaseData(productID));
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            GlobalEventHandler.OnPurchaseFailed?.Invoke(failureDescription);
        }
        #endregion IDetailedStoreListner Callbacks

        #endregion Public Methods

        #region Private Methods

        //Update it to remove hardcoded values later...
        private void _InitDictioanries()
        {
            m_bundlePacksDict = new Dictionary<string, BundlePackData>
            {
                {Konstants.STARTER_PACK,m_storeCatalogue.bundlePacks.Find(x=>x.bundleType==BundleType.Starter) },
                {Konstants.MASTER_PACK,m_storeCatalogue.bundlePacks.Find(x=>x.bundleType== BundleType.Master) },
            };
            m_singlePacksDict = new Dictionary<string, SinglePackData>
            {
                {Konstants.COIN_PACK_1,GetSinglePackData(BundleType.CoinPack_1)},
                {Konstants.COIN_PACK_2,GetSinglePackData(BundleType.CoinPack_2)},
                {Konstants.COIN_PACK_3,GetSinglePackData(BundleType.CoinPack_3)},
                {Konstants.COIN_PACK_4,GetSinglePackData(BundleType.CoinPack_4)},
                {Konstants.COIN_PACK_5,GetSinglePackData(BundleType.CoinPack_5)},
                {Konstants.FRUIT_BOMB_PACK_1,GetSinglePackData(BundleType.FruitBombPack_1)},
                {Konstants.TRIPLE_BOMB_PACK_1,GetSinglePackData(BundleType.TripleBombPack_1)},
                {Konstants.HINT_POWERUP_PACK_1,GetSinglePackData(BundleType.HindPowerupPack_1)},
            };

            SinglePackData GetSinglePackData(BundleType bundleType)
            {
                return m_storeCatalogue.singlePacks.Find(x => x.bundleType == bundleType);
            }
            //m_singlePacksDict;
        }
        private void _InitializePurchase(string productID)
        {
            MyUtils.Log($"{productID} purchase Initiated..");
            m_storeController.InitiatePurchase(productID);
        }

        private PurchaseData _GetPurchaseData(string productID)
        {
            PurchaseData purchasedData = new PurchaseData();
            purchasedData.productID = productID;
            if (m_bundlePacksDict.ContainsKey(productID))
            {
                BundlePackData pack = m_bundlePacksDict[productID];
                purchasedData = new PurchaseData(productID, pack.GetResourceCountIfHave(ResourceType.Coin), pack.GetResourceCountIfHave(ResourceType.FruitBomb), pack.GetResourceCountIfHave(ResourceType.TripleBomb), pack.GetResourceCountIfHave(ResourceType.FruitDumper), false);
            }
            else if (m_singlePacksDict.ContainsKey(productID))
            {
                SinglePackData pack = m_singlePacksDict[productID];
                switch (pack.resourceType)
                {
                    case ResourceType.Coin:
                        purchasedData.coins = pack.itemCount;
                        break;
                    case ResourceType.FruitBomb:
                        purchasedData.fruitBombs = pack.itemCount;
                        break;
                    case ResourceType.TripleBomb:
                        purchasedData.tripleBombs = pack.itemCount;
                        break;
                    case ResourceType.FruitDumper:
                        purchasedData.hintPowerups = pack.itemCount;
                        break;
                }
            }
            else if (productID == Konstants.NO_ADS)
            {
                purchasedData.isNoAds = true;
            }
            return purchasedData;
        }

        //use this method to show a OS specific Native prompt.
        private void _OnGooglePlayServiceDisconnected()
        {

        }

        #endregion Private Methods

        #region Callbacks
        private void Callback_On_Purchase_Initialize_Requested(string productID)
        {
            _InitializePurchase(productID);
        }

        #endregion Callbacks

    }
    public struct PurchaseData
    {
        public string productID;
        public int coins;
        public int fruitBombs;
        public int tripleBombs;
        public int hintPowerups;
        public bool isNoAds;
        public PurchaseData(string productID, int coins = 0, int fruitBombs = 0, int tripleBombs = 0, int hintPowerups = 0, bool isNoAds = false)
        {
            this.productID = productID;
            this.coins = coins;
            this.fruitBombs = fruitBombs;
            this.tripleBombs = tripleBombs;
            this.hintPowerups = hintPowerups;
            this.isNoAds = isNoAds;
        }
    }
}