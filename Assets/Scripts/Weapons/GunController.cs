﻿using System.Collections;
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

    [Header("Bullet Fire Vars")]
    public float range = 10f;
    public float autoFireRate = 0.2f;
    public float burstFireRate = 0.1f;
    public FireModes fireMode;
    public float fireCooldownTime = 0.5f;
    public bool canFire = false;
    public bool shooting = false;
    public int damage = 20;

    [Header("Ammunation Vars")]
    public int currentAmmoInClip;
    public int currentExtraAmmoAmount;
    public int ammoClipCapacity = 15;
    public int maxExtraAmmoAmount = 75;

    private void Awake()
    {
        lineRenderer.useWorldSpace = true;
        StartCoroutine(StartFireCooldown());
    }

    void Update()
    {
        CheckShootInput();
    }

    IEnumerator Shoot()
    {
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
        StopCoroutine(Shoot());
        StartCoroutine(Shoot());
        yield return new WaitForSeconds(burstFireRate);
        StopCoroutine(Shoot());
        StartCoroutine(Shoot());
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
        switch(fireMode)
        {
            case FireModes.Semi:
                if (Input.GetButtonDown("Fire1") && canFire)
                {
                    StopCoroutine(StartFireCooldown());
                    StartCoroutine(StartFireCooldown());

                    StopCoroutine(Shoot());
                    StartCoroutine(Shoot());
                }
                break;
            case FireModes.Burst:
                if (Input.GetButtonDown("Fire1") && canFire)
                {
                    StopCoroutine(StartFireCooldown());
                    StartCoroutine(StartFireCooldown());

                    StopCoroutine(BurstShoot());
                    StartCoroutine(BurstShoot());
                }
                break;
            case FireModes.Auto:
                if (Input.GetButton("Fire1") && canFire)
                {
                    StopCoroutine(AutoShoot());
                    StartCoroutine(AutoShoot());
                }
                break;
        }
    }
}