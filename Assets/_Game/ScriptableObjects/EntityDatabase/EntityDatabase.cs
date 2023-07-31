using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityDatabase", menuName = "ScriptableObjects/EntityDatabase", order = 1)]
public class EntityDatabase : ScriptableObject
{
    public List<Sprite> fruitSprites;
    public List<Sprite> outlineFruitSprites;

    public List<Sprite> fruitCallOutlineSprites;
}
