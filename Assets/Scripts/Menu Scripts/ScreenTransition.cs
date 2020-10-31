using UnityEngine;
using System.Collections;

public class ScreenTransition : MonoBehaviour
{
    #region Public Variables
    // Variáveis para o canvas
    public GameObject canvas;
    public CanvasGroup canvasAlpha;

    // Próximo painel
    public GameObject nextPanel;

    // Tempo da transição
    public float fadeTime;
    #endregion

    #region Private Variables
    // Painel atual
    private GameObject currentPanel;

    // Coroutine da atualização e desativação dos paineis
    private Coroutine coroutine_PU_PD;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Determina o painel atual
        currentPanel = gameObject.transform.parent.gameObject;
    }
    #endregion

    #region Methods
    public void StartTransition()
    {
        if (!DataHolder.animating)
        {
            // Animação de fade out
            canvas.GetComponent<Fade>().coroutine_FT = StartCoroutine(canvas.GetComponent<Fade>().FadeTo(0F, fadeTime));

            // Inicia a atualização do painel
            coroutine_PU_PD = StartCoroutine(PanelUpdate());
        }
    }

    IEnumerator PanelUpdate()
    {
        while (true)
        {
            // Se a animação de fade out acabou
            if (!DataHolder.animating && canvasAlpha.alpha < 1F)
            {
                // Fade in
                canvas.GetComponent<Fade>().coroutine_FT = StartCoroutine(canvas.GetComponent<Fade>().FadeTo(1F, fadeTime));

                // Rotaciona o painel atual em 90°
                currentPanel.transform.Rotate(90, 0, 0);

                // Ativa o próximo painel
                nextPanel.SetActive(true);

                StopCoroutine(coroutine_PU_PD);
                coroutine_PU_PD = StartCoroutine(PanelDeactivation());
            }

            yield return null;
        }
    }

    IEnumerator PanelDeactivation()
    {
        while (true)
        {
            // Se a animação de fade in acabou
            if (!DataHolder.animating && canvasAlpha.alpha > 0F)
            {
                // Desativa o painel atual
                currentPanel.SetActive(false);

                // Rotaciona o painel para a sua posição original
                currentPanel.transform.Rotate(-90, 0, 0);

                StopCoroutine(coroutine_PU_PD);
            }

            yield return null;
        }
    }
    #endregion
}