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

        protected int _health;
        protected bool _isAlive;

        private List<string> damageSources = new List<string>(); // ✅ Track attackers for assists

        public int HealthValue
        {
            get => _health;
            set
            {
                if (_health == value)
                    return;
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
                if (_isAlive == value)
                    return;
                _isAlive = value;
                if (!_isAlive)
                    OnDeath?.Invoke();
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
            if (!IsAlive) return; // ✅ Prevents damage from affecting already dead characters

            HealthValue = Mathf.Clamp(HealthValue - damage, 0, _maxHealth);

            if (HealthValue <= 0)
            {
                Debug.Log($"💀 {gameObject.name} has 0 HP! Triggering death event...");
                OnDeath?.Invoke(); // ✅ Triggers `CharacterDeathHandler()` BEFORE setting IsAlive
            }
        }


        public void Heal(int heal)
        {
            if (!IsAlive)
                return;

            HealthValue = Mathf.Clamp(HealthValue + heal, 0, _maxHealth);
        }

        public void SetHealth(int health)
        {
            HealthValue = Mathf.Clamp(health, 0, _maxHealth);
            IsAlive = HealthValue > 0;
        }

        // ✅ Allow ScoreSystem to check who damaged this object
        public List<string> GetDamageSources()
        {
            return new List<string>(damageSources);
        }
    }
}
