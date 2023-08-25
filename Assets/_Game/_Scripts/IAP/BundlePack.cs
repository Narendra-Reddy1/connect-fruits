using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace BenStudios.IAP
{
    public class BundlePack : BasePack
    {
        [SerializeField] private TextureDatabase m_textureDatabase;
        [SerializeField] private ItemUnit m_bundleItemPack;
        [SerializeField] private Transform m_contentTransform;
        [SerializeField] private TextMeshProUGUI m_bundleNameTxt;
        [SerializeField] private TextMeshProUGUI m_priceTxt;
        public void Init(BundlePackData packData)
        {
            transform.localScale = Vector3.one;
            m_bundleNameTxt.SetText(packData.bundleTitle);
            AssignProductID(packData.bundleType);
            m_priceTxt.SetText(InAppPurchasingManager.instance.GetLocalizedPrice(productID));
            foreach (KeyValuePair<ResourceType, int> itemData in packData.resourcesDictionary)
                SetUp(m_textureDatabase.GetSpriteWithID(itemData.Key), itemData.Value);
            void SetUp(Sprite sprite, int value)
            {
                ItemUnit item = Instantiate(m_bundleItemPack, m_contentTransform);
                item.transform.SetParent(m_contentTransform);
                item.Init(sprite, value);
            }
        }

        ///Assigned to IAP
        ///Methods which calls on click IAP button
        public override void Buy()
        {
            base.Buy();
        }
        public override void SetLocalizedPrice()
        {
        }



    }
}