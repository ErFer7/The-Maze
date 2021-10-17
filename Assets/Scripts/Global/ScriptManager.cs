using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    #region Public Variables
    // Managers
    public UIManager uiManager;
    public MusicManager musicManager;
    public Abstrato.SceneManager sceneManager;
    public GameplayManager gameplayManager;

    // Labirinto
    public int width;
    public int height;
    public bool useSavedSeed;
    public int seed;
    public bool progressive;
    public int level;
    public bool regressiveTime;
    public bool continueLastMaze;
    public bool dark;

    // Menu
    public bool animating;

    // Loading control
    public int loadingStage;

    // Gameplay
    public bool playerCanMove;

    // Localiza��o;
    public Dictionary<string, string> dynamicLocalizedText;
    public int scoreFontSize;

    // Audio
    public bool sound;
    public bool music;

    public GameState gameState;
    public GameMode gameMode;
    public bool preserveSave;

    //Estado de jogo
    public enum GameState
    {
        Initializing,
        LoadingMenu,
        Menu,
        LoadingGameplay,
        Gameplay
    }

    public enum GameMode
    {
        Null,
        Classic,
        Time,
        Dark,
        Custom
    }

    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Impede que duas int�ncias da classe existam ao mesmo tempo (Singleton)
        if (GameObject.FindGameObjectsWithTag(tag).Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Inicializa os atributos (colocar em um Start no futuro, esta maneira � apenas tempor�ria)
        width = 2;
        height = 2;
        useSavedSeed = false;
        seed = 0;
        progressive = false;
        level = 0;
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

    private void Start()
    {
        // Define o FPS alvo para 60 FPS
        Application.targetFrameRate = 60;

        gameState = GameState.Initializing;
        gameMode = GameMode.Null;
    }
    #endregion
}
