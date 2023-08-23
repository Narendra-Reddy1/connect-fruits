using UnityEngine;


namespace BenStudios.Economy
{

    [System.Serializable]
    public class Consumable : VirtualItem
    {
        public int purchasePrice;
        public string purchaseCurrency;

        public int balance;


        public Consumable(string itemId, string purchaseCurrency, int purchasePrice)
            : base(itemId, VirtualItemType.CONSUMABLE)
        {
            this.purchaseCurrency = purchaseCurrency;
            this.purchasePrice = purchasePrice;
            this.balance = 0;
        }

        public override int GetBalance()
        {
            return balance;
        }

        public override bool Buy()
        {
            if (CanAfford())
            {
                //StoreInventory.TakeItem(SoomlaStoreDefinition1.COIN_CURRENCY_ITEM_ID,getUpgradeValue());
                DeductCurrencyBalance(purchaseCurrency, purchasePrice);
                Give(1);
                return true;
            }
            else
            {
                Debug.LogWarning("Not enough cash!!");
                return false;
            }
        }

        public override void Give(int quantity)
        {
            balance += quantity;
        }

        public override void Take(int quantity)
        {
            if (quantity <= GetBalance())
            {
                balance -= quantity;
            }
            else
            {
                balance = 0;
            }
        }

        /// <summary>
        /// Return true if user can afford to buy the product.
        /// Color text GREEN if returns true
        /// Color text RED if returns false
        /// </summary>
        public override bool CanAfford()
        {
            Debug.Log("Currency in Bank - " + PlayerResourceManager.GetBalance(purchaseCurrency) + " Required - " + GetPurchaseValue());
            //Adding a plus 1 to current level as we want to get that level and we start of at level 0
            return (PlayerResourceManager.GetBalance(purchaseCurrency) >= GetPurchaseValue());
        }

        public override string GetPurchaseCurrency()
        {
            return purchaseCurrency;
        }

        public override int GetPurchaseValue()
        {
            return purchasePrice;
        }

        public void SetPurchaseCurrency(string purchaseCurrency)
        {
            this.purchaseCurrency = purchaseCurrency;
        }

        public void SetPurchaseValue(int purchaseValue)
        {
            this.purchasePrice = purchaseValue;
        }

        public bool isAvailable()
        {
            return (balance > 0);

        }
    }
}