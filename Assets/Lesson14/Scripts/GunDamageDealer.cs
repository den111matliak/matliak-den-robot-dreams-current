using System;
using UnityEngine;
using System.Collections;

namespace Lesson14
{
    public class GunDamageDealer : MonoBehaviour
    {
        public event Action<int, bool> OnHit;
        public event Action OnShot;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);

            ScoreSystem scoreSystem = FindObjectOfType<ScoreSystem>();
            if (scoreSystem != null)
            {
                Debug.Log("🔄 GunDamageDealer: Subscribing OnHit to ScoreSystem...");
                OnHit -= scoreSystem.HitHandler;
                OnHit += scoreSystem.HitHandler;

                OnShot -= scoreSystem.ShotHandler;
                OnShot += scoreSystem.ShotHandler;
            }

            if (OnHit == null)
                Debug.LogError("❌ GunDamageDealer: OnHit is STILL NULL after delay! No subscribers.");
            else
                Debug.Log($"✅ GunDamageDealer: OnHit has subscribers: {OnHit.GetInvocationList().Length}");
        }

        public void GunHitHandler(Collider hitCollider)
        {
            Health health = hitCollider.GetComponentInParent<Health>();

            if (health != null && health.IsAlive)
            {
                bool isHeadshot = health.IsColliderAWeakPoint(hitCollider); // ✅ Use method instead of tag
                int damage = isHeadshot ? health.MaxHealthValue : 25;

                health.TakeDamage(damage, gameObject.name);
                OnHit?.Invoke(1, isHeadshot);

                Debug.Log(isHeadshot
                    ? $"🎯 HEADSHOT! {hitCollider.name} took {damage} damage!"
                    : $"🔥 Body hit! {hitCollider.name} took {damage} damage!");
            }

            OnShot?.Invoke();
        }
    }
}
