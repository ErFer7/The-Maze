using System.Collections;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    #region Public Variables
    // Configurações da animação
    public bool spin;
    public float rotationSpeed;
    public float updateTime;
    #endregion

    #region Private Variables
    // Taxa de atualização da animação
    private WaitForSeconds waitTime;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Inicializa a taxa de atualização da animação
        waitTime = new WaitForSeconds(updateTime);
    }

    private void OnEnable()
    {
        // Inicia a animação
        StartCoroutine(SpinAnimation());
    }
    #endregion

    #region Methods
    IEnumerator SpinAnimation ()
    {
        while (spin)
        {
            // Rotaciona o objeto na velocidade especificada
            transform.Rotate(0F, 0F, rotationSpeed);

            yield return waitTime;
        }
    }
    #endregion
}