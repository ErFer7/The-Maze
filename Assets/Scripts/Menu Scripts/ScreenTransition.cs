using UnityEngine;
using System.Collections;

public class ScreenTransition : MonoBehaviour
{
    // REFATORAR

    public GameObject canvas;
    public CanvasGroup canvasAlpha;
    public GameObject nextPanel;
    public float fadeTime;

    private GameObject currentPanel;
    private Coroutine coroutine;

    public void StartTransition()
    {
        if (DataHolder.animating == false)
        {
            // Fade out
            canvas.GetComponent<Fade>().coroutine_FT = StartCoroutine(canvas.GetComponent<Fade>().FadeTo(0F, fadeTime));

            coroutine = StartCoroutine(PanelUpdate());
        }
    }

    IEnumerator PanelUpdate()
    {
        while (true)
        {
            // If the fading out is complete
            if (DataHolder.animating == false && canvasAlpha.alpha < 1F)
            {
                // Fade in
                canvas.GetComponent<Fade>().coroutine_FT = StartCoroutine(canvas.GetComponent<Fade>().FadeTo(1F, fadeTime));

                // Rotate the current panel in 90°
                currentPanel.transform.Rotate(90, 0, 0);

                // Activates the next panel
                nextPanel.SetActive(true);

                StopCoroutine(coroutine);
                coroutine = StartCoroutine(PanelDeactivation());
            }

            yield return null;
        }
    }

    IEnumerator PanelDeactivation()
    {
        while (true)
        {
            // If the fading in is complete
            if (DataHolder.animating == false && canvasAlpha.alpha > 0F)
            {
                // Deactivates the current panel
                currentPanel.SetActive(false);

                // Rotates the panel back
                currentPanel.transform.Rotate(-90, 0, 0);

                StopCoroutine(coroutine);
            }

            yield return null;
        }
    }

    private void Start()
    {
        currentPanel = gameObject.transform.parent.gameObject;
    }
}