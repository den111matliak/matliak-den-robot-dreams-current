using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _trainingRangeButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private string _trainingRangeSceneName;

        private void Awake()
        {
            Debug.Log("🟢 MainMenu Awake");

            _trainingRangeButton.onClick.AddListener(TrainingRangeButtonHandler);
            _quitButton.onClick.AddListener(QuitButtonHandler);
        }

        private void TrainingRangeButtonHandler()
        {
            Debug.Log("🎯 Training button clicked! Trying to load scene: " + _trainingRangeSceneName);
            SceneManager.LoadSceneAsync(_trainingRangeSceneName, LoadSceneMode.Single);
        }

        private void QuitButtonHandler()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}