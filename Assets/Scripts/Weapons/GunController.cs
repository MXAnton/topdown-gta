using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public enum FireModes
    {
        Semi,
        Burst,
        Auto
    };

    [Header("Components")]
    public Transform firePoint;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;

    private Animator animator;

    private AudioSource audioSource;
    public AudioClip gunShotSound;
    public AudioClip emptyClipSound;
    public AudioClip reloadSound;

    [Header("Bullet Fire Vars")]
    public float range = 10f;
    public float autoFireRate = 0.2f;
    public float burstFireRate = 0.1f;
    public FireModes fireMode;
    public float fireCooldownTime = 0.5f;
    public bool canFire = false;
    public int damage = 20;

    [Header("Ammunation Vars")]
    public int currentAmmoInClip;
    public int currentExtraAmmoAmount;
    public int ammoClipCapacity = 15;
    public int maxExtraAmmoAmount = 75;
    public float reloadTime = 2f;
    public bool reloading = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        lineRenderer.useWorldSpace = true;
        StartCoroutine(StartFireCooldown());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Fire1") && currentAmmoInClip == 0)
        {
            StartCoroutine(Reload());
        }

        CheckShootInput();
    }

    IEnumerator Shoot()
    {
        animator.SetTrigger("Shot");
        audioSource.PlayOneShot(gunShotSound);

        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);

        if (!hitInfo || hitInfo.distance > range)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * range);
        }
        else
        {
            ObjectPrefs objectPrefs = hitInfo.transform.GetComponent<ObjectPrefs>();
            if (objectPrefs != null)
            {
                objectPrefs.TakeDamage(damage);
            }

            Instantiate(impactEffect, hitInfo.point, Quaternion.identity);

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.01f);

        lineRenderer.enabled = false;

        currentAmmoInClip -= 1;
    }

    IEnumerator StartFireCooldown()
    {
        canFire = false;

        yield return new WaitForSeconds(fireCooldownTime);

        canFire = true;
    }

    IEnumerator BurstShoot()
    {
        StopCoroutine(Shoot());
        StartCoroutine(Shoot());
        yield return new WaitForSeconds(burstFireRate);

        if (currentAmmoInClip == 0)
        {
            StopCoroutine(Shoot());
            StartCoroutine(Shoot());
            yield return new WaitForSeconds(burstFireRate);
        }
        if (currentAmmoInClip == 0)
        {
            StopCoroutine(Shoot());
            StartCoroutine(Shoot());
        }
        yield return new WaitForSeconds(burstFireRate);
    }

    IEnumerator AutoShoot()
    {
        canFire = false;
        StopCoroutine(Shoot());
        StartCoroutine(Shoot());

        yield return new WaitForSeconds(autoFireRate);

        canFire = true;
    }

    void CheckShootInput()
    {
        if (currentAmmoInClip > 0 && canFire)
        {
            switch (fireMode)
            {
                case FireModes.Semi:
                    if (Input.GetButtonDown("Fire1"))
                    {
                        StopCoroutine(StartFireCooldown());
                        StartCoroutine(StartFireCooldown());

                        StopCoroutine(Shoot());
                        StartCoroutine(Shoot());
                    }
                    break;
                case FireModes.Burst:
                    if (Input.GetButtonDown("Fire1"))
                    {
                        StopCoroutine(StartFireCooldown());
                        StartCoroutine(StartFireCooldown());

                        StopCoroutine(BurstShoot());
                        StartCoroutine(BurstShoot());
                    }
                    break;
                case FireModes.Auto:
                    if (Input.GetButton("Fire1"))
                    {
                        StopCoroutine(AutoShoot());
                        StartCoroutine(AutoShoot());
                    }
                    break;
            }
        }
        else if (Input.GetButtonDown("Fire1") && currentAmmoInClip <= 0 && currentExtraAmmoAmount <= 0 && canFire)
        {
            Debug.Log("Clip is empty");
            PlayEmptyClipSound(audioSource, emptyClipSound);
        }
    }

    IEnumerator Reload()
    {
        if (currentExtraAmmoAmount <= 0)
        {
            Debug.Log("No extra reload ammo");
            yield break;
        }
        else if (currentAmmoInClip >= ammoClipCapacity)
        {
            Debug.Log("Ammo clip is already full");
            yield break;
        }

        if (reloading == false)
        {
            animator.SetTrigger("Reload");
            audioSource.PlayOneShot(reloadSound);
            reloading = true;
            canFire = false;
            yield return new WaitForSeconds(reloadTime / 2);

            int reloadAmount = ammoClipCapacity - currentAmmoInClip;
            if (reloadAmount > currentExtraAmmoAmount)
            {
                reloadAmount = currentExtraAmmoAmount;
            }
            currentAmmoInClip += reloadAmount;
            currentExtraAmmoAmount -= reloadAmount;

            yield return new WaitForSeconds(reloadTime / 2);
            canFire = true;
            reloading = false;
        }
    }

    public static void PlayEmptyClipSound(AudioSource audioSource, AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }
}
