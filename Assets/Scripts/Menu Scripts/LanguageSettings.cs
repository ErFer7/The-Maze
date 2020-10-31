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
        English_US,
        Spanish_419,
        Portuguese_BR
    }
    #endregion

    #region Methods
    public void ChangeCurrentlanguage()
    {
        // Chama a função de mudança de linguagem com base na preferência do jogador
        switch (language)
        {
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
            default:
                break;
        }
    }
    #endregion
}
