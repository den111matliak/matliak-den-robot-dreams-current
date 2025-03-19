using System;
using UnityEngine;
using System.Collections;

namespace Lesson14
{
    public class GunDamageDealer : MonoBehaviour
    {
        public event Action<int, bool> OnHit; // ✅ Tracks hit & headshots
        public event Action OnShot; // ✅ Now properly used

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f); // ✅ Delay execution to allow ScoreSystem to subscribe

            ScoreSystem scoreSystem = FindObjectOfType<ScoreSystem>();
            if (scoreSystem != null)
            {
                Debug.Log("🔄 GunDamageDealer: Subscribing OnHit to ScoreSystem...");
                OnHit -= scoreSystem.HitHandler; // ✅ Prevent duplicate subscriptions
                OnHit += scoreSystem.HitHandler;

                OnShot -= scoreSystem.ShotHandler; // ✅ Ensure ScoreSystem tracks shots
                OnShot += scoreSystem.ShotHandler;
            }
            else
            {
                Debug.LogError("❌ GunDamageDealer: No ScoreSystem found! OnHit will be NULL.");
            }

            if (OnHit == null)
            {
                Debug.LogError("❌ GunDamageDealer: OnHit is STILL NULL after delay! No subscribers.");
            }
            else
            {
                Debug.Log($"✅ GunDamageDealer: OnHit has subscribers: {OnHit.GetInvocationList().Length}");
            }
        }

        public void GunHitHandler(Collider hitCollider)
        {
            if (OnHit == null)
            {
                Debug.LogError("❌ GunDamageDealer: OnHit is NULL before damage! Forcing re-subscription...");
                ScoreSystem scoreSystem = FindObjectOfType<ScoreSystem>();
                if (scoreSystem != null)
                {
                    Debug.Log("🔄 GunDamageDealer: Re-subscribing OnHit...");
                    OnHit -= scoreSystem.HitHandler;
                    OnHit += scoreSystem.HitHandler;

                    OnShot -= scoreSystem.ShotHandler; // ✅ Ensure ShotHandler is subscribed
                    OnShot += scoreSystem.ShotHandler;
                }
            }

            Health health = hitCollider.GetComponentInParent<Health>();

            if (health != null && health.IsAlive)
            {
                bool isHeadshot = hitCollider.CompareTag("Head");
                int damage = isHeadshot ? health.MaxHealthValue : 25;

                health.TakeDamage(damage, gameObject.name);
                OnHit?.Invoke(1, isHeadshot); // ✅ Ensure hits are tracked
            }

            OnShot?.Invoke(); // ✅ Notify ScoreSystem that a shot was fired
        }
    }
}
