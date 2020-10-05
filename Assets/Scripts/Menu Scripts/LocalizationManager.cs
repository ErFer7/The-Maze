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
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa a linguagem do sistema
        systemLanguage = Application.systemLanguage;

        // Caso o jogador não tenha uma linguagem de preferência
        if (PlayerPrefs.GetString("Culture", "null") == "null")
        {
            switch (systemLanguage)
            {
                case SystemLanguage.Afrikaans:
                    SetLanguage("af");
                    break;
                case SystemLanguage.Spanish:
                    SetLanguage("es-419");
                    break;
                case SystemLanguage.English:
                    SetLanguage("en-US");
                    break;
                case SystemLanguage.Portuguese:
                    SetLanguage("pt-BR");
                    break;
                case SystemLanguage.German:
                    SetLanguage("pt-BR");
                    break;
                case SystemLanguage.ChineseSimplified:
                    SetLanguage("zn-Hans");
                    break;
                case SystemLanguage.ChineseTraditional:
                    SetLanguage("zn-Hant");
                    break;
                case SystemLanguage.Russian:
                    SetLanguage("ru-RU");
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

    public void SetLanguage(string cultureCode)
    {
        // Define a linguagem com base no código cultural
        switch (cultureCode)
        {
            // Afrikâner
            case "af":

                // Textos do menu
                menuExitMessage.text = "Wil jy die spel afsluit?";
                yes.text = "JA";
                no.text = "GEEN";
                ok.text = "OK";
                gameMode.text = "SPEL AF";
                classic.text = "KLASSIEKE";
                time.text = "TYD";
                dark.text = "DONKER";
                custom.text = "PERSOONLIKE";
                continueMessage.text = "Wil jy die laaste wedstryd voortgaan?";
                newGame.text = "NUWE SPEL";
                continue_.text = "AANHOU";
                informations.text = "INLIGTING";
                creator.text = "Geskep deur Eric F. Evaristo";
                settings.text = "INSTELLINGS";
                language.text = "TAAL";
                sound.text = "KLINK";
                music.text = "MUSIEK";
                languageTitle.text = "TAAL";
                customTitle.text = "PERSOONLIKE";
                width.text = "WYDTE";
                height.text = "HOOGTE";
                seed.text = "SAAD";
                seedTitle.text = "SAAD";
                seedInfo.text = "Die saad is 'n nommer wat gebruik word om die doolhof te genereer. U kan hierdie nommer verander of dit leeg laat om 'n ewekansige saad te genereer.";
                done.text = "VOLTOOI";

                // Tamanhos de fonte do menu
                continue_.fontSize = 50;
                newGame.fontSize = 50;
                ok.fontSize = 50;

                // Tamanhos de botão do menu
                buttonSize = new Vector2(400, 100);
                labelSize = new Vector2(190, 65);
                doneButtonSize = new Vector2(240, 75);
                classicButton.sizeDelta = buttonSize;
                timeButton.sizeDelta = buttonSize;
                darkButton.sizeDelta = buttonSize;
                customButton.sizeDelta = buttonSize;
                widthLabel.sizeDelta = labelSize;
                heightLabel.sizeDelta = labelSize;
                seedLabel.sizeDelta = labelSize;
                doneButton.sizeDelta = doneButtonSize;

                // Textos do gameplay
                DataHolder.dynamicLocalizedText.Clear();
                DataHolder.dynamicLocalizedText.Add("LoggingIn", "Teken aan...");
                DataHolder.dynamicLocalizedText.Add("LoginComplete", "Aanmelding voltooi");
                DataHolder.dynamicLocalizedText.Add("LoginFailed", "Aanmelding misluk");
                DataHolder.dynamicLocalizedText.Add("GeneratingMaze", "GENEREER DOOLHOF");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Level", "VLAK");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Seed", "SAAD");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "AFSTAND GEREIS");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Size", "GROOTTE");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "VLAK {0} VOLTOOI");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "VLAK VOLTOOI");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Failure", "JY HET GEFAAL");
                DataHolder.dynamicLocalizedText.Add("exitScreen_subTitle", "Doolhof {0}");
                DataHolder.dynamicLocalizedText.Add("exitScreen_Score", "Telling");

                // Tamanhos de fonte do gameplay
                DataHolder.scoreFontSize = 60;
                break;
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
                DataHolder.dynamicLocalizedText.Clear();
                DataHolder.dynamicLocalizedText.Add("LoggingIn", "Logging in...");
                DataHolder.dynamicLocalizedText.Add("LoginComplete", "Login complete");
                DataHolder.dynamicLocalizedText.Add("LoginFailed", "Login failed");
                DataHolder.dynamicLocalizedText.Add("GeneratingMaze", "GENERATING MAZE");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Level", "LEVEL");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Seed", "SEED");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "DISTANCE TRAVELLED");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Size", "SIZE");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "LEVEL {0} COMPLETED");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "LEVEL COMPLETED");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Failure", "YOU FAILED");
                DataHolder.dynamicLocalizedText.Add("exitScreen_subTitle", "Maze {0}");
                DataHolder.dynamicLocalizedText.Add("exitScreen_Score", "Score");

                // Tamanhos de fonte do gameplay
                DataHolder.scoreFontSize = 60;
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
                DataHolder.dynamicLocalizedText.Clear();
                DataHolder.dynamicLocalizedText.Add("LoggingIn", "Iniciando sesión...");
                DataHolder.dynamicLocalizedText.Add("LoginComplete", "Sesión iniciada");
                DataHolder.dynamicLocalizedText.Add("LoginFailed", "Error de inicio de sesión");
                DataHolder.dynamicLocalizedText.Add("GeneratingMaze", "GENERANDO LABERINTO");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Level", "NIVEL");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Seed", "SEMILLA");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "DISTANCIA RECORRIDA");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Size", "TAMAÑO");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "NIVEL {0} COMPLETADO");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "NIVEL COMPLETADO");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Failure", "FALLASTE");
                DataHolder.dynamicLocalizedText.Add("exitScreen_subTitle", "Laberinto {0}");
                DataHolder.dynamicLocalizedText.Add("exitScreen_Score", "Puntuación");

                // Tamanhos de fonte do gameplay
                DataHolder.scoreFontSize = 45;
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
                DataHolder.dynamicLocalizedText.Clear();
                DataHolder.dynamicLocalizedText.Add("LoggingIn", "Fazendo login...");
                DataHolder.dynamicLocalizedText.Add("LoginComplete", "Login concluído");
                DataHolder.dynamicLocalizedText.Add("LoginFailed", "Falha no login");
                DataHolder.dynamicLocalizedText.Add("GeneratingMaze", "GERANDO LABIRINTO");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Level", "NÍVEL");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Seed", "SEMENTE");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "DISTÂNCIA PERCORRIDA");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Size", "TAMANHO");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "NÍVEL {0} CONCLUÍDO");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "NÍVEL CONCLUÍDO");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Failure", "VOCÊ FALHOU");
                DataHolder.dynamicLocalizedText.Add("exitScreen_subTitle", "Labirinto {0}");
                DataHolder.dynamicLocalizedText.Add("exitScreen_Score", "Pontuação");

                // Tamanhos de fonte do gameplay
                DataHolder.scoreFontSize = 45;
                break;
            // Alemão - Alemanha
            case "de-DE":

                // Textos do menu
                menuExitMessage.text = "Willst du das Spiel beenden?";
                yes.text = "JA";
                no.text = "NEIN";
                ok.text = "OK";
                gameMode.text = "SPIELMODUS";
                classic.text = "KLASSISCH";
                time.text = "ZEIT";
                dark.text = "DUNKEL";
                custom.text = "ANGEPASST";
                continueMessage.text = "Willst du das letzte Spiel fortsetzen?";
                newGame.text = "NEUES SPIEL";
                continue_.text = "FORTSETZEN";
                informations.text = "INFORMATIONEN";
                creator.text = "Erstellt von Eric F. Evaristo";
                settings.text = "EINSTELLUNGEN";
                language.text = "SPRACHE";
                sound.text = "KLINGEN";
                music.text = "MUSIK";
                languageTitle.text = "SPRACHE";
                customTitle.text = "ANGEPASST";
                width.text = "BREITE";
                height.text = "HÖHE";
                seed.text = "SAME";
                seedTitle.text = "SAME";
                seedInfo.text = "Der Same ist eine Zahl, mit der das Labyrinth erzeugt wird. Sie können diese Zahl ändern oder leer lassen, um einen zufälligen Startwert zu generieren.";
                done.text = "GETAN";

                // Tamanhos de fonte do menu
                continue_.fontSize = 40;
                newGame.fontSize = 40;
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
                DataHolder.dynamicLocalizedText.Clear();
                DataHolder.dynamicLocalizedText.Add("LoggingIn", "Einloggen...");
                DataHolder.dynamicLocalizedText.Add("LoginComplete", "Login abgeschlossen");
                DataHolder.dynamicLocalizedText.Add("LoginFailed", "Login fehlgeschlagen");
                DataHolder.dynamicLocalizedText.Add("GeneratingMaze", "LABYRINTH ERZEUGEN");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Level", "NIVEAU");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Seed", "SAME");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "ZURÜCKGELEGTE STRECKE");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Size", "GRÖẞE");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "NIVEAU {0} ABGESCHLOSSEN");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "NIVEAU ABGESCHLOSSEN");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Failure", "DU HAST VERSAGT");
                DataHolder.dynamicLocalizedText.Add("exitScreen_subTitle", "Labyrinth {0}");
                DataHolder.dynamicLocalizedText.Add("exitScreen_Score", "Ergebnis");

                // Tamanhos de fonte do gameplay
                DataHolder.scoreFontSize = 45;
                break;
            // Chinês (Simplificado) - China
            case "zn-Hans":

                // Textos do menu
                menuExitMessage.text = "要退出游戏吗？";
                yes.text = "是";
                no.text = "没有";
                ok.text = "好";
                gameMode.text = "游戏模式";
                classic.text = "经典";
                time.text = "时间";
                dark.text = "暗";
                custom.text = "定制的";
                continueMessage.text = "想继续上一场比赛吗？";
                newGame.text = "新游戏";
                continue_.text = "继续";
                informations.text = "资讯资讯";
                creator.text = "由Eric F. Evaristo创建";
                settings.text = "设定值";
                language.text = "语言";
                sound.text = "声音";
                music.text = "音乐";
                languageTitle.text = "语言";
                customTitle.text = "定制的";
                width.text = "宽度";
                height.text = "高度";
                seed.text = "种子";
                seedTitle.text = "种子";
                seedInfo.text = "种子是用于生成迷宫的数字。 您可以更改此数字或将其保留为空白以生成随机种子。";
                done.text = "总结";

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
                DataHolder.dynamicLocalizedText.Clear();
                DataHolder.dynamicLocalizedText.Add("LoggingIn", "在登录...");
                DataHolder.dynamicLocalizedText.Add("LoginComplete", "登录完成");
                DataHolder.dynamicLocalizedText.Add("LoginFailed", "登录失败");
                DataHolder.dynamicLocalizedText.Add("GeneratingMaze", "产生迷宫");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Level", "水平");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Seed", "种子");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "行驶距离");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Size", "尺寸");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "{0}级完成");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "等级完成");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Failure", "你失败了");
                DataHolder.dynamicLocalizedText.Add("exitScreen_subTitle", "迷宫{0}");
                DataHolder.dynamicLocalizedText.Add("exitScreen_Score", "得分");

                // Tamanhos de fonte do gameplay
                DataHolder.scoreFontSize = 60;
                break;
            // Chinês (Tradicional) - China
            case "zn-Hant":

                // Textos do menu
                menuExitMessage.text = "想退出遊戲嗎？";
                yes.text = "是";
                no.text = "沒有";
                ok.text = "好";
                gameMode.text = "遊戲模式";
                classic.text = "經典";
                time.text = "時間";
                dark.text = "暗";
                custom.text = "客制化";
                continueMessage.text = "想繼續上一場比賽嗎？";
                newGame.text = "新遊戲";
                continue_.text = "繼續";
                informations.text = "資訊資訊";
                creator.text = "由Eric F. Evaristo創建";
                settings.text = "設定值";
                language.text = "語言";
                sound.text = "聲音";
                music.text = "音樂";
                languageTitle.text = "語言";
                customTitle.text = "客制化";
                width.text = "寬度";
                height.text = "高度";
                seed.text = "種子";
                seedTitle.text = "種子";
                seedInfo.text = "種子是用於生成迷宮的數字。 您可以更改此數字或將其保留為空白以生成隨機種子。";
                done.text = "總結";

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
                DataHolder.dynamicLocalizedText.Clear();
                DataHolder.dynamicLocalizedText.Add("LoggingIn", "在登錄...");
                DataHolder.dynamicLocalizedText.Add("LoginComplete", "登錄完成");
                DataHolder.dynamicLocalizedText.Add("LoginFailed", "登錄失敗");
                DataHolder.dynamicLocalizedText.Add("GeneratingMaze", "產生迷宮");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Level", "水平");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Seed", "種子");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "行駛距離");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Size", "尺寸");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "{0}級完成");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "等級完成");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Failure", "你失敗了");
                DataHolder.dynamicLocalizedText.Add("exitScreen_subTitle", "迷宮{0}");
                DataHolder.dynamicLocalizedText.Add("exitScreen_Score", "得分");

                // Tamanhos de fonte do gameplay
                DataHolder.scoreFontSize = 60;
                break;
            // Russo - Rússia
            case "ru-RU":

                // Textos do menu
                menuExitMessage.text = "Хотите выйти из игры?";
                yes.text = "да";
                no.text = "нет";
                ok.text = "хорошо";
                gameMode.text = "Игровой режим";
                classic.text = "классический";
                time.text = "Время";
                dark.text = "Тьма";
                custom.text = "Индивидуальные";
                continueMessage.text = "Хотите продолжить последнюю игру?";
                newGame.text = "Новая игра";
                continue_.text = "Продолжать";
                informations.text = "Информация";
                creator.text = "Создано Эрик Ф. Эваристо";
                settings.text = "настройки";
                language.text = "язык";
                sound.text = "Звук";
                music.text = "Музыка";
                languageTitle.text = "язык";
                customTitle.text = "Индивидуальные";
                width.text = "ширина";
                height.text = "Рост";
                seed.text = "семена";
                seedTitle.text = "семена";
                seedInfo.text = "Семя - это число, используемое для создания лабиринта. Вы можете изменить это число или оставить его пустым, чтобы создать случайное начальное число.";
                done.text = "завершено";

                // Tamanhos de fonte do menu
                continue_.fontSize = 45;
                newGame.fontSize = 45;
                ok.fontSize = 35;

                // Tamanhos de botão do menu
                buttonSize = new Vector2(465, 100);
                labelSize = new Vector2(200, 65);
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
                DataHolder.dynamicLocalizedText.Clear();
                DataHolder.dynamicLocalizedText.Add("LoggingIn", "Вход в систему...");
                DataHolder.dynamicLocalizedText.Add("LoginComplete", "Авторизация завершена");
                DataHolder.dynamicLocalizedText.Add("LoginFailed", "Ошибка входа");
                DataHolder.dynamicLocalizedText.Add("GeneratingMaze", "Создание лабиринта");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Level", "уровень");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Seed", "семена");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_DistanceTravelled", "Пройденное расстояние");
                DataHolder.dynamicLocalizedText.Add("pausingScreen_Size", "Размер");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Progressive", "Уровень {0} завершен");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Success_Non-Progressive", "Уровень пройден");
                DataHolder.dynamicLocalizedText.Add("exitScreen_title_Failure", "Вы потерпели неудачу");
                DataHolder.dynamicLocalizedText.Add("exitScreen_subTitle", "Лабиринт {0}");
                DataHolder.dynamicLocalizedText.Add("exitScreen_Score", "Гол");

                // Tamanhos de fonte do gameplay
                DataHolder.scoreFontSize = 60;
                break;
            default:
                break;
        }
    }
}