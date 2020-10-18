using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuExit : MonoBehaviour
{
    #region Public Variables
    #region Editor Acessible
    // Objeto da caixa de texto
    public GameObject exit;

    // Tempo da animação
    public float animationTime;

    // Cores (O canal alfa é o único usado)
    public Color colorUp;
    public Color colorDown;

    // Fundo
    public Image exitBackGround;

    // Caixa de texto
    public RectTransform exitMessageBox;

    // Acesso ao Music Manager
    public MusicManager musicManager;
    #endregion

    // Coroutine da animação
    [System.NonSerialized]
    public Coroutine coroutine_MBA;
    #endregion

    #region Private Variables
    // Posições
    private Vector2 targetPositionUp;
    private Vector2 targetPositionDown;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Inicializa as posições
        targetPositionUp = new Vector2(0, 0);
        targetPositionDown = new Vector2(0, -485);
    }

    void Update()
    {
        // Se o usuário pressina ESC ou o botão de retornar no smartphone
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Se não há nenhuma animação tocando
            if (!DataHolder.animating)
            {
                // Ativa a caixa de texto
                if (exitMessageBox.anchoredPosition.y < targetPositionUp.y - 10)
                {
                    // Abafa a música usando o filtro passa-baixa
                    musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = true;
                    if (musicManager.publicCoroutine_LPFF != null)
                    {
                        StopCoroutine(musicManager.publicCoroutine_LPFF);
                    }
                    musicManager.publicCoroutine_LPFF = StartCoroutine(musicManager.LowPassFilterFade(200F, 0.65F));

                    // Move a caixa de texto para a tela
                    coroutine_MBA = StartCoroutine(MessageBoxAnimation(targetPositionUp, colorUp, animationTime));
                }
                // Desativa a caixa de texto
                else
                {
                    // Toca o áudio da caixa de texto
                    exitMessageBox.GetComponent<AudioSource>().Play();

                    // Abafa a música
                    if (musicManager.publicCoroutine_LPFF != null)
                    {
                        StopCoroutine(musicManager.publicCoroutine_LPFF);
                    }
                    musicManager.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 22000;

                    // Desativa o filtro
                    musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = false;

                    // Move a caixa de texto para baixo da tela
                    coroutine_MBA = StartCoroutine(MessageBoxAnimation(targetPositionDown, colorDown, animationTime));
                }
            }
        }
    }
    #endregion

    #region MessageBoxAnimation
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
        StopCoroutine(coroutine_MBA);
    }
    #endregion
}