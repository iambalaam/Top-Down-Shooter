using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public enum GunType { SemiAuto, Burst, Auto };
    private LineRenderer tracer;
    private AudioSource audioData;
    private Light muzzleFlash;
    private float damage = 10;

    private float MAX_RAY_LENGTH = 30f;
    private float ROUNDS_PER_MINUTE = 700f;
    private int MAGAZINE_SIZE = 30;
    private int roundsLoaded;
    private bool isReloading = false;
    private bool hasRoundInChamber = true;
    private float secondsBetweenRounds;
    private float timeToNextRoundInChamber;


    // Connected via unity editor
    public GunType gunType;
    public Transform barrel;
    public Transform chamber;
    public AudioClip gunshot;
    public AudioClip hitMarker;
    public AudioClip reload;
    public Rigidbody shellCasing;

    public void Start()
    {
        tracer = GetComponent<LineRenderer>();
        tracer.enabled = false;
        muzzleFlash = GetComponent<Light>();
        muzzleFlash.enabled = false;
        audioData = GetComponent<AudioSource>();
        secondsBetweenRounds = 60f / ROUNDS_PER_MINUTE;
        roundsLoaded = MAGAZINE_SIZE;
    }

    public int GetAmmunition()
    {
        return roundsLoaded;
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        audioData.PlayOneShot(reload);
        yield return new WaitForSeconds(1f);
        roundsLoaded = roundsLoaded == 0 ? MAGAZINE_SIZE : MAGAZINE_SIZE + 1;
        isReloading = false;        
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
        if (roundsLoaded > 0 && !isReloading && hasRoundInChamber)
        {
            roundsLoaded--;
            // Shoot
            Ray ray = new Ray(barrel.position, barrel.forward);
            RaycastHit raycastHit;
            bool hit = Physics.Raycast(ray, out raycastHit);
            float rayLength = hit ? raycastHit.distance : MAX_RAY_LENGTH;
            if (hit)
            {
                Health health = raycastHit.collider.GetComponent<Health>();
                if (health)
                {
                    health.TakeDamage(damage, ray.direction * 500, ray.GetPoint(raycastHit.distance));
                }

            }
            // Render
            StartCoroutine(RenderTracer(barrel.position, barrel.position + (ray.direction * rayLength)));
            StartCoroutine(RenderMuzzleFlash());
            // Add shell ejection
            // TODO: Align the shell casing rotation with the gun
            Rigidbody newShellCasing = Instantiate(shellCasing, chamber.position, Quaternion.identity);
            newShellCasing.AddForce(chamber.forward * Random.Range(100f, 200f) + chamber.right * Random.Range(-10f, 10f));
            newShellCasing.AddRelativeTorque(new Vector3(-600, -1000, 0));
            // Play sound effect
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

    IEnumerator RenderMuzzleFlash()
    {
        muzzleFlash.enabled = true;
        yield return null;
        muzzleFlash.enabled = false;
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
