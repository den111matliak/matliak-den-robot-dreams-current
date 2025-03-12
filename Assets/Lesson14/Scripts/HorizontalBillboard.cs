using System;
using UnityEngine;

namespace Lesson14
{
    public class HorizontalBillboard : BillboardBase
    {
        private Transform _cameraTransform;
        private Camera _camera;
        private Transform _transform;

        private void Start()
        {
            // Automatically find and assign the main camera
            SetCamera(Camera.main);
        }

        public override void SetCamera(Camera camera)
        {
            _transform = transform;
            _camera = camera;
            _cameraTransform = _camera.transform;
        }

        private void LateUpdate()
        {
            Vector3 direction = _camera.transform.position - _transform.position;
            direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
            _transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }
}