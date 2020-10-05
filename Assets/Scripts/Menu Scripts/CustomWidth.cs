using UnityEngine;
using UnityEngine.UI;

public class CustomWidth : MonoBehaviour
{
    #region Public Variables
    public float Width
    {
        // Retorna o valor no DataHolder
        get
        {
            return DataHolder.width;
        }

        // Define um valor de 2 a 200
        set
        {
            // Multiplica o valor recebido (0 <-> 1) por 100
            DataHolder.width = Mathf.RoundToInt(value * 100);

            // Limita o valor mínimo a 2
            if (DataHolder.width < 2)
            {
                DataHolder.width = 2;
            }

            // Exibe o valor na barra de customização
            gameObject.GetComponentInChildren<Text>().text = "" + DataHolder.width;
        }
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Inicializa a largura como 2 (0.02 * 100)
        Width = 0.02F;
    }
    #endregion
}