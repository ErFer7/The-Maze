using UnityEngine;
using UnityEngine.UI;

public class CustomHeight : MonoBehaviour
{
    #region Public Variables
    public float Height
    {
        // Retorna o valor no Script manager
        get
        {
            return scriptManager.height;
        }

        // Define um valor de 2 a 200
        set
        {
            // Multiplica o valor recebido (0 <-> 1) por 100
            scriptManager.height = Mathf.RoundToInt(value * 100);

            // Limita o valor mínimo a 2
            if (scriptManager.height < 2)
            {
                scriptManager.height = 2;
            }

            // Exibe o valor na barra de customização
            gameObject.GetComponentInChildren<Text>().text = "" + scriptManager.height;
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

        // Inicializa a altura como 2 (0.02 * 100)
        Height = 0.02F;
    }
    #endregion
}