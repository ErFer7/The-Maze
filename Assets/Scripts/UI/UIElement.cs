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

    public void QuitGame()
    {
        // Sai do aplicativo
        Application.Quit();
    }

    #endregion
}

[CustomEditor(typeof(UIElement))]
public class UIElementEditor : Editor
{
    private int selected = 0;

    public override void OnInspectorGUI()
    {
        var uiElementScript = target as UIElement;

        string[] options = new string[]
        {
            "Panel", "PopUp", "Width Slider", "Height Slider", "Quit"
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
            default:
                break;
        }
    }
}
