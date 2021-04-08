using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathAnimation : MonoBehaviour
{
    #region Public Variables
    // Define quando a animação terminou
    [System.NonSerialized]
    public bool finished;
    #endregion

    #region Private Variables
    // Acesso ao Music Player
    private GameObject musicPlayer;

    // Acesso a tela branca
    private Image whiteScreenImage;

    // Acesso ao script manager
    private ScriptManager scriptManager;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Acessa o script manager
        scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();

        // Acessa o Music Player
        musicPlayer = GameObject.FindWithTag("GameplayMusicPlayer");

        // Se o som está desabilitado
        if (!scriptManager.sound)
        {
            // Desabilita o som da animação
            GetComponent<AudioSource>().enabled = false;
        }

        // Se a música está habilitada
        if (scriptManager.music)
        {
            // Para o sistema de música
            musicPlayer.GetComponent<MonoBehaviour>().StopCoroutine(musicPlayer.GetComponent<MusicPlayer>().coroutine_PM);

            // Para a música
            if (musicPlayer.GetComponent<AudioSource>().isPlaying)
            {
                musicPlayer.GetComponent<AudioSource>().Stop();
            }
        }
    }
    #endregion

    #region Animation
    public IEnumerator Flash()
    {
        // Condição inicial
        finished = false;

        // Acessa a tela branca
        whiteScreenImage = gameObject.GetComponent<Image>();

        // Reseta o alfa
        whiteScreenImage.canvasRenderer.SetAlpha(0.01F);

        // Operação de Cross Fade no alfa até 1 em 0.25 segundos (Tempo não escalado)
        whiteScreenImage.CrossFadeAlpha(1F, 0.25F, false);

        // Checa se o fade in acabou
        while (true)
        {
            // Se o alfa é maior que 0.95 o fade in é finalizado
            if (whiteScreenImage.canvasRenderer.GetAlpha() > 0.95F)
            {
                whiteScreenImage.CrossFadeAlpha(1F, 0, false);
                break;
            }

            yield return null;
        }

        // Operação de Cross Fade no alfa até 0 me 0.5 segundos (Tempo não escalado)
        whiteScreenImage.CrossFadeAlpha(0F, 0.5F, false);

        // Checa se o fade out acabou
        while (true)
        {
            // Se o alfa é menor que 0.05 o fade out é finalizado e a animação é dada como finalizada
            if (whiteScreenImage.canvasRenderer.GetAlpha() < 0.05F)
            {
                whiteScreenImage.CrossFadeAlpha(0, 0, false);
                finished = true;
                break;
            }

            yield return null;
        }
    }
    #endregion
}