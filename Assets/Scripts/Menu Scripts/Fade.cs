using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour
{
    #region Public Variables
    #region Editor Acessible
    // Tempo de fading
    public float fadeTime;
    #endregion

    // Coroutine do operação de fading
    [System.NonSerialized]
    public Coroutine coroutine_FT;
    #endregion

    #region Private Variables
    // Alfa do canvas principal
    private CanvasGroup canvasAlpha;

    // Acesso ao Script Manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Acessa o CanvasGroup do canvas principal
        canvasAlpha = gameObject.GetComponent<CanvasGroup>();

        // Inicializa o alfa como 0
        canvasAlpha.alpha = 0F;

        // Inicializa a operação de fade in
        coroutine_FT = StartCoroutine(FadeTo(1F, fadeTime));
    }
    #endregion

    #region Fade To
    public IEnumerator FadeTo(float value, float time)
    {
        // Define que uma animação começou
        scriptManager.animating = true;

        // Operação de fading
        for (float i = 0; i <= 1F; i += Time.deltaTime / time)
        {
            // Condição de convergência de +-0.05
            if (Mathf.Abs(canvasAlpha.alpha - value) < 0.05F)
            {
                canvasAlpha.alpha = value;
                i = 2;
            }

            // Interpola o alfa
            canvasAlpha.alpha = Mathf.Lerp(canvasAlpha.alpha, value, i);

            yield return null;
        }

        // Define que a animação terminou
        scriptManager.animating = false;
        StopCoroutine(coroutine_FT);
    }
    #endregion
}