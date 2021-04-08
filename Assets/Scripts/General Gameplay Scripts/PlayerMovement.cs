using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region Public Variables
    #region Editor Acessible
    // Velocidade do jogador
    public float speed;
    
    // Raio do joystick
    public float radius;

    // Acesso a tela de pausa
    public GameObject pausingScreen;

    // Acesso aos elementos no joystick
    public GameObject joystickBase;
    public GameObject joystickButton;
    
    // Tempo para a operação de fading do joystick
    public float joystickFadeTime;

    // Acesso a GUI
    public RectTransform GUI_rect;

    // Acesso ao botão de pausa
    public Button pauseButton;
    #endregion

    // Distância percorrida
    [System.NonSerialized]
    public float distanceTravelled;
    #endregion

    #region Private Variables
    // Rigidbody2D do jogador
    private Rigidbody2D player;

    // Estado do toque na tela
    private bool touchStart;

    // Vetores do joystick
    private Vector2 movementCenter;
    private Vector2 controlPoint;
    private Vector2 offSet;
    private Vector2 direction;
    private Vector2 lastPosition;
    private Vector2 setupPosition;
    private Vector2 joystickPosition;

    // Coroutines
    private Coroutine coroutine_FT;
    private Coroutine coroutine_JC;

    // Canvas Group do joystick
    private CanvasGroup joystickAlpha;

    // Timer da desativação
    private float deactivationTimer;

    // Estados do joystick
    private bool active;
    private bool deactivationComplete;

    // Acesso ao Script manager
    private ScriptManager scriptManager;

    #region DEBUG
    // DEBUG
    private bool debugControl;
    private Vector2 debugUp;
    private Vector2 debugDown;
    private Vector2 debugRight;
    private Vector2 debugLeft;
    #endregion
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Acessa o Rigidbody2D do jogador e o CanvasGroup do joystick
        player = gameObject.GetComponent<Rigidbody2D>();
        joystickAlpha = joystickBase.GetComponent<CanvasGroup>();

        // Define os estados iniciais
        touchStart = false;
        scriptManager.playerCanMove = false;

        // Inicialização para a distância
        setupPosition = new Vector2(-1, -1);
        lastPosition = setupPosition;

        // Usa os controles do debug no editor
        if (Application.isEditor)
        {
            debugControl = true;
        }

        // Direções do movimento no modo debug
        debugUp = new Vector2(0, speed);
        debugDown = new Vector2(0, -speed);
        debugRight = new Vector2(speed, 0);
        debugLeft = new Vector2(-speed, 0);

        // Muda a cor do joystick no modo escuro
        if (scriptManager.dark)
        {
            joystickBase.GetComponent<Image>().color = new Color(0.898F, 0.898F, 0.898F);
            joystickButton.GetComponent<Image>().color = new Color(0.898F, 0.898F, 0.898F);
        }
    }

    private void Update()
    {
        // Se o jogador pode se mover o input é habilitado
        if (scriptManager.playerCanMove)
        {
            GetInputDirection();

            // Debug
            if (debugControl)
            {
                MovePlayer_Debug();
            }
        }
        // Faz a operação de fading out no joystick caso o jogador não possa se mover
        else if (coroutine_JC != null)
        {
            StopCoroutine(coroutine_JC);
            coroutine_JC = null;

            if (coroutine_FT != null)
            {
                StopCoroutine(coroutine_FT);
            }

            coroutine_FT = StartCoroutine(FadeTo(0, joystickFadeTime));
        }
    }

    private void FixedUpdate()
    {
        // Move o jogador
        MovePlayer();

        // Calcula a distância entre as posições caso ela não seja menor que -1
        if (player.position.y > -1)
        {
            if (lastPosition == setupPosition)
            {
                lastPosition = player.position;
            }

            distanceTravelled += Vector2.Distance(player.position, lastPosition);
            lastPosition = player.position;
        }
    }
    #endregion

    #region Movement Controls
    private void GetInputDirection()
    {
        // Pausa o jogo quando o jogador pressiona retornar
        if (Input.GetKeyDown(KeyCode.Escape) && !pausingScreen.activeSelf)
        {
            pauseButton.onClick.Invoke();
        }

        // Quando o jogador está tocando a tela
        if (Input.touchCount > 0)
        {
            // Registra a posição inicial do toque
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                movementCenter = Input.GetTouch(0).position;

                // Ativa a UI do joystick
                joystickBase.SetActive(true);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(GUI_rect, movementCenter, null, out joystickPosition);
                joystickBase.GetComponent<RectTransform>().anchoredPosition = joystickPosition;

                // Ativa as animações da UI do joystick
                if (coroutine_JC == null)
                {
                    coroutine_JC = StartCoroutine(JoystickControl());
                }
            }

            // Registra a posição depois do movimento
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // Registra a posição
                touchStart = true;
                controlPoint = Input.GetTouch(0).position;
            }
            else
            {
                // Continua registrando as posições até que o jogador pare de tocar na tela
                if (Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                    controlPoint = Input.GetTouch(0).position;
                }
                else
                {
                    touchStart = false;
                }
            }
        }
    }

    private void MovePlayer()
    {
        if (touchStart)
        {
            // Calcula o vetor offset que vai do primeiro ponto até o segundo ponto do toque
            offSet = controlPoint - movementCenter;

            // Corta os valores do vetor acima do raio máximo, dando a direção não normalizada do movimento
            direction = Vector2.ClampMagnitude(offSet, radius);

            // Move a UI do joystick
            joystickButton.GetComponent<RectTransform>().anchoredPosition = direction;

            // Normaliza a direção
            direction /= radius;

            // Adiciona velocidade ao jogador na direção calculada
            player.velocity = direction * speed;
        }
    }
    #endregion

    #region UI
    private IEnumerator FadeTo(float value, float time)
    {
        for (float i = 0; i <= 1F; i += Time.deltaTime / time)
        {
            // Para quando o valor do alfa converge para +- 0.05 do valor objetivo
            if (Mathf.Abs(joystickAlpha.alpha - value) < 0.05F)
            {
                joystickAlpha.alpha = value;
                i = 2;

                if (value < 0.05F)
                {
                    deactivationComplete = true;
                }

                StopCoroutine(coroutine_FT);
                coroutine_FT = null;
            }

            // Interpola o alfa
            joystickAlpha.alpha = Mathf.Lerp(joystickAlpha.alpha, value, i);

            yield return null;
        }
    }

    private IEnumerator JoystickControl()
    {
        // Faz a operação de fading in no joystick
        coroutine_FT = StartCoroutine(FadeTo(1, joystickFadeTime));

        // Definições iniciais
        active = true;
        deactivationComplete = false;

        while (!deactivationComplete)
        {
            // Se o jogador não está tocando a tela o timer de desativação da UI do joystick inicia
            if (!touchStart && active)
            {
                deactivationTimer += Time.deltaTime;
            }
            else
            {
                deactivationTimer = 0;
            }

            // Após 1 segundo sem um toque na tela a operação de fading out do joystick inicia
            if (deactivationTimer > 1F)
            {
                active = false;

                if (coroutine_FT != null)
                {
                    StopCoroutine(coroutine_FT);
                }

                coroutine_FT = StartCoroutine(FadeTo(0, joystickFadeTime));
            }

            yield return null;
        }

        // Retorna o botão do joystick para o meio
        joystickButton.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        // Finaliza o processo
        joystickBase.SetActive(false);
        StopCoroutine(coroutine_JC);
        coroutine_JC = null;
    }
    #endregion

    #region DEBUG
    private void MovePlayer_Debug()
    {
        // Move o jogador de acordo com as setas no teclado (DEBUG)

        if (Input.GetKey("up"))
        {
            player.velocity = debugUp;
        }

        if (Input.GetKey("down"))
        {
            player.velocity = debugDown;
        }

        if (Input.GetKey("right"))
        {
            player.velocity = debugRight;
        }

        if (Input.GetKey("left"))
        {
            player.velocity = debugLeft;
        }
    }
    #endregion
}