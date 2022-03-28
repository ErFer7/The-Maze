using UnityEngine;
using UnityEngine.UI;

public class CustomWidth : MonoBehaviour
{
    #region Public Variables
    public float Width
    {
        // Retorna o valor no Script manager
        get
        {
            return scriptManager.width;
        }

        // Define um valor de 2 a 200
        set
        {
            // Multiplica o valor recebido (0 <-> 1) por 100
            scriptManager.width = Mathf.RoundToInt(value * 100);

            // Limita o valor mínimo a 2
            if (scriptManager.width < 2)
            {
                scriptManager.width = 2;
            }

            // Exibe o valor na barra de customização
            gameObject.GetComponentInChildren<Text>().text = "" + scriptManager.width;
        }
    }
    #endregion

    #region Private Variables
    // Acesso ao Script Manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Inicializa a largura como 2 (0.02 * 100)
        Width = 0.02F;
    }
    #endregion
}