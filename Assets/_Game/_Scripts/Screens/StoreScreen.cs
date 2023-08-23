using BenStudios.ScreenManagement;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace BenStudios
{
    public class StoreScreen : ScreenBase
    {
        [SerializeField] private AssetReference m_bundlePack;
        [SerializeField] private AssetReference m_singleItemPack;
        [SerializeField] private Transform m_contentTransform;
        [SerializeField] private ScrollRect m_scrollRect;
        [SerializeField] private GameObject m_closebtn;
        [SerializeField] private GameObject m_connectingToServerPanel;
        [SerializeField] private StoreCatalogue m_storeCatalogue;
        [SerializeField] private TextureDatabase m_textureDatabase;
        private async void Start()
        {
            await m_textureDatabase.LoadAllTextures();
            _Init();
        }
        private void OnDestroy()
        {
            m_textureDatabase.ReleaseAllTextureAssets();
        }
        private async void _Init()
        {
            MyUtils.Log($"Store screen initializing....");
            List<BundlePackData> bundlePacks = m_storeCatalogue.bundlePacks;
            for (int i = 0, count = bundlePacks.Count; i < count; i++)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(m_bundlePack);
                await handle.Task;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject item = handle.Result;
                    item.transform.SetParent(m_contentTransform);
                    item.GetComponent<BundlePack>().Init(bundlePacks[i]);
                }
            }
            List<SinglePackData> singlePacks = m_storeCatalogue.singlePacks;
            for (int i = 0, count = singlePacks.Count; i < count; i++)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(m_singleItemPack);
                await handle.Task;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var item = handle.Result;
                    item.transform.SetParent(m_contentTransform);
                    item.GetComponent<SingleItemPack>().Init(singlePacks[i]);
                }
            }
            await Task.Delay(1000);
            m_connectingToServerPanel.SetActive(false);
            m_scrollRect.normalizedPosition = new Vector2(0, 1);
        }

    }
}