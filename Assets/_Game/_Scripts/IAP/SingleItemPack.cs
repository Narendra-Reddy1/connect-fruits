using BenStudios;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BenStudios.IAP
{
    public class SingleItemPack : BasePack
    {
        [SerializeField] private Image m_itemIcon;
        [SerializeField] private TextMeshProUGUI m_itemCount;
        [SerializeField] private TextMeshProUGUI m_itemPriceTxt;
        [SerializeField] private TextureDatabase m_textureDatabase;
        public void Init(SinglePackData singlePackData)
        {
            transform.localScale = Vector3.one;
            m_itemIcon.sprite = m_textureDatabase.GetSpriteWithID(singlePackData.resourceType).Result;
            AssignProductID(singlePackData.bundleType);
            m_itemPriceTxt.SetText(InAppPurchasingManager.instance.GetLocalizedPrice(productID));
            m_itemCount.SetText(singlePackData.itemCount.ToString());
        }

        public override void Buy()
        {
            base.Buy();
        }
        public override void SetLocalizedPrice()
        {
        }

    }
}