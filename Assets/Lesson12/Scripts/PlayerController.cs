using Lesson12;
using UnityEngine;

namespace Lesson8
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Transform _yawAnchor; // Add reference to camera yaw anchor
        [SerializeField] private float _speed;

        private Vector2 _moveInput;

        private void Start()
        {
            InputController.OnMoveInput += MoveHandler;
        }

        private void FixedUpdate()
        {
            if (_moveInput.sqrMagnitude > 0.01f)
            {
                // Get camera's forward and right vectors (ignore vertical movement)
                Vector3 forward = _yawAnchor.forward;
                Vector3 right = _yawAnchor.right;

                forward.y = 0f; // Remove vertical influence
                right.y = 0f;

                forward.Normalize();
                right.Normalize();

                // Convert input to world space direction
                Vector3 movement = forward * _moveInput.y + right * _moveInput.x;

                // Move player
                _characterController.SimpleMove(movement * _speed);

                // Rotate player towards movement direction
                if (movement != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(movement);
                }
            }
        }

        private void MoveHandler(Vector2 moveInput)
        {
            _moveInput = moveInput;
        }
    }
}