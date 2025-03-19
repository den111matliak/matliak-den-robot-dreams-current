#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson14
{
    public class HealthSystem : MonoBehaviour
    {
        public event Action<Health> OnCharacterDeath;

        [SerializeField] private Health[] _healths;

        protected Dictionary<Collider, Health> _charactersHealth = new();

        public IEnumerable<Health> CharactersHealth => _charactersHealth.Values;

        /// <summary>
        /// Editor only method
        /// </summary>
        [ContextMenu("Find Healths")]
        private void FindHealths()
        {
#if UNITY_EDITOR
            _healths = FindObjectsOfType<Health>();
            EditorUtility.SetDirty(this);
#endif
        }

        protected virtual void Awake()
        {
            Debug.Log($"🟢 HealthSystem initializing. Expected health components: {_healths.Length}");

            if (_healths.Length == 0)
            {
                Debug.LogError("❌ HealthSystem: _healths is EMPTY! No Health components found.");
                return;
            }

            for (int i = 0; i < _healths.Length; ++i)
            {
                Health health = _healths[i];

                if (health == null)
                {
                    Debug.LogError($"❌ HealthSystem: _healths[{i}] is NULL!");
                    continue;
                }

                Debug.Log($"✅ Checking Health component on: {health.gameObject.name}");

                if (health.CharacterController == null)
                {
                    Debug.LogError($"❌ HealthSystem: {health.gameObject.name} has NO CharacterController assigned!");
                    continue;
                }

                if (!_charactersHealth.ContainsKey(health.CharacterController))
                {
                    _charactersHealth.Add(health.CharacterController, health);
                    Debug.Log($"📌 Added {health.gameObject.name} to HealthSystem dictionary.");
                }

                health.OnDeath += () =>
                {
                    Debug.Log($"🔗 HealthSystem subscribed to OnDeath for {health.gameObject.name}");
                    CharacterDeathHandler(health);
                };
            }
        }


        public virtual bool GetHealth(Collider characterController, out Health health) =>
            _charactersHealth.TryGetValue(characterController, out health);

        protected void CharacterDeathHandler(Health health)
        {
            Debug.Log($"🛑 HealthSystem detected death of {health.gameObject.name}");
            OnCharacterDeath?.Invoke(health);
        }
    }
}