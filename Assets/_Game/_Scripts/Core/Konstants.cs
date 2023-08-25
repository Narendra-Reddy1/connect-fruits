
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

    //ECONOMY
    public static int DEFAULT_COIN_BALANCE = 10;
    public static int FRUIT_DUMPER_POWERUP_COST = 10;
    public static int FRUIT_BOMB_POWERUP_COST = 20;
    public static int TRIPLE_BOMB_POWERUP_COST = 40;
    ///Default data
    public static int DEFAULT_FRUIT_BOMB_POWERUP_BALANCE = 1;
    public static int DEFAULT_TRIPLE_BOMB_POWERUP_BALANCE = 1;
    public static int DEFAULT_FRUIT_DUMPER_POWERUP_BALANCE = 1;

    //IAP
    public const string PURCHASE_INITIALIZING_TEXT = "Purchase Initializing...";
    public const string PURCHASE_SUCCESS_TEXT = "Purchase Success...";
    public const string PURCHASE_FAILED_TEXT = "Purchase Failed...";

    //IAP Product IDs

    //Bundles
    public const string STARTER_PACK = "starter_pack";
    public const string MASTER_PACK = "master_pack";
    public const string MONSTER_PACK = "monster_pack";

    //Single packs
    public const string COIN_PACK_1 = "coin_pack_1";
    public const string COIN_PACK_2 = "coin_pack_2";
    public const string COIN_PACK_3 = "coin_pack_3";
    public const string COIN_PACK_4 = "coin_pack_4";
    public const string COIN_PACK_5 = "coin_pack_5";

    public const string FRUIT_BOMB_PACK_1 = "fruit_bomb_pack_1";
    public const string TRIPLE_BOMB_PACK_1 = "triple_bomb_pack_1";
    public const string FRUIT_DUMPER_PACK_1 = "fruit_dumper_pack_1";
    //No Ads
    public const string NO_ADS = "no_ads";
}
