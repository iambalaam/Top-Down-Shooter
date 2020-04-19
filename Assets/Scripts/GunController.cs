using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private float maxRayLength = 30f;
    public enum GunType { SemiAuto, Burst, Auto };

    // Connected via unity editor
    public GunType gunType;
    public Transform barrel;

    public void Shoot()
    {
        switch (gunType)
        {
            case GunType.SemiAuto:
                SemiAuto();
                break;
            case GunType.Burst:
                StartCoroutine(Burst());
                break;
            case GunType.Auto:
                SemiAuto();
                break;
            default:
                SemiAuto();
                break;
        }
    }

    public void SingleShot()
    {
        Ray ray = new Ray(barrel.position, barrel.forward);
        RaycastHit hit;
        float rayLength = (Physics.Raycast(ray, out hit)) ? hit.distance : maxRayLength;
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.yellow, 0.1f);
    }

    public void SemiAuto()
    {
        SingleShot();
    }

    public IEnumerator Burst ()
    {
        // This works for now, but deferring should not be done like this
        // This will make the fire rate of the weapon dependent on the frame rate of the game.
        SingleShot();
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        SingleShot();
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        SingleShot();
    }
}
