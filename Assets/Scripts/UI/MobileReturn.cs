using UnityEngine;
using UnityEngine.UI;

public class KeyListener : MonoBehaviour
{
    public KeyCode triggerKey;

    #region Unity Methods
    private void Update()
    {
        // Se o usuário pressior a tecla de gatilho o botão é invocado
        if (Input.GetKeyDown(triggerKey))
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
    #endregion
}
