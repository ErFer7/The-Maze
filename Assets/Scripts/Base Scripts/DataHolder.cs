using System.Collections.Generic;

public static class DataHolder
{
    /*
        Esta classe é ineficiente e será substituída por um singleton     
    */

    // Labirinto
    public static int width;
    public static int height;
    public static bool useSavedSeed;
    public static int seed;
    public static bool progressive;
    public static int level;
    public static bool restarting;
    public static bool regressiveTime;
    public static bool continueLastMaze;
    public static bool dark;

    // Menu
    public static bool animating;

    // Loading control
    public static int loadingStage;

    // Gameplay
    public static bool playerCanMove;

    // Localização;
    public static Dictionary<string, string> dynamicLocalizedText = new Dictionary<string, string>();
    public static int scoreFontSize;

    // Audio
    public static bool sound;
    public static bool music;
}