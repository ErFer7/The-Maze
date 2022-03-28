using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingControl : MonoBehaviour
{
    /* Estados de carregamento
     *
     * Os estados são compostos por estados de transição e estados de progresso.
     * Um estado de transição é o estado que fica ativo durante uma operação, são os números negativos com excessão do 7;
     * Um estado de progresso é o estado que é ativo brevemente para definir que uma operação acabou e a próxima deve começar,
     * são os números positivos;
     * 
     * Estados (Algumas vezes os números são usados em mais de um estado):
     * 
     * 0 (Progresso): Inicia a animação de fade in da tela de carregamento e define o estado -1;
     * -1 (Transição): Ativo durande o fade in da tela de carregamento, ao final é definido o estado 1;
     * 1 (Progresso): Inicia a criação dos muros e define o estado -1;
     * -1 (Transição): Ativo durante a criação dos muros, ao final é definido o estado 2;
     * 2 (Progresso): Inicia a geração do labirinto e define o estado -1;
     * -1 (Transição): Ativo durante a geração do labirinto, ao final é definido o estado 3;
     * 3 (Progresso): Inicia a geração do spawn e define o estado -1;
     * -1 (Transição): Ativo durante a geração do spawn, ao final é definido o estado 4;
     * 4 (Progresso): Destrói o gerador de labirintos, inicia a animação de fade out da tela de carregamento e define o estado como -2;
     * -2 (Transição): Ativo durante o fade out da tela de carregamento, ao final é definido o estado 5;
     * 5 (Progresso): Destrói a tela de carregamento, inicia a animação de fade in do jogo, toca o som de início e define o estado como -3;
     * -3 (Transição): Ativo durante o fade in do jogo, ao final é definido o estado 6;
     * 6 (Progresso): Inicia o timer, destrava o jogador, marca o tempo inicial (AdManager) e define o estado como 7;
     * 7 (Progresso / Transição): Aguarda o som de início parar de tocar e então destrói este objeto;
     */

    #region Public Variables
    #region Editor Acessible
    // Acessoa a tela preta
    public GameObject blackScreen;

    // Acesso ao gerador de labirintos
    public MazeGenerator mazeGenerator;

    // Acesso a tela de carregamento
    public GameObject loadingScreen;

    // Acesso ao timer
    public Timer timer;

    // Acesso ao texto
    public Text generatingMaze;
    #endregion

    // Coroutine das operações
    [System.NonSerialized]
    public Coroutine coroutine;
    #endregion

    #region Private Variables
    // Coroutine do controle de carregamento
    private Coroutine controlCoroutine;

    // Acesso ao AudioSource
    private AudioSource audioSource;

    // Acesso ao Script manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Estado inicial
        scriptManager.loadingStage = 0;

        // Inicia o carregamento
        controlCoroutine = StartCoroutine(Control());

        // Acessa o AudioSource
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region Loading Control
    IEnumerator Control()
    {
        // Condição inicial
        scriptManager.animating = true;

        while (true)
        {
            switch (scriptManager.loadingStage)
            {
                case 0:
                    scriptManager.loadingStage = -1;

                    // Traduz o texto da tela de carregamento se possível
                    if (scriptManager.dynamicLocalizedText.ContainsKey("GeneratingMaze"))
                    {
                        generatingMaze.text = scriptManager.dynamicLocalizedText["GeneratingMaze"];
                    }

                    // Inicia o fade in da tela de carregamento
                    coroutine = StartCoroutine(blackScreen.GetComponent<FadeImage>().FadeImageTo(0, 1));
                    break;
                case 1:
                    scriptManager.loadingStage = -1;

                    // Para a coroutine do fade in da tela de carregamento
                    StopCoroutine(coroutine);

                    // Desativa a tela preta
                    blackScreen.SetActive(false);

                    // Inicia a criação dos muros
                    coroutine = StartCoroutine(mazeGenerator.CreateWalls());
                    break;
                case 2:
                    scriptManager.loadingStage = -1;

                    // Para a coroutine da criação de muros
                    StopCoroutine(coroutine);

                    // Inicia a geração do labirinto
                    coroutine = StartCoroutine(mazeGenerator.GeneratePath());
                    break;
                case 3:
                    scriptManager.loadingStage = -1;

                    // Para a coroutine da geração do labirinto
                    StopCoroutine(coroutine);

                    // Inicia a geração do spawn
                    coroutine = StartCoroutine(mazeGenerator.SetSpawn());
                    break;
                case 4:
                    scriptManager.loadingStage = -2;

                    // Para a coroutine de geração do spawn
                    StopCoroutine(coroutine);

                    // Destrói o gerador de labirintos
                    Destroy(mazeGenerator.gameObject);

                    // Ativa a tela preta
                    blackScreen.SetActive(true);

                    // Inicia o fade out da tela de carregamento
                    coroutine = StartCoroutine(blackScreen.GetComponent<FadeImage>().FadeImageTo(1, 1));
                    break;
                case 5:
                    scriptManager.loadingStage = -3;

                    // Para a coroutine do fade out da tela de carregamento
                    StopCoroutine(coroutine);

                    // Destrói a tela de carregamento
                    Destroy(loadingScreen);

                    // Inicia o fade in do jogo
                    coroutine = StartCoroutine(blackScreen.GetComponent<FadeImage>().FadeImageTo(0, 0.5F));

                    // Se o som está habilitado
                    if (scriptManager.sound)
                    {
                        // Toca o som de início do jogo
                        audioSource.PlayOneShot(audioSource.clip);
                    }
                    break;
                case 6:
                    scriptManager.loadingStage = 7;

                    // Para a coroutine do fade in do jogo
                    StopCoroutine(coroutine);

                    // Desabilita a tela preta
                    blackScreen.SetActive(false);

                    // Inicia o timer
                    coroutine = StartCoroutine(timer.StartTimer());

                    // Destrava o jogador
                    scriptManager.playerCanMove = true;

                    // Condição final
                    scriptManager.animating = false;
                    break;
                case 7:

                    // Quando o som de início parar de tocar
                    if (!audioSource.isPlaying)
                    {
                        // Para o controle de carregamento
                        StopCoroutine(controlCoroutine);

                        // Destrói este objeto
                        Destroy(gameObject);
                    }

                    break;
                default:
                    break;
            }

            yield return null;
        }
    }
    #endregion
}
