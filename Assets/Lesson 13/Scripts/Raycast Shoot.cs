using System.Collections;
using UnityEngine;
using Lesson8; // Make sure this matches your namespace in InputController

public class RaycastShoot : MonoBehaviour
{
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

        // Subscribe to the InputController event for shooting
        InputController.OnPrimaryInput += FireWeapon;
    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        InputController.OnPrimaryInput -= FireWeapon;
    }

    private void FireWeapon()
    {
        if (Time.time < nextFire) return; // Prevents shooting too fast

        nextFire = Time.time + fireRate;
        StartCoroutine(ShotEffect());

        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        laserLine.SetPosition(0, gunEnd.position);

        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
        {
            laserLine.SetPosition(1, hit.point);
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