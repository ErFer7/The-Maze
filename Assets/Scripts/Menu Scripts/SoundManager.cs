using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // REFATORAR

    public Toggle soundToggle;
    public AudioSource startButtonAudio;
    public AudioSource playGamesButtonAudio;
    public AudioSource leaderboardButtonAudio;
    public AudioSource achievementsButtonAudio;
    public AudioSource infoButtonAudio;
    public AudioSource settingsButtonAudio;
    public AudioSource menuMessageBoxAudio;
    public AudioSource yesButtonAudio;
    public AudioSource noButtonAudio;
    public AudioSource playGamesMessageBoxAudio;
    public AudioSource okButtonAudio;
    public AudioSource classicButtonAudio;
    public AudioSource timeButtonAudio;
    public AudioSource darkButtonAudio;
    public AudioSource customButtonAudio;
    public AudioSource returnButtonAudio_0;
    public AudioSource startMessageBoxAudio;
    public AudioSource continueButtonAudio;
    public AudioSource newGameButtonAudio;
    public AudioSource returnButtonAudio_1;
    public AudioSource languageButtonAudio;
    public AudioSource soundToggleAudio;
    public AudioSource musicToggleAudio;
    public AudioSource returnButtonAudio_2;
    public AudioSource widthScrollbarAudio;
    public AudioSource heightScrollbarAudio;
    public AudioSource seedAudio;
    public AudioSource seedButtonAudio;
    public AudioSource doneButtonAudio;
    public AudioSource returnButtonAudio_3;
    public AudioSource returnButtonAudio_4;
    public AudioSource af_ButtonAudio;
    public AudioSource enUSButtonAudio;
    public AudioSource es419_ButtonAudio;
    public AudioSource ptBR_ButtonAudio;
    public AudioSource deDE_ButtonAudio;
    public AudioSource cnCN_ButtonAudio;
    public AudioSource cnHK_ButtonAudio;
    public AudioSource ruRU_ButtonAudio;
    public AudioSource returnButtonAudio_5;

    public void SoundSwitch()
    {
        // Activates the sound
        if (soundToggle.isOn == true)
        {
            DataHolder.sound = true;
            PlayerPrefs.SetInt("Sound", 1);

            startButtonAudio.volume = 1;
            playGamesButtonAudio.volume = 1;
            leaderboardButtonAudio.volume = 1;
            achievementsButtonAudio.volume = 1;
            infoButtonAudio.volume = 1;
            settingsButtonAudio.volume = 1;
            menuMessageBoxAudio.volume = 1;
            yesButtonAudio.volume = 1;
            noButtonAudio.volume = 1;
            playGamesMessageBoxAudio.volume = 1;
            okButtonAudio.volume = 1;
            classicButtonAudio.volume = 1;
            timeButtonAudio.volume = 1;
            darkButtonAudio.volume = 1;
            customButtonAudio.volume = 1;
            returnButtonAudio_0.volume = 1;
            startMessageBoxAudio.volume = 1;
            continueButtonAudio.volume = 1;
            newGameButtonAudio.volume = 1;
            returnButtonAudio_1.volume = 1;
            languageButtonAudio.volume = 1;
            soundToggleAudio.volume = 1;
            musicToggleAudio.volume = 1;
            returnButtonAudio_2.volume = 1;
            widthScrollbarAudio.volume = 1;
            heightScrollbarAudio.volume = 1;
            seedAudio.volume = 1;
            seedButtonAudio.volume = 1;
            doneButtonAudio.volume = 1;
            returnButtonAudio_3.volume = 1;
            returnButtonAudio_4.volume = 1;
            af_ButtonAudio.volume = 1;
            enUSButtonAudio.volume = 1;
            es419_ButtonAudio.volume = 1;
            ptBR_ButtonAudio.volume = 1;
            deDE_ButtonAudio.volume = 1;
            cnCN_ButtonAudio.volume = 1;
            cnHK_ButtonAudio.volume = 1;
            ruRU_ButtonAudio.volume = 1;
            returnButtonAudio_5.volume = 1;
        }
        // Deactivates the sound
        else
        {
            DataHolder.sound = false;
            PlayerPrefs.SetInt("Sound", 0);
            
            startButtonAudio.volume = 0;
            playGamesButtonAudio.volume = 0;
            leaderboardButtonAudio.volume = 0;
            achievementsButtonAudio.volume = 0;
            infoButtonAudio.volume = 0;
            settingsButtonAudio.volume = 0;
            menuMessageBoxAudio.volume = 0;
            yesButtonAudio.volume = 0;
            noButtonAudio.volume = 0;
            playGamesMessageBoxAudio.volume = 0;
            okButtonAudio.volume = 0;
            classicButtonAudio.volume = 0;
            timeButtonAudio.volume = 0;
            darkButtonAudio.volume = 0;
            customButtonAudio.volume = 0;
            returnButtonAudio_0.volume = 0;
            startMessageBoxAudio.volume = 0;
            continueButtonAudio.volume = 0;
            newGameButtonAudio.volume = 0;
            returnButtonAudio_1.volume = 0;
            languageButtonAudio.volume = 0;
            soundToggleAudio.volume = 0;
            musicToggleAudio.volume = 0;
            returnButtonAudio_2.volume = 0;
            widthScrollbarAudio.volume = 0;
            heightScrollbarAudio.volume = 0;
            seedAudio.volume = 0;
            seedButtonAudio.volume = 0;
            doneButtonAudio.volume = 0;
            returnButtonAudio_3.volume = 0;
            returnButtonAudio_4.volume = 0;
            af_ButtonAudio.volume = 0;
            enUSButtonAudio.volume = 0;
            es419_ButtonAudio.volume = 0;
            ptBR_ButtonAudio.volume = 0;
            deDE_ButtonAudio.volume = 0;
            cnCN_ButtonAudio.volume = 0;
            cnHK_ButtonAudio.volume = 0;
            ruRU_ButtonAudio.volume = 0;
            returnButtonAudio_5.volume = 0;
        }
    }

    private void Start()
    {
        switch (PlayerPrefs.GetInt("Sound", -1))
        {
            case 0:
                soundToggle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
                soundToggle.isOn = false;
                DataHolder.sound = false;
                break;
            case 1:
                DataHolder.sound = true;
                break;
            default:
                DataHolder.sound = true;
                break;
        }

        soundToggle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
    }
}