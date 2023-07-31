
using UnityEngine;

public static class Konstants
{

    //FOR LEVELDATA
    public const byte REAL_COLUMN_SIZE = 6;
    public const byte REAL_ROW_SIZE = 8;

    //FOR GRID GENERATION
    //DO NOT TOUCH THESE VALUES
    public const byte ROW_SIZE = REAL_ROW_SIZE + 2;//10 //8
    public const byte COLUMN_SIZE = REAL_COLUMN_SIZE + 2;//8 //6

    public const byte MIN_FRUITS_TO_MATCH = 2;
    public const byte LEVEL_STARTUP_TIMER_IN_SECONDS = 4;
    public const byte FRUIT_CALL_TIMER = 4;

    //SCORE
    public const short PAIR_MATCH_SCORE = 300;
    public const byte QUICK_MATCH_BONUS = 100;//ranges from 0 to 100
    public const short ROW_CLEAR_BONUS = 300;
    public const short COLUMN_CLEAR_BONUS = 400; 
    public const short ENTIRE_BOARD_CLEAR_BONUS = 1000;
    public const byte REMAINING_TIMER_PER_SECOND_BONUS = 100;


    public const string INIT_SCENE = "InitScene";
    public const string PERSISTENT_MANAGERS = "PersistentManagers";
    public const string HOME_SCENE = "HomeScene";


}
