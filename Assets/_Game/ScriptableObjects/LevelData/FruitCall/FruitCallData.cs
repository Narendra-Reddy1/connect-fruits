using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newFruitCallData", menuName = "ScriptableObjects/FruitCallData", order = 1)]
public class FruitCallData : ScriptableObject
{
    [SerializeField]
    List<List<int>> m_fruitCallIndices;

    public AYellowpaper.SerializedCollections.SerializedDictionary<int, List<int>> dict;


    public List<int> GetFruitCallIndices(int level)
    {
        if (level >= 0 && level < m_fruitCallIndices.Count)
            return m_fruitCallIndices[level];
        else return null;
    }

}
