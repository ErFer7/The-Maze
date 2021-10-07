using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ExitScreen : MonoBehaviour
{
    #region Public Variables
    // Tempo de animação
    public float fadeTime;

    // Tempo da animação da pontuação
    public float scoreTime;

    // Dificuldade (Divisor da pontuação)
    public float difficulty;

    // Dificuldade do modo escuro (Divisor da pontuação)
    public float difficulty_DarkMode;

    // Textos
    public Text title;
    public Text subTitle;
    public Text scoreText;
    public Text scoreDisplay;

    // Botões
    public GameObject nextButton;
    public GameObject menuButton;
    public GameObject restartButton;

    // Acesso ao Timer
    public Timer timer;
    #endregion

    #region Private Variables
    // Pontuação dinâmica (Usada na animação da pontuãção)
    private float dynamicScore;

    // Pontuação
    private int score;

    // Acesso ao Canvas
    private CanvasGroup canvas;

    // Coroutine da tela de saída
    private Coroutine coroutine;

    // Acesso ao Music Player
    private GameObject musicPlayer;

    // Acesso ao PlayGames Manager
    private PlayGamesManager playGamesManager;

    // Acesso ao Script Manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Inicialização para o tempo não regressivo
        if (!scriptManager.regressiveTime)
        {
            // Para modos progressivos (Clássico)
            if (scriptManager.progressive)
            {
                // Traduz o título se possível
                if (scriptManager.dynamicLocalizedText.ContainsKey("exitScreen_title_Success_Progressive"))
                {
                    title.text = string.Format(scriptManager.dynamicLocalizedText["exitScreen_title_Success_Progressive"], scriptManager.level);
                }
                else
                {
                    title.text = "LEVEL " + scriptManager.level + " COMPLETED";
                }

                // Traduz o subtítulo se possível
                if (scriptManager.dynamicLocalizedText.ContainsKey("exitScreen_subTitle"))
                {
                    subTitle.text = string.Format(scriptManager.dynamicLocalizedText["exitScreen_subTitle"], scriptManager.seed);
                }
                else
                {
                    subTitle.text = "Maze " + scriptManager.seed;
                }

                // Desabilita o botão de "próximo" no nível 99, labirinto 100x100
                if (scriptManager.level == 99)
                {
                    nextButton.SetActive(false);

                    // Reorganiza o layout da tela de saída
                    menuButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0.5F);
                    menuButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5F);
                    menuButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 0);
                }
            }
            // Para modos não progressivos (Personalizado)
            else
            {
                // Traduz o título se possível
                if (scriptManager.dynamicLocalizedText.ContainsKey("exitScreen_title_Success_Non-Progressive"))
                {
                    title.text = scriptManager.dynamicLocalizedText["exitScreen_title_Success_Non-Progressive"];
                }
                else
                {
                    title.text = "LEVEL COMPLETED";
                }

                // Traduz o subtítulo se possível
                if (scriptManager.dynamicLocalizedText.ContainsKey("exitScreen_subTitle"))
                {
                    subTitle.text = string.Format(scriptManager.dynamicLocalizedText["exitScreen_subTitle"], scriptManager.seed);
                }
                else
                {
                    subTitle.text = "Maze " + scriptManager.seed;
                }

                // Desabilita o botão de "próximo"
                nextButton.SetActive(false);

                // Reorganiza o layout da tela de saída
                menuButton.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0.5F);
                menuButton.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5F);
                menuButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-300, 0);
            }
        }

        // Traduz o texto da pontuação se possível
        if (scriptManager.dynamicLocalizedText.ContainsKey("exitScreen_Score"))
        {
            scoreText.text = scriptManager.dynamicLocalizedText["exitScreen_Score"];
            scoreText.fontSize = scriptManager.scoreFontSize;
        }
        else
        {
            scoreText.text = "Score";
            scoreText.fontSize = 60;
        }
    }

    private void OnEnable()
    {
        // Inicializa a tela de saída
        coroutine = StartCoroutine(Exit());
    }
    #endregion

    #region Exit Operations
    IEnumerator Exit()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Condição inicial
        scriptManager.animating = true;

        // Acessa os objetos
        canvas = gameObject.GetComponent<CanvasGroup>();
        musicPlayer = GameObject.FindWithTag("GameplayMusicPlayer");
        playGamesManager = GameObject.FindWithTag("PlayGamesManager").GetComponent<PlayGamesManager>();

        // Reseta o alfa da tela de saída para 0
        canvas.alpha = 0F;

        // Salva o progresso
        SaveProgress();

        // Se o labirinto não é regressivo
        if (!scriptManager.regressiveTime)
        {
            // Pontuação do modo clássico
            if (!scriptManager.dark)
            {
                score = CalculateScore(scriptManager.width, scriptManager.height, timer.currentTime, false, difficulty);
            }
            // Pontuação do modo escuro
            else
            {
                score = CalculateScore(scriptManager.width, scriptManager.height, timer.currentTime, false, difficulty_DarkMode);
            }

            if (PlayerPrefs.GetInt("Authenticated", 0) == 1)
            {
                // Conquista de término de campanha
                if (scriptManager.level == 99)
                {
                    // Clássico
                    if (scriptManager.dark == false)
                    {
                        playGamesManager.RunEndingAchievement("classic", true);
                    }
                    // Escuro
                    else
                    {
                        playGamesManager.RunEndingAchievement("dark", true);
                    }
                }
            }
        }
        // Se o jogador perdeu no modo tempo
        else if (timer.currentTime < 0)
        {
            // Exibe o título, traduzido se possível
            if (scriptManager.dynamicLocalizedText.ContainsKey("exitScreen_title_Failure"))
            {
                title.text = scriptManager.dynamicLocalizedText["exitScreen_title_Failure"];
            }
            else
            {
                title.text = "YOU FAILED";
            }

            // Define a cor do título como vermelho
            title.color = new Color(0.5F, 0, 0);

            // Traduz o subtítulo se possível
            if (scriptManager.dynamicLocalizedText.ContainsKey("exitScreen_subTitle") == true)
            {
                subTitle.text = string.Format(scriptManager.dynamicLocalizedText["exitScreen_subTitle"], scriptManager.seed);
            }
            else
            {
                subTitle.text = "Maze " + scriptManager.seed;
            }

            // Define a cor do subtítulo como vermelho
            subTitle.color = title.color;

            // Define a pontuação como 0
            score = 0;

            // Desabilita o botão de "próximo" e de "reiniciar"
            nextButton.SetActive(false);
            restartButton.SetActive(false);
        }
        // Se o jogador ganhou no modo tempo
        else if (timer.currentTime > 0)
        {
            // Exibe o título, traduzido se possível
            if (scriptManager.dynamicLocalizedText.ContainsKey("exitScreen_title_Success_Progressive"))
            {
                title.text = string.Format(scriptManager.dynamicLocalizedText["exitScreen_title_Success_Progressive"], scriptManager.level);
            }
            else
            {
                title.text = "LEVEL " + scriptManager.level + " COMPLETED";
            }

            // Traduz o subtítulo se possível
            if (scriptManager.dynamicLocalizedText.ContainsKey("exitScreen_subTitle") == true)
            {
                subTitle.text = string.Format(scriptManager.dynamicLocalizedText["exitScreen_subTitle"], scriptManager.seed);
            }
            else
            {
                subTitle.text = "Maze " + scriptManager.seed;
            }

            // Calcula a pontuação
            score = CalculateScore(scriptManager.width, scriptManager.height, timer.currentTime, true);

            if (PlayerPrefs.GetInt("Authenticated", 0) == 1)
            {
                // Conquista de término de campanha (Tempo)
                if (scriptManager.level == 99)
                {
                    playGamesManager.RunEndingAchievement("time", true);
                }

                // Conquista de término do labirinto com 0.1 segundos restantes
                if (timer.currentTime <= 0.1F)
                {
                    playGamesManager.TimeAchievement(true);
                }
            }
        }

        if (PlayerPrefs.GetInt("Authenticated", 0) == 1)
        {
            // Conquistas relacionadas a pontuaçãp
            if (score > 0)
            {
                playGamesManager.PostMaximumScoreOnLeaderboard(score);
                playGamesManager.SubmitScore(score);
                playGamesManager.LevelAchievements();
            }

            // Conquista de tamanho de labirinto
            playGamesManager.MazeSizeAchievement(scriptManager.width, scriptManager.height, true);
        }

        // Abafa a música
        if (musicPlayer.GetComponent<AudioLowPassFilter>().cutoffFrequency != 22000)
        {
            musicPlayer.GetComponent<MonoBehaviour>().StopCoroutine(musicPlayer.GetComponent<MusicPlayer>().coroutine_SC_LPFF);
            musicPlayer.GetComponent<MusicPlayer>().coroutine_SC_LPFF = StartCoroutine(musicPlayer.GetComponent<MusicPlayer>().LowPassFilterFade(22000F, 0.5F));
        }
        musicPlayer.GetComponent<AudioLowPassFilter>().enabled = true;
        musicPlayer.GetComponent<MusicPlayer>().coroutine_SC_LPFF = musicPlayer.GetComponent<MonoBehaviour>().StartCoroutine(musicPlayer.GetComponent<MusicPlayer>().LowPassFilterFade(400F, fadeTime));

        // Operação de fade in do canvas
        for (float i = 0; i <= 1F; i += Time.deltaTime / fadeTime)
        {
            // Para quando o alfa é maior que 0.95
            if (canvas.alpha > 0.95F)
            {
                canvas.alpha = 1F;
                i = 2;
            }
            // Operação de lerp no alfa até 1
            else
            {
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1F, i);
            }

            yield return null;
        }

        // Condição final, permite que o jogador use os botões
        scriptManager.animating = false;

        // Anima a pontuação
        for (float i = 0; i <= 1F; i += Time.deltaTime / scoreTime)
        {
            // Operação de lerp da pontuação de 0 até (pontuação + 5)
            if (dynamicScore < score)
            {
                dynamicScore = Mathf.Lerp(dynamicScore, score + 5, i);
            }
            // Termina a animação
            else
            {
                dynamicScore = score;
                i = 2;
            }

            // Exibe o texto
            scoreDisplay.text = "" + Mathf.Floor(dynamicScore);

            yield return null;
        }

        // Para a coroutine da saída
        StopCoroutine(coroutine);

        yield return null;
    }
    #endregion

    #region Useful Methods
    private void SaveProgress()
    {
        // Se o labirinto é progressivo (Clássico, tempo ou escuro)
        if (scriptManager.progressive)
        {
            // Salva apenas se o nível não é o nível final
            if (scriptManager.level < 99)
            {
                // Para modos não regressivos (Clássico ou escuro)
                if (!scriptManager.regressiveTime)
                {
                    // Clássico
                    if (!scriptManager.dark)
                    {
                        PlayerPrefs.SetInt("classicLevel", (scriptManager.level + 1));
                        PlayerPrefs.SetInt("classicSeed", (int)System.DateTime.Now.Ticks);
                    }
                    // Escuro
                    else
                    {
                        PlayerPrefs.SetInt("darkLevel", (scriptManager.level + 1));
                        PlayerPrefs.SetInt("darkSeed", (int)System.DateTime.Now.Ticks);
                    }
                }
                // Tempo
                else
                {
                    // Se o jogador ganhou no modo tempo
                    if (timer.currentTime > 0)
                    {
                        PlayerPrefs.SetInt("timeLevel", (scriptManager.level + 1));
                        PlayerPrefs.SetInt("timeSeed", (int)System.DateTime.Now.Ticks);
                    }
                    // Se o jogador perdeu no modo tempo a campanha é descartada
                    else
                    {
                        PlayerPrefs.SetInt("timeSaved", -1);
                    }
                }
            }
            // Nível final (99)
            else
            {
                // Para modos não regressivos (Clássico ou escuro)
                if (!scriptManager.regressiveTime)
                {
                    // Clássico
                    if (!scriptManager.dark)
                    {
                        // Descarta a campanha
                        PlayerPrefs.SetInt("classicSaved", -1);
                    }
                    // Escuro
                    else
                    {
                        // Descarta a campanha
                        PlayerPrefs.SetInt("darkSaved", -1);
                    }
                }
                // Tempo
                else
                {
                    // Descarta a campanha
                    PlayerPrefs.SetInt("timeSaved", -1);
                }
            }
        }
    }

    private int CalculateScore(int width, int height, float time, bool isRegressive, float difficulty = 1F)
    {
        int result;

        if (!isRegressive)
        {
            // Clássico, Escuro ou Personalizado
            result = Mathf.FloorToInt((Mathf.Pow(width, 2) * Mathf.Pow(height, 2) * 2) / (time * 7 * difficulty));
        }
        else
        {
            // Tempo
            result = Mathf.FloorToInt((14F * time * width * height) / ((2F * width * height) + 7F));
        }

        return result;
    }
    #endregion
}