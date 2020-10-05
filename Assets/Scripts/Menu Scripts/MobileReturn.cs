using UnityEngine;
using UnityEngine.UI;

public class MobileReturn : MonoBehaviour
{
    private void Update()
    {
        // If the user press "esc" or the "return button on smartphones", the return button will be activated
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }
}
