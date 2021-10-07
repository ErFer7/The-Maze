using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitInput : MonoBehaviour
{
    #region Public Variables
    // Acessoa a tela preta
    public FadeImage blackScreen;

    // Acesso ao AudioSource
    public AudioSource audioSource;
    #endregion

    #region Private Variables
    // Coroutine da controle de carregamento
    private Coroutine coroutine;

    // Acesso ao MusicPlayer
    private GameObject musicPlayer;

    // Acesso ao Script manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Acessa o Music Player
        musicPlayer = GameObject.FindWithTag("GameplayMusicPlayer");

        // Se o som está desabilitado os botões não emitirão sons
        if (!scriptManager.sound)
        {
            GetComponent<AudioSource>().volume = 0;
        }
    }
    #endregion

    #region Buttons
    // Botão de "Próximo"
    public void Next()
    {
        // Checa se uma animação já está em execução
        if (!scriptManager.animating)
        {
            // Condições do botão
            scriptManager.loadingStage = -1;
            scriptManager.animating = true;

            // Inicia a controlador de carregamento
            StartCoroutine(LoadingOutControl_E());
        }
    }

    // Botão de "Menu"
    public void Menu()
    {
        // Checa se uma animação já está em execução
        if (!scriptManager.animating)
        {
            // Condições do botão
            scriptManager.loadingStage = -2;
            scriptManager.animating = true;

            // Inicia a controlador de carregamento
            StartCoroutine(LoadingOutControl_E());
        }
    }

    // Botão de "Reiniciar"
    public void Restart()
    {
        // Checa se uma animação já está em execução
        if (!scriptManager.animating)
        {
            // Condições do botão
            scriptManager.loadingStage = -3;
            scriptManager.animating = true;

            // Inicia a controlador de carregamento
            StartCoroutine(LoadingOutControl_E());
        }
    }
    #endregion

    #region LoadingOut Control
    IEnumerator LoadingOutControl_E()
    {
        // Inicia a animação de escurecimento da tela
        blackScreen.gameObject.SetActive(true);
        coroutine = StartCoroutine(blackScreen.FadeImageTo(1, 0.25F));

        // Retorna a música ao normal (Frequência normal)
        if (musicPlayer.GetComponent<AudioLowPassFilter>().cutoffFrequency != 22000)
        {
            musicPlayer.GetComponent<MonoBehaviour>().StopCoroutine(musicPlayer.GetComponent<MusicPlayer>().coroutine_SC_LPFF);
            musicPlayer.GetComponent<MusicPlayer>().coroutine_SC_LPFF = StartCoroutine(musicPlayer.GetComponent<MusicPlayer>().LowPassFilterFade(22000F, 0.25F));
        }

        // Reduz o volume da música a 0 caso esteja retornando para o menu
        if (scriptManager.loadingStage == -2)
        {
            musicPlayer.GetComponent<MusicPlayer>().coroutine_VF = StartCoroutine(musicPlayer.GetComponent<MusicPlayer>().volumeFade(0, 0.25F));
        }

        // Espera até a tela escurecer totalmente
        while (true)
        {
            // Para o som de início do jogo (Caso ele esteja tocando)
            if (audioSource != null && audioSource.isPlaying)
            {
                StopCoroutine(audioSource.GetComponent<LoadingControl>().coroutine);
                Destroy(audioSource.gameObject);
            }

            switch (scriptManager.loadingStage)
            {
                // Próximo
                case 1:
                    // Para o controle de carregamento
                    StopCoroutine(coroutine);

                    // Condição final
                    scriptManager.animating = false;

                    // Incrementa o nível
                    ++scriptManager.level;

                    // Carrega o próximo nível
                    SceneManager.LoadScene(1);
                    break;
                // Menu
                case 5:
                    // Para o controle de carregamento
                    StopCoroutine(coroutine);

                    // Condição final
                    scriptManager.animating = false;

                    // Carrega o menu
                    SceneManager.LoadScene(0);
                    break;
                // Reiniciando
                case 6:
                    // Para o controle de carregamento
                    StopCoroutine(coroutine);

                    // Condição final
                    scriptManager.animating = false;

                    // Define que o jogo está reiniciando
                    scriptManager.restarting = true;

                    // Carrega o mesmo nível
                    SceneManager.LoadScene(1);
                    break;
                default:
                    break;
            }

            yield return null;
        }
    }
    #endregion
}