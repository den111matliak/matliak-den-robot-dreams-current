using UnityEngine;

public class RayViewer : MonoBehaviour
{
    public float weaponRange = 50f;
    private Camera fpsCam;

    void Start()
    {
        fpsCam = GetComponentInParent<Camera>();

        if (fpsCam == null)
        {
            fpsCam = Camera.main; // Fallback to main camera
        }

        if (fpsCam == null)
        {
            Debug.LogError("RayViewer: No Camera found in parent or tagged as MainCamera!");
        }
    }

    void Update()
    {
        if (fpsCam == null) return; // Prevents null reference error

        Vector3 lineOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(lineOrigin, fpsCam.transform.forward * weaponRange, Color.green);
    }
}