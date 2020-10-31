using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    // AINDA EM REFATORAÇÃO

    #region Public Variables
    // Acesso a objetos
    public GameObject Canvas;
    public GameObject messageBox;
    public GameObject returnButton;
    public GameObject musicManager;
    
    // Modo de jogo
    public GameMode mode;
    
    // Acesso ao plano de fundo
    public Image backGround;

    // Acesso a caixa de texto
    public RectTransform messageBoxPosition;

    // Cores (Para o uso do alfa)
    public Color colorUp;
    public Color colorDown;

    // Tempo de animação
    public float animationTime;

    // Botões
    public Button continueButton;
    public Button newGameButton;

    // Modos de jogo
    public enum GameMode
    {
        Classic,
        Time,
        Dark,
        Custom
    }
    #endregion

    #region Private Variables
    // Coroutines
    private Coroutine coroutine_MBA;
    private Coroutine coroutine_R;

    // Posições da caixa de texto
    private Vector2 targetPositionUp;
    private Vector2 targetPositionDown;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Adiciona receptores de eventos para os botões
        continueButton.onClick.AddListener(Continue);
        newGameButton.onClick.AddListener(NewGame);

        // Inicializa as posições alvo
        targetPositionUp = new Vector2(0, 0);
        targetPositionDown = new Vector2(0, -510);
    }
    #endregion

    #region Loading Core
    public void LoadScene()
    {
        // Caso nenhuma animação esteja ocorrendo
        if (!DataHolder.animating)
        {
            // Muda as configurações para cada modo
            switch (mode)
            {
                // Clássico
                case GameMode.Classic:

                    DataHolder.progressive = true;
                    DataHolder.regressiveTime = false;
                    DataHolder.dark = false;

                    // Se tem um labirinto clássico salvo exibe a caixa de texto
                    if (PlayerPrefs.GetInt("classicSaved", -1) > 0)
                    {   
                        // Abafa a música
                        musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
                        if (musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF != null)
                        {
                            StopCoroutine(musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF);
                        }
                        musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF = StartCoroutine(musicManager.GetComponent<MusicManager>().LowPassFilterFade(200F, 0.65F));

                        // Exibe a caixa de texto
                        coroutine_MBA = StartCoroutine(MessageBoxAnimation(targetPositionUp, colorUp, animationTime));
                    }
                    else
                    {
                        DataHolder.continueLastMaze = false;
                        DataHolder.level = 1;

                        FadeOutAnimation();
                    }

                    break;
                // Tempo
                case GameMode.Time:

                    DataHolder.progressive = true;
                    DataHolder.regressiveTime = true;
                    DataHolder.dark = false;

                    // Se tem um labirinto de tempo salvo exibe a caixa de texto
                    if (PlayerPrefs.GetInt("timeSaved", -1) > 0)
                    {
                        musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
                        if (musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF != null)
                        {
                            StopCoroutine(musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF);
                        }
                        musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF = StartCoroutine(musicManager.GetComponent<MusicManager>().LowPassFilterFade(200F, 0.65F));

                        coroutine_MBA = StartCoroutine(MessageBoxAnimation(targetPositionUp, colorUp, animationTime));
                    }
                    else
                    {
                        DataHolder.continueLastMaze = false;
                        DataHolder.level = 1;

                        FadeOutAnimation();
                    }

                    break;
                // Escuro
                case GameMode.Dark:

                    DataHolder.progressive = true;
                    DataHolder.regressiveTime = false;
                    DataHolder.dark = true;

                    // Se tem um labirinto escuro salvo exibe a caixa de texto
                    if (PlayerPrefs.GetInt("darkSaved", -1) > 0)
                    {
                        musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
                        if (musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF != null)
                        {
                            StopCoroutine(musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF);
                        }
                        musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF = StartCoroutine(musicManager.GetComponent<MusicManager>().LowPassFilterFade(200F, 0.65F));

                        coroutine_MBA = StartCoroutine(MessageBoxAnimation(targetPositionUp, colorUp, animationTime));
                    }
                    else
                    {
                        DataHolder.continueLastMaze = false;
                        DataHolder.level = 1;

                        FadeOutAnimation();
                    }

                    break;
                // Personalizado
                case GameMode.Custom:

                    DataHolder.progressive = false;
                    DataHolder.regressiveTime = false;
                    DataHolder.dark = false;

                    FadeOutAnimation();

                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator Return()
    {
        // Desabilita o retorno da página principal
        returnButton.GetComponent<MobileReturn>().enabled = false;

        while (true)
        {
            // Caso o usuário pressione ESC ou o botão de retorno no smartphone
            if (Input.GetKeyDown(KeyCode.Escape))
            {   
                // Retorna a música ao normal (desabilita o filtro passa-baixa)
                if (musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF != null)
                {
                    StopCoroutine(musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF);
                }
                musicManager.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 22000;
                musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = false;

                // Fecha a caixa de texto
                messageBox.GetComponentInChildren<AudioSource>().Play();
                coroutine_MBA = StartCoroutine(MessageBoxAnimation(targetPositionDown, colorDown, animationTime));

                // Habilita o retorno da página principal
                returnButton.GetComponent<MobileReturn>().enabled = true;

                StopCoroutine(coroutine_R);
            }

            yield return null;
        }
    }

    private void Continue()
    {
        // Definição de continuação
        DataHolder.continueLastMaze = true;

        if (!DataHolder.regressiveTime)
        {
            // Clássico
            if (!DataHolder.dark)
            {
                DataHolder.level = PlayerPrefs.GetInt("classicLevel");
            }
            // Escuro
            else
            {
                DataHolder.level = PlayerPrefs.GetInt("darkLevel");
            }
        }
        // Tempo
        else
        {
            DataHolder.level = PlayerPrefs.GetInt("timeLevel");
        }

        FadeOutAnimation();
    }

    private void NewGame()
    {
        // Definições de novo jogo
        DataHolder.continueLastMaze = false;
        DataHolder.level = 1;

        FadeOutAnimation();
    }

    IEnumerator Load()
    {
        // Carrega quando todas as animações terminarem
        while (true)
        {
            if (!DataHolder.animating)
            {
                StopCoroutine(Canvas.GetComponent<Fade>().coroutine_FT);
                SceneManager.LoadScene(1);
                StopCoroutine(coroutine_MBA);
            }

            yield return null;
        }
    }
    #endregion

    #region Loading animations
    IEnumerator MessageBoxAnimation(Vector2 targetPosition, Color targetAlpha, float time)
    {
        // Condições iniciais da animação
        DataHolder.animating = true;
        messageBox.SetActive(true);
        continueButton.interactable = false;
        newGameButton.interactable = false;

        // Animação da abertura da caixa de animações
        for (float i = 0; i <= 1F; i += Time.deltaTime / time)
        {
            // Para a interpolação quando o alfa converge abaixo de 0.05
            if (Mathf.Abs(backGround.color.a - targetAlpha.a) < 0.05F)
            {
                backGround.color = targetAlpha;
            }

            // Para a interpolação quando a posição converge abaixo de 0.01
            if (Mathf.Abs(messageBoxPosition.anchoredPosition.y - targetPosition.y) < 0.1F)
            {
                messageBoxPosition.anchoredPosition = targetPosition;
                i = 2;
            }

            // Interpola a cor do plano de fundo
            backGround.color = Color.Lerp(backGround.color, targetAlpha, i);

            // Interpola a posição da caixa de texto
            messageBoxPosition.anchoredPosition = Vector2.Lerp(messageBoxPosition.anchoredPosition, targetPosition, i);

            yield return null;
        }

        // Quando a caixa de texto está totalmente escondida ela é desabilitada
        if (messageBoxPosition.anchoredPosition.y < targetPositionUp.y - 10F)
        {
            messageBox.SetActive(false);
        }
        else
        {
            coroutine_R = StartCoroutine(Return());
        }

        // Condições finais da animação
        DataHolder.animating = false;
        continueButton.interactable = true;
        newGameButton.interactable = true;
        StopCoroutine(coroutine_MBA);
    }

    private void FadeOutAnimation()
    {
        // Faz a animação de fade out
        if (!DataHolder.animating)
        {
            Canvas.GetComponent<Fade>().coroutine_FT = StartCoroutine(Canvas.GetComponent<Fade>().FadeTo(0F, Canvas.GetComponent<Fade>().fadeTime));
            musicManager.GetComponent<MusicManager>().publicCoroutine_MF = StartCoroutine(musicManager.GetComponent<MusicManager>().MusicFade(0, 3));
            coroutine_MBA = StartCoroutine(Load());
        }
    }
    #endregion
}