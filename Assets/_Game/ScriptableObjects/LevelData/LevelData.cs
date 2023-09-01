using System.Collections.Generic;

public static class LevelData
{
    public static Dictionary<int, int[,]> LEVELDATA = new Dictionary<int, int[,]>()
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
            },{
               3,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {0,2,-1,-1,4,0},
                    {1,3,-1,-1,3,1},
                    {0,4,-1,-1,0,3},
                    {1,3,2,3,0,4},
                    {2,2,1,1,4,2},
                    {3,0,-1,-1,3,3},
                    {4,0,-1,-1,4,1},
                    {0,4,-1,-1,2,4},
                }
            },{
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
       { 11,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
                {
                    {5,0,1,2,3,4},
                    {5,0,1,2,3,4},
                    {1,2,3,4,5,0},
                    {2,3,4,5,0,1},
                    {3,4,5,0,1,2},
                    {4,5,0,1,2,3},
                    {0,1,2,3,4,5},
                    {0,1,2,3,4,5},
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
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
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
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
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
            },{
                15,
                new int[Konstants.REAL_ROW_SIZE,Konstants.REAL_COLUMN_SIZE]
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




    };

    public static int[,] GetLevelDataByIndex(int level)
    {
        int[,] leveldata = new int[Konstants.REAL_ROW_SIZE, Konstants.REAL_COLUMN_SIZE];
        if (LEVELDATA.ContainsKey(level))
        {
            leveldata = LEVELDATA[level];
        }
        else
            leveldata = LEVELDATA[GetMappedLevel(level)];//Mapping Logic should be here
        return leveldata;
    }


    ///DUMMY Formula
    private static int offset = 6;
    private static int GetMappedLevel(int index)
    {
        return (index % 10) + offset;
      
    }
}
