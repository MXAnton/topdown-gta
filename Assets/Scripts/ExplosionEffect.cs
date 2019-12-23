using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public float explosionPower;
    public float maxExplosionPower = 10f;
    public float explosionRadius = 10f;

    Vector2 point = new Vector2(0, 0);

    void Start()
    {
        explosionPower = maxExplosionPower;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        int i = 0;
        while (i < hitColliders.Length)
        {
            hitColliders[i].gameObject.GetComponent<Rigidbody2D>().AddForce(-hitColliders[i].gameObject.transform.up * explosionPower, ForceMode2D.Force);
            i++;
        }

        StartCoroutine(FadeOutForce(hitColliders));
    }

    IEnumerator FadeOutForce(Collider2D[] hits)
    {
        yield return new WaitForSeconds(1f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.6f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.5f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.3f);
        DivideAllVelocityAndAngularDrag(hits);

        yield return new WaitForSeconds(0.15f);
        hits[0].gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        hits[0].gameObject.GetComponent<Rigidbody2D>().angularDrag = 0;
    }

    void DivideAllVelocityAndAngularDrag(Collider2D[] hits)
    {
        int i = 0;
        while (i < hits.Length)
        {
            hits[i].gameObject.GetComponent<Rigidbody2D>().velocity = hits[i].gameObject.GetComponent<Rigidbody2D>().velocity / 2;
            hits[i].gameObject.GetComponent<Rigidbody2D>().angularDrag = hits[i].gameObject.GetComponent<Rigidbody2D>().angularDrag / 2;
            i++;
        }
    }
}
