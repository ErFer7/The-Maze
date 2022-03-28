using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PauseInput : MonoBehaviour
{
    #region Public Variables
    // Acesso a imagem da tela escura
    public FadeImage blackScreen;

    // Acesso a tela de pausa
    public GameObject pausingScreen;
    #endregion

    #region Private Variables
    // Acesso ao Music Player
    private GameObject musicPlayer;

    // Acesso ao script da tela de pausa
    private Pause pausingScreenScript;

    // Coroutine da operação de fading
    private Coroutine coroutine_FIT;

    // Acesso ao Script Manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Acessa o script da tela de pausa neste objeto
        pausingScreenScript = gameObject.GetComponentInParent<Pause>();

        // Procura e acessa o Music Player
        musicPlayer = GameObject.FindWithTag("GameplayMusicPlayer");

        // Se o modo é escuro torna o botão branco
        if (scriptManager.dark && gameObject.name == "Pause Button")
        {
            gameObject.GetComponent<Image>().color = new Color(0.898F, 0.898F, 0.898F);
        }

        // Se o som está desativado os botões não emitirão sons
        if (!scriptManager.sound)
        {
            GetComponent<AudioSource>().volume = 0;
        }
    }
    #endregion

    #region Control
    IEnumerator LoadingOutControl_P()
    {
        // Essa coroutine controla o carregamento entre cenas

        // Inicia a operação de fade out
        blackScreen.gameObject.SetActive(true);
        coroutine_FIT = StartCoroutine(blackScreen.FadeImageTo(1, 0.25F));

        // Inicia a interpolação do filtro passa-baixa no Music Player para que a música retorne ao normal
        if (musicPlayer.GetComponent<AudioLowPassFilter>().cutoffFrequency != 22000)
        {
            musicPlayer.GetComponent<MonoBehaviour>().StopCoroutine(musicPlayer.GetComponent<MusicPlayer>().coroutine_SC_LPFF);
            musicPlayer.GetComponent<MusicPlayer>().coroutine_SC_LPFF = StartCoroutine(musicPlayer.GetComponent<MusicPlayer>().LowPassFilterFade(22000F, 0.5F));
        }

        // Interpola o volume do Music Player para 0 caso o carregamento esteja voltando para o menu
        if (scriptManager.loadingStage == -1)
        {
            musicPlayer.GetComponent<MusicPlayer>().coroutine_VF = StartCoroutine(musicPlayer.GetComponent<MusicPlayer>().volumeFade(0, 0.25F));
        }

        // Controla os estágios
        while (true)
        {
            switch (scriptManager.loadingStage)
            {
                // Carregamento para o menu
                case 1:
                    StopCoroutine(coroutine_FIT);
                    scriptManager.animating = false;
                    SceneManager.LoadScene(0);
                    break;
                // Reinicia o labirinto
                case 5:
                    scriptManager.preserveSave = true;
                    scriptManager.animating = false;
                    SceneManager.LoadScene(1);
                    break;
                default:
                    break;
            }

            yield return null;
        }
    }
    #endregion

    #region Buttons
    public void Pause()
    {
        // Ativa a tela de pausa caso nenhuma animação esteja ativa
        if (!scriptManager.animating)
        {
            pausingScreen.gameObject.SetActive(true);
        }
    }

    public void Continue()
    {
        // Continua o jogo
        pausingScreenScript.resume = true;
    }

    public void Menu()
    {
        // Caso nenhuma animação esteja ativa o controle de carregamento é iniciado no estado -1 (Menu)
        if (!scriptManager.animating)
        {
            scriptManager.loadingStage = -1;
            scriptManager.animating = true;
            StartCoroutine(LoadingOutControl_P());
        }
    }

    public void Restart()
    {
        // Caso nenhuma animação esteja ativa o controle de carregamento é iniciado no estado -1 (Reiniciando)
        if (!scriptManager.animating)
        {
            scriptManager.loadingStage = -2;
            scriptManager.animating = true;
            StartCoroutine(LoadingOutControl_P());
        }
    }
    #endregion
}