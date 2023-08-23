using System;
using UnityEngine;

namespace BenStudios.Economy
{
    [Serializable]
    public class PowerUp : VirtualItem
    {
        [Tooltip("Purchase price is price for the PowerUp to buy")]
        public int purchasePrice;

        [Tooltip("Purchase currency is a type of currency for exchange")]
        public string purchaseCurrency;

        [Tooltip("Available balance for PowerUp")]
        public int balance;

        public int collectedBalance;

        public PowerUp(string itemId, string purchaseCurrency, int purchasePrice)
            : base(itemId, VirtualItemType.POWERUP)
        {
            this.purchaseCurrency = purchaseCurrency;
            this.purchasePrice = purchasePrice;
            this.balance = 0;
        }

        public override int GetBalance()
        {
            return balance + GetCollectedBalance();
        }

        //This method will return the non collected balance
        public int GetNonCollectedBalance()
        {
            return balance;
        }

        //This method will return the collected balance
        public int GetCollectedBalance()
        {
            return collectedBalance;
        }

        public override void Give(int quantity)
        {
            collectedBalance = collectedBalance + quantity;

            //balance += quantity;
        }

        public override void Take(int quantity)
        {
            if (quantity <= GetNonCollectedBalance() + GetCollectedBalance())
            {
                if (quantity <= GetNonCollectedBalance())
                {
                    balance -= quantity;
                }
                else
                {
                    collectedBalance -= quantity - balance;
                    balance = 0;
                }
            }
            else
            {
                balance = 0;
                collectedBalance = 0;
            }
        }

        //This method updates the actual balance and clears the collected balance
        public void UpdateBalance()
        {
            balance += GetCollectedBalance();
            collectedBalance = 0;
        }

        #region Future References

        /// <summary>
        /// These methods might be needed in future implementations like changing the exchange currency for powerUps.
        /// </summary>
        /// <returns></returns>

        public override string GetPurchaseCurrency()
        {
            return purchaseCurrency;
        }

        public override int GetPurchaseValue()
        {
            return purchasePrice;
        }


        /// <summary>
        /// This Method is used to set the PurchaseCurrency for the power up.
        /// </summary>
        /// <param name="purchaseCurrency"></param>
        public void SetPurchaseCurrency(string purchaseCurrency)
        {
            this.purchaseCurrency = purchaseCurrency;
        }

        /// <summary>
        /// This Method is used to set the Purchase Value for the power up.
        /// </summary>
        /// <param name="purchaseValue"></param>
        public void SetPurchaseValue(int purchaseValue)
        {
            this.purchasePrice = purchaseValue;
        }

        #endregion

        public bool isAvailable()
        {
            return balance > 0;
        }
    }
}