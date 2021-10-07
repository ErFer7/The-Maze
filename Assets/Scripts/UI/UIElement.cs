using UnityEngine;
using UnityEditor;

public class UIElement : MonoBehaviour
{
    #region Public Variables
    #region Editor Accessible
    // Troca de painéis
    public Object nextPanel;
    // Pop Up
    public Object popUp;
    public Object background;
    public Vector2 targetPosition;
    #endregion

    [HideInInspector]
    public int selected;
    #endregion

    #region Private Variables
    // Atributo para o acesso ao UIManager
    private UIManager uiManager;
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
            uiManager.PanelTransition(nextPanel as GameObject);
        }
    }

    public void OpenPopUp()
    {
        if (uiManager.uiState != UIManager.UIState.InTransition)
        {
            uiManager.PopUpAction(popUp as GameObject, background as GameObject, targetPosition, true);
        }
    }

    public void ClosePopUp()
    {
        if (uiManager.uiState != UIManager.UIState.InTransition)
        {
            uiManager.PopUpAction(popUp as GameObject, background as GameObject, targetPosition, false);
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
            "Panel", "PopUp", "Quit"
        };
        uiElementScript.selected = EditorGUILayout.Popup("Element Target", uiElementScript.selected, options);

        switch (uiElementScript.selected)
        {
            case 0:
                uiElementScript.nextPanel = EditorGUILayout.ObjectField("Next Panel", uiElementScript.nextPanel, typeof(GameObject), true);
                break;
            case 1:
                uiElementScript.popUp = EditorGUILayout.ObjectField("Pop Up", uiElementScript.popUp, typeof(GameObject), true);
                uiElementScript.background = EditorGUILayout.ObjectField("Background", uiElementScript.background, typeof(GameObject), true);
                uiElementScript.targetPosition = EditorGUILayout.Vector2Field("Target Position", uiElementScript.targetPosition);
                break;
            default:
                break;
        }
    }
}
