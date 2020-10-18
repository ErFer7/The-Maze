using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public GameObject Canvas;
    public GameMode mode;
    public GameObject messageBox;
    public Image backGround;
    public RectTransform messageBoxPosition;
    public Color colorUp;
    public Color colorDown;
    public float animationTime;
    public Button continueButton;
    public Button newGameButton;
    public GameObject returnButton;
    public GameObject musicManager;

    private Coroutine coroutine;
    private Coroutine coroutine_2;
    private Vector2 targetPositionUp;
    private Vector2 targetPositionDown;

    public enum GameMode
    {
        Classic,
        Time,
        Dark,
        Custom
    }

    public void LoadScene()
    {
        if (DataHolder.animating == false)
        {
            // Changes the settings for each mode
            switch (mode)
            {
                // Classic
                case GameMode.Classic:

                    DataHolder.progressive = true;
                    DataHolder.regressiveTime = false;
                    DataHolder.dark = false;

                    // If there is a classic maze saved shows a message box
                    if (PlayerPrefs.GetInt("classicSaved", -1) > 0)
                    {
                        musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
                        if (musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF != null)
                        {
                            StopCoroutine(musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF);
                        }
                        musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF = StartCoroutine(musicManager.GetComponent<MusicManager>().LowPassFilterFade(200F, 0.65F));

                        coroutine = StartCoroutine(MessageBoxAnimation(targetPositionUp, colorUp, animationTime));
                    }
                    // Continues normally
                    else
                    {
                        DataHolder.continueLastMaze = false;
                        DataHolder.level = 1;

                        FadeOutAnimation();
                    }

                    break;
                // Time
                case GameMode.Time:

                    DataHolder.progressive = true;
                    DataHolder.regressiveTime = true;
                    DataHolder.dark = false;

                    // If there is a time maze saved shows a message box
                    if (PlayerPrefs.GetInt("timeSaved", -1) > 0)
                    {
                        musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
                        if (musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF != null)
                        {
                            StopCoroutine(musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF);
                        }
                        musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF = StartCoroutine(musicManager.GetComponent<MusicManager>().LowPassFilterFade(200F, 0.65F));

                        coroutine = StartCoroutine(MessageBoxAnimation(targetPositionUp, colorUp, animationTime));
                    }
                    // Continues normally
                    else
                    {
                        DataHolder.continueLastMaze = false;
                        DataHolder.level = 1;

                        FadeOutAnimation();
                    }

                    break;
                // Dark
                case GameMode.Dark:

                    DataHolder.progressive = true;
                    DataHolder.regressiveTime = false;
                    DataHolder.dark = true;

                    // If there is a classic maze saved shows a message box
                    if (PlayerPrefs.GetInt("darkSaved", -1) > 0)
                    {
                        musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
                        if (musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF != null)
                        {
                            StopCoroutine(musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF);
                        }
                        musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF = StartCoroutine(musicManager.GetComponent<MusicManager>().LowPassFilterFade(200F, 0.65F));

                        coroutine = StartCoroutine(MessageBoxAnimation(targetPositionUp, colorUp, animationTime));
                    }
                    // Continues normally
                    else
                    {
                        DataHolder.continueLastMaze = false;
                        DataHolder.level = 1;

                        FadeOutAnimation();
                    }

                    break;
                // Custom
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

    // Animates the message box
    IEnumerator MessageBoxAnimation(Vector2 targetPosition, Color targetAlpha, float time)
    {
        DataHolder.animating = true;
        messageBox.SetActive(true);
        continueButton.interactable = false;
        newGameButton.interactable = false;

        // Opening message box animation
        for (float i = 0; i <= 1F; i += Time.deltaTime / time)
        {
            // Stops lerping the color when the alpha is +-0.05 the value
            if (Mathf.Abs(backGround.color.a - targetAlpha.a) < 0.05F)
            {
                backGround.color = targetAlpha;
            }

            // Stops lerping the position when the message box is +-0.1 the value
            if (Mathf.Abs(messageBoxPosition.anchoredPosition.y - targetPosition.y) < 0.1F)
            {
                messageBoxPosition.anchoredPosition = targetPosition;
                i = 2;
            }

            // Lerp the color of the back ground
            backGround.color = Color.Lerp(backGround.color, targetAlpha, i);

            // Lerp the position of the message box
            messageBoxPosition.anchoredPosition = Vector2.Lerp(messageBoxPosition.anchoredPosition, targetPosition, i);

            yield return null;
        }

        // When the message box is completely down it is disabled
        if (messageBoxPosition.anchoredPosition.y < targetPositionUp.y - 10F)
        {
            messageBox.SetActive(false);
        }
        else
        {
            coroutine_2 = StartCoroutine(Return());
        }

        DataHolder.animating = false;
        continueButton.interactable = true;
        newGameButton.interactable = true;
        StopCoroutine(coroutine);
    }

    // Close the message box
    IEnumerator Return()
    {
        // Disables the start page return
        returnButton.GetComponent<MobileReturn>().enabled = false;

        while (true)
        {
            // If the user press "Esc" or the mobile escape button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF != null)
                {
                    StopCoroutine(musicManager.GetComponent<MusicManager>().publicCoroutine_LPFF);
                }
                musicManager.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 22000;
                musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = false;

                // Closes the message box
                messageBox.GetComponentInChildren<AudioSource>().Play();
                coroutine = StartCoroutine(MessageBoxAnimation(targetPositionDown, colorDown, animationTime));
                // Enables the start page return
                returnButton.GetComponent<MobileReturn>().enabled = true;

                StopCoroutine(coroutine_2);
            }

            yield return null;
        }
    }

    // Continues from the last maze
    private void Continue()
    {
        DataHolder.continueLastMaze = true;

        if (DataHolder.regressiveTime == false)
        {
            // Classic
            if (!DataHolder.dark)
            {
                DataHolder.level = PlayerPrefs.GetInt("classicLevel");
            }
            // Dark
            else
            {
                DataHolder.level = PlayerPrefs.GetInt("darkLevel");
            }
        }
        // Time
        else
        {
            DataHolder.level = PlayerPrefs.GetInt("timeLevel");
        }

        FadeOutAnimation();
    }

    // Start a new game
    private void NewGame()
    {
        DataHolder.continueLastMaze = false;
        DataHolder.level = 1;

        FadeOutAnimation();
    }

    // Fade out before loading
    private void FadeOutAnimation()
    {
        // Fade the screen to black and wait for the end of the animation to load
        if (DataHolder.animating == false)
        {
            Canvas.GetComponent<Fade>().coroutine_FT = StartCoroutine(Canvas.GetComponent<Fade>().FadeTo(0F, Canvas.GetComponent<Fade>().fadeTime));
            musicManager.GetComponent<MusicManager>().publicCoroutine_MF = StartCoroutine(musicManager.GetComponent<MusicManager>().MusicFade(0, 3));
            coroutine = StartCoroutine(Load());
        }
    }

    IEnumerator Load()
    {
        // Loads when all animations are finished
        while (true)
        {
            if (DataHolder.animating == false)
            {
                StopCoroutine(Canvas.GetComponent<Fade>().coroutine_FT);
                SceneManager.LoadScene(1);
                StopCoroutine(coroutine);
            }

            yield return null;
        }
    }

    private void Start()
    {
        // Adds listeners to the buttons
        continueButton.onClick.AddListener(Continue);
        newGameButton.onClick.AddListener(NewGame);

        // Initializes the target positions
        targetPositionUp = new Vector2(0, 0);
        targetPositionDown = new Vector2(0, -510);
    }
}