using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    #region Public Variables
    #region Editor Acessible
    // Tempo de animação de fading
    public float fadeTime;

    // Acesso ao texto para a exibição de informações
    public Text size;
    public Text level;
    public Text seed;
    public Text distanceTravelled;

    // Acesso aos textos de título
    public Text levelTitle;
    public Text seedTitle;
    public Text distanceTravelledTitle;
    public Text sizeTitle;

    // Acesso ao jogador e botão de pausa
    public GameObject player;
    public Button pauseButton;
    #endregion
    // Estado da pausa
    [HideInInspector]
    public bool resume;
    // Coroutine
    [HideInInspector]
    public Coroutine coroutine_PG;
    #endregion

    #region Private Variables
    // Acesso ao canvas
    private CanvasGroup canvas;

    // Acesso ao Audio Source
    private AudioSource audioSource;

    // Acesso ao Music Player
    private GameObject musicPlayer;

    // Acesso ao Script manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        // Inicia a coroutine da pausa
        coroutine_PG = StartCoroutine(PauseGame());
    }

    private void Start()
    {
        // Acessa o Audio Source deste objeto
        audioSource = GetComponent<AudioSource>();

        // Traduz todos os textos
        if (scriptManager.dynamicLocalizedText.ContainsKey("pausingScreen_Level"))
        {
            levelTitle.text = scriptManager.dynamicLocalizedText["pausingScreen_Level"];
        }
        if (scriptManager.dynamicLocalizedText.ContainsKey("pausingScreen_Seed"))
        {
            seedTitle.text = scriptManager.dynamicLocalizedText["pausingScreen_Seed"];
        }
        if (scriptManager.dynamicLocalizedText.ContainsKey("pausingScreen_DistanceTravelled"))
        {
            distanceTravelledTitle.text = scriptManager.dynamicLocalizedText["pausingScreen_DistanceTravelled"];
        }
        if (scriptManager.dynamicLocalizedText.ContainsKey("pausingScreen_Size"))
        {
            sizeTitle.text = scriptManager.dynamicLocalizedText["pausingScreen_Size"];
        }

        // Inicializa os displays de informação
        size.text = scriptManager.width + " X " + scriptManager.height;
        seed.text = "" + scriptManager.seed;

        // Exibe o nível se o modo é progressivo
        if (scriptManager.progressive)
        {
            level.text = "" + scriptManager.level;
        }
        else
        {
            level.text = "N/A";
        }
    }
    #endregion

    #region Pause Game
    public IEnumerator PauseGame()
    {
        // Define o estado como pausado
        resume = false;

        // Acessa os componentes
        canvas = gameObject.GetComponent<CanvasGroup>();
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();
        musicPlayer = GameObject.FindWithTag("GameplayMusicPlayer");

        // Desabilita o botão
        pauseButton.interactable = false;

        // Reseta o alfa
        canvas.alpha = 0F;

        // Paraliza o jogador
        scriptManager.playerCanMove = false;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        // Acessa a distância percorrida
        distanceTravelled.text = Mathf.Floor(player.GetComponent<PlayerMovement>().distanceTravelled) + "m";

        // Define o que uma animação iniciou
        scriptManager.animating = true;

        // Abafa o áudio
        musicPlayer.GetComponent<AudioLowPassFilter>().enabled = true;
        musicPlayer.GetComponent<MusicPlayer>().coroutine_SC_LPFF = musicPlayer.GetComponent<MonoBehaviour>().StartCoroutine(musicPlayer.GetComponent<MusicPlayer>().LowPassFilterFade(400F, fadeTime));

        // Operação de fade in
        for (float i = 0; i <= 1F; i += Time.deltaTime / fadeTime)
        {
            // Condição de convergência
            if (canvas.alpha > 0.95F)
            {
                canvas.alpha = 1F;
                i = 2;
            }
            // Interpola o alfa
            else
            {
                canvas.alpha = Mathf.Lerp(canvas.alpha, 1F, i);
            }

            yield return null;
        }

        // Define o que uma animação terminou
        scriptManager.animating = false;

        // Espera o jogo ser continuado
        while (!resume)
        {
            // Se o jogador pressionar uma tecla de saída
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Toca o som de saída
                if (scriptManager.sound)
                {
                    audioSource.Play();
                }

                // Define que o jogo deve continuar
                resume = true;
            }

            yield return null;
        }

        // Define o que uma animação iniciou
        scriptManager.animating = true;

        // Operação de fade out
        for (float i = 0; i <= 1F; i += Time.deltaTime / fadeTime)
        {
            // Condição de convergência
            if (canvas.alpha < 0.05F)
            {
                canvas.alpha = 0F;
                i = 2;
            }
            // Interpola o alfa
            else
            {
                canvas.alpha = Mathf.Lerp(canvas.alpha, 0F, i);
            }

            yield return null;
        }

        // Restora a frequência da música
        musicPlayer.GetComponent<MusicPlayer>().coroutine_SC_LPFF = StartCoroutine(musicPlayer.GetComponent<MusicPlayer>().LowPassFilterFade(22000F, fadeTime));

        // Define o que uma animação terminou
        scriptManager.animating = false;

        // Reabilita o botão de pausa
        pauseButton.interactable = true;

        // Liberta o jogador
        scriptManager.playerCanMove = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        // Espera a frequência retornar ao normal
        while (musicPlayer.GetComponent<AudioLowPassFilter>().cutoffFrequency != 22000)
        {
            yield return null;
        }

        // Desabilita o componente de filtro passa-baixa
        musicPlayer.GetComponent<AudioLowPassFilter>().enabled = false;

        // Para esta coroutine
        StopCoroutine(coroutine_PG);

        // Desabilita a tela de pausa
        gameObject.SetActive(false);

        yield return null;
    }
    #endregion
}