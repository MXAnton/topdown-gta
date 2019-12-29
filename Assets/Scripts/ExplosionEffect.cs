using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip explosionSound;

    public float explosionPower;
    public float maxExplosionPower = 10f;
    public float explosionRadius = 10f;

    public int maxExplosionDamage = 50;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(explosionSound);

        explosionPower = maxExplosionPower;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        int i = 0;
        float distanceToHit;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i] != null)
            {
                distanceToHit = Vector3.Distance(transform.position, hitColliders[i].gameObject.transform.position) * 2;
                //Debug.Log(distanceToHit);

                StartCoroutine(GiveDamage(hitColliders[i], distanceToHit));

                hitColliders[i].gameObject.GetComponent<Rigidbody2D>().AddForce(-hitColliders[i].gameObject.transform.up * explosionPower / distanceToHit, ForceMode2D.Impulse);
                i++;
            }
        }

        StartCoroutine(FadeOutForce(hitColliders));
    }

    IEnumerator FadeOutForce(Collider2D[] hits)
    {
        yield return new WaitForSeconds(0.4f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.3f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.2f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.1f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.05f);
        int i = 0;
        while (i < hits.Length)
        {
            if (hits[i] != null)
            {
                hits[i].gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                hits[i].gameObject.GetComponent<Rigidbody2D>().angularDrag = 0;
            }
            i++;
        }
    }

    void DivideAllVelocityAndAngularDrag(Collider2D[] hits)
    {
        int i = 0;
        float distanceToHit;
        while (i < hits.Length)
        {
            if (hits[i] != null)
            {
                distanceToHit = Vector3.Distance(transform.position, hits[i].gameObject.transform.position) * 2;
                //Debug.Log(distanceToHit);

                hits[i].gameObject.GetComponent<Rigidbody2D>().velocity = hits[i].gameObject.GetComponent<Rigidbody2D>().velocity / 3 / distanceToHit;
                hits[i].gameObject.GetComponent<Rigidbody2D>().angularDrag = hits[i].gameObject.GetComponent<Rigidbody2D>().angularDrag / 3 / distanceToHit;
            }
            i++;
        }
    }

    IEnumerator GiveDamage(Collider2D hit, float hitDistance)
    {
        yield return new WaitForSeconds(0.2f);

        ObjectPrefs objectPrefs = hit.GetComponent<ObjectPrefs>();
        if (objectPrefs != null)
        {
            int explosionDamage = Mathf.RoundToInt(maxExplosionDamage / hitDistance);
            objectPrefs.TakeDamage(explosionDamage);
        }
    }
}
