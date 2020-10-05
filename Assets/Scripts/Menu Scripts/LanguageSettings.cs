using UnityEngine;

public class LanguageSettings : MonoBehaviour
{
    #region Public Variables
    // Linguagem
    public Language language;
    
    // Acesso ao Localization Manager
    public LocalizationManager localizationManager;

    // Códigos culturais das linguagens
    public enum Language
    {
        Afrikaans,
        English_US,
        Spanish_419,
        Portuguese_BR,
        German_DE,
        Chinese_CN,
        Chinese_HK,
        Russian
    }
    #endregion

    #region Methods
    public void ChangeCurrentlanguage()
    {
        // Chama a função de mudança de linguagem com base na preferência do jogador
        switch (language)
        {
            case Language.Afrikaans:
                localizationManager.SetLanguage("af");
                PlayerPrefs.SetString("Culture", "af");
                break;
            case Language.English_US:
                localizationManager.SetLanguage("en-US");
                PlayerPrefs.SetString("Culture", "en-US");
                break;
            case Language.Spanish_419:
                localizationManager.SetLanguage("es-419");
                PlayerPrefs.SetString("Culture", "es-419");
                break;
            case Language.Portuguese_BR:
                localizationManager.SetLanguage("pt-BR");
                PlayerPrefs.SetString("Culture", "pt-BR");
                break;
            case Language.German_DE:
                localizationManager.SetLanguage("de-DE");
                PlayerPrefs.SetString("Culture", "de-DE");
                break;
            case Language.Chinese_CN:
                localizationManager.SetLanguage("zn-Hans");
                PlayerPrefs.SetString("Culture", "zn-Hans");
                break;
            case Language.Chinese_HK:
                localizationManager.SetLanguage("zn-Hant");
                PlayerPrefs.SetString("Culture", "zn-Hant");
                break;
            case Language.Russian:
                localizationManager.SetLanguage("ru-RU");
                PlayerPrefs.SetString("Culture", "ru-RU");
                break;
            default:
                break;
        }
    }
    #endregion
}
