using UnityEngine;
using UnityEngine.UI;

public class CustomHeight : MonoBehaviour
{
    #region Public Variables
    public float Height
    {
        // Retorna o valor no DataHolder
        get
        {
            return DataHolder.height;
        }

        // Define um valor de 2 a 200
        set
        {
            // Multiplica o valor recebido (0 <-> 1) por 100
            DataHolder.height = Mathf.RoundToInt(value * 100);

            // Limita o valor mínimo a 2
            if (DataHolder.height < 2)
            {
                DataHolder.height = 2;
            }

            // Exibe o valor na barra de customização
            gameObject.GetComponentInChildren<Text>().text = "" + DataHolder.height;
        }
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Inicializa a altura como 2 (0.02 * 100)
        Height = 0.02F;
    }
    #endregion
}