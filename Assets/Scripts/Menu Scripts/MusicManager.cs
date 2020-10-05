using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public Toggle musicToggle;
    public Coroutine publicCoroutine;
    public Coroutine publicCoroutine_2;

    private AudioSource audioSource;
    private Coroutine coroutine;
    private AudioLowPassFilter lowPassFilter;

    private IEnumerator MusicPlayer()
    {
        while (DataHolder.animating == true)
        {
            yield return null;
        }

        audioSource.Play();

        yield return null;
    }

    public void MusicSwitch()
    {
        if (musicToggle.isOn == true)
        {
            DataHolder.music = true;
            PlayerPrefs.SetInt("Music", 1);

            if (coroutine == null)
            {
                coroutine = StartCoroutine(MusicPlayer());
            }
        }
        else
        {
            DataHolder.music = false;
            PlayerPrefs.SetInt("Music", 0);
            audioSource.Stop();

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }

    public IEnumerator musicFade(float value, float time)
    {
        for (float i = 0; i <= 1F; i += Time.deltaTime / time)
        {
            if (Mathf.Abs(audioSource.volume - value) < 0.05F)
            {
                audioSource.volume = value;
                i = 2;
            }

            audioSource.volume = Mathf.Lerp(audioSource.volume, value, i);

            yield return null;
        }
    }

    public IEnumerator lowPassFilterFade(float value, float fadeTime)
    {
        for (float i = 0; i <= 1F; i += Time.deltaTime / fadeTime)
        {
            if (Mathf.Abs(lowPassFilter.cutoffFrequency - value) < 250F)
            {
                lowPassFilter.cutoffFrequency = value;
                i = 2;
            }

            lowPassFilter.cutoffFrequency = Mathf.Lerp(lowPassFilter.cutoffFrequency, value, i);

            yield return null;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lowPassFilter = GetComponent<AudioLowPassFilter>();

        switch (PlayerPrefs.GetInt("Music", -1))
        {
            case 0:
                musicToggle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
                DataHolder.music = false;
                musicToggle.isOn = false;
                break;
            case 1:
                DataHolder.music = true;
                break;
            default:
                DataHolder.music = true;
                break;
        }

        musicToggle.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);

        if (DataHolder.music == true)
        {
            coroutine = StartCoroutine(MusicPlayer());
        }
    }
}