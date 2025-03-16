using System;
using System.Collections;
using Lesson14;
using Lesson8;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    public event Action OnShot;
    public event Action<Collider> OnHit; // ✅ Event for notifying hits

    public int gunDamage = 1;
    public float fireRate = 0.25f;
    public float weaponRange = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;
    public Camera fpsCam;

    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    private AudioSource gunAudio;
    private LineRenderer laserLine;
    private float nextFire;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();

        if (fpsCam == null)
            fpsCam = GetComponentInParent<Camera>();

        InputController.OnPrimaryInput += FireWeapon;
    }

    void OnDestroy()
    {
        InputController.OnPrimaryInput -= FireWeapon;
    }

    private void FireWeapon()
    {
        if (Time.time < nextFire) return; // Prevents shooting too fast

        nextFire = Time.time + fireRate;
        StartCoroutine(ShotEffect());

        // Fire shot event before checking hit
        OnShot?.Invoke();  // 🔥 Notifies ScoreSystem that a shot happened!

        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        laserLine.SetPosition(0, gunEnd.position);

        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
        {
            laserLine.SetPosition(1, hit.point);

            // ✅ Notify GunDamageDealer to apply damage
            GunDamageDealer damageDealer = GetComponent<GunDamageDealer>();
            if (damageDealer != null)
            {
                damageDealer.GunHitHandler(hit.collider);
            }
            else
            {
                Debug.LogError("❌ GunDamageDealer component not found on gun!");
            }

            // ✅ Notify any listeners that something was hit
            OnHit?.Invoke(hit.collider);
        }
        else
        {
            laserLine.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
        }
    }

    private IEnumerator ShotEffect()
    {
        gunAudio.Play();
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }
}
