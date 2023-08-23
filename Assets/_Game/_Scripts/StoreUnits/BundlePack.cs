using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BenStudios
{
    public class BundlePack : MonoBehaviour
    {
        [SerializeField] private TextureDatabase m_textureDatabase;
        [SerializeField] private ItemUnit m_bundleItemPack;
        [SerializeField] private Transform m_contentTransform;
        [SerializeField] private TextMeshProUGUI m_bundleNameTxt;
        [SerializeField] private TextMeshProUGUI m_priceTxt;
        public void Init(BundlePackData packData)
        {
            m_bundleNameTxt.SetText(packData.packID);
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
        public void Buy()
        {

        }


        private void _AssignProductID()
        {

        }
        private void _SetLocalizedPrice()
        {

        }


    }
}