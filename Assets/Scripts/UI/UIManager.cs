using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    #region Public Variables
    public float fadeTime;
    public float popUpMoveTime;
    public float popUpBGAlpha;
    [HideInInspector]
    public UIState uiState;

    public enum UIState
    {
        Void,
        Menu,
        Start,
        Settings,
        Info,
        Language,
        Custom,
        SeedInfo,
        PopUp,
        Load,
        Gameplay,
        Paused,
        EndGame,
        InTransition
    }
    #endregion

    #region Private Variables
    private ScriptManager scriptManager;
    private Image voidImage;
    private GameObject currentPanel;
    private Coroutine coroutine_PTC;  // Panel transition coroutine
    private Coroutine coroutine_STC;  // Scene transition coroutine 
    private Coroutine coroutine_LFI;  // Linear fade image
    private Coroutine coroutine_PUA;  // Pop up animation
    private Coroutine coroutine_PUAC;  // Pop up action coroutine
    private bool finishFlag;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag(tag).Length == 1)
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
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();
        scriptManager.uiManager = gameObject.GetComponent<UIManager>();
        uiState = UIState.Void;
        finishFlag = false;

        SceneTransition(true);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        voidImage = GameObject.FindWithTag("VoidImage").GetComponent<Image>();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            currentPanel = GameObject.FindWithTag("Menu");
        }
    }
    #endregion

    #region Public Methods
    public void PanelTransition(GameObject nextPanel)
    {
        uiState = UIState.InTransition;
        coroutine_PTC = StartCoroutine(PanelTransitionCoroutine(nextPanel));
    }

    public void SceneTransition(bool start)
    {
        uiState = UIState.InTransition;
        coroutine_STC = StartCoroutine(SceneTransitionCoroutine(start));
    }

    public void PopUpAction(GameObject popUp, GameObject background, Vector2 targetPosition, bool open)
    {
        uiState = UIState.InTransition;
        coroutine_PUAC = StartCoroutine(PopUpActionCoroutine(popUp, background, targetPosition, open));
    }

    public void UpdateMazeSize(int width = 0, int height = 0)
    {
        if (height < 2)
        {
            scriptManager.width = width;
        }
        else
        {
            scriptManager.height = height;
        }
    }
    #endregion

    #region Private Methods
    private IEnumerator PopUpActionCoroutine(GameObject popUp, GameObject background, Vector2 targetPosition, bool open)
    {
        if (open)
        {
            background.SetActive(true);
            popUp.SetActive(true);

            // Toca o áudio da caixa de texto
            popUp.GetComponent<AudioSource>().Play();

            // Abafa a música usando o filtro passa-baixa
            scriptManager.musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
            if (scriptManager.musicManager.publicCoroutine_LPFF != null)
            {
                StopCoroutine(scriptManager.musicManager.publicCoroutine_LPFF);
            }
            scriptManager.musicManager.publicCoroutine_LPFF = StartCoroutine(scriptManager.musicManager.LowPassFilterFade(200F, 0.65F));

            // Move a caixa de texto para a tela
            coroutine_PUA = StartCoroutine(PopUpAnimation(popUp.GetComponent<RectTransform>(), background.GetComponent<Image>(), targetPosition, popUpBGAlpha, popUpMoveTime));

            while (!finishFlag)
            {
                yield return null;
            }

            uiState = UIState.PopUp;
        }
        else
        {
            // Toca o áudio da caixa de texto
            popUp.GetComponent<AudioSource>().Play();

            // Retorna o áudio ao normal
            if (scriptManager.musicManager.publicCoroutine_LPFF != null)
            {
                StopCoroutine(scriptManager.musicManager.publicCoroutine_LPFF);
            }
            scriptManager.musicManager.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 22000;
            scriptManager.musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = false;

            // Move a caixa de texto para baixo
            coroutine_PUA = StartCoroutine(PopUpAnimation(popUp.GetComponent<RectTransform>(), background.GetComponent<Image>(), targetPosition, 0F, popUpMoveTime));

            while (!finishFlag)
            {
                yield return null;
            }

            popUp.SetActive(false);
            background.SetActive(false);

            uiState = UIState.Menu;
        }

        yield return null;
    }

    private IEnumerator PanelTransitionCoroutine(GameObject nextPanel)
    {
        voidImage.gameObject.SetActive(true);
        coroutine_LFI = StartCoroutine(LinearFadeImage(voidImage, true, fadeTime));

        while (!finishFlag)
        {
            yield return null;
        }

        currentPanel.SetActive(false);
        nextPanel.SetActive(true);

        coroutine_LFI = StartCoroutine(LinearFadeImage(voidImage, false, fadeTime));

        while (!finishFlag)
        {
            yield return null;
        }

        voidImage.gameObject.SetActive(false);

        switch (nextPanel.name)
        {
            case "Menu":

                uiState = UIState.Menu;
                break;
            case "Start":

                uiState = UIState.Start;
                break;
            case "Settings":

                uiState = UIState.Settings;
                break;
            case "Info":

                uiState = UIState.Info;
                break;
            case "Language":

                uiState = UIState.Language;
                break;
            case "Custom":

                uiState = UIState.Custom;
                break;
            case "Seed Info":

                uiState = UIState.SeedInfo;
                break;
            default:
                uiState = UIState.Void;
                break;
        }

        currentPanel = nextPanel;
    }

    private IEnumerator SceneTransitionCoroutine(bool start)
    {
        if (start)
        {
            voidImage.gameObject.SetActive(true);
            coroutine_LFI = StartCoroutine(LinearFadeImage(voidImage, false, fadeTime));

            while (!finishFlag)
            {
                yield return null;
            }

            voidImage.gameObject.SetActive(false);

            uiState = UIState.Menu;
            // No scene manager o sistema deve definir o GameState como "Menu"
        }
        else
        {
            voidImage.gameObject.SetActive(true);
            coroutine_LFI = StartCoroutine(LinearFadeImage(voidImage, false, fadeTime));

            while (!finishFlag)
            {
                yield return null;
            }

            uiState = UIState.Void;
            // No scene manager o sistema deve esperar até que a operação seja concluída

            // Espere pelo sinal de que o jogo carregou a cena

            // Fade in da tela de carregamento

            // Espere pelo sinal de que o labirinto foi gerado

            // Fade out

            // Espere pelo fim do fade out

            // Fade in

            // Espere pelo fim do fade in

            // Finalizado
        }
    }

    private IEnumerator LinearFadeImage(Image image, bool fadeIn, float time)
    {
        finishFlag = false;
        Color color = image.color;

        // Transparente para opaco
        if (fadeIn)
        {
            // Definição inicial
            color.a = 0F;
            image.color = color;

            // Loop que dura conforme o tempo especificado
            for (float i = 0; i <= time; i += Time.deltaTime)
            {
                color.a = i / time;
                image.color = color;
                yield return null;
            }

            // Definição final para evitar erros
            color.a = 1F;
            image.color = color;
        }
        // Opaco para transparente
        else
        {
            // Definição inicial
            color.a = 1F;
            image.color = color;

            // Loop que dura conforme o tempo especificado
            for (float i = time; i >= 0; i -= Time.deltaTime)
            {
                color.a = i / time;
                image.color = color;
                yield return null;
            }

            // Definição final para evitar erros
            color.a = 0F;
            image.color = color;
        }

        finishFlag = true;
        yield return null;
    }

    private IEnumerator PopUpAnimation(RectTransform popUp, Image background, Vector2 targetPosition, float targetAlpha, float time)
    {
        finishFlag = false;
        float initialAplha = background.color.a;
        Color color = background.color;

        for (float i = 0; i <= time; i += Time.deltaTime)
        {
            // Incremento linear (alfa final = alfa inicial + velocidade * tempo)
            color.a = initialAplha + ((targetAlpha - initialAplha) / time) * i;
            background.color = color;
            // Interpolação linear (sigmoide)
            popUp.anchoredPosition = Vector2.Lerp(popUp.anchoredPosition, targetPosition, i / time);

            yield return null;
        }

        color.a = targetAlpha;
        background.color = color;
        popUp.anchoredPosition = targetPosition;

        finishFlag = true;
        yield return null;
    }
    #endregion
}
