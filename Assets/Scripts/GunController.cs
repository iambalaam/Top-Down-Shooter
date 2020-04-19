using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class GunController : MonoBehaviour
{
    public enum GunType { SemiAuto, Burst, Auto };
    private LineRenderer tracer;
    private AudioSource audioData;
    public AudioClip gunshot;
    public AudioClip hitMarker;

    private float MAX_RAY_LENGTH = 30f;
    private float roundsPerMinute = 400f;
    private bool isReloading = false;
    private bool hasRoundInChamber = true;
    private float secondsBetweenRounds;
    private float timeToNextRoundInChamber;


    // Connected via unity editor
    public GunType gunType;
    public Transform barrel;

    public void Start()
    {
        tracer = GetComponent<LineRenderer>();
        tracer.enabled = false;
        audioData = GetComponent<AudioSource>();
        secondsBetweenRounds = 60f / roundsPerMinute;
    }

    public void Shoot()
    {
        switch (gunType)
        {
            case GunType.SemiAuto:
                SemiAuto();
                break;
            case GunType.Burst:
                break;
            case GunType.Auto:
                break;
            default:
                SemiAuto();
                break;
        }
    }

    public void SingleShot()
    {
        if (!isReloading && hasRoundInChamber)
        {
            // Shoot
            Ray ray = new Ray(barrel.position, barrel.forward);
            RaycastHit raycastHit;
            bool hit = Physics.Raycast(ray, out raycastHit);
            float rayLength = hit ? raycastHit.distance : MAX_RAY_LENGTH;
            StartCoroutine(RenderTracer(barrel.position, barrel.position + (ray.direction * rayLength)));
            audioData.PlayOneShot(gunshot);
            if (hit)
            {
                audioData.PlayOneShot(hitMarker);
            }
            // Queue next round
            hasRoundInChamber = false;
            timeToNextRoundInChamber = secondsBetweenRounds;
        }
    }

    public void SemiAuto()
    {
        SingleShot();
    }

    IEnumerator RenderTracer(Vector3 start, Vector3 end)
    {
        tracer.SetPosition(0, start);
        tracer.SetPosition(1, end);
        tracer.enabled = true;
        yield return null;
        tracer.enabled = false;
    }

    private void Update()
    {
        if (!hasRoundInChamber && timeToNextRoundInChamber <= 0)
        {
            hasRoundInChamber = true;
        } else
        {
            timeToNextRoundInChamber -= Time.deltaTime;
        }
    }
}
