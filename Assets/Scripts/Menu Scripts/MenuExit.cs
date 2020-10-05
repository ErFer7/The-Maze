using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuExit : MonoBehaviour
{
    public GameObject exit;
    public float animationTime;
    public Color colorUp;
    public Color colorDown;
    public Image exitBackGround;
    public RectTransform exitMessageBox;
    public MusicManager musicManager;

    [System.NonSerialized]
    public Coroutine coroutine;

    private Vector2 targetPositionUp;
    private Vector2 targetPositionDown;

    public IEnumerator MessageBoxAnimation(Vector2 targetPosition, Color targetAlpha, float time)
    {
        DataHolder.animating = true;
        exit.SetActive(true);

        // Opening message box animation
        for (float i = 0; i <= 1F; i += Time.deltaTime / time)
        {
            // Stops lerping the color when the alpha is +-0.05 the value
            if (Mathf.Abs(exitBackGround.color.a - targetAlpha.a) < 0.05F)
            {
                exitBackGround.color = targetAlpha;
            }

            // Stops lerping the position when the message box is +-0.1 the value
            if (Mathf.Abs(exitMessageBox.anchoredPosition.y - targetPosition.y) < 0.1F)
            {
                exitMessageBox.anchoredPosition = targetPosition;
                i = 2;
            }

            // Lerp the color of the back ground
            exitBackGround.color = Color.Lerp(exitBackGround.color, targetAlpha, i);

            // Lerp the position of the message box
            exitMessageBox.anchoredPosition = Vector2.Lerp(exitMessageBox.anchoredPosition, targetPosition, i);

            yield return null;
        }

        // When the message box is completely down it is disabled
        if (exitMessageBox.anchoredPosition.y < targetPositionUp.y - 10F)
        {
            exit.SetActive(false);
        }

        DataHolder.animating = false;
        StopCoroutine(coroutine);
    }

    private void Start()
    {
        // Initializes the target positions
        targetPositionUp = new Vector2(0, 0);
        targetPositionDown = new Vector2(0, -485);
    }

    void Update()
    {
        // If the user press "Esc" or "Return on a smartphone"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If there is no animaton playing
            if (DataHolder.animating == false)
            {
                // turn the message box on
                if (exitMessageBox.anchoredPosition.y < targetPositionUp.y - 10)
                {
                    musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
                    if (musicManager.publicCoroutine_2 != null)
                    {
                        StopCoroutine(musicManager.publicCoroutine_2);
                    }
                    musicManager.publicCoroutine_2 = StartCoroutine(musicManager.lowPassFilterFade(200F, 0.65F));

                    coroutine = StartCoroutine(MessageBoxAnimation(targetPositionUp, colorUp, animationTime));
                }
                // turn the message box off
                else
                {
                    exitMessageBox.GetComponent<AudioSource>().Play();

                    if (musicManager.publicCoroutine_2 != null)
                    {
                        StopCoroutine(musicManager.publicCoroutine_2);
                    }
                    musicManager.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 22000;
                    musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = false;

                    coroutine = StartCoroutine(MessageBoxAnimation(targetPositionDown, colorDown, animationTime));
                }
            }
        }
    }
}