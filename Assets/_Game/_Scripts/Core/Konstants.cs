
using UnityEngine;

public static class Konstants
{
    //FOR levelData
    public const byte REAL_COLUMN_SIZE = 6;
    public const byte REAL_ROW_SIZE = 8;

    //FOR GRID GENERATION
    //DO NOT TOUCH THESE VALUES
    public const byte ROW_SIZE = REAL_ROW_SIZE + 2;//10 //8
    public const byte COLUMN_SIZE = REAL_COLUMN_SIZE + 2;//8 //6

    public const byte MIN_FRUITS_TO_MATCH = 2;
    public const byte LEVEL_STARTUP_TIMER_IN_SECONDS = 4;
    public const byte FRUIT_CALL_TIMER_IN_SECONDS = 4;
    public const byte TIME_TO_WAIT_TO_SHOW_AVAILABLE_PAIR_TO_MATCH = 10;

    public const byte MIN_LEVEL_FOR_TIMER = 6;
    public const byte MIN_LEVEL_FOR_STREAK = 6;
    public const byte MAX_LEVEL_TO_SHOW_PAIR_HINT = 6;

    public const float DEFAULT_STAR_MULTIPLIER_TIMER_IN_SECONDS = 4;
    public const float STAR_MULTIPLIER_DECAY_RATE = 0.5f;//how much the timer should be reduced.
    public const byte STAR_MULTIPLIER_DECAY_FREQUENCY = 5;//frequency to reduce the star multiplier timer
    public const float STAR_MULTIPLIER_LOW_CAP_TIMER = 2.5f;//Lowest time for star multiplier.

    //SCORE
    public const short PAIR_MATCH_SCORE = 50;
    public const byte QUICK_MATCH_BONUS = 10;//ranges from 0 to 100
    public const short ROW_CLEAR_BONUS = 30;
    public const short COLUMN_CLEAR_BONUS = 40;
    public const short ENTIRE_BOARD_CLEAR_BONUS = 100;
    public const short REMAINING_TIMER_PER_SECOND_BONUS = 10;

    //Powerup Tutorial
    public const byte HINT_POWERUP_UNLOCK_LEVEL = 4;//3
    public const byte FRUIT_BOMB_UNLOCK_LEVEL = 6;//6
    public const byte TRIPLE_BOMB_UNLOCK_LEVEL = 8;//8

    public const string STUCK_MESSAGE = "Got STUCK?? Use powerup!";
    public const string HINT_POWERUP_MESSAGE = "Auto match a random pair.";
    public const string TRIPLE_BOMB_POWERUP_MESSAGE = "Eliminate 3 fruit pairs of your wish.";
    public const string FRUIT_BOMB_POWERUP_MESSAGE = "Eliminate a fruit pair of your wish.";
    ///CHEST
    public const short MAX_STARS_REQUIRED_FOR_STAR_CHEST = 1000;



    public const string INIT_SCENE = "InitScene";
    public const string PERSISTENT_MANAGERS = "PersistentManagers";
    public const string HOME_SCENE = "HomeScene";

    //ECONOMY
    public static int DEFAULT_COIN_BALANCE = 30;//COINS
    public static int HINT_POWERUP_COST = 20;//COINS
    public static int FRUIT_BOMB_POWERUP_COST = 20;//COINS;
    public static int TRIPLE_BOMB_POWERUP_COST = 40;//COINS
    public const byte OUT_OF_TIME_COST = 60;//COINS


    ///Default data
    public static int DEFAULT_FRUIT_BOMB_POWERUP_BALANCE = 1;
    public static int DEFAULT_TRIPLE_BOMB_POWERUP_BALANCE = 1;
    public static int DEFAULT_HINT_POWERUP_BALANCE = 1;


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
    public const string HINT_POWERUP_PACK_1 = "fruit_dumper_pack_1";
    //MINI STORE
    public const string MINI_STORE_FRUIT_BOMB_NANO_PACK= "ms_fruit_bomb_nano";
    //No Ads
    public const string NO_ADS = "no_ads";
}
