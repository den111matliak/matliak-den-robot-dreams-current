using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class GameplayMenu : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private Button _confrimButton;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private string _lobbySceneName;
        [SerializeField] private Lesson8.InputController _inputController;

        public bool Enabled
        {
            get => _canvas.enabled;
            set
            {
                if (_canvas.enabled == value)
                    return;
                _canvas.enabled = value;
                _inputController.enabled = !value;
            }
        }

        private void Awake()
        {
            _confrimButton.onClick.AddListener(ConfirmButtonHandler);
            _cancelButton.onClick.AddListener(CancelButtonHandler);
        }

        private void Start()
        {
            Lesson8.InputController.OnEscape += EscapeHandler;
        }

        // ✅ CLEANUP to avoid MissingReferenceException
        private void OnDestroy()
        {
            Lesson8.InputController.OnEscape -= EscapeHandler;
        }

        private void EscapeHandler()
        {
            if (this == null || _canvas == null) return; // 🔒 Safeguard against destroyed components
            Enabled = !Enabled;
        }

        private void ConfirmButtonHandler()
        {
            SceneManager.LoadSceneAsync(_lobbySceneName, LoadSceneMode.Single);
        }

        private void CancelButtonHandler()
        {
            Enabled = false;
        }
    }
}