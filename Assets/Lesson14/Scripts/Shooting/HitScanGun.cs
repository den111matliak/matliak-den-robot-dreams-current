using System;
using System.Collections;
using System.Numerics;
using UnityEngine;

namespace Shooting
{
    public class HitScanGun : MonoBehaviour
    {
        public event Action<Collider> OnHit;
        public event Action OnShot;

        [SerializeField] protected HitscanShotAspect _shotPrefab;
        [SerializeField] protected Transform _muzzleTransform;
        [SerializeField] protected float _decaySpeed;
        [SerializeField] protected UnityEngine.Vector3 _shotScale;
        [SerializeField] protected float _shotRadius;
        [SerializeField] protected float _shotVisualDiameter;
        [SerializeField] protected string _tilingName;
        [SerializeField] protected float _range;
        [SerializeField] protected LayerMask _layerMask;

        protected int _tilingId;

        protected virtual void Start()
        {
            _tilingId = Shader.PropertyToID(_tilingName);
        }

        protected void OnEnable()
        {
            Lesson8.InputController.OnPrimaryInput += PrimaryInputHandler;
        }

        protected void OnDisable()
        {
            Lesson8.InputController.OnPrimaryInput -= PrimaryInputHandler;
        }

        protected virtual void PrimaryInputHandler()
        {
            UnityEngine.Vector3 muzzlePosition = _muzzleTransform.position;
            UnityEngine.Vector3 muzzleForward = _muzzleTransform.forward;
            Ray ray = new Ray(muzzlePosition, muzzleForward);
            UnityEngine.Vector3 hitPoint = muzzlePosition + muzzleForward * _range;
            if (Physics.SphereCast(ray, _shotRadius, out RaycastHit hitInfo, _range, _layerMask))
            {
                UnityEngine.Vector3 directVector = hitInfo.point - _muzzleTransform.position;
                UnityEngine.Vector3 rayVector = UnityEngine.Vector3.Project(directVector, ray.direction);
                hitPoint = muzzlePosition + rayVector;

                OnHit?.Invoke(hitInfo.collider);
            }

            HitscanShotAspect shot = Instantiate(_shotPrefab, hitPoint, _muzzleTransform.rotation);
            shot.distance = (hitPoint - _muzzleTransform.position).magnitude;
            shot.outerPropertyBlock = new MaterialPropertyBlock();
            StartCoroutine(ShotRoutine(shot));

            OnShot?.Invoke();
        }

        protected IEnumerator ShotRoutine(HitscanShotAspect shot)
        {
            float interval = _decaySpeed * Time.deltaTime;
            while (shot.distance >= interval)
            {
                EvaluateShot(shot);
                yield return null;
                shot.distance -= interval;
                interval = _decaySpeed * Time.deltaTime;
            }

            Destroy(shot.gameObject);
        }

        protected void EvaluateShot(HitscanShotAspect shot)
        {
            shot.Transform.localScale = new UnityEngine.Vector3(_shotScale.x, _shotScale.y, shot.distance * 0.5f);
            UnityEngine.Vector4 tiling = UnityEngine.Vector4.one;
            tiling.y = shot.distance * 0.5f / _shotVisualDiameter;
            shot.outerPropertyBlock.SetVector(_tilingId, tiling);
            shot.Outer.SetPropertyBlock(shot.outerPropertyBlock);
        }
    }
}