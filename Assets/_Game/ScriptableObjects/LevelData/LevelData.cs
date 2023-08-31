
using System.Collections.Generic;

public static class LevelData
{

    public static int[,] leveldata = new int[Konstants.REAL_ROW_SIZE, Konstants.REAL_COLUMN_SIZE]
    {
        {1,5,8,10,5,7},
        {10,3,9,8,7,9},
        {4,3,11,6,1,6},
        {1,10,5,2,5,8},
        {4,6,11,10,7,9},
        {1,6,10,2,4,4},
        {6,2,3,3,7,8},
        {2,6,11,10,11,9},
    };
    public static Dictionary<int, List<int>> LEveldata = new Dictionary<int, List<int>>()
    {
        {1,new List<int>
        {
        1,5,8,10,5,7,
        10,3,9,8,7,9,
        4,3,11,6,1,6,
        1,10,5,2,5,8,
        4,6,11,10,7,9,
        1,6,10,2,4,4,
        6,2,3,3,7,8,
        2,6,11,10,11,9,
        } },
        {2,new List<int>
        {
        9,1,8,10,1,7,
        10,3,9,8,7,9,
        4,3,11,6,9,6,
        1,10,5,2,5,8,
        4,6,11,10,7,5,
        1,6,10,2,4,4,
        6,2,3,3,7,8,
        2,6,11,10,11,5,

        } },
    };

    public static int[,] GetLevelDataByIndex(int level)
    {
        int[,] leveldata = new int[Konstants.REAL_ROW_SIZE, Konstants.REAL_COLUMN_SIZE];
        if (LEveldata.ContainsKey(level))
        {
            leveldata = GetConvertedListOfLevelData(LEveldata[level]);
        }
        else
            leveldata = GetConvertedListOfLevelData(LEveldata[1]);//Mapping Logic should be here
        return leveldata;
    }
    private static int[,] GetConvertedListOfLevelData(List<int> leveldata)
    {
        int[,] data = new int[Konstants.REAL_ROW_SIZE, Konstants.REAL_COLUMN_SIZE];
        int index = 0;
        for (int i = 0, count = Konstants.REAL_ROW_SIZE; i < count; i++)
        {
            for (int j = 0, count1 = Konstants.REAL_COLUMN_SIZE; j < count1; j++)
            {
                data[i, j] = leveldata[index];
                index++;
            }
        }
        return data;
    }
}
