using UnityEngine;
using UnityEngine.SceneManagement;

namespace Abstrato
{
    public class SceneManager : MonoBehaviour
    {
        [HideInInspector]
        public bool loading;

        private ScriptManager scriptManager;

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

        private void Start()
        {
            loading = false;
            scriptManager = GameObject.FindWithTag("ScriptManager").GetComponent<ScriptManager>();
            scriptManager.sceneManager = GetComponent<Abstrato.SceneManager>();
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode gameMode)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0)
            {
                // ...
            }
            else
            {
                if (loading)
                {
                    loading = false;
                }
            }
        }

        public bool SaveExists(ScriptManager.GameMode gameMode)
        {
            switch (gameMode)
            {
                case ScriptManager.GameMode.Classic:
                    return PlayerPrefs.GetInt("classicSaved", -1) > 0;
                case ScriptManager.GameMode.Time:
                    return PlayerPrefs.GetInt("timeSaved", -1) > 0;
                case ScriptManager.GameMode.Dark:
                    return PlayerPrefs.GetInt("darkSaved", -1) > 0;
                default:
                    return false;
            }
        }

        public void LoadScene()
        {
            loading = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
