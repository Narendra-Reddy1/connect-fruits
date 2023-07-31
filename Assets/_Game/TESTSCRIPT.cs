using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//X represents ROW
//Y Represents COLUMN
public class TESTSCRIPT : MonoBehaviour
{

    //public Vector2Int startCell;//3,2
    //public Vector2Int endCell;//7,5

    public static List<List<Vector2Int>> FindPossiblePaths(Vector2Int startCell, Vector2Int endCell)
    {
        List<List<Vector2Int>> allPossiblePaths = new List<List<Vector2Int>>();
        List<Vector2Int> rowElementsList = GetRowElements(Vector2Int.zero);//(3,0)(3,1)(3,2)(3,3)(3,4)(3,5)
        List<Vector2Int> columnElements = GetColumnElements(Vector2Int.zero);//(0,2)(1,2)(2,2)(3,2)(4,2)(5,2)(6,2)(7,2)

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

            #endregion To The END CEll


            /*
            else if (columnElement.y > endCell.y)
            {
                paths.Add(new Vector2Int(endCell.x, columnElement.y));
            }*/
            // paths.Add(endCell);
            allPossiblePaths.Add(paths);
        }


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
                for (int i = columnElement.y + 1; i <= endCell.y; i++)
                {
                    pathElement.y = i;
                    paths.Add(pathElement);
                }

            }

            #endregion Colument Element Column ----> End Cell Column

            #region To The END CELL

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
            #endregion To The END CELL

            allPossiblePaths.Add(paths);
        }
        return allPossiblePaths;


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
}
