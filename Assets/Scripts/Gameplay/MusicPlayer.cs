using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    #region Public Variables
    #region Editor Acessible
    // Clássico
    public AudioClip classicTheme_1_Short;
    public AudioClip classicTheme_2_Short;
    public AudioClip classicTheme_1;
    public AudioClip classicTheme_2;

    // Tempo
    public AudioClip timeTheme_1_Short;
    public AudioClip timeTheme_2_Short;
    public AudioClip timeTheme_1;
    public AudioClip timeTheme_2;

    // Escuro
    public AudioClip darkTheme_1_Short;
    public AudioClip darkTheme_2_Short;
    public AudioClip darkTheme_1;
    public AudioClip darkTheme_2;
    #endregion

    // Coroutine do controle de inicialização e efeitos de áudio
    public Coroutine coroutine_SC_LPFF;

    // Coroutine do efeito de volume
    public Coroutine coroutine_VF;

    // Coroutine do controle da música
    public Coroutine coroutine_PM;
    #endregion

    #region Private Variables
    // Acesso ao AudioSource
    private AudioSource audioSource;
    
    // Define o modo de jogo
    private GameMode gameMode;

    // Define o índice da música
    private int themeIndex;

    // Acesso ao LowPassFilter
    private AudioLowPassFilter lowPassFilter;

    // Acesso ao Script Manager
    private ScriptManager scriptManager;

    // Modos de jogo
    private enum GameMode
    {
        classic_or_Custom,
        time,
        dark,
    }
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Define este objeto como singleton
        if (SceneManager.GetActiveScene().buildIndex == 1 && GameObject.FindGameObjectsWithTag("GameplayMusicPlayer").Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destrói este objeto o qualquer cópia do mesmo no menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("GameplayMusicPlayer").Length; ++i)
            {
                Destroy(GameObject.FindGameObjectsWithTag("GameplayMusicPlayer")[i]);
            }
        }
    }

    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Acessa os componentes
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();

        // Se a música está ativada ativa o controle de inicialização
        if (scriptManager.music)
        {
            coroutine_SC_LPFF = StartCoroutine(StartingControl());
        }
    }
    #endregion

    #region Starting Control
    private IEnumerator StartingControl()
    {
        // Inicia a música quando todas as animações acabaram
        while (true)
        {
            if (scriptManager.loadingStage == -3)
            {
                coroutine_PM = StartCoroutine(PlayMusic());
                StopCoroutine(coroutine_SC_LPFF);
            }

            yield return null;
        }
    }
    #endregion

    #region Play Music
    private IEnumerator PlayMusic()
    {
        // Define a música inicial //

        // Modo clássico ou customizado
        if (!scriptManager.regressiveTime && !scriptManager.dark)
        {
            gameMode = GameMode.classic_or_Custom;

            // Escolhe uma das duas músicas
            if (Random.Range(0, 2) == 0)
            {
                // Toca a música e define o tema como 1
                themeIndex = 1;
                audioSource.clip = classicTheme_1_Short;
                audioSource.Play();
            }
            else
            {
                // Toca a música e define o tema como 2
                themeIndex = 2;
                audioSource.clip = classicTheme_2_Short;
                audioSource.Play();
            }
        }
        // Modo tempo
        else if (scriptManager.regressiveTime)
        {
            gameMode = GameMode.time;

            // Escolhe uma das duas músicas
            if (Random.Range(0, 2) == 0)
            {
                // Toca a música e define o tema como 1
                themeIndex = 1;
                audioSource.clip = timeTheme_1_Short;
                audioSource.Play();
            }
            else
            {
                // Toca a música e define o tema como 2
                themeIndex = 2;
                audioSource.clip = timeTheme_2_Short;
                audioSource.Play();
            }
        }
        // Modo escuro
        else if (scriptManager.dark)
        {
            gameMode = GameMode.dark;

            // Escolhe uma das duas músicas
            if (Random.Range(0, 2) == 0)
            {
                // Toca a música e define o tema como 1
                themeIndex = 1;
                audioSource.clip = darkTheme_1_Short;
                audioSource.Play();
            }
            else
            {
                // Toca a música e define o tema como 2
                themeIndex = 2;
                audioSource.clip = darkTheme_2_Short;
                audioSource.Play();
            }
        }

        // Escolhe a música continuamente
        while (true)
        {
            // Se a música terminou
            if (!audioSource.isPlaying)
            {
                switch (gameMode)
                {
                    // Modo clássico ou customizado
                    case GameMode.classic_or_Custom:

                        // Se o tema é 2
                        if (themeIndex == 2)
                        {
                            // Toca a música com um delay de 5 a 30 segundos e define o tema como 1
                            themeIndex = 1;
                            audioSource.clip = classicTheme_1;
                            audioSource.PlayDelayed(Random.Range(5, 30));
                        }
                        // Se o tema é 1
                        else
                        {
                            // Toca a música com um delay de 5 a 30 segundos e define o tema como 2
                            themeIndex = 2;
                            audioSource.clip = classicTheme_2;
                            audioSource.PlayDelayed(Random.Range(5, 30));
                        }

                        break;
                    // Modo tempo
                    case GameMode.time:

                        // Se o tema é 2
                        if (themeIndex == 2)
                        {
                            // Toca a música com um delay de 5 a 30 segundos e define o tema como 1
                            themeIndex = 1;
                            audioSource.clip = timeTheme_1;
                            audioSource.PlayDelayed(Random.Range(5, 30));
                        }
                        // Se o tema é 1
                        else
                        {
                            // Toca a música com um delay de 5 a 30 segundos e define o tema como 2
                            themeIndex = 2;
                            audioSource.clip = timeTheme_2;
                            audioSource.PlayDelayed(Random.Range(5, 30));
                        }

                        break;
                    // Modo escuro
                    case GameMode.dark:

                        // Se o tema é 2
                        if (themeIndex == 2)
                        {
                            // Toca a música com um delay de 5 a 30 segundos e define o tema como 1
                            themeIndex = 1;
                            audioSource.clip = darkTheme_1;
                            audioSource.PlayDelayed(Random.Range(5, 30));
                        }
                        // Se o tema é 1
                        else
                        {
                            // Toca a música com um delay de 5 a 30 segundos e define o tema como 2
                            themeIndex = 2;
                            audioSource.clip = darkTheme_2;
                            audioSource.PlayDelayed(Random.Range(5, 30));
                        }

                        break;
                    default:
                        break;
                }
            }

            yield return null;
        }
    }

    // Faz a operação de fading no fitro passa-baixa
    public IEnumerator LowPassFilterFade(float value, float fadeTime)
    {
        for (float i = 0; i <= 1F; i += Time.deltaTime / fadeTime)
        {
            // Condição de convergência
            if (Mathf.Abs(lowPassFilter.cutoffFrequency - value) < 250F)
            {
                lowPassFilter.cutoffFrequency = value;
                i = 2;
            }

            // Interpola linearmente da frequência inicial até o valor definido
            lowPassFilter.cutoffFrequency = Mathf.Lerp(lowPassFilter.cutoffFrequency, value, i);

            yield return null;
        }
    }

    // Faz a operação de fading no volume
    public IEnumerator volumeFade(float value, float fadeTime)
    {
        for (float i = 0; i <= 1F; i += Time.deltaTime / fadeTime)
        {
            // Condição de convergência
            if (Mathf.Abs(audioSource.volume - value) < 0.05F)
            {
                audioSource.volume = value;
                i = 2;
            }

            // Interpola linearmente do volume inicial até o valor definido
            audioSource.volume = Mathf.Lerp(audioSource.volume, value, i);

            yield return null;
        }
    }
    #endregion
}