using BenStudios.ScreenManagement;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI m_coinsTxt;
        [SerializeField] private float m_delayToEnableCloseBtn = 3f;
        private List<AsyncOperationHandle<GameObject>> m_loadedPackHandles = new List<AsyncOperationHandle<GameObject>>();
        private async void Start()
        {
            await m_textureDatabase.LoadAllTextures();
            _Init();
            Invoke(nameof(_EnableCloseBtn), m_delayToEnableCloseBtn);
        }
        private void OnDisable()
        {
            m_textureDatabase.ReleaseAllTextureAssets();
            ReleaseLoadedPacks();
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
                    m_loadedPackHandles.Add(handle);
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
                    m_loadedPackHandles.Add(handle);
                }
            }
            await Task.Delay(1000);
            m_connectingToServerPanel.SetActive(false);
            m_scrollRect.normalizedPosition = new Vector2(0, 1);
        }

        private void _EnableCloseBtn() => m_closebtn.SetActive(true);

        private void ReleaseLoadedPacks()
        {
            foreach (AsyncOperationHandle<GameObject> item in m_loadedPackHandles)
            {
                AddressableAssetLoader.Instance.Release(item.Result);
                // Addressables.ReleaseInstance(item);
            }
        }

        public void OnClickCloseBtn()
        {
            ScreenManager.Instance.CloseLastAdditiveScreen();
        }
    }
}