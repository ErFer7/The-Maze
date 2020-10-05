using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    #region Public Variables
    // Acesso ao alvo
    public Transform target;

    // Tempo de delay
    public float smoothTime;
    #endregion

    #region Private Variables
    // Vetores
    private Vector3 velocity;
    private Vector3 TargetPosition;
    private Vector3 cameraPoint;
    #endregion

    #region Unity Methods
    private void Start()
    {
        // Inicializa o ponto da câmera
        cameraPoint = new Vector3(0, 0, -10);
    }

    private void Update()
    {
        // Acessa a posição do alvo
        TargetPosition = target.TransformPoint(cameraPoint);

        // Segue o alvo suavementes
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref velocity, smoothTime);
    }
    #endregion
}