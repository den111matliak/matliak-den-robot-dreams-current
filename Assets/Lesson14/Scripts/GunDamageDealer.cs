using System;
using UnityEngine;

namespace Lesson14
{
    public class GunDamageDealer : MonoBehaviour
    {
        public event Action<int> OnHit;

        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private RaycastShoot _gun; // ✅ Use RaycastShoot instead of HitScanGun
        [SerializeField] private int _damage;

        public RaycastShoot Gun => _gun;

        private void Start()
        {
            _gun.OnHit += GunHitHandler; // ✅ Subscribe to RaycastShoot's OnHit event
        }

        public void GunHitHandler(Collider collider)
        {
            Health health = collider.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(_damage);
                Debug.Log($"✅ Raycast hit {collider.name}, applied {_damage} damage!");
            }
            else
            {
                Debug.Log($"⚠️ {collider.name} was hit, but it has no Health. Ignoring.");
            }
            OnHit?.Invoke(health ? 1 : 0);
        }
    }
}
