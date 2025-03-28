using System;
using UnityEngine;

namespace Lesson14
{
    public class ScoreSystem : MonoBehaviour
    {
        public event Action OnDataUdpated;

        [SerializeField] private HealthSystem _healthSystem;
        [SerializeField] private GunDamageDealer _gunDamageDealer;

        private Vector3Int _kda;  // Kills, Deaths, Assists (X = Kills, Y = Deaths, Z = Assists)
        private int _shotCount;
        private int _hitCount;
        private int _headshotCount; // ✅ Tracks total headshots
        private int _accuracy; // ✅ Stores accuracy percentage

        public Vector3Int KDA => _kda;
        public int Accuracy => _accuracy;
        public int Headshots => _headshotCount;

        private void Awake()
        {
            if (_gunDamageDealer == null)
            {
                Debug.LogError("❌ ScoreSystem: GunDamageDealer not assigned in Inspector!");
            }

            if (_healthSystem == null)
            {
                Debug.LogError("❌ ScoreSystem: HealthSystem not assigned in Inspector!");
            }
        }

        private void Start()
        {
            _gunDamageDealer.OnHit += HitHandler;
            _gunDamageDealer.OnShot += ShotHandler;
            Debug.Log("✅ ScoreSystem: Subscribed to GunDamageDealer events.");

            _healthSystem.OnCharacterDeath += CharacterDeathHandler;
            Debug.Log("✅ ScoreSystem: Subscribed to HealthSystem events.");
        }


        public void HitHandler(int hits, bool isHeadshot)
        {
            _hitCount += hits;

            if (isHeadshot)
                _headshotCount++;

            UpdateAccuracy();
            OnDataUdpated?.Invoke();
        }

        public void ShotHandler()
        {
            _shotCount++; // ✅ Always count shots, even if they miss
            Debug.Log($"🔫 Shot fired! Total shots: {_shotCount}");
            UpdateAccuracy();
            OnDataUdpated?.Invoke();
        }


        public void CharacterDeathHandler(Health health)
        {
            if (health == null)
            {
                Debug.LogError("❌ CharacterDeathHandler: Health is NULL!");
                return;
            }

            if (!health.IsAlive)
            {
                Debug.Log($"⚠️ {health.gameObject.name} is already dead. Skipping kill count.");
                return;
            }

            Debug.Log($"💀 {health.gameObject.name} died! Counting kill...");

            _kda.x++; // ✅ Increase kill count BEFORE setting IsAlive to false
            Debug.Log($"💀 Kill counted! Total Kills: {_kda.x}");

            OnDataUdpated?.Invoke();
            health.IsAlive = false; // ✅ Set IsAlive AFTER counting the kill
        }





        private void UpdateAccuracy()
        {
            _accuracy = _shotCount == 0 ? 0 : (int)((_hitCount / (float)_shotCount) * 100f);
            Debug.Log($"🎯 Accuracy Updated: {_accuracy}%");
        }

    }
}
