using UnityEngine;

namespace Lesson14
{
    public class CameraSystem : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public Camera Camera => _camera;
    }
}