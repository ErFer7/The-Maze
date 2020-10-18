using UnityEngine;
using System.Collections;

public class UIAnimation : MonoBehaviour
{
    // REFATORAR

    public bool spin;
    public float rotationSpeed;
    public float updateTime;
    private WaitForSeconds waitTime;

    // Spin Animation
    IEnumerator SpinAnimation ()
    {
        while (spin == true)
        {
            // Rotates the object at a the specified speed
            transform.Rotate(0F, 0F, rotationSpeed);

            yield return waitTime;
        }
    }

    private void Start()
    {
        // Initialize update rate of the animation
        waitTime = new WaitForSeconds(updateTime);
    }

    private void OnEnable()
    {
        StartCoroutine(SpinAnimation());
    }
}
