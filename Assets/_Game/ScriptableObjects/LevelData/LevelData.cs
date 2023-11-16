using System.Collections.Generic;

public static class LevelData
{
    private static Dictionary<int, int[,]> levelData = new Dictionary<int, int[,]>()
    {
        { 1,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {-1,-1,-1,-1,-1,-1},
                    {-1,-1,-1,-1,-1,-1},
                    {-1,2,0,1,2,-1},
                    {-1,6,1,3,6,-1},
                    {-1,5,0,3,5,-1},
                    {-1,-1,-1,-1,-1,-1},
                    {-1,-1,-1,-1,-1,-1},
                    {-1,-1,-1,-1,-1,-1},
                }
            },
        {
                2,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,3,4,4,3,0},
                    {2,4,0,0,4,1},
                    {-1,-1,2,1,-1,-1},
                    {-1,-1,3,0,-1,-1},
                    {-1,-1,0,1,-1,-1},
                    {-1,-1,3,2,-1,-1},
                    {2,3,1,4,4,1},
                    {3,0,2,2,0,1},
                }
            },
        {
            3,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,2,-1,-1,4,0},
                    {1,3,-1,-1,3,1},
                    {0,4,-1,-1,0,4},
                    {1,3,2,3,0,4},
                    {2,2,1,1,4,2},
                    {3,0,-1,-1,3,3},
                    {3,0,-1,-1,4,1},
                    {0,4,-1,-1,2,4},
                }
            },
        {
                4,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {4,0,-1,-1,1,3},
                    {1,3,-1,-1,0,2},
                    {-1,-1,4,3,-1,-1},
                    {-1,-1,0,0,-1,-1},
                    {-1,-1,3,4,-1,-1},
                    {-1,-1,0,3,-1,-1},
                    {1,2,-1,-1,2,4},
                    {2,0,-1,-1,1,3},
                }
            },

        {
                5,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {3,4,3,4,3,2},
                    {4,3,4,3,0,1},
                    {0,2,-1,-1,4,0},
                    {3,1,-1,-1,1,3},
                    {2,0,-1,-1,0,2},
                    {0,1,-1,-1,3,2},
                    {2,4,0,1,0,0},
                    {1,1,0,4,4,1},
                }
            },
        { 6,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,1,2,3,4,5},
                    {5,0,1,2,3,4},
                    {4,5,0,1,2,3},
                    {3,4,5,0,1,2},
                    {2,3,4,5,0,1},
                    {1,2,3,4,5,0},
                    {0,1,2,3,4,5},
                    {5,0,1,2,3,4},
                }
            },

        {7,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {5,2,3,2,3,5},
                    {4,5,0,4,5,3},
                    {1,0,3,0,3,1},
                    {4,2,1,1,2,4},
                    {2,5,0,3,0,0},
                    {1,0,2,2,4,1},
                    {3,0,4,1,5,2},
                    {5,3,1,4,5,4},
                }
            },

        {8,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {2,3,2,1,4,1},
                    {3,1,5,4,0,5},
                    {0,1,2,5,3,2},
                    {4,0,4,5,2,3},
                    {5,1,4,2,1,3},
                    {0,5,0,3,0,5},
                    {2,5,2,4,4,3},
                    {4,1,0,3,0,1},
                }
            },
        {9,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,5,4,3,2,1},
                    {1,3,0,0,1,0},
                    {2,2,3,0,5,5},
                    {3,4,0,4,3,1},
                    {4,0,0,4,0,1},
                    {5,1,3,4,2,3},
                    {0,1,2,3,5,3},
                    {1,2,3,4,5,4},
                }
            },

        {
                10,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,2,4,3,2,1},
                    {1,3,0,0,1,0},
                    {2,2,3,0,5,5},
                    {3,4,0,4,3,1},
                    {4,0,0,4,0,1},
                    {5,5,3,4,2,3},
                    {0,1,2,3,5,3},
                    {1,1,3,4,5,4},
                }
            },
        {
                11,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {3,10,12,0,5,11},
                    {2,12,9,5,3,9},
                    {1,4,0,1,6,4},
                    {0,3,5,7,2,3},
                    {10,7,6,4,0,10},
                    {11,2,1,3,1,8},
                    {9,11,8,6,4,11},
                    {10,6,3,2,9,5},
                }
            },
       {12,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {5,1,4,2,1,3},
                    {2,5,2,4,4,3},
                    {4,0,4,5,2,3},
                    {2,3,2,1,4,1},
                    {0,5,0,3,0,5},
                    {4,1,0,3,0,1},
                    {0,1,2,5,3,2},
                    {3,1,5,4,0,5},
                }
            },


        {13,
                new int[Konstants.REAL_ROW_SIZE, Konstants.REAL_COLUMN_SIZE]
                {
                    {4,1,0,3,0,1},
                    {2,5,2,4,4,3},
                    {0,5,0,3,0,5},
                    {5,1,4,2,1,3},
                    {4,0,4,5,2,3},
                    {0,1,2,5,3,2},
                    {3,1,5,4,0,5},
                    {2,3,2,1,4,1},

                }
            },
        {
    14,
                new int[Konstants.REAL_ROW_SIZE, Konstants.REAL_COLUMN_SIZE]
                {
                    {0,4,2,4,6,1},
                    {6,5,1,2,5,6},
                    {2,3,3,1,6,2},
                    {5,6,0,6,0,4},
                    {6,4,0,3,0,2},
                    {5,4,3,5,2,6},
                    {0,2,1,3,3,4},
                    {1,0,5,2,1,0},
                }
            },
        {
    15,
                new int[Konstants.REAL_ROW_SIZE, Konstants.REAL_COLUMN_SIZE]
                {
                    {1,7,6,7,1,6},
                    {2,3,5,0,5,1},
                    {7,3,5,3,4,6},
                    {2,4,0,4,2,2},
                    {0,3,5,3,1,7},
                    {0,2,4,0,5,3},
                    {7,2,4,6,5,7},
                    {6,1,0,6,1,4},
                }
            },
        {
                16,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {10,6,4,9,1,7},
                    {2,11,9,10,12,3},
                    {7,5,7,1,5,3},
                    {8,9,1,12,10,8},
                    {6,3,2,3,0,4},
                    {5,0,12,7,1,2},
                    {4,11,6,12,2,6},
                    {1,1,4,9,10,5},
                }
            },
                {
                17,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {12,0,3,6,0,1},
                    {3,4,6,8,1,7},
                    {2,2,11,10,7,11},
                    {5,4,9,8,10,5},
                    {9,11,7,6,5,11},
                    {8,10,6,12,10,8},
                    {5,7,4,1,0,3},
                    {2,4,0,3,1,2},
                }
            },
        {
                18,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,3,5,8,6,2},
                    {6,1,8,2,5,4},
                    {3,6,11,4,6,0},
                    {7,10,1,1,7,10},
                    {10,11,12,8,10,6},
                    {7,5,7,6,2,3},
                    {5,6,0,12,3,0},
                    {6,8,4,4,1,2},
                }
            },
        {
                19,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {5,2,6,3,6,7},
                    {3,5,3,5,4,5},
                    {7,3,1,2,2,6},
                    {3,7,4,4,6,4},
                    {1,0,1,0,7,1},
                    {1,2,6,5,2,7},
                    {5,0,6,5,3,0},
                    {7,3,2,1,3,5},
                }
            },
        {
                20,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,5,3,3,5,0},
                    {8,2,1,0,2,1},
                    {3,1,2,9,7,3},
                    {10,5,10,5,1,7},
                    {0,4,9,9,4,7},
                    {1,1,10,0,1,0},
                    {7,3,2,3,7,7},
                    {1,9,6,10,8,6},
                }
            },

        {
                21,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {10,0,4,9,1,7},
                    {4,11,9,10,12,6},
                    {7,5,7,1,5,11},
                    {8,9,1,12,10,8},
                    {0,6,2,6,0,4},
                    {5,0,3,7,1,2},
                    {4,11,6,3,2,11},
                    {1,1,2,9,10,5},
                }
            },
                {
                22,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,12,4,3,11,0},
                    {7,10,8,10,5,6},
                    {1,4,12,1,4,2},
                    {1,5,6,0,4,11},
                    {8,11,4,5,10,7},
                    {6,10,6,10,8,4},
                    {3,10,8,3,5,12},
                    {2,12,1,3,11,0},
                }
            },
        {
                23,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {5,8,11,8,9,5},
                    {4,2,4,7,6,2},
                    {7,9,11,9,8,3},
                    {3,10,6,10,9,1},
                    {12,2,2,7,12,1},
                    {3,7,6,3,3,8},
                    {10,6,10,3,12,9},
                    {4,12,10,9,4,10},
                }
            },
        {
                24,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {6,0,0,6,2,5},
                    {10,3,8,7,4,4},
                    {8,5,7,5,11,2},
                    {1,1,12,10,3,5},
                    {7,10,11,6,12,8},
                    {2,7,6,9,1,8},
                    {2,4,5,3,0,1},
                    {4,5,10,9,3,0},
                }
            },
        {
                25,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,4,5,7,6,7},
                    {3,0,3,5,10,12},
                    {9,4,11,9,12,10},
                    {6,2,2,5,8,5},
                    {10,8,11,10,9,6},
                    {12,12,9,3,5,7},
                    {7,6,3,6,2,5},
                    {6,4,0,0,4,2},
                }
            },

        {
                26,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {10,4,6,9,1,7},
                    {2,11,9,10,12,3},
                    {1,5,7,1,5,3},
                    {8,9,1,12,10,8},
                    {6,3,2,3,0,4},
                    {5,0,12,7,1,2},
                    {4,11,6,12,2,6},
                    {7,1,4,9,5,10},
                }
            },
                {
            27,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {6,1,3,0,1,2},
                    {3,8,2,10,10,7},
                    {11,0,7,1,6,1},
                    {5,4,10,4,5,9},
                    {7,10,8,4,8,2},
                    {3,5,3,6,1,5},
                    {4,11,6,7,9,0},
                    {3,0,2,3,1,8},
                }
            },{
                28,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {1,7,0,6,4,7},
                    {12,1,0,11,7,10},
                    {2,3,10,4,6,2},
                    {3,11,2,6,8,3},
                    {4,0,4,7,5,9},
                    {1,10,5,0,12,9},
                    {12,2,11,6,3,1},
                    {0,11,8,0,10,12},
                }
            },
        {
                29,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {5,2,1,8,11,7},
                    {2,5,0,6,1,10},
                    {9,10,4,0,8,12},
                    {10,7,4,12,6,7},
                    {7,12,5,11,9,4},
                    {6,9,11,9,1,2},
                    {8,6,0,5,4,12},
                    {11,0,1,2,10,8},
                }
            },
        {
                30,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {12,6,8,1,5,4},
                    {10,2,6,7,4,9},
                    {8,11,10,11,9,12},
                    {1,2,3,7,1,5},
                    {10,12,8,6,3,7},
                    {3,5,2,10,11,4},
                    {4,11,9,1,2,3},
                    {8,6,7,5,12,9},
                }
            },{
               31,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {2,5,3,1,3,0},
                    {4,6,5,2,1,11},
                    {6,7,9,11,0,4},
                    {12,8,10,9,6,7},
                    {7,12,8,12,9,6},
                    {11,3,10,0,4,5},
                    {9,0,1,12,2,4},
                    {1,3,11,7,5,2},
                }
            },
        {
                32,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {6,2,1,10,12,9},
                    {4,5,11,7,8,1},
                    {0,8,10,4,0,11},
                    {11,2,1,2,1,0},
                    {3,3,6,2,12,10},
                    {9,6,10,1,11,6},
                    {5,2,4,3,7,5},
                    {0,3,1,5,2,4},
                }
            },
        {
            33,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {3,2,5,10,0,11},
                    {1,7,4,1,4,8},
                    {6,10,9,6,10,12},
                    {0,11,8,3,0,2},
                    {4,5,6,2,12,4},
                    {9,7,10,1,10,0},
                    {3,1,3,8,6,5},
                    {2,11,5,10,11,8},
                }
            },
        {
                34,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {8,12,10,6,8,5},
                    {1,6,9,3,5,0},
                    {7,1,0,1,8,12},
                    {12,10,11,11,10,7},
                    {2,3,6,3,8,3},
                    {4,10,9,5,3,11},
                    {7,1,7,5,3,6},
                    {4,4,11,2,4,12},
                }
            },

        {
                35,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,3,5,6,2,0},
                    {6,7,7,1,7,9},
                    {10,1,4,6,8,1},
                    {4,2,3,0,7,1},
                    {4,1,8,3,6,7},
                    {9,7,1,5,7,2},
                    {5,2,4,5,0,6},
                    {0,10,7,6,3,0},
                }
            },
        {
                36,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {10,6,4,9,1,7},
                    {2,11,9,10,12,3},
                    {7,5,7,1,5,3},
                    {8,9,1,12,10,8},
                    {6,3,2,3,0,4},
                    {5,0,12,7,1,2},
                    {4,11,6,12,2,6},
                    {1,1,4,9,10,5},
                }
            },
        {
                37,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {1,5,7,7,5,2},
                    {3,2,8,1,1,0},
                    {0,5,6,0,4,3},
                    {7,5,10,1,8,7},
                    {7,4,10,4,5,10},
                    {4,3,6,5,1,4},
                    {3,2,1,0,10,6},
                    {2,9,9,4,7,6},
                }
            },
        {
                38,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {2,5,4,3,5,7},
                    {1,3,9,12,6,5},
                    {7,1,12,6,7,11},
                    {11,5,2,11,10,9},
                    {9,11,4,6,3,10},
                    {6,5,8,12,2,1},
                    {7,10,12,8,3,4},
                    {4,5,10,1,2,9},
                }
            },
        {
                39,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {3,9,2,12,8,6},
                    {11,5,3,4,12,8},
                    {10,11,4,9,1,4},
                    {7,5,10,4,0,2},
                    {7,6,2,1,2,3},
                    {6,8,4,0,6,4},
                    {12,10,11,3,5,9},
                    {8,5,12,10,9,11},
                }
            },{
                57,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {12,0,9,4,2,5},
                    {6,3,5,4,11,10},
                    {7,6,0,10,2,3},
                    {10,9,1,1,6,7},
                    {11,12,6,3,9,6},
                    {7,5,2,4,0,10},
                    {3,9,11,6,12,11},
                    {2,4,12,7,5,0},
                }
            },{
                58,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {3,4,2,4,1,0},
                    {7,5,7,3,0,1},
                    {12,9,11,8,5,8},
                    {10,12,9,2,7,11},
                    {10,8,11,10,11,10},
                    {2,1,0,5,3,4},
                    {3,2,1,5,7,12},
                    {8,12,7,0,4,7},
                }
            },
        {
                40,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {7,1,4,9,5,10},
                    {4,11,6,12,2,6},
                    {2,11,9,10,12,3},
                    {5,0,12,7,1,2},
                    {8,9,1,12,10,8},
                    {6,3,2,3,0,4},
                    {1,5,7,1,5,3},
                    {10,4,6,9,1,7},
                }
            },

        {
                41,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,2,1,8,11,3},
                    {5,4,9,5,4,6},
                    {12,7,10,11,10,7},
                    {10,6,3,9,6,4},
                    {7,1,5,11,1,9},
                    {9,3,8,3,3,12},
                    {4,1,7,11,4,5},
                    {0,6,10,3,2,4},
                }
            },
        {
                46,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {9,12,5,4,9,11},
                    {4,7,3,1,4,3},
                    {1,0,11,8,2,6},
                    {6,4,5,0,10,9},
                    {10,1,1,4,3,7},
                    {9,0,5,3,5,6},
                    {7,4,11,6,2,10},
                    {11,8,10,0,12,7},
                }
            },
        {
                51,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {10,0,9,4,1,0},
                    {12,3,11,2,4,11},
                    {6,1,1,4,0,6},
                    {5,2,3,7,2,1},
                    {7,9,10,11,10,2},
                    {3,6,12,8,11,6},
                    {4,0,5,4,3,7},
                    {7,4,8,10,5,5},
                }
            },
        {
                52,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {8,11,2,5,4,0},
                    {12,8,10,7,4,10},
                    {5,2,8,1,9,5},
                    {7,9,11,6,1,0},
                    {6,10,6,3,7,11},
                    {10,12,4,5,4,4},
                    {1,0,8,3,2,1},
                    {6,7,2,4,11,0},
                }
            },
    };

    public static int[,] GetLevelDataByIndex(int level)
    {
        int[,] leveldata = new int[Konstants.REAL_ROW_SIZE, Konstants.REAL_COLUMN_SIZE];
        if (levelData.ContainsKey(level))
        {
            leveldata = levelData[level];
        }
        else
            leveldata = levelData[GetMappedLevel(level)];//Mapping Logic should be here ||Download from server
        return leveldata;
    }


    ///DUMMY Formula
    //private static int offset = 20;
    private static int GetMappedLevel(int index)
    {
        return (index % 10) + levelData.Count / 2;
    }
}
