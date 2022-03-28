using UnityEngine;

public class QuitGame : MonoBehaviour
{
    #region Public Variables
    // Objeto do painel do menu
    public GameObject menu;

    // Objeto do Music Manager
    public MusicManager musicManager;
    #endregion

    #region Private Variables
    // Acesso ao Script manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();
    }
    #endregion

    #region Methods
    public void Quit()
    {
        // Sai do aplicativo
        Application.Quit();
    }

    public void Continue()
    {
        // Esconde a caixa de texto
        if (!scriptManager.animating)
        {
            // Retorna o áudio ao normal
            if (musicManager.publicCoroutine_LPFF != null)
            {
                StopCoroutine(musicManager.publicCoroutine_LPFF);
            }
            musicManager.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 22000;
            musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = false;

            // Move a caixa de texto para baixo
            menu.GetComponent<MenuExit>().coroutine_MBA = StartCoroutine(menu.GetComponent<MenuExit>().MessageBoxAnimation(new Vector2(0, -485), new Color(1,1,1,0), 0.25F));
        }
    }
    #endregion
}