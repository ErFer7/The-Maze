using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    #region Public Variables
    // Labirinto
    public int width;
    public int height;
    public bool useSavedSeed;
    public int seed;
    public bool progressive;
    public int level;
    public bool restarting;
    public bool regressiveTime;
    public bool continueLastMaze;
    public bool dark;

    // Menu
    public bool animating;

    // Loading control
    public int loadingStage;

    // Gameplay
    public bool playerCanMove;

    // Localização;
    public Dictionary<string, string> dynamicLocalizedText;
    public int scoreFontSize;

    // Audio
    public bool sound;
    public bool music;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Impede que duas intâncias da classe existam ao mesmo tempo (Singleton)
        if (GameObject.FindGameObjectsWithTag("ScriptManager").Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializa os atributos (colocar em um Start no futuro, esta maneira é apenas temporária)
        width = 0;
        height = 0;
        useSavedSeed = false;
        seed = 0;
        progressive = false;
        level = 0;
        restarting = false;
        regressiveTime = false;
        continueLastMaze = false;
        dark = false;
        animating = false;
        loadingStage = 0;
        playerCanMove = false;
        dynamicLocalizedText = new Dictionary<string, string>();
        scoreFontSize = 0;
        sound = false;
        music = false;
    }
    #endregion
}
