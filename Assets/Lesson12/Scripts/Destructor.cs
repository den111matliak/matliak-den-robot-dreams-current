using System;
using System.Collections;
using System.Collections.Generic;
using Lesson12;
using Lesson14;
using Lesson8;
using UnityEngine;

namespace Lesson12
{
    public class Destructor : MonoBehaviour
    {
        public Action<Vector3> OnPrimaryFire;

        [SerializeField] private DestructableSystem _destructableSystem;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private float _rayDistance;
        [SerializeField] private LayerMask _rayMask;
        [SerializeField] private LayerMask _explsionMask;

        [SerializeField] private ExplosionController _explosionController;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _verticalOffset;

        [SerializeField] private AudioSource explosionAudio; // 🎆 Explosion sound source
        [SerializeField] private AudioClip explosionSound; // 🎆 Explosion sound clip

        private float _radiusReciprocal;
        private bool _canExplode = true; // ✅ Prevents multiple explosions per click

        private void Start()
        {
            // ✅ Remove existing event listeners before adding them (prevents duplicate calls)
            InputController.OnPrimaryInput -= PrimaryFireHandler;
            InputController.OnPrimaryInput += PrimaryFireHandler;

            InputController.OnSecondaryInput -= SecondaryFireHandler;
            InputController.OnSecondaryInput += SecondaryFireHandler;
        }

        private void OnEnable()
        {
            _radiusReciprocal = 1f / _explosionRadius;
            _explosionController.ApplyRadius(_explosionRadius);
        }

        private void PrimaryFireHandler()
        {
            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            Vector3 _hitPoint = _cameraTransform.position + _cameraTransform.forward * _rayDistance;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance, _rayMask))
            {
                _hitPoint = hitInfo.point;
                _destructableSystem.Destruct(hitInfo.rigidbody);
            }
            OnPrimaryFire?.Invoke(_hitPoint);
        }

        private void SecondaryFireHandler(bool performed)
        {
            if (!_canExplode) return; // ✅ If already exploding, ignore extra calls
            _canExplode = false; // ✅ Block future explosions temporarily

            Debug.Log($"🚀 Secondary Fire Triggered at {Time.time}"); // ✅ Print exact time

            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            Vector3 _hitPoint = _cameraTransform.position + _cameraTransform.forward * _rayDistance;
            if (Physics.Raycast(ray, out RaycastHit hitInfo, _rayDistance, _rayMask))
            {
                _hitPoint = hitInfo.point;
                Debug.Log("💥 Explosion Triggered at " + _hitPoint); // Debug explosion position

                Collider[] colliders = Physics.OverlapSphere(_hitPoint, _explosionRadius, _explsionMask);
                HashSet<Rigidbody> _targets = new HashSet<Rigidbody>();
                HashSet<Health> damagedObjects = new HashSet<Health>(); // Prevent double damage

                for (int i = 0; i < colliders.Length; ++i)
                {
                    Rigidbody rigidbody = colliders[i].attachedRigidbody;
                    _ = _targets.Add(rigidbody);

                    // Debug log for explosion hit detection
                    Debug.Log("🎯 Explosion hit " + colliders[i].name);

                    // Apply damage if the object has a Health component
                    Health health = colliders[i].GetComponent<Health>();
                    if (health != null && !damagedObjects.Contains(health))
                    {
                        health.TakeDamage(50, "Explosion"); // ✅ Pass "Explosion" as the attacker
                        damagedObjects.Add(health);
                        Debug.Log($"🔥 Explosion applied 50 damage to {colliders[i].name}!");
                    }
                }

                // Apply force to objects
                foreach (Rigidbody rigidbody in _targets)
                {
                    if (rigidbody == null)
                        continue;

                    Vector3 direction = rigidbody.position - (_hitPoint + Vector3.up * _verticalOffset);
                    rigidbody.AddForce(
                        direction.normalized * _explosionForce * Mathf.Clamp01(1f - direction.magnitude * _radiusReciprocal),
                        ForceMode.Impulse);
                }

                // Play explosion effect
                Instantiate(_explosionController, _hitPoint, Quaternion.identity).Play();

                // Play explosion sound
                if (explosionAudio != null && explosionSound != null && !explosionAudio.isPlaying)
                {
                    Debug.Log("🔊 Playing explosion sound!"); // Debug log for sound playback
                    explosionAudio.PlayOneShot(explosionSound);
                }
            }

            // ✅ Allow explosion again after a short delay
            StartCoroutine(EnableExplosionCooldown());
        }

        private IEnumerator EnableExplosionCooldown()
        {
            yield return new WaitForSeconds(0.1f); // Adjust delay if needed
            _canExplode = true; // ✅ Allow explosion again
        }
    }
}
