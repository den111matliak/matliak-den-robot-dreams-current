using System;
using System.Collections.Generic;
using Lesson14;
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
            _healthSystem.OnCharacterDeath += CharacterDeathHandler;
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
            _kda.x++;
            OnDataUdpated?.Invoke();
        }

    }
}