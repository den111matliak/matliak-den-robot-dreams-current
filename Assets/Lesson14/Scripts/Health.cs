using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson14
{
    public class Health : MonoBehaviour
    {
        public event Action<int> OnHealthChanged;
        public event Action<float> OnHealthChanged01;
        public event Action OnDeath;

        [SerializeField] protected CharacterController _characterController;
        [SerializeField] protected int _maxHealth;

        [SerializeField] private Collider _weakPointCollider; // ✅ Reference to head collider

        protected int _health;
        protected bool _isAlive;

        private List<string> damageSources = new List<string>();

        public int HealthValue
        {
            get => _health;
            set
            {
                if (_health == value) return;
                _health = value;
                OnHealthChanged?.Invoke(_health);
                OnHealthChanged01?.Invoke(_health / (float)_maxHealth);
            }
        }

        public bool IsAlive
        {
            get => _isAlive;
            set
            {
                if (_isAlive == value) return;
                _isAlive = value;
                if (!_isAlive) OnDeath?.Invoke();
            }
        }

        public float HealthValue01 => HealthValue / (float)_maxHealth;
        public int MaxHealthValue => _maxHealth;
        public CharacterController CharacterController => _characterController;

        protected virtual void Awake()
        {
            SetHealth(MaxHealthValue);
        }

        public void TakeDamage(int damage, string attackerName)
        {
            if (!IsAlive) return;

            HealthValue = Mathf.Clamp(HealthValue - damage, 0, _maxHealth);

            if (HealthValue <= 0)
            {
                Debug.Log($"💀 {gameObject.name} has 0 HP! Triggering death event...");
                OnDeath?.Invoke();
            }

            if (!damageSources.Contains(attackerName))
                damageSources.Add(attackerName);
        }

        public void Heal(int heal)
        {
            if (!IsAlive) return;
            HealthValue = Mathf.Clamp(HealthValue + heal, 0, _maxHealth);
        }

        public void SetHealth(int health)
        {
            HealthValue = Mathf.Clamp(health, 0, _maxHealth);
            IsAlive = HealthValue > 0;
        }

        public List<string> GetDamageSources()
        {
            return new List<string>(damageSources);
        }

        // ✅ New method to check for weak point (e.g. head)
        public bool IsColliderAWeakPoint(Collider hitCollider)
        {
            return _weakPointCollider != null && hitCollider == _weakPointCollider;
        }
    }
}
