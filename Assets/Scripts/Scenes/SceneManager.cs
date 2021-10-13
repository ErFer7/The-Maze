using UnityEngine;
using UnityEngine.SceneManagement;

namespace Abstrato
{
    public class SceneManager : MonoBehaviour
    {
        private void Awake()
        {
            if (GameObject.FindGameObjectsWithTag(tag).Length == 1)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
            {
                // ...
            }
        }
    }
}
