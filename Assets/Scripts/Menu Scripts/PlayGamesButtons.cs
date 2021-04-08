using UnityEngine;
using UnityEngine.UI;

public class PlayGamesButtons : MonoBehaviour
{
    #region Public Variables
    // Define o estado de espera do botão
    [System.NonSerialized]
    public bool freeToProceed;
    #endregion

    #region Private Variables
    // Objeto que permite a interação com o plugin do Play Games
    private PlayGamesManager playGamesManager;

    // Evento de tecla pressionada
    private Event keyPressed;

    // Acesso ao Script Manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Procura e define o objeto
        playGamesManager = GameObject.FindWithTag("PlayGamesManager").GetComponent<PlayGamesManager>();

        // Faz a diferênciação de qual objeto esse script está anexado
        if (gameObject.name == "Achievements Button" && PlayerPrefs.GetInt("Authenticated", 0) == 0)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else if (gameObject.name == "Leaderboards Button" && PlayerPrefs.GetInt("Authenticated", 0) == 0)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    private void OnGUI()
    {
        // Condições de pressionamente de botão
        keyPressed = Event.current;
        if (keyPressed.keyCode == KeyCode.Escape && freeToProceed && playGamesManager.playGamesMessageBoxTransform.anchoredPosition.y > -1F)
        {
            playGamesManager.MessageBoxControl(PlayGamesManager.MessageBoxState.OkButtonPressed);
        }
    }
    #endregion

    #region Interact
    public void Interact()
    {   
        // Se não há nenhuma animação no momento
        if (!scriptManager.animating)
        {
            // Faz o login caso o jogador não esteja logado, caso ele esteja logado faz o logoff
            if (PlayerPrefs.GetInt("Authenticated", 0) == 0)
            {
                // Login
                playGamesManager.LogIn();
            }
            else
            {   
                // Logoff
                playGamesManager.SignOut();
            }
        }
    }
    #endregion

    #region Buttons
    public void OkButton()
    {
        // Comunica para o controle da caixa de texto que o botão de OK foi pressionado
        if (freeToProceed)
        {
            playGamesManager.MessageBoxControl(PlayGamesManager.MessageBoxState.OkButtonPressed);
        }
    }

    public void ShowAchievements()
    {
        // Exibe conquistas
        playGamesManager.ShowAchievements();
    }

    public void ShowLeaderboards()
    {
        // Exibe placares
        playGamesManager.ShowLeaderboard();
    }
    #endregion
}
