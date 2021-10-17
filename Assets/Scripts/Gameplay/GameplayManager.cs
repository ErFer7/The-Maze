using System.Collections;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [HideInInspector] public bool mazeGenerated;
    public MazeGenerator mazeGenerator;
    public GameObject player;
    public Timer timer;

    private ScriptManager scriptManager;
    private Coroutine coroutine_MGC;
    private Coroutine coroutine_MGIC; // Coroutines internas do gerador de labirintos

    private void Start()
    {
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();
        scriptManager.gameplayManager = GetComponent<GameplayManager>();
        mazeGenerated = false;
    }

    public void StartMazeGeneration()
    {
        SetMazeParameters();
        coroutine_MGC = StartCoroutine(MazeGenerationCoroutine());
    }

    public void StartGameplay()
    {
        scriptManager.playerCanMove = true;
        timer.StartTimer();
    }

    private IEnumerator MazeGenerationCoroutine()
    {
        mazeGenerator.finishFlag = false;
        coroutine_MGIC = StartCoroutine(mazeGenerator.CreateWalls(scriptManager.width,
                                                                  scriptManager.height,
                                                                  scriptManager.gameMode));

        while (!mazeGenerator.finishFlag)
        {
            yield return null;
        }

        mazeGenerator.finishFlag = false;
        coroutine_MGIC = StartCoroutine(mazeGenerator.GeneratePath(scriptManager.width,
                                                                   scriptManager.height,
                                                                   scriptManager.seed));

        while (!mazeGenerator.finishFlag)
        {
            yield return null;
        }

        mazeGenerator.finishFlag = false;
        coroutine_MGIC = StartCoroutine(mazeGenerator.SetSpawn(scriptManager.width,
                                                               scriptManager.height,
                                                               scriptManager.gameMode,
                                                               player));

        while (!mazeGenerator.finishFlag)
        {
            yield return null;
        }

        mazeGenerated = true;
        yield return null;
    }

    private void SetMazeParameters()
    {
        switch (scriptManager.gameMode)
        {
            case ScriptManager.GameMode.Classic:
                if (scriptManager.preserveSave)
                {
                    scriptManager.level = PlayerPrefs.GetInt("classicLevel");
                    scriptManager.seed = PlayerPrefs.GetInt("classicSeed");
                }
                else
                {
                    scriptManager.level = 1;
                    scriptManager.seed = (int)System.DateTime.Now.Ticks;

                    PlayerPrefs.SetInt("classicSaved", 1);
                    PlayerPrefs.SetInt("classicLevel", scriptManager.level);
                    PlayerPrefs.SetInt("classicSeed", scriptManager.seed);
                }

                scriptManager.width = scriptManager.level + 1;
                scriptManager.height = scriptManager.level + 1;
                break;
            case ScriptManager.GameMode.Time:
                if (scriptManager.preserveSave)
                {
                    scriptManager.level = PlayerPrefs.GetInt("timeLevel");
                    scriptManager.seed = PlayerPrefs.GetInt("timeSeed");
                }
                else
                {
                    scriptManager.seed = (int)System.DateTime.Now.Ticks;
                    scriptManager.level = 1;

                    PlayerPrefs.SetInt("timeSaved", 1);
                    PlayerPrefs.SetInt("timeLevel", scriptManager.level);
                    PlayerPrefs.SetInt("timeSeed", scriptManager.seed);
                }

                scriptManager.width = scriptManager.level + 1;
                scriptManager.height = scriptManager.level + 1;
                break;
            case ScriptManager.GameMode.Dark:
                if (scriptManager.preserveSave)
                {
                    scriptManager.level = PlayerPrefs.GetInt("darkLevel");
                    scriptManager.seed = PlayerPrefs.GetInt("darkSeed");
                }
                else
                {
                    scriptManager.seed = (int)System.DateTime.Now.Ticks;
                    scriptManager.level = 1;

                    PlayerPrefs.SetInt("darkSaved", 1);
                    PlayerPrefs.SetInt("darkLevel", scriptManager.level);
                    PlayerPrefs.SetInt("darkSeed", scriptManager.seed);
                }

                scriptManager.width = scriptManager.level + 1;
                scriptManager.height = scriptManager.level + 1;
                break;
            case ScriptManager.GameMode.Custom:
                scriptManager.level = 0;

                if (!scriptManager.useSavedSeed)
                {
                    scriptManager.seed = (int)System.DateTime.Now.Ticks;
                }
                break;
            default:
                break;
        }

        scriptManager.preserveSave = false;
        scriptManager.useSavedSeed = false;
    }
}
