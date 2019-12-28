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
            ItemPickup itemPickup = GetComponent<ItemPickup>();
            if (itemPickup != null)
            {
                if (itemPickup.item != null)
                {
                    StartCoroutine(itemPickup.ThrowItem());
                }
            }

            Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
