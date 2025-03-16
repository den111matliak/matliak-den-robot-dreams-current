using System;
using Lesson14;
using Shooting;
using UnityEngine;

namespace Lesson14
{
    public class GunDamageDealer : MonoBehaviour
    {
        public event Action<int> OnHit;

        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private RaycastShoot _gun;
        [SerializeField] private int _damage;

        public RaycastShoot Gun => _gun;

        private void Start()
        {
            _gun.OnHit += GunHitHandler;
        }

        private void GunHitHandler(Collider collider)
        {
            /*Health health = collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(_damage);
            }*/
            Health health = collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(_damage);
                Debug.Log($"✅ Raycast hit {collider.name}, applied {_damage} damage!");
                OnHit?.Invoke(1);
            }
            else
            {
                Debug.LogError($"No Health component found on {collider.name}");
            }
            OnHit?.Invoke(health ? 1 : 0);
        }
    }
}