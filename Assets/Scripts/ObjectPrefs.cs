using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrefs : MonoBehaviour
{
    public GameObject deathEffect;

    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
