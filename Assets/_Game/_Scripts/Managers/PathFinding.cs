#define AStar
using System.Collections.Generic;
using UnityEngine;

namespace FruitFrenzy
{
    ///Can make this Script Generic to work with different types
    public class PathFinding : MonoBehaviour
    {


        #region SINGLETON
        public static PathFinding instance { get; private set; }
        #endregion SINGLETON

        #region Variables

        #endregion Variables

        #region Unity Methods

        private void Awake()
        {
            instance = this;
        }

        #endregion Unity Methods

        #region Public Methods

#if Astar
    #region A*
    public List<FruitEntity> openList = new List<FruitEntity>();
        public List<FruitEntity> closedList = new List<FruitEntity>();
        public const byte STRAIGHT_COST = 10;
    public int linesToBeDrawn = 0;
    FruitEntity observableEntity = default;
    public List<FruitEntity> FindPathWithAStartAlgo(FruitEntity startEntity, FruitEntity endEntity)
    {

        openList.Clear();
        closedList.Clear();
        openList.Add(startEntity);
        FruitEntity[,] entityArray = GameplayManager.instance.GetFruitEntitiesOnTheBoard();
        for (int i = 0; i < Konstants.ROW_SIZE; i++)
        {
            for (int j = 0; j < Konstants.COLUMN_SIZE; j++)
            {
                FruitEntity entity = entityArray[i, j];
                entity.gCost = int.MaxValue;
                entity.CalculateFCost();
                entity.cameFrom = null;
                entity.CanShowSelectedEffect(false);
            }
        }
        startEntity.gCost = 0;
        startEntity.hCost = CalculateDistance(startEntity, endEntity);
        startEntity.CalculateFCost();
        observableEntity = openList[0];
        linesToBeDrawn = 1;
        while (openList.Count > 0)
        {
            FruitEntity currentEntity = GetLowestFCostEntity(openList);
            if (currentEntity == endEntity)
            {
                //Reached Destination
                return CalculatedPath(endEntity);
            }
            openList.Remove(currentEntity);
            closedList.Add(currentEntity);
            foreach (FruitEntity neighbourEntity in currentEntity.neighbours.Values)
            {
                if (closedList.Contains(neighbourEntity)) continue;
                if (!neighbourEntity.IsDestroyed && neighbourEntity != endEntity) continue;
                int tentativeGCost = CalculateDistance(startEntity, neighbourEntity, startEntity.Row != neighbourEntity.Row, startEntity.Column != neighbourEntity.Column);
                if (tentativeGCost < neighbourEntity.gCost)
                {
                    neighbourEntity.cameFrom = currentEntity;
                    neighbourEntity.gCost = tentativeGCost;
                    neighbourEntity.hCost = CalculateDistance(neighbourEntity, endEntity);
                    neighbourEntity.CalculateFCost();
                    if (!openList.Contains(neighbourEntity)) openList.Add(neighbourEntity);
                }
            }
            MyUtils.Log($"***Lines Drawn: {linesToBeDrawn}");
        }
        //out of nodes and no path found.
        return null;
    }

    public List<FruitEntity> CalculatedPath(FruitEntity endEntity)
    {
        List<FruitEntity> path = new List<FruitEntity>();
        path.Add(endEntity);
        FruitEntity currEntity = endEntity;
        while (currEntity.cameFrom != null)
        {
            path.Add(currEntity.cameFrom);
            currEntity = currEntity.cameFrom;
        }
        observableEntity = path[0];
        for (int i = 1, count = path.Count; i < count; i++)
        {
            if (observableEntity.Row != path[i].Row && observableEntity.Column != path[i].Column)
            {
                linesToBeDrawn++;
                observableEntity = path[i - 1];
                MyUtils.Log($"***Lines Drawn b/w {observableEntity.name}::{path[i]?.name} : {linesToBeDrawn}");
            }
        }
        // path.Reverse();
        return path;
    }
    public int CalculateDistance(FruitEntity startEntity, FruitEntity endEntity, bool addRowChangeCost = false, bool addColumnChangeCost = false)
    {
        if (startEntity == endEntity) return 0;
        int xDistance = Mathf.Abs(startEntity.Row - endEntity.Row);
        int yDistance = Mathf.Abs(startEntity.Column - endEntity.Column);
        int remaining = Mathf.Abs(xDistance - yDistance);

        if (addRowChangeCost)
            remaining += 200;
        if (addColumnChangeCost)
            remaining += 200;
        MyUtils.Log($"CalcDist:{startEntity.name} :: {endEntity.name} cost: {remaining * STRAIGHT_COST}");
        return remaining * STRAIGHT_COST;
    }
    public FruitEntity GetLowestFCostEntity(List<FruitEntity> openList)
    {
        FruitEntity lowestFCostEntity = openList[0];
        for (int i = 1, count = openList.Count; i < count; i++)
        {
            if (openList[i].fCost < lowestFCostEntity.fCost)
            {
                lowestFCostEntity = openList[i];
            }
        }
        return lowestFCostEntity;
    }

    #endregion A*
#endif

        #region Linear Algorithm

        /*
         * 5x2----->7x3
         * 5x2==>0x2,1x2,2x2,3x2,4x2,5x2,6x2,7x2[Column elements]
         * 5x2==>5x0,5x1,5x2,5x3,5x4,5x5[Row Elements]
         */
        public static List<List<Vector2Int>> FindPossiblePathsLinearAlgo(Vector2Int startCell, Vector2Int endCell)
        {
            List<List<Vector2Int>> allPossiblePaths = new List<List<Vector2Int>>();
            List<Vector2Int> rowElementsList = GetRowElements(startCell);
            List<Vector2Int> columnElements = GetColumnElements(startCell);

            //Row
            foreach (Vector2Int rowElement in rowElementsList)
            {
                if (rowElement == startCell) continue;
                List<Vector2Int> paths = new List<Vector2Int>
            {
                startCell
            };

                #region Start Cell---->RowElement Cell

                if (rowElement.y > startCell.y)///(3,2)----->(3,5) SameRow Elements towards right side
                {
                    Vector2Int pathElement = Vector2Int.zero;
                    pathElement.x = startCell.x;
                    for (int k = startCell.y + 1; k <= rowElement.y; k++)
                    {
                        pathElement.y = k;
                        paths.Add(pathElement);
                    }
                }
                else///(3,0)<------(3,2) // Same Row towards left side
                {
                    Vector2Int pathElement = Vector2Int.zero;
                    pathElement.x = startCell.x;
                    for (int k = startCell.y - 1; k >= rowElement.y; k--)
                    {
                        pathElement.y = k;
                        paths.Add(pathElement);
                    }
                }

                #endregion Start Cell---->RowElement Cell

                #region RowElement Cell ROW----->End Cell ROW

                if (rowElement.x < endCell.x) //3,1.....7,5
                {
                    Vector2Int pathElement = Vector2Int.zero;
                    pathElement.y = rowElement.y;
                    for (int i = rowElement.x + 1; i <= endCell.x; i++)
                    {
                        pathElement.x = i;
                        paths.Add(pathElement);
                    }
                }
                else if (rowElement.x > endCell.x)
                {
                    Vector2Int pathElement = Vector2Int.zero;
                    pathElement.y = rowElement.y;
                    for (int i = rowElement.x - 1; i >= endCell.x; i--)
                    {
                        pathElement.x = i;
                        paths.Add(pathElement);
                    }
                }

                #endregion RowElement Cell----->End Cell

                #region To The END CEll

                if (!paths.Contains(endCell))
                {
                    Vector2Int turnElement = paths[^1];
                    if (turnElement.y > endCell.y)
                    {
                        Vector2Int pathElement = Vector2Int.zero;
                        pathElement.x = endCell.x;
                        for (int i = turnElement.y - 1; i >= endCell.y; i--)
                        {
                            pathElement.y = i;
                            paths.Add(pathElement);
                        }
                    }
                    else if (turnElement.y < endCell.y)
                    {
                        Vector2Int pathElement = Vector2Int.zero;
                        pathElement.x = endCell.x;
                        for (int i = turnElement.y + 1; i <= endCell.y; i++)
                        {
                            pathElement.y = i;
                            paths.Add(pathElement);
                        }
                    }
                    else
                    {
                        paths.Add(endCell);
                    }
                }
                #endregion To The END CEll


                /*
                else if (columnElement.y > endCell.y)
                {
                    paths.Add(new Vector2Int(endCell.x, columnElement.y));
                }*/
                // paths.Add(endCell);
                allPossiblePaths.Add(paths);
            }

            //Column
            foreach (Vector2Int columnElement in columnElements)
            {
                if (columnElement == startCell) continue;
                List<Vector2Int> paths = new List<Vector2Int>
            {
                startCell
            };

                #region StartCell ----> ColumnElement

                if (columnElement.x > startCell.x)//7,2
                {
                    Vector2Int pathElement = Vector2Int.zero;
                    pathElement.y = startCell.y;
                    for (int i = startCell.x + 1; i <= columnElement.x; i++)
                    {
                        pathElement.x = i;
                        paths.Add(pathElement);
                    }
                }

                else//0,2
                {
                    Vector2Int pathElement = Vector2Int.zero;
                    pathElement.y = startCell.y;
                    for (int i = startCell.x - 1; i >= columnElement.x; i--)
                    {
                        pathElement.x = i;
                        paths.Add(pathElement);
                    }
                }

                #endregion StartCell ----> ColumnElement

                #region Colument Element Column ----> End Cell Column

                if (columnElement.y < endCell.y)//0,2--->7,5
                {
                    Vector2Int pathElement = Vector2Int.zero;
                    pathElement.x = columnElement.x;
                    for (int i = columnElement.y + 1; i <= endCell.y; i++)
                    {
                        pathElement.y = i;
                        paths.Add(pathElement);
                    }
                    // paths.Add(pathElement);
                }

                else
                {
                    Vector2Int pathElement = Vector2Int.zero;
                    pathElement.x = columnElement.x;
                    for (int i = columnElement.y - 1; i >= endCell.y; i--)
                    {
                        pathElement.y = i;
                        paths.Add(pathElement);
                    }

                }

                #endregion Colument Element Column ----> End Cell Column

                #region To The END CELL

                if (!paths.Contains(endCell))
                {
                    Vector2Int turnCell = paths[^1];

                    if (turnCell.x < endCell.x)
                    {
                        Vector2Int pathElement = Vector2Int.zero;
                        pathElement.y = endCell.y;
                        for (int i = turnCell.x + 1; i <= endCell.x; i++)
                        {
                            pathElement.x = i;
                            paths.Add(pathElement);
                        }
                    }
                    else if (turnCell.x > endCell.x)
                    {
                        Vector2Int pathElement = Vector2Int.zero;
                        pathElement.y = endCell.y;
                        for (int i = turnCell.x - 1; i >= endCell.x; i--)
                        {
                            pathElement.x = i;
                            paths.Add(pathElement);
                        }
                    }
                    else
                    {
                        paths.Add(endCell);
                    }
                }
                #endregion To The END CELL

                allPossiblePaths.Add(paths);
            }

            return MergeSort(allPossiblePaths);


            List<Vector2Int> GetRowElements(Vector2Int startCell)//3,2
            {
                List<Vector2Int> rowElements = new List<Vector2Int>();
                Vector2Int rowElement = Vector2Int.zero;
                for (int i = 0, count = Konstants.COLUMN_SIZE; i < count; i++)
                {
                    rowElement.x = startCell.x;
                    rowElement.y = i;
                    rowElements.Add(rowElement);
                }
                return rowElements;
            }
            List<Vector2Int> GetColumnElements(Vector2Int startCell)
            {
                List<Vector2Int> columnElements = new List<Vector2Int>();
                Vector2Int columnElement = Vector2Int.zero;
                for (int i = 0, count = Konstants.ROW_SIZE; i < count; i++)
                {
                    columnElement.x = i;
                    columnElement.y = startCell.y;
                    columnElements.Add(columnElement);
                }
                return columnElements;
            }
        }

        #endregion Linear Algorithm

        #endregion Public Methods

        #region MergeSort

        public static List<List<Vector2Int>> MergeSort(List<List<Vector2Int>> listOfLists)
        {
            if (listOfLists.Count <= 1)
                return listOfLists;

            int middle = listOfLists.Count / 2;
            List<List<Vector2Int>> left = new List<List<Vector2Int>>(middle);
            List<List<Vector2Int>> right = new List<List<Vector2Int>>(listOfLists.Count - middle);

            for (int i = 0; i < middle; i++)
            {
                left.Add(listOfLists[i]);
            }

            for (int i = middle; i < listOfLists.Count; i++)
            {
                right.Add(listOfLists[i]);
            }

            left = MergeSort(left);
            right = MergeSort(right);

            return _Merge(left, right);
        }

        private static List<List<Vector2Int>> _Merge(List<List<Vector2Int>> left, List<List<Vector2Int>> right)
        {
            List<List<Vector2Int>> result = new List<List<Vector2Int>>();
            int leftIndex = 0;
            int rightIndex = 0;

            while (leftIndex < left.Count && rightIndex < right.Count)
            {
                if (left[leftIndex].Count <= right[rightIndex].Count)
                {
                    result.Add(left[leftIndex]);
                    leftIndex++;
                }
                else
                {
                    result.Add(right[rightIndex]);
                    rightIndex++;
                }
            }

            while (leftIndex < left.Count)
            {
                result.Add(left[leftIndex]);
                leftIndex++;
            }

            while (rightIndex < right.Count)
            {
                result.Add(right[rightIndex]);
                rightIndex++;
            }

            return result;
        }

        #endregion MergeSort



    }
}