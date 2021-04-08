using UnityEngine;
using UnityEngine.UI;

public class CustomSeed : MonoBehaviour
{
    #region Public Variables
    // Acesso ao leitor
    public GameObject placeHolder;
    // Acesso ao texto
    public Text text;
    // Acesso ao botão
    public Button done;
    #endregion

    #region Private Variables
    // Acesso ao Script Manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();
    }
    #endregion

    #region Custom Seed
    public void SetSeed()
    {
        // Ignora o string se ele for vazio
        if (gameObject.GetComponent<InputField>().text == "")
        {
            scriptManager.useSavedSeed = false;
        }
        else
        {
            scriptManager.useSavedSeed = true;

            // Se o string não puder ser convertido em números inteiros então uma seed aleatória será usada
            if (!int.TryParse(gameObject.GetComponent<InputField>().text, out scriptManager.seed))
            {
                scriptManager.seed = GetDeterministicHashCode(gameObject.GetComponent<InputField>().text);
            }
        }
    }
    #endregion

    #region Utilities
    // Código hash deterministico. Retorna sempre o mesmo valor para cada string
    private int GetDeterministicHashCode(string str)
    {
        unchecked
        {
            int hash1 = (5381 << 16) + 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1)
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }
    #endregion
}