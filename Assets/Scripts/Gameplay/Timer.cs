using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    #region Public Variables
    #region Editor Acessible
    // Acesso ao Loading Control
    public GameObject loadingControl;

    // Acesso a tela de pausa
    public GameObject pausingScreen;

    // Acesso a saída
    public Exit exit;

    // Acesso a tela branca
    public GameObject whiteScreen;

    // Acesso ao botão de pausa
    public Button pauseButton;

    // Acesso ao jogador
    public GameObject player;

    // Acesso a tela de saída
    public GameObject exitScreen;
    #endregion

    // Definições de tempo
    [HideInInspector]
    public int seconds;
    [HideInInspector]
    public int minutes;
    [HideInInspector]
    public int hours;

    // Tempo do timer
    [HideInInspector]
    public float currentTime;
    #endregion

    #region Private Variables
    // Contador
    private Text counter;

    // Valor dos segundos antigos
    private int oldSeconds;

    // Coroutines
    private Coroutine coroutine_TC;
    private Coroutine coroutine_DAC;
    private Coroutine coroutine_F;

    // Estado de inicialização da coroutine da animação da morte
    private bool coroutine_DAC_Started;

    // Acesso ao Script Manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Acessa o texto de display
        counter = gameObject.GetComponent<Text>();

        // Inicializa os segundos antigos
        oldSeconds = -1;

        // Inicializa o modo de tempo (regressivo)
        if (scriptManager.regressiveTime)
        {
            // Define a cor da base do display
            gameObject.GetComponentInParent<Image>().color = new Color(0.4F, 0F, 0F);

            // Define o tempo inicial restante
            currentTime = Mathf.FloorToInt((scriptManager.width * scriptManager.height * 2) / 7) + 1;

            // Converte o tempo para o formato HH:MM:SS
            TimeInHMS();

            // Exibe o tempo
            TimeDisplay();
        }

        // Inicialização para o modo escuro
        if (scriptManager.dark)
        {
            counter.color = new Color(0F, 0F, 0F);
            gameObject.GetComponentInParent<Image>().color = new Color(0.898F, 0.898F, 0.898F);
        }
    }
    #endregion

    #region Timer
    public void StartTimer()
    {
        // Inicializa o timer
        coroutine_TC = StartCoroutine(TimeCounter());
    }

    private IEnumerator TimeCounter()
    {
        // Acessa o display do tempo
        counter = gameObject.GetComponent<Text>();

        // Define os segundos antigos
        oldSeconds = -1;

        // Estado inicial da coroutine
        coroutine_DAC_Started = false;

        // Tempo inicial para o modo tempo (regressivo)
        if (scriptManager.regressiveTime)
        {
            currentTime = Mathf.FloorToInt((scriptManager.width * scriptManager.height * 2) / 7) + 1;
        }

        while (true)
        {
            // Se o jogo está ocorrendo normalmente
            if (!pausingScreen.activeSelf && !exit.exiting)
            {
                // Conta o tempo
                if (!scriptManager.regressiveTime)
                {
                    currentTime += Time.deltaTime;
                }
                else
                {
                    // Conta o tempo regressivamente até 0
                    if (currentTime > 0)
                    {
                        currentTime -= Time.deltaTime;
                    }
                    // Game over
                    else if (!coroutine_DAC_Started)
                    {
                        coroutine_DAC = StartCoroutine(DeathAnimationControl());
                    }
                }
            }

            // Define o tempo em horas, minutos e segundos
            TimeInHMS();

            // Exibe o tempo em HH:MM:SS
            if (seconds != oldSeconds)
            {
                TimeDisplay();
                oldSeconds = seconds;
            }

            yield return null;
        }
    }

    private void TimeInHMS()
    {
        // Coverte o tempo de segundos (float) para HH:MM:SS

        seconds = Mathf.RoundToInt(currentTime % 60);
        minutes = Mathf.FloorToInt((currentTime / 60) % 60);
        hours = Mathf.FloorToInt(currentTime / 3600);
    }

    private void TimeDisplay()
    {
        // Corrige o tempo
        if (seconds == 60)
        {
            seconds = 0;

            if (minutes > 59)
            {
                minutes = 0;
            }
            else
            {
                ++minutes;
            }
        }

        if (hours < 1)
        {
            if (minutes < 1)
            {
                // Exibe: SS
                counter.text = "" + seconds;
            }
            else
            {
                if (seconds < 10)
                {
                    // Exibe: MM:0S
                    counter.text = "" + minutes + ":" + "0" + seconds;
                }
                else
                {
                    // Exibe: MM:SS
                    counter.text = "" + minutes + ":" + seconds;
                }
            }
        }
        else
        {
            if (minutes < 10)
            {
                if (seconds < 10)
                {
                    // Exibe: HH:0M:0S
                    counter.text = "" + hours + ":" + "0" + minutes + ":" + "0" + seconds;
                }
                else
                {
                    // Exibe: HH:0M:SS
                    counter.text = "" + hours + ":" + "0" + minutes + ":" + seconds;
                }
            }
            else
            {
                if (seconds < 10)
                {
                    // Exibe: HH:MM:0S
                    counter.text = "" + hours + ":" + minutes + ":" + "0" + seconds;
                }
                else
                {
                    // Exibe: HH:MM:SS
                    counter.text = "" + hours + ":" + minutes + ":" + seconds;
                }
            }
        }
    }
    #endregion

    #region Death Animation
    private IEnumerator DeathAnimationControl()
    {
        // Estado inicial da coroutine
        coroutine_DAC_Started = true;

        // Desabilita o botão de pausa
        pauseButton.interactable = false;

        // Desabilita o movimento do jogador
        player.GetComponent<PlayerMovement>().enabled = false;

        // Congela o jogador
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        // Exibe a animação de flash
        whiteScreen.SetActive(true);
        coroutine_F = StartCoroutine(whiteScreen.GetComponent<DeathAnimation>().Flash());

        // Exibe a tela de saída quando a animação acaba
        while (true)
        {
            if (whiteScreen.GetComponent<DeathAnimation>().finished)
            {
                StopCoroutine(coroutine_F);
                exitScreen.SetActive(true);
                StopCoroutine(coroutine_DAC);
            }

            yield return null;
        }
    }
    #endregion
}