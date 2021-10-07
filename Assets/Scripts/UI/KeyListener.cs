using UnityEngine;
using UnityEngine.UI;

// RENOMEAR ARQUIVO

public class MobileReturn : MonoBehaviour
{
    #region Unity Methods
    private void Update()
    {
        // Se o usuário pressior ESC ou o botão de retornar no smartphone o botão de retornar é invocado
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
    #endregion
}
