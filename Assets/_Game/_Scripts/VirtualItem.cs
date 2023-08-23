using UnityEngine;
using System;

namespace BenStudios.Economy
{
	[Serializable]
	public abstract class VirtualItem
	{
		public delegate void CurrencyBalance();
		public static event CurrencyBalance onCurrencyBalanceChanged;

		public string itemId;
		public VirtualItemType virtualItemType;

		public VirtualItem(string itemId, VirtualItemType virtualItemType)
		{
			this.itemId = itemId;
			this.virtualItemType = virtualItemType;
		}

		public virtual int GetBalance()
		{
			return 0;
		}

		public virtual bool Buy()
		{
			return false;
		}

		//public virtual void Give (){}

		public virtual void Give(int quantity)
		{
			Debug.LogWarning("NOT APPLICABLE BUT INVESTIGATE IF HAPENING FOR ANYTHING EXCEPT UPGRADABLE");
		}

		//public virtual void Take (){}

		public virtual void Take(int quantity)
		{
			Debug.LogWarning("NOT APPLICABLE BUT INVESTIGATE IF HAPENING FOR ANYTHING EXCEPT UPGRADABLE");
		}

		public virtual bool CanAfford()
		{
			return false;
		}

		public virtual string GetPurchaseCurrency()
		{
			return PlayerResourceManager.COINS_ITEM_ID;
		}

		public virtual int GetPurchaseValue()
		{
			return 100;
		}

		public virtual void DeductCurrencyBalance(string purchaseCurrency, int puchasePrice)
		{
			Debug.Log(this.GetType().Name + "  " + System.Reflection.MethodBase.GetCurrentMethod().Name);
			PlayerResourceManager.Take(purchaseCurrency, puchasePrice);
			invokeCurrencyBalanceChangedListeners();
		}

		public void invokeCurrencyBalanceChangedListeners()
		{
			if (onCurrencyBalanceChanged != null)
			{
				onCurrencyBalanceChanged();
			}
		}

	}

	public enum VirtualItemType
	{
		CURRENCY,
		CONSUMABLE,
		EQUIPABLE,
		UPGRADABLE,
		POWERUP
	}
}