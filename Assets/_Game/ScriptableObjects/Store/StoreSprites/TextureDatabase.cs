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
    private bool m_areAllTexturesLoaded = false;
    public async Task LoadAllTextures()
    {
        if (m_areAllTexturesLoaded) return;
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
        m_areAllTexturesLoaded = true;
        MyUtils.Log($"All textured Loaded:: {m_loadedSpriteDictionary.Count} {m_spriteDictionary.Count}");
    }
    public void ReleaseAllTextureAssets()
    {
        try
        {
            foreach (KeyValuePair<ResourceType, AsyncOperationHandle<Sprite>> item in m_loadedSpriteDictionary)
            {
                //Addressables.Release(item.Value);
                AddressableAssetLoader.Instance.Release(item.Value);
            }
        }
        catch (System.Exception e)
        {
            MyUtils.Log($"Exception from release AllTextureAssets::{e}", LogType.Exception);
        }
        finally
        {
            m_loadedSpriteDictionary.Clear();
            m_areAllTexturesLoaded = false;
        }
    }

    public async Task<Sprite> GetSpriteWithID(ResourceType resourceType)
    {
        await LoadAllTextures();
        if (m_loadedSpriteDictionary.ContainsKey(resourceType))
        {
            if (m_loadedSpriteDictionary[resourceType].IsValid())
                return m_loadedSpriteDictionary[resourceType].Result;
        }
        return null;
    }

    [SerializeField] private SerializedDictionary<ResourceType, Sprite> m_resourceSprites;
    public Sprite GetSprite(ResourceType resourceType) => m_resourceSprites[resourceType];
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
    HintPowerup,
}
