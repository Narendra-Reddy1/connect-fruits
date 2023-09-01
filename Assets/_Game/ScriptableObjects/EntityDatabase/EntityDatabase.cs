using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityDatabase", menuName = "ScriptableObjects/EntityDatabase", order = 1)]
public class EntityDatabase : ScriptableObject
{
    [SerializeField] private List<Sprite> fruitSprites;
    [SerializeField] private List<Sprite> outlineFruitSprites;

    public Sprite GetFruitSprite(int index)
    {
        Sprite sprite = fruitSprites[0];
        if (index != -1) sprite = fruitSprites[index];
        return sprite;
    }
    public Sprite GetFruitOutlineSprite(int index)
    {
        Sprite sprite = outlineFruitSprites[0];
        if (index != -1) sprite = outlineFruitSprites[index];
        return sprite;
    }


}
