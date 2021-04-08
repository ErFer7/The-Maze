using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    #region Public Variables
    // Botão da configuração da música
    public Toggle musicToggle;

    // Coroutines
    public Coroutine publicCoroutine_MF;
    public Coroutine publicCoroutine_LPFF;
    #endregion

    #region Private Variables
    // Fonte de áudio da música
    private AudioSource audioSource;

    // Coroutine do Music Player
    private Coroutine coroutine_MP;

    // Filtro passa-baixa
    private AudioLowPassFilter lowPassFilter;

    // Acesso ao Script manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Acessa os objetos da fonte de áudio e filtro passa-baixa
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();

        // Lê as configurações
        switch (PlayerPrefs.GetInt("Music", -1))
        {
            // Atualiza os botões e o sistema
            case 0:
                musicToggle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
                scriptManager.music = false;
                musicToggle.isOn = false;
                break;
            // Atualiza o sistema
            case 1:
                scriptManager.music = true;
                break;
            default:
                scriptManager.music = true;
                break;
        }

        musicToggle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);

        // Toca a música caso o ela esteja definida como ativa
        if (scriptManager.music)
        {
            coroutine_MP = StartCoroutine(MusicPlayer());
        }
    }
    #endregion

    #region Music Core
    private IEnumerator MusicPlayer()
    {   
        // Espera até o fim da animação para tocar a música
        while (scriptManager.animating)
        {
            yield return null;
        }

        audioSource.Play();

        yield return null;
    }

    public void MusicSwitch()
    {   
        // Se o botão de música é ativado
        if (musicToggle.isOn)
        {
            // Define a música como ativa no sistema
            scriptManager.music = true;
            PlayerPrefs.SetInt("Music", 1);

            // Toca a música
            if (coroutine_MP == null)
            {
                coroutine_MP = StartCoroutine(MusicPlayer());
            }
        }
        else
        {
            // Define a música como inativa no sistema
            scriptManager.music = false;
            PlayerPrefs.SetInt("Music", 0);

            // Para a música
            audioSource.Stop();
            if (coroutine_MP != null)
            {
                StopCoroutine(coroutine_MP);
                coroutine_MP = null;
            }
        }
    }
    #endregion

    #region Music Animation
    public IEnumerator MusicFade(float value, float time)
    {
        // Operação de fade no volume
        for (float i = 0; i <= 1F; i += Time.deltaTime / time)
        {
            if (Mathf.Abs(audioSource.volume - value) < 0.05F)
            {
                audioSource.volume = value;
                i = 2;
            }

            // Interpola linearmente o volume entre os intervalos definidos pela diferença de tempo entre frames
            audioSource.volume = Mathf.Lerp(audioSource.volume, value, i);

            yield return null;
        }
    }

    public IEnumerator LowPassFilterFade(float value, float fadeTime)
    {
        // Operação de fade no filtro passa baixa
        for (float i = 0; i <= 1F; i += Time.deltaTime / fadeTime)
        {
            if (Mathf.Abs(lowPassFilter.cutoffFrequency - value) < 250F)
            {
                lowPassFilter.cutoffFrequency = value;
                i = 2;
            }

            // Interpola linearmente o a frequência base entre os intervalos definidos pela diferença de tempo entre frames
            lowPassFilter.cutoffFrequency = Mathf.Lerp(lowPassFilter.cutoffFrequency, value, i);

            yield return null;
        }
    }
    #endregion
}