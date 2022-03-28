using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeImage : MonoBehaviour
{
    #region Private Variables
    // Acesso a tela preta
    private Image blackScreenImage;

    // Acesso ao script manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();
    }
    #endregion

    #region Animation
    public IEnumerator FadeImageTo(float value, float time)
    {
        // Acessa a imagem da tela preta
        blackScreenImage = gameObject.GetComponent<Image>();

        // Se a operação for de fade in o alfa é definido como 1
        if (value == 1F)
        {
            blackScreenImage.canvasRenderer.SetAlpha(0.01F);
        }
        // Se a operação for de fade out o alfa é definido como 0
        else
        {
            blackScreenImage.canvasRenderer.SetAlpha(1F);
        }

        // Cross fade
        blackScreenImage.CrossFadeAlpha(value, time, false);

        // Checa se o cross fade acabou
        while (true)
        {
            // Se o alfa é igual a +- 0.05 o valor
            if (Mathf.Abs(blackScreenImage.canvasRenderer.GetAlpha() - value) < 0.05F)
            {
                // Para o cross fade
                blackScreenImage.CrossFadeAlpha(value, 0, false);

                // Estados de saída (Explicados em LoadinControl.cs)
                switch (scriptManager.loadingStage)
                {
                    case -1:
                        scriptManager.loadingStage = 1;
                        break;
                    case -2:
                        scriptManager.loadingStage = 5;
                        break;
                    case -3:
                        scriptManager.loadingStage = 6;
                        break;
                    default:
                        break;
                }
            }

            yield return null;
        }
    }
    #endregion
}