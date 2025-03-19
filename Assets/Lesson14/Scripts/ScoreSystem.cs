using System;
using System.Collections.Generic;
using Shooting;
using UnityEngine;

namespace Lesson14
{
    public class ScoreSystem : MonoBehaviour
    {
        public event Action OnDataUdpated;

        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private GunDamageDealer _gunDamageDealer;

        private Vector3Int _kda;
        private int _shotCount;
        private int _hitCount;

        public Vector3Int KDA => _kda;
        public int Accuracy => _shotCount == 0f ? 0 : (int)((_hitCount / (float)_shotCount) * 100f);

        private void Start()
        {
            _gunDamageDealer.OnHit += HitHandler;
            _gunDamageDealer.Gun.OnShot += ShotHandler;

            if (_healthSystem != null)
            {
                _healthSystem.OnCharacterDeath += CharacterDeathHandler;
                Debug.Log("✅ OnCharacterDeath event subscribed!");
            }
            else
            {
                Debug.LogError("❌ HealthSystem is missing in ScoreSystem!");
            }
        }

        private void HitHandler(int hits)
        {
            _hitCount += hits;
            OnDataUdpated?.Invoke();
        }

        private void ShotHandler()
        {
            _shotCount++;
            OnDataUdpated?.Invoke();
        }

        private void CharacterDeathHandler(Health health)
        {
            Debug.Log($"💀 Character {health.gameObject.name} died!");

            _kda.x++; // ✅ Increase kills
            Debug.Log($"💀 Kill counted! Total Kills: {_kda.x}");

            OnDataUdpated?.Invoke();
        }


    }
}