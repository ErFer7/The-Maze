using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    #region Public Variables
    // Tempo da animação
    public float animationTime;

    // Cores (O canal alfa é o único usado)
    public Color colorUp;
    public Color colorDown;

    // Fundo
    public Image exitBackGround;

    // Caixa de texto
    public RectTransform exitMessageBox;
    #endregion
}
