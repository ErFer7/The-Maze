using UnityEngine;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    #region Public Variables
    // Textos no menu
    public Text menuExitMessage;
    public Text yes;
    public Text no;
    public Text ok;
    public Text gameMode;
    public Text classic;
    public Text time;
    public Text dark;
    public Text custom;
    public Text continueMessage;
    public Text newGame;
    public Text continue_;
    public Text informations;
    public Text creator;
    public Text settings;
    public Text language;
    public Text sound;
    public Text music;
    public Text languageTitle;
    public Text customTitle;
    public Text width;
    public Text height;
    public Text seed;
    public Text seedTitle;
    public Text seedInfo;
    public Text done;

    // Tamanhos dos botões no menu
    public RectTransform classicButton;
    public RectTransform timeButton;
    public RectTransform darkButton;
    public RectTransform customButton;
    public RectTransform languageButton;
    public RectTransform widthLabel;
    public RectTransform heightLabel;
    public RectTransform seedLabel;
    public RectTransform doneButton;
    #endregion

    #region Private variables
    // Linguagem do sistema
    private SystemLanguage systemLanguage;

    // Tamanho dos botões
    private Vector2 buttonSize;
    private Vector2 doneButtonSize;
    private Vector2 labelSize;

    // Acesso ao Script manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Acessa a linguagem do sistema
        systemLanguage = Application.systemLanguage;

        // Caso o jogador não tenha uma linguagem de preferência
        if (PlayerPrefs.GetString("Culture", "null") == "null")
        {
            switch (systemLanguage)
            {
                case SystemLanguage.Spanish:
                    SetLanguage("es-419");
                    break;
                case SystemLanguage.English:
                    SetLanguage("en-US");
                    break;
                case SystemLanguage.Portuguese:
                    SetLanguage("pt-BR");
                    break;

                // Usa o inglês caso não seja encontrada a linguagem padrão do sistema
                default:
                    SetLanguage("en-US");
                    break;
            }
        }
        else
        {
            // Define a linguagem como a linguagem padrão do sistema
            SetLanguage(PlayerPrefs.GetString("Culture"));
        }
    }
    #endregion

    #region Methods
    public void SetLanguage(string cultureCode)
    {
        // Define a linguagem com base no código cultural
        switch (cultureCode)
        {
            // Inglês - Estados Unidos
            case "en-US":

                // Textos do menu
                menuExitMessage.text = "Want to quit the game?";
                yes.text = "YES";
                no.text = "NO";
                ok.text = "OK";
                gameMode.text = "GAME MODE";
                classic.text = "CLASSIC";
                time.text = "TIME";
                dark.text = "DARK";
                custom.text = "CUSTOM";
                continueMessage.text = "Want to continue the last game?";
                newGame.text = "NEW GAME";
                continue_.text = "CONTINUE";
                informations.text = "INFORMATIONS";
                creator.text = "Created by Eric F. Evaristo";
                settings.text = "SETTINGS";
                language.text = "LANGUAGE";
                sound.text = "SOUND";
                music.text = "MUSIC";
                languageTitle.text = "LANGUAGE";
                customTitle.text = "CUSTOM";
                width.text = "WIDTH";
                height.text = "HEIGHT";
                seed.text = "SEED";
                seedTitle.text = "SEED";
                seedInfo.text = "The seed is a number used to generate the maze. You can change this number or leave it blank to generate a random seed.";
                done.text = "DONE";

                // Tamanhos de fonte do menu
                continue_.fontSize = 50;
                newGame.fontSize = 50;
                ok.fontSize = 50;

                // Tamanhos de botão do menu
                buttonSize = new Vector2(400, 100);
                labelSize = new Vector2(175, 65);
                doneButtonSize = new Vector2(175, 75);
                classicButton.sizeDelta = buttonSize;
                timeButton.sizeDelta = buttonSize;
                darkButton.sizeDelta = buttonSize;
                customButton.sizeDelta = buttonSize;
                widthLabel.sizeDelta = labelSize;
                heightLabel.sizeDelta = labelSize;
                seedLabel.sizeDelta = labelSize;
                doneButton.sizeDelta = doneButtonSize;

                // Textos do gameplay
                scriptManager.dynamicLocalizedText.Clear();
                scriptManager.dynamicLocalizedText.Add("LoggingIn", "Logging in...");
                scriptManager.dynamicLocalizedText.Add("LoginComplete", "Login complete");
                scriptManager.dynamicLocalizedText.Add("LoginFailed", "Login failed");
                scriptManager.dynamicLocalizedText.Add("GeneratingMaze", "GENERATING MAZE");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_Level", "LEVEL");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_Seed", "SEED");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "DISTANCE TRAVELLED");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_Size", "SIZE");
                scriptManager.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "LEVEL {0} COMPLETED");
                scriptManager.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "LEVEL COMPLETED");
                scriptManager.dynamicLocalizedText.Add("exitScreen_title_Failure", "YOU FAILED");
                scriptManager.dynamicLocalizedText.Add("exitScreen_subTitle", "Maze {0}");
                scriptManager.dynamicLocalizedText.Add("exitScreen_Score", "Score");

                // Tamanhos de fonte do gameplay
                scriptManager.scoreFontSize = 60;
                break;
            // Espanhol - América Latina
            case "es-419":

                // Textos do menu
                menuExitMessage.text = "¿Quieres salir del juego?";
                yes.text = "SÍ";
                no.text = "NO";
                ok.text = "OK";
                gameMode.text = "MODO DE JUEGO";
                classic.text = "CLÁSICO";
                time.text = "TIEMPO";
                dark.text = "OSCURO";
                custom.text = "PERSONALIZADO";
                continueMessage.text = "¿Quieres continuar el último juego?";
                newGame.text = "NUEVO JUEGO";
                continue_.text = "CONTINUAR";
                informations.text = "INFORMACIONES";
                creator.text = "Creado por Eric F. Evaristo";
                settings.text = "AJUSTES";
                language.text = "IDIOMA";
                sound.text = "SONAR";
                music.text = "MÚSICA";
                languageTitle.text = "IDIOMA";
                customTitle.text = "PERSONALIZADO";
                width.text = "ANCHURA";
                height.text = "ALTURA";
                seed.text = "SEMILLA";
                seedTitle.text = "SEMILLA";
                seedInfo.text = "La semilla es un número usado para generar el laberinto. Puede cambiar este número o dejarlo en blanco para generar una semilla aleatoria.";
                done.text = "COMPLETADO";

                // Tamanhos de fonte do menu
                continue_.fontSize = 40;
                newGame.fontSize = 40;
                ok.fontSize = 50;

                // Tamanhos de botão do menu
                buttonSize = new Vector2(500, 100);
                labelSize = new Vector2(215, 65);
                doneButtonSize = new Vector2(340, 75);
                classicButton.sizeDelta = buttonSize;
                timeButton.sizeDelta = buttonSize;
                darkButton.sizeDelta = buttonSize;
                customButton.sizeDelta = buttonSize;
                widthLabel.sizeDelta = labelSize;
                heightLabel.sizeDelta = labelSize;
                seedLabel.sizeDelta = labelSize;
                doneButton.sizeDelta = doneButtonSize;

                // Textos do gameplay
                scriptManager.dynamicLocalizedText.Clear();
                scriptManager.dynamicLocalizedText.Add("LoggingIn", "Iniciando sesión...");
                scriptManager.dynamicLocalizedText.Add("LoginComplete", "Sesión iniciada");
                scriptManager.dynamicLocalizedText.Add("LoginFailed", "Error de inicio de sesión");
                scriptManager.dynamicLocalizedText.Add("GeneratingMaze", "GENERANDO LABERINTO");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_Level", "NIVEL");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_Seed", "SEMILLA");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "DISTANCIA RECORRIDA");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_Size", "TAMAÑO");
                scriptManager.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "NIVEL {0} COMPLETADO");
                scriptManager.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "NIVEL COMPLETADO");
                scriptManager.dynamicLocalizedText.Add("exitScreen_title_Failure", "FALLASTE");
                scriptManager.dynamicLocalizedText.Add("exitScreen_subTitle", "Laberinto {0}");
                scriptManager.dynamicLocalizedText.Add("exitScreen_Score", "Puntuación");

                // Tamanhos de fonte do gameplay
                scriptManager.scoreFontSize = 45;
                break;
            // Português - Brasil
            case "pt-BR":

                // Textos do menu
                menuExitMessage.text = "Quer sair do jogo?";
                yes.text = "SIM";
                no.text = "NÃO";
                ok.text = "OK";
                gameMode.text = "MODO DE JOGO";
                classic.text = "CLÁSSICO";
                time.text = "TEMPO";
                dark.text = "ESCURO";
                custom.text = "PERSONALIZADO";
                continueMessage.text = "Quer continuar o último jogo?";
                newGame.text = "NOVO JOGO";
                continue_.text = "CONTINUAR";
                informations.text = "INFORMAÇÕES";
                creator.text = "Criado por Eric F. Evaristo";
                settings.text = "CONFIGURAÇÕES";
                language.text = "IDIOMA";
                sound.text = "SOM";
                music.text = "MÚSICA";
                languageTitle.text = "IDIOMA";
                customTitle.text = "PERSONALIZADO";
                width.text = "LARGURA";
                height.text = "ALTURA";
                seed.text = "SEMENTE";
                seedTitle.text = "SEMENTE";
                seedInfo.text = "A semente é um número usado para gerar o labirinto. Você pode mudar esse número ou deixá-lo em branco para gerar uma semente aleatória.";
                done.text = "CONCLUÍDO";

                // Tamanhos de fonte do menu
                continue_.fontSize = 50;
                newGame.fontSize = 50;
                ok.fontSize = 50;

                // Tamanhos de botão do menu
                buttonSize = new Vector2(500, 100);
                labelSize = new Vector2(215, 65);
                doneButtonSize = new Vector2(300, 75);
                classicButton.sizeDelta = buttonSize;
                timeButton.sizeDelta = buttonSize;
                darkButton.sizeDelta = buttonSize;
                customButton.sizeDelta = buttonSize;
                widthLabel.sizeDelta = labelSize;
                heightLabel.sizeDelta = labelSize;
                seedLabel.sizeDelta = labelSize;
                doneButton.sizeDelta = doneButtonSize;

                // Textos do gameplay
                scriptManager.dynamicLocalizedText.Clear();
                scriptManager.dynamicLocalizedText.Add("LoggingIn", "Fazendo login...");
                scriptManager.dynamicLocalizedText.Add("LoginComplete", "Login concluído");
                scriptManager.dynamicLocalizedText.Add("LoginFailed", "Falha no login");
                scriptManager.dynamicLocalizedText.Add("GeneratingMaze", "GERANDO LABIRINTO");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_Level", "NÍVEL");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_Seed", "SEMENTE");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "DISTÂNCIA PERCORRIDA");
                scriptManager.dynamicLocalizedText.Add("pausingScreen_Size", "TAMANHO");
                scriptManager.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "NÍVEL {0} CONCLUÍDO");
                scriptManager.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "NÍVEL CONCLUÍDO");
                scriptManager.dynamicLocalizedText.Add("exitScreen_title_Failure", "VOCÊ FALHOU");
                scriptManager.dynamicLocalizedText.Add("exitScreen_subTitle", "Labirinto {0}");
                scriptManager.dynamicLocalizedText.Add("exitScreen_Score", "Pontuação");

                // Tamanhos de fonte do gameplay
                scriptManager.scoreFontSize = 45;
                break;
            default:
                break;
        }
    }
    #endregion
}