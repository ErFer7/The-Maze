using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    #region Public Variables
    #region Editor Acessible
    // Acesso á camera
    public Camera mainCamera;

    // Acesso ao jogador
    public GameObject player;

    // Acesso a tela de saída
    public GameObject exitScreen;

    // Acesso ao botão de pausa
    public Button pauseButton;
    #endregion

    // Define que o jogador está saindo do labirinto (para acesso externo)
    [HideInInspector]
    public bool exiting;
    #endregion

    #region Private Variables
    // Direção do jogador para a saída
    private Vector2 direction;

    // Escala final do jogador
    private Vector3 finalScale;

    // Taxa de redução da escala
    private Vector3 scaleReductionRate;

    // Coroutine da animação
    private Coroutine coroutine;

    // AudioSource da saída
    private AudioSource audioSource;

    // Acesso ao script manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Acessa o AudioSource da saída
        audioSource = GetComponent<AudioSource>();

        // Inicializa as escalas
        finalScale = new Vector3(0, 0, 1);
        scaleReductionRate = new Vector3(0.016F, 0.016F, 0);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Definie que o jogador está saindo
        exiting = true;

        // Toca o som da saída
        if (scriptManager.sound)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        // Desabilita o botão de pausa
        pauseButton.interactable = false;

        // Trava o jogador
        scriptManager.playerCanMove = false;

        // Inicia a animação da saída
        coroutine = StartCoroutine(ExitingAnimation());
    }
    #endregion

    #region Animation
    IEnumerator ExitingAnimation()
    {
        // Condição inicial
        scriptManager.animating = true;

        // Define a saída como o novo alvo da camera
        mainCamera.GetComponent<SmoothFollow>().target = transform;

        while (true)
        {
            // Move o jogador para a saída
            direction = transform.position - player.transform.position;
            player.transform.Translate(direction * Time.deltaTime * 10);

            // Diminui o tamanho da calda do jogador
            if (player.GetComponent<TrailRenderer>().widthMultiplier > 0)
            {
                player.GetComponent<TrailRenderer>().widthMultiplier -= 0.016F;
            }

            // Reduz o tamanho do jogador até a escala final
            if (player.transform.localScale.x > finalScale.x)
            {
                player.transform.localScale -= scaleReductionRate;
            }
            else
            {
                // Finaliza a animação e ativa a tela de saída
                scriptManager.animating = false;
                exitScreen.SetActive(true);
                StopCoroutine(coroutine);
            }

            yield return null;
        }
    }
    #endregion
}