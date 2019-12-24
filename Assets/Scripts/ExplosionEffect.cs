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

    Vector2 point = new Vector2(0, 0);

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(explosionSound);

        explosionPower = maxExplosionPower;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        int i = 0;
        while (i < hitColliders.Length)
        {
            hitColliders[i].gameObject.GetComponent<Rigidbody2D>().AddForce(-hitColliders[i].gameObject.transform.up * explosionPower, ForceMode2D.Impulse);
            i++;
        }

        StartCoroutine(FadeOutForce(hitColliders));
    }

    IEnumerator FadeOutForce(Collider2D[] hits)
    {
        yield return new WaitForSeconds(0.5f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.4f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.3f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.2f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.1f);
        int i = 0;
        while (i < hits.Length)
        {
            hits[i].gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            hits[i].gameObject.GetComponent<Rigidbody2D>().angularDrag = 0;
            i++;
        }
    }

    void DivideAllVelocityAndAngularDrag(Collider2D[] hits)
    {
        int i = 0;
        while (i < hits.Length)
        {
            hits[i].gameObject.GetComponent<Rigidbody2D>().velocity = hits[i].gameObject.GetComponent<Rigidbody2D>().velocity / 3;
            hits[i].gameObject.GetComponent<Rigidbody2D>().angularDrag = hits[i].gameObject.GetComponent<Rigidbody2D>().angularDrag / 3;
            i++;
        }
    }
}
