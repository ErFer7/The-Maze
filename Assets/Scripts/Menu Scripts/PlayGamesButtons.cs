using UnityEngine;
using UnityEngine.UI;

public class PlayGamesButtons : MonoBehaviour
{
    [System.NonSerialized]
    public bool freeToProceed;

    private PlayGamesManager playGamesManager;
    private Event keyPressed;

    public void Interact()
    {
        if (!DataHolder.animating)
        {
            if (PlayerPrefs.GetInt("Authenticated", 0) == 0)
            {
                playGamesManager.LogIn();
            }
            else
            {
                playGamesManager.SignOut();
            }
        }
    }

    public void OkButton()
    {
        if (freeToProceed)
        {
            playGamesManager.MessageBoxControl(PlayGamesManager.MessageBoxState.OkButtonPressed);
        }
    }

    public void ShowAchievements()
    {
        playGamesManager.ShowAchievements();
    }

    public void ShowLeaderboards()
    {
        playGamesManager.ShowLeaderboard();
    }

    private void Start()
    {
        playGamesManager = GameObject.FindWithTag("PlayGamesManager").GetComponent<PlayGamesManager>();

        if (gameObject.name == "Achievements Button" && PlayerPrefs.GetInt("Authenticated", 0) == 0)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
        else if (gameObject.name == "Leaderboards Button" && PlayerPrefs.GetInt("Authenticated", 0) == 0)
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    private void OnGUI()
    {
        keyPressed = Event.current;
        if (keyPressed.keyCode == KeyCode.Escape && freeToProceed && playGamesManager.playGamesMessageBoxTransform.anchoredPosition.y > -1F)
        {
            playGamesManager.MessageBoxControl(PlayGamesManager.MessageBoxState.OkButtonPressed);
        }
    }
}
