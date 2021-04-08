using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class PlayGamesManager : MonoBehaviour
{
    #region Public Variables
    // Acesso a caixa de mensagens
    [System.NonSerialized]
    public RectTransform playGamesMessageBoxTransform;

    // Estados da caixa de mensagens
    public enum MessageBoxState
    {
        WaitingForLogIn,
        FailedLogIn,
        LogInConcluded,
        OkButtonPressed
    }
    #endregion

    #region Private Variables
    // Acesso a caixa de mensagens e seus itens
    private GameObject playGamesMessageBox;
    private Image playGamesMessageBoxBackgroud;
    private Text message;
    private Button okButton;

    // Posições da caixa de mensagens
    private Vector2 targetPositionUp;
    private Vector2 targetPositionDown;

    // Tempo de animação da caixa de mensagens
    private float animationTime;

    // Coroutine da animação da caixa de mensagens
    private Coroutine coroutine;

    // Acesso aos botões
    private GameObject playGamesButton;
    private GameObject achievementsButton;
    private GameObject leaderboardsButton;

    // Cores
    private Color colorUp;
    private Color colorDown;
    private Color red;
    private Color grey;

    // Acesso ao music manager
    private MusicManager musicManager;

    // Acesso ao script manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Impede que duas intâncias da classe existam ao mesmo tempo (Singleton)
        if (GameObject.FindGameObjectsWithTag("PlayGamesManager").Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Definição do script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Inicialização da plataforma Play Games
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        // Definição de cores
        red = new Color(0.60F, 0F, 0F);
        grey = new Color(0.196F, 0.196F, 0.196F);
        colorUp = new Color(0F, 0F, 0F, 0.392F);
        colorDown = new Color(0F, 0F, 0F, 0F);

        // Definição de posições
        targetPositionUp = new Vector2(0, 0);
        targetPositionDown = new Vector2(0, -485);

        // Definição do tempo de animação
        animationTime = 0.65F;

        // Verifica se o usuário está autenticado (Offline)
        if (PlayerPrefs.GetInt("Authenticated", 0) == 1)
        {
            // Verifica se o usuário não está autenticado (Online)
            if (!Social.localUser.authenticated)
            {
                // Autenticação
                Social.localUser.Authenticate(success =>
                {
                    if (success)
                    {
                        // Habilita os botões e sincroniza os dados
                        PlayerPrefs.SetString("localUserID", Social.localUser.id);
                        achievementsButton.GetComponent<Button>().interactable = true;
                        leaderboardsButton.GetComponent<Button>().interactable = true;
                        Synchronize();
                    }
                    else
                    {
                        // Erro na autenticação
                        playGamesButton.GetComponent<Image>().color = red;
                        achievementsButton.GetComponent<Button>().interactable = false;
                        leaderboardsButton.GetComponent<Button>().interactable = false;
                    }
                });
            }
            else
            {
                // Sincroniza os dados offline e online
                Synchronize();
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Acessa os objetos necessários
            playGamesButton = GameObject.FindWithTag("PlayGamesButton");
            achievementsButton = GameObject.FindWithTag("AchievementsButton");
            leaderboardsButton = GameObject.FindWithTag("LeaderboardsButton");
            playGamesMessageBox = GameObject.FindWithTag("PlayGamesMessageBox").transform.GetChild(0).gameObject;
            musicManager = GameObject.FindWithTag("MusicManager").GetComponent<MusicManager>();
            playGamesMessageBoxBackgroud = playGamesMessageBox.GetComponent<Image>();
            playGamesMessageBoxTransform = playGamesMessageBox.transform.GetChild(0).GetComponent<RectTransform>();
            message = playGamesMessageBox.transform.GetChild(0).GetChild(1).GetComponent<Text>();
            okButton = playGamesMessageBox.transform.GetChild(0).GetChild(2).GetComponent<Button>();

            // Sistema de autenticação idêntico ao sistema na método Start()
            if (PlayerPrefs.GetInt("Authenticated", 0) == 1)
            {
                if (!Social.localUser.authenticated)
                {
                    Social.localUser.Authenticate(success =>
                    {
                        if (success)
                        {
                            PlayerPrefs.SetString("localUserID", Social.localUser.id);
                            achievementsButton.GetComponent<Button>().interactable = true;
                            leaderboardsButton.GetComponent<Button>().interactable = true;
                            Synchronize();
                        }
                        else
                        {
                            playGamesButton.GetComponent<Image>().color = red;
                            achievementsButton.GetComponent<Button>().interactable = false;
                            leaderboardsButton.GetComponent<Button>().interactable = false;
                        }
                    });
                }
                else
                {
                    Synchronize();
                }
            }
        }
    }
    #endregion

    #region PlayGames Core
    public void LogIn()
    {
        // Faz o login
        Social.localUser.Authenticate(success =>
        {
            // Inicializa a caixa de mensagens no estado de espera
            MessageBoxControl(MessageBoxState.WaitingForLogIn);

            if (success)
            {
                // Em caso de sucesso faz a autenticação (Offline)
                if (PlayerPrefs.GetInt("Authenticated", 0) == 0)
                {
                    PlayerPrefs.SetInt("Authenticated", 1);
                }

                // Define o ID do usuário
                PlayerPrefs.SetString("localUserID", Social.localUser.id);

                // Habilita os botões
                playGamesButton.GetComponent<Image>().color = grey;
                achievementsButton.GetComponent<Button>().interactable = true;
                leaderboardsButton.GetComponent<Button>().interactable = true;

                // Muda o estado da caixa de mensagens para "login concluído"
                MessageBoxControl(MessageBoxState.LogInConcluded);
            }
            else
            {
                // Em caso de falha determina o login offline como não autenticado
                PlayerPrefs.SetInt("Authenticated", 0);

                // Muda a cor do botão de login para vermelho
                playGamesButton.GetComponent<Image>().color = red;

                // Muda o estado da caixa de mensagens para "falha no login"
                MessageBoxControl(MessageBoxState.FailedLogIn);
            }
        });
    }

    public void SignOut()
    {
        // Sai da conta
        PlayGamesPlatform.Instance.SignOut();

        // Sai da conta (Offline)
        PlayerPrefs.SetInt("Authenticated", 0);

        // Limpa o ID do jogador
        PlayerPrefs.SetString("localUserID", "");

        // Reseta os botões para o seu estado inicial
        playGamesButton.GetComponent<Image>().color = grey;
        achievementsButton.GetComponent<Button>().interactable = false;
        leaderboardsButton.GetComponent<Button>().interactable = false;
    }

    public void ShowLeaderboard()
    {
        // Mostra o placar
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_top_score_by_maze);
    }

    public void ShowAchievements()
    {
        // Mostra as conquistas
        Social.ShowAchievementsUI();
    }

    private void Synchronize()
    {
        // Sincroniza as conquistas de tamanho
        MazeSizeAchievement(25, 25, false);
        MazeSizeAchievement(50, 50, false);
        MazeSizeAchievement(75, 75, false);
        MazeSizeAchievement(100, 100, false);

        // Sincroniza as conquistas de término de campanha
        RunEndingAchievement("classic", false);
        RunEndingAchievement("time", false);
        RunEndingAchievement("dark", false);

        // Sincroniza a conquista de tempo
        TimeAchievement(false);

        // Sincroniza as conquistas de níveis
        LevelAchievements();
    }
    #endregion

    #region Achievements
    public void MazeSizeAchievement(int width, int height, bool unlocking)
    {
        // NOTA: unlocking existe para que a sincronização possa ser feita sem desbloquear a conquista

        // Tamanhos para a conquista ser desbloqueada: 25x25, 50x50, 75x75 e 100x100
        // Os estados da conquista são armazenados na chave "[Lado]x[Lado]_MazeCompleted_[ID do usuário]"
        // Estados:
        // 0 = Não desbloqueada
        // 1 = Desbloaqueda offline mas não sincronizada online
        // 2 = Desbloaquada e sincronizada

        // Funcionamento (idêntico para todos os tamanhos da conquista)

        // O tamanho é checado
        if (width == 25 && height == 25)
        {
            // Se a conquista não está desbloqueada online
            if (PlayerPrefs.GetInt(string.Format("25x25_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) != 2)
            {
                // Se a conquista deve ser desbloqueada
                if (unlocking)
                {
                    // Desbloqueia offline no estado 1
                    PlayerPrefs.SetInt(string.Format("25x25_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 1);

                    // Tenta desbloquear online
                    Social.ReportProgress(GPGSIds.achievement_a_simple_maze, 100F, (bool success) =>
                    {
                        if (success)
                        {
                            // Define a conquista no estado 2
                            PlayerPrefs.SetInt(string.Format("25x25_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                        }
                    });
                }
                // Se a conquista está apenas sendo sincronizada, é checado se ela está desbloqueada offline
                else if (PlayerPrefs.GetInt(string.Format("25x25_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) == 1)
                {
                    // Tenta desbloquear online
                    Social.ReportProgress(GPGSIds.achievement_a_simple_maze, 100F, (bool success) =>
                    {
                        if (success)
                        {
                            // Define a conquista no estado 2
                            PlayerPrefs.SetInt(string.Format("25x25_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                        }
                    });
                }
            }
        }

        if (width == 50 && height == 50)
        {
            if (PlayerPrefs.GetInt(string.Format("50x50_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) != 2)
            {
                if (unlocking)
                {
                    PlayerPrefs.SetInt(string.Format("50x50_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 1);
                    Social.ReportProgress(GPGSIds.achievement_a_little_big_maze, 100F, (bool success) =>
                    {
                        if (success)
                        {
                            PlayerPrefs.SetInt(string.Format("50x50_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                        }
                    });
                }
                else if (PlayerPrefs.GetInt(string.Format("50x50_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) == 1)
                {
                    Social.ReportProgress(GPGSIds.achievement_a_little_big_maze, 100F, (bool success) =>
                    {
                        if (success)
                        {
                            PlayerPrefs.SetInt(string.Format("50x50_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                        }
                    });
                }
            }
        }

        if (width == 75 && height == 75)
        {
            if (PlayerPrefs.GetInt(string.Format("75x75_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) != 2)
            {
                if (unlocking)
                {
                    PlayerPrefs.SetInt(string.Format("75x75_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 1);
                    Social.ReportProgress(GPGSIds.achievement_a_big_maze, 100F, (bool success) =>
                    {
                        if (success)
                        {
                            PlayerPrefs.SetInt(string.Format("75x75_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                        }
                    });
                }
                else if (PlayerPrefs.GetInt(string.Format("75x75_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) == 1)
                {
                    Social.ReportProgress(GPGSIds.achievement_a_big_maze, 100F, (bool success) =>
                    {
                        if (success)
                        {
                            PlayerPrefs.SetInt(string.Format("75x75_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                        }
                    });
                }
            }
        }

        if (width == 100 && height == 100)
        {
            if (PlayerPrefs.GetInt(string.Format("100x100_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) != 2)
            {
                if (unlocking)
                {
                    PlayerPrefs.SetInt(string.Format("100x100_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 1);
                    Social.ReportProgress(GPGSIds.achievement_a_really_big_maze, 100F, (bool success) =>
                    {
                        if (success)
                        {
                            PlayerPrefs.SetInt(string.Format("100x100_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                        }
                    });
                }
                else if (PlayerPrefs.GetInt(string.Format("100x100_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) == 1)
                {
                    Social.ReportProgress(GPGSIds.achievement_a_really_big_maze, 100F, (bool success) =>
                    {
                        if (success)
                        {
                            PlayerPrefs.SetInt(string.Format("100x100_MazeCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                        }
                    });
                }
            }
        }
    }

    public void RunEndingAchievement(string gameMode, bool unlocking)
    {
        /* Funcionamento similar ao método MazeSizeAchievement(int width, int height, bool unlocking),
         * a diferença é que o caso checado é o modo de jogo
         */

        switch (gameMode)
        {
            case "classic":
                if (PlayerPrefs.GetInt(string.Format("classicRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) != 2)
                {
                    if (unlocking)
                    {
                        PlayerPrefs.SetInt(string.Format("classicRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 1);
                        Social.ReportProgress(GPGSIds.achievement_the_best_of_the_classic, 100F, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(string.Format("classicRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                            }
                        });
                    }
                    else if (PlayerPrefs.GetInt(string.Format("classicRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) == 1)
                    {
                        Social.ReportProgress(GPGSIds.achievement_the_best_of_the_classic, 100F, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(string.Format("classicRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                            }
                        });
                    }
                }
                break;

            case "time":
                if (PlayerPrefs.GetInt(string.Format("timeRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) != 2)
                {
                    if (unlocking)
                    {
                        PlayerPrefs.SetInt(string.Format("timeRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 1);
                        Social.ReportProgress(GPGSIds.achievement_the_fastest, 100F, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(string.Format("timeRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                            }
                        });
                    }
                    else if (PlayerPrefs.GetInt(string.Format("timeRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) == 1)
                    {
                        Social.ReportProgress(GPGSIds.achievement_the_fastest, 100F, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(string.Format("timeRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                            }
                        });
                    }
                }
                break;

            case "dark":
                if (PlayerPrefs.GetInt(string.Format("darkRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) != 2)
                {
                    if (unlocking)
                    {
                        PlayerPrefs.SetInt(string.Format("darkRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 1);
                        Social.ReportProgress(GPGSIds.achievement_a_light_in_the_dark, 100F, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(string.Format("darkRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                            }
                        });
                    }
                    else if (PlayerPrefs.GetInt(string.Format("darkRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) == 1)
                    {
                        Social.ReportProgress(GPGSIds.achievement_a_light_in_the_dark, 100F, (bool success) =>
                        {
                            if (success)
                            {
                                PlayerPrefs.SetInt(string.Format("darkRunCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                            }
                        });
                    }
                }
                break;

            default:
                break;
        }
    }

    public void TimeAchievement(bool unlocking)
    {
        // Funcionamento similar ao método MazeSizeAchievement(int width, int height, bool unlocking)

        if (PlayerPrefs.GetInt(string.Format("lastHundredthOfASecondCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) != 2)
        {
            if (unlocking)
            {
                PlayerPrefs.SetInt(string.Format("lastHundredthOfASecondCompleted_{0}", PlayerPrefs.GetString("localUserID")), 1);
                Social.ReportProgress(GPGSIds.achievement_the_last_hundredth_of_a_second, 100F, (bool success) =>
                {
                    if (success)
                    {
                        PlayerPrefs.SetInt(string.Format("lastHundredthOfASecondCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                    }
                });
            }
            else if (PlayerPrefs.GetInt(string.Format("lastHundredthOfASecondCompleted_{0}", PlayerPrefs.GetString("localUserID")), 0) == 1)
            {
                Social.ReportProgress(GPGSIds.achievement_the_last_hundredth_of_a_second, 100F, (bool success) =>
                {
                    if (success)
                    {
                        PlayerPrefs.SetInt(string.Format("lastHundredthOfASecondCompleted_{0}", PlayerPrefs.GetString("localUserID")), 2);
                    }
                });
            }
        }
    }

    public void LevelAchievements()
    {
        // Carrega todas as conquistas
        Social.LoadAchievements((achievements) =>
        {
            // Se as conquistas foram carregadas
            if (achievements.Length > 0)
            {
                // Passos completados (Online), cada passo equivale a 100 pontos
                int stepsCompleted;

                for (int i = 0; i < achievements.Length; ++i)
                {
                    // A conquista é checada para saber qual é a conquista em questão
                    switch (achievements[i].id)
                    {
                        /* Todas essas conquistas seguem a mesma lógica demonstrada com o nível 1,
                         * porém com diferentes valores máximos 
                         */

                        // Nível I
                        case GPGSIds.achievement_level_i:

                            // Obtém os passos completdados da conquista
                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 10F);

                            // Se a conquista não está completa (10 é o número máximo de passos do nível 1)
                            if (stepsCompleted != 10)
                            {
                                // Checa se a conquista (online) está desatualizada
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    // Adiciona os pontos a conquista (online)
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_i, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                // Checa se a conquista (offline) está desatualizada
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    // Adiciona os pontos a conquista (offline)
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível II
                        case GPGSIds.achievement_level_ii:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 25F);

                            if (stepsCompleted != 25)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_ii, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível III
                        case GPGSIds.achievement_level_iii:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 50F);

                            if (stepsCompleted != 50)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_iii, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível IV
                        case GPGSIds.achievement_level_iv:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 100F);

                            if (stepsCompleted != 100)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_iv, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível V
                        case GPGSIds.achievement_level_v:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 150F);

                            if (stepsCompleted != 150)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_v, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível VI
                        case GPGSIds.achievement_level_vi:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 200F);

                            if (stepsCompleted != 200)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_vi, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível VII
                        case GPGSIds.achievement_level_vii:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 300F);

                            if (stepsCompleted != 300)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_vii, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível VIII
                        case GPGSIds.achievement_level_viii:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 400F);

                            if (stepsCompleted != 400)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_viii, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível IX
                        case GPGSIds.achievement_level_ix:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 500F);

                            if (stepsCompleted != 500)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_ix, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível X
                        case GPGSIds.achievement_level_x:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 750F);

                            if (stepsCompleted != 750)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_x, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível XI
                        case GPGSIds.achievement_level_xi:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 1000F);

                            if (stepsCompleted != 1000)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_xi, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível XII
                        case GPGSIds.achievement_level_xii:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 1250F);

                            if (stepsCompleted != 1250)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_xii, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível XIII
                        case GPGSIds.achievement_level_xiii:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 1500F);

                            if (stepsCompleted != 1500)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_xiii, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível XIV
                        case GPGSIds.achievement_level_xiv:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 1750F);

                            if (stepsCompleted != 1750)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_xiv, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível XV
                        case GPGSIds.achievement_level_xv:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 2000F);

                            if (stepsCompleted != 2000)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_xv, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível XVI
                        case GPGSIds.achievement_level_xvi:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 2500F);

                            if (stepsCompleted != 2500)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_xvi, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível XVII
                        case GPGSIds.achievement_level_xvii:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 3000F);

                            if (stepsCompleted != 3000)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_xvii, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível XVIII
                        case GPGSIds.achievement_level_xviii:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 4000F);

                            if (stepsCompleted != 4000)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_xviii, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível XIX
                        case GPGSIds.achievement_level_xix:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 5000F);

                            if (stepsCompleted != 5000)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_xix, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        // Nível XX
                        case GPGSIds.achievement_level_xx:

                            stepsCompleted = Mathf.RoundToInt(((float)achievements[i].percentCompleted / 100) * 10000F);

                            if (stepsCompleted != 10000)
                            {
                                if (stepsCompleted < GetScoreInSteps())
                                {
                                    PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_level_xx, GetScoreInSteps() - stepsCompleted, (bool success) => { });
                                }
                                else if (stepsCompleted > GetScoreInSteps())
                                {
                                    SubmitScore((stepsCompleted - GetScoreInSteps()) * 100);
                                }
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        });
    }
    #endregion

    #region Leaderboards
    public void PostMaximumScoreOnLeaderboard(int score)
    {
        // Se a pontuação atingida por maior que a pontuação máxima (offline)
        if (score > PlayerPrefs.GetInt("MaximumScore", 0))
        {
            // Define esta pontuação como a pontuação máxima atingida
            PlayerPrefs.SetInt("MaximumScore", score);
        }

        // Envia a pontuação máxima para o servidor (online)
        Social.ReportScore(PlayerPrefs.GetInt("MaximumScore", 0), GPGSIds.leaderboard_top_score_by_maze, (bool success) => { });
    }
    #endregion

    #region Score
    public void SubmitScore(int score)
    {
        // Obtém a pontuação armazenada
        int tempScore = PlayerPrefs.GetInt("PlayerScore", 0);

        // Pontuação segura
        int safeScore;

        // Operação segura para captar erros de overflow
        try
        {
            // Adiciona a pontuação nova a pontuação atual e checa se há erros de overflow
            safeScore = checked(tempScore + score);
        }
        catch (System.OverflowException)
        {
            // Se o valor máximo para um inteiro de 32 bits for alcançada a pontuação segura é redefinida para o valor máximo
            safeScore = int.MaxValue;
        }

        // Armazena a pontuação segura 
        PlayerPrefs.SetInt("PlayerScore", safeScore);
    }

    private int GetScoreInSteps()
    {
        // Obtém a pontuação em passos (cada passo é 100 pontos)
        return Mathf.FloorToInt(PlayerPrefs.GetInt("PlayerScore", 0) / 100);
    }
    #endregion

    #region Message Box
    public void MessageBoxControl(MessageBoxState state)
    {
        switch (state)
        {
            case MessageBoxState.WaitingForLogIn:

                // Define as condições iniciais do estado
                scriptManager.animating = true;
                message.color = grey;

                // Traduz a mensagem se possível
                if (scriptManager.dynamicLocalizedText.ContainsKey("LoggingIn") == true)
                {
                    message.text = scriptManager.dynamicLocalizedText["LoggingIn"];
                }
                else
                {
                    message.text = "Logging in...";
                }

                // Definições do botão
                okButton.interactable = false;
                okButton.GetComponent<PlayGamesButtons>().freeToProceed = false;

                // Se a caixa de mensagens está escondida
                if (playGamesMessageBoxTransform.anchoredPosition.y < targetPositionUp.y - 10)
                {
                    // Abafa a música
                    musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
                    if (musicManager.publicCoroutine_LPFF != null)
                    {
                        StopCoroutine(musicManager.publicCoroutine_LPFF);
                    }
                    musicManager.publicCoroutine_LPFF = StartCoroutine(musicManager.LowPassFilterFade(200F, 0.65F));

                    // Inicia a animãção de subida
                    coroutine = StartCoroutine(MessageBoxAnimation(targetPositionUp, colorUp, animationTime));
                }
                break;

            case MessageBoxState.LogInConcluded:

                // Define as condições iniciais do estado
                message.color = grey;

                // Traduz a mensagem se possível
                if (scriptManager.dynamicLocalizedText.ContainsKey("LoginComplete") == true)
                {
                    message.text = scriptManager.dynamicLocalizedText["LoginComplete"];
                }
                else
                {
                    message.text = "Login complete";
                }

                // Definições do botão
                okButton.interactable = true;
                okButton.GetComponent<PlayGamesButtons>().freeToProceed = true;
                break;

            case MessageBoxState.FailedLogIn:

                // Define as condições iniciais do estado
                message.color = red;

                // Traduz a mensagem se possível
                if (scriptManager.dynamicLocalizedText.ContainsKey("LoginFailed") == true)
                {
                    message.text = scriptManager.dynamicLocalizedText["LoginFailed"];
                }
                else
                {
                    message.text = "Login Failed";
                }

                // Definições do botão
                okButton.interactable = true;
                okButton.GetComponent<PlayGamesButtons>().freeToProceed = true;
                break;

            case MessageBoxState.OkButtonPressed:

                // Toca o som do botão
                playGamesMessageBoxTransform.GetComponent<AudioSource>().Play();

                // Retorna a música ao normal
                if (musicManager.publicCoroutine_LPFF != null)
                {
                    StopCoroutine(musicManager.publicCoroutine_LPFF);
                }
                musicManager.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 22000;
                musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = false;

                // Inicia a animação de descida
                coroutine = StartCoroutine(MessageBoxAnimation(targetPositionDown, colorDown, animationTime));
                break;

            default:
                break;
        }
    }

    private IEnumerator MessageBoxAnimation(Vector2 targetPosition, Color targetAlpha, float time)
    {
        // Condições iniciais
        scriptManager.animating = true;
        playGamesMessageBox.SetActive(true);

        // Animação
        for (float i = 0; i <= 1F; i += Time.deltaTime / time)
        {
            // Para o lerping do fundo quando o alfa é +-0.05 o valor
            if (Mathf.Abs(playGamesMessageBoxBackgroud.color.a - targetAlpha.a) < 0.05F)
            {
                playGamesMessageBoxBackgroud.color = targetAlpha;
            }

            // Para o lerping da caixa de mensagens quando a posição é +-0.05 o valor
            if (Mathf.Abs(playGamesMessageBoxTransform.anchoredPosition.y - targetPosition.y) < 0.1F)
            {
                playGamesMessageBoxTransform.anchoredPosition = targetPosition;
                i = 2;
            }

            // Lerp do fundo
            playGamesMessageBoxBackgroud.color = Color.Lerp(playGamesMessageBoxBackgroud.color, targetAlpha, i);

            // Lerp da posição
            playGamesMessageBoxTransform.anchoredPosition = Vector2.Lerp(playGamesMessageBoxTransform.anchoredPosition, targetPosition, i);

            yield return null;
        }

        // Quando a caixa de mensagens está completamente abaixada ela é desabilitada
        if (playGamesMessageBoxTransform.anchoredPosition.y < targetPositionUp.y - 10F)
        {
            playGamesMessageBox.SetActive(false);
        }

        // Condições finais
        scriptManager.animating = false;
        StopCoroutine(coroutine);
    }
    #endregion
}