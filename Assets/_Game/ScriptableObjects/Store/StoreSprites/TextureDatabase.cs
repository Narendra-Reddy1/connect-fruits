using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "newTextureDatabase", menuName = "ScriptableObjects/TextureDatabase", order = 1)]
public class TextureDatabase : ScriptableObject
{
    [SerializeField] private List<SpriteDitctionary> m_spriteDictionary;
    private ConcurrentDictionary<ResourceType, AsyncOperationHandle<Sprite>> m_loadedSpriteDictionary = new ConcurrentDictionary<ResourceType, AsyncOperationHandle<Sprite>>();
    // [SerializeField] private SerializedDictionary<ResourceType, AssetReferenceSprite> m_textureDatabase;

    public async Task LoadAllTextures()
    {
        foreach (SpriteDitctionary item in m_spriteDictionary)
        {
            AssetReferenceSprite sprite = item.sprite;
            AsyncOperationHandle<Sprite> handle1 = sprite.LoadAssetAsync<Sprite>();
            await handle1.Task;
            if (handle1.Status == AsyncOperationStatus.Succeeded)
            {
                m_loadedSpriteDictionary.TryAdd(item.resourceType, handle1);
            }
        }
        MyUtils.Log($"All textured Loaded:: {m_loadedSpriteDictionary.Count} {m_spriteDictionary.Count}");
    }
    public void ReleaseAllTextureAssets()
    {
        try
        {
            foreach (var item in m_loadedSpriteDictionary)
            {
                Addressables.Release(item.Value);
            }
        }
        catch (System.Exception e)
        {

        }
    }
    //public async Task<Sprite> GetSpriteWithID(ResourceType resourceType)
    //{
    //    AssetReferenceSprite spriteReference = m_spriteDictionary.Find(x => x.resourceType == resourceType).sprite;
    //    AsyncOperationHandle<Sprite> handle1 = spriteReference.LoadAssetAsync<Sprite>();
    //    await handle1.Task;
    //    if (handle1.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        m_loadedSpriteDictionary.TryAdd(resourceType, handle1);
    //        return handle1.Result;
    //    }
    //    return null;
    //}
    public Sprite GetSpriteWithID(ResourceType resourceType)
    {
        if (m_loadedSpriteDictionary.ContainsKey(resourceType))
        {
            return m_loadedSpriteDictionary[resourceType].Result;
        }
        return null;
    }

}


[System.Serializable]
public class SpriteDitctionary
{
    public ResourceType resourceType;
    public AssetReferenceSprite sprite;
}

public enum ResourceType
{
    Coin,
    FruitBomb,
    TripleBomb,
    FruitDumper,
}
