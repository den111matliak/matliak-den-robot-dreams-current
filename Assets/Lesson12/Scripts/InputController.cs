using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Lesson8
{
    public class InputController : MonoBehaviour
    {
        public static event Action<Vector2> OnMoveInput;
        public static event Action<Vector2> OnLookInput;
        public static event Action<bool> OnCameraLock;
        public static event Action OnPrimaryInput;
        public static event Action<bool> OnSecondaryInput;
        public static event Action OnGrenadeInput;
        public static event Action<bool> OnScoreInput; // ✅ Event for showing/hiding score
        public static event Action OnEscape;

        [SerializeField] private InputActionAsset _inputActionAsset;
        [SerializeField] private string _mapName;
        [SerializeField] private string _moveName;
        [SerializeField] private string _lookAroundName;
        [SerializeField] private string _cameraLockName;
        [SerializeField] private string _primaryFireName;
        [SerializeField] private string _secondaryFireName;
        [SerializeField] private string _grenadeName;
        [SerializeField] private string _scoreName; // ✅ Score input action
        [SerializeField] private string _escapeName;

        private InputAction _moveAction;
        private InputAction _lookAroundAction;
        private InputAction _cameraLockAction;
        private InputAction _primaryFireAction;
        private InputAction _secondaryFireAction;
        private InputAction _grenadeAction;
        private InputAction _scoreAction; // ✅ Score input action reference
        private InputAction _escapeAction;
        private InputActionMap _actionMap;

        private bool _inputUpdated;

        private void OnEnable()
        {
            if (_inputActionAsset == null)
            {
                Debug.LogError("InputController: InputActionAsset is missing!");
                return;
            }

            _inputActionAsset.Enable();
            _actionMap = _inputActionAsset.FindActionMap(_mapName);

            if (_actionMap == null)
            {
                Debug.LogError($"InputController: Action map '{_mapName}' not found!");
                return;
            }

            _moveAction = _actionMap.FindAction(_moveName);
            _lookAroundAction = _actionMap.FindAction(_lookAroundName);
            _cameraLockAction = _actionMap.FindAction(_cameraLockName);
            _primaryFireAction = _actionMap.FindAction(_primaryFireName);
            _secondaryFireAction = _actionMap.FindAction(_secondaryFireName);
            _grenadeAction = _actionMap.FindAction(_grenadeName);
            _scoreAction = _actionMap.FindAction(_scoreName); // ✅ Find score action
            _escapeAction = _actionMap.FindAction(_escapeName);

            if (_moveAction == null || _lookAroundAction == null || _cameraLockAction == null ||
                _primaryFireAction == null || _secondaryFireAction == null || _grenadeAction == null)
            {
                Debug.LogError("InputController: One or more InputActions are not properly assigned!");
                return;
            }

            _moveAction.Enable();
            _lookAroundAction.Enable();
            _cameraLockAction.Enable();
            _primaryFireAction.Enable();
            _secondaryFireAction.Enable();
            _grenadeAction.Enable();
            _scoreAction?.Enable(); // ✅ Enable score action

            _moveAction.performed += MovePerformedHandler;
            _moveAction.canceled += MoveCanceledHandler;

            _lookAroundAction.performed += LookPerformedHandler;
            _lookAroundAction.canceled += LookCanceledHandler;

            _cameraLockAction.performed += CameraLockPerformedHandler;
            _cameraLockAction.canceled += CameraLockCanceledHandler;

            _primaryFireAction.performed += PrimaryFirePerformedHandler;

            _secondaryFireAction.performed += SecondaryFirePerformedHandler;
            _secondaryFireAction.canceled += SecondaryFireCanceledHandler;

            _grenadeAction.performed += GrenadePerformedHandler;
            _escapeAction.performed += EscapePerformedHandler;

            if (_scoreAction != null)
            {
                _scoreAction.performed += ScorePerformedHandler;
                _scoreAction.canceled += ScorePerformedHandler;
            }
        }

        private void OnDisable()
        {
            _inputActionAsset?.Disable();
        }

        private void OnDestroy()
        {
            if (_moveAction != null)
            {
                _moveAction.performed -= MovePerformedHandler;
                _moveAction.canceled -= MoveCanceledHandler;
            }

            if (_lookAroundAction != null)
            {
                _lookAroundAction.performed -= LookPerformedHandler;
                _lookAroundAction.canceled -= LookCanceledHandler;
            }

            if (_cameraLockAction != null)
            {
                _cameraLockAction.performed -= CameraLockPerformedHandler;
                _cameraLockAction.canceled -= CameraLockCanceledHandler;
            }

            if (_primaryFireAction != null)
            {
                _primaryFireAction.performed -= PrimaryFirePerformedHandler;
            }

            if (_secondaryFireAction != null)
            {
                _secondaryFireAction.performed -= SecondaryFirePerformedHandler;
                _secondaryFireAction.canceled -= SecondaryFireCanceledHandler;
            }

            if (_grenadeAction != null)
            {
                _grenadeAction.performed -= GrenadePerformedHandler;
            }

            if (_scoreAction != null)
            {
                _scoreAction.performed -= ScorePerformedHandler;
                _scoreAction.canceled -= ScorePerformedHandler;
            }

            _escapeAction.performed -= EscapePerformedHandler;

            OnMoveInput = null;
            OnLookInput = null;
            OnPrimaryInput = null;
            OnSecondaryInput = null;
            OnGrenadeInput = null;
            OnScoreInput = null; // ✅ Prevent memory leaks
        }

        private void MovePerformedHandler(InputAction.CallbackContext context)
        {
            OnMoveInput?.Invoke(context.ReadValue<Vector2>());
        }

        private void MoveCanceledHandler(InputAction.CallbackContext context)
        {
            OnMoveInput?.Invoke(Vector2.zero);
        }

        private void LookPerformedHandler(InputAction.CallbackContext context)
        {
            OnLookInput?.Invoke(context.ReadValue<Vector2>());
        }

        private void LookCanceledHandler(InputAction.CallbackContext context)
        {
            OnLookInput?.Invoke(Vector2.zero);
        }

        private void CameraLockPerformedHandler(InputAction.CallbackContext context)
        {
            OnCameraLock?.Invoke(true);
        }

        private void CameraLockCanceledHandler(InputAction.CallbackContext context)
        {
            OnCameraLock?.Invoke(false);
        }

        private void PrimaryFirePerformedHandler(InputAction.CallbackContext context)
        {
            Debug.Log($"🔫 PrimaryFirePerformedHandler() called at {Time.time}");
            OnPrimaryInput?.Invoke();
        }

        private void SecondaryFirePerformedHandler(InputAction.CallbackContext context)
        {
            OnSecondaryInput?.Invoke(true);
        }

        private void SecondaryFireCanceledHandler(InputAction.CallbackContext context)
        {
            OnSecondaryInput?.Invoke(false);
        }

        private void GrenadePerformedHandler(InputAction.CallbackContext context)
        {
            OnGrenadeInput?.Invoke();
        }

        private void ScorePerformedHandler(InputAction.CallbackContext context)
        {
            bool isPressed = context.ReadValue<float>() > 0; // ✅ Detect if Tab is pressed
            OnScoreInput?.Invoke(isPressed);
        }

        private void EscapePerformedHandler(InputAction.CallbackContext context)
        {
            OnEscape?.Invoke();
        }
    }
}