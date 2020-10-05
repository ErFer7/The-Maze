using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public GameObject menu;
    public MusicManager musicManager;

    public void Quit()
    {
        Application.Quit();
    }

    public void Continue()
    {
        // Hides the message box
        if (DataHolder.animating == false)
        {
            if (musicManager.publicCoroutine_2 != null)
            {
                StopCoroutine(musicManager.publicCoroutine_2);
            }
            musicManager.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 22000;
            musicManager.gameObject.GetComponent<AudioLowPassFilter>().enabled = false;

            menu.GetComponent<MenuExit>().coroutine = StartCoroutine(menu.GetComponent<MenuExit>().MessageBoxAnimation(new Vector2(0, -485), new Color(1,1,1,0), 0.25F));
        }
    }
}