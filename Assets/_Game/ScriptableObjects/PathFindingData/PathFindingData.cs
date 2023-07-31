using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPathFindingData", menuName = "ScriptableObjects/PathFindingData", order = 10)]
public class PathFindingData : ScriptableObject
{
    public SerializedDictionary<StartAndEndCell, List<PathData>> startCellToEndCellData;

}
[System.Serializable]
public class StartAndEndCell
{
    public Vector2 startingCell;
    public Vector2 endingCell;
}
[System.Serializable]
public class PathData
{
    public List<Vector2> pathCells;
}
