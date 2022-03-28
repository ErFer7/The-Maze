using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class UIElement : MonoBehaviour
{
    #region Public Variables
    #region Editor Accessible
    // Troca de painéis
    public GameObject nextPanel;
    // Pop Up
    public GameObject popUp;
    public GameObject background;
    public Vector2 targetPosition;
    public float sliderValue
    {
        get { return (float)_sliderValue; }
        set
        {
            int _sliderValue = Mathf.RoundToInt(value * 100);

            if (_sliderValue < 2)
            {
                _sliderValue = 2;
            }

            if (selected == 2)
            {
                uiManager.UpdateMazeSize(width: _sliderValue);
            }
            else if (selected == 3)
            {
                uiManager.UpdateMazeSize(height: _sliderValue);
            }

            sliderText.text = _sliderValue.ToString();
        }
    }
    public Text sliderText;
    public InputField inputField;
    public ScriptManager.GameMode gameMode;
    public int selectedMode;
    #endregion

    [HideInInspector]
    public int selected;
    #endregion

    #region Private Variables
    // Atributo para o acesso ao UIManager
    private UIManager uiManager;
    private int _sliderValue;
    #endregion

    #region Unity Methods
    private void Start()
    {
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
    }
    #endregion

    #region Methods
    public void ChangePanel()
    {
        if (uiManager.uiState != UIManager.UIState.InTransition)
        {
            uiManager.PanelTransition(nextPanel);
        }
    }

    public void OpenPopUp()
    {
        if (uiManager.uiState != UIManager.UIState.InTransition)
        {
            uiManager.PopUpAction(popUp, background, targetPosition, true);
        }
    }

    public void ClosePopUp()
    {
        if (uiManager.uiState != UIManager.UIState.InTransition)
        {
            uiManager.PopUpAction(popUp, background, targetPosition, false);
        }
    }

    public void UpdateSeed()
    {
        // Ignora o string se ele for vazio
        if (inputField.text == "")
        {
            uiManager.UpdateSeed(0, false);
        }
        else
        {
            int seed;
            // Se o string não puder ser convertido em números inteiros então uma seed aleatória será usada
            if (!int.TryParse(inputField.text, out seed))
            {
                seed = GetDeterministicHashCode(inputField.text);
            }

            uiManager.UpdateSeed(seed, true);
        }
    }

    public void OpenScenePopUp()
    {
        if (uiManager.uiState != UIManager.UIState.InTransition)
        {
            uiManager.SetGameMode(gameMode);
            if (uiManager.SaveExists(gameMode))
            {
                OpenPopUp();
            }
            else
            {
                uiManager.SceneTransition(false, false);
            }
        }
    }

    public void ChangeScene(bool preserveSave)
    {
        if (uiManager.uiState != UIManager.UIState.InTransition)
        {
            uiManager.SceneTransition(false, preserveSave);
        }
    }

    public void QuitGame()
    {
        // Sai do aplicativo
        Application.Quit();
    }

    private int GetDeterministicHashCode(string str)
    {
        unchecked
        {
            int hash1 = (5381 << 16) + 5381;
            int hash2 = hash1;

            for (int i = 0; i < str.Length; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1)
                    break;
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }
    #endregion
}

[CustomEditor(typeof(UIElement))]
public class UIElementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var uiElementScript = target as UIElement;

        string[] options = new string[]
        {
            "Panel", "PopUp", "Width Slider", "Height Slider", "Seed Input", "Scene PopUp", "Scene Load", "Quit"
        };
        uiElementScript.selected = EditorGUILayout.Popup("Element Target", uiElementScript.selected, options);

        switch (uiElementScript.selected)
        {
            case 0:
                uiElementScript.nextPanel = EditorGUILayout.ObjectField("Next Panel",
                                                                        uiElementScript.nextPanel,
                                                                        typeof(GameObject),
                                                                        true) as GameObject;
                break;
            case 1:
                uiElementScript.popUp = EditorGUILayout.ObjectField("Pop Up",
                                                                    uiElementScript.popUp,
                                                                    typeof(GameObject),
                                                                    true) as GameObject;

                uiElementScript.background = EditorGUILayout.ObjectField("Background",
                                                                         uiElementScript.background,
                                                                         typeof(GameObject),
                                                                         true) as GameObject;

                uiElementScript.targetPosition = EditorGUILayout.Vector2Field("Target Position",
                                                                              uiElementScript.targetPosition);
                break;
            case 2:
                uiElementScript.sliderText = EditorGUILayout.ObjectField("Slider Output",
                                                                         uiElementScript.sliderText,
                                                                         typeof(Text),
                                                                         true) as Text;
                break;
            case 3:
                uiElementScript.sliderText = EditorGUILayout.ObjectField("Slider Output",
                                                                         uiElementScript.sliderText,
                                                                         typeof(Text),
                                                                         true) as Text;
                break;
            case 4:
                uiElementScript.inputField = EditorGUILayout.ObjectField("Input Field",
                                                                         uiElementScript.inputField,
                                                                         typeof(InputField),
                                                                         true) as InputField;
                break;
            case 5:
                uiElementScript.popUp = EditorGUILayout.ObjectField("Pop Up",
                                                                    uiElementScript.popUp,
                                                                    typeof(GameObject),
                                                                    true) as GameObject;

                uiElementScript.background = EditorGUILayout.ObjectField("Background",
                                                                         uiElementScript.background,
                                                                         typeof(GameObject),
                                                                         true) as GameObject;

                uiElementScript.targetPosition = EditorGUILayout.Vector2Field("Target Position",
                                                                              uiElementScript.targetPosition);

                string[] subOptions = new string[]
                {
                    "Classic", "Time", "Dark", "Custom"
                };
                uiElementScript.selectedMode = EditorGUILayout.Popup("Game mode",
                                                                     uiElementScript.selectedMode,
                                                                     subOptions);

                switch (uiElementScript.selectedMode)
                {
                    case 0:
                        uiElementScript.gameMode = ScriptManager.GameMode.Classic;
                        break;
                    case 1:
                        uiElementScript.gameMode = ScriptManager.GameMode.Time;
                        break;
                    case 2:
                        uiElementScript.gameMode = ScriptManager.GameMode.Dark;
                        break;
                    case 3:
                        uiElementScript.gameMode = ScriptManager.GameMode.Custom;
                        break;
                    default:
                        uiElementScript.gameMode = ScriptManager.GameMode.Null;
                        break;
                }
                break;
            default:
                break;
        }
    }
}
