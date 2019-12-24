using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject itemHolder;
    public float itemPickupRange = 1;
    public LayerMask itemLayerMask;

    public GameObject item;

    [Header("Throw Item Vars")]
    public float throwPower = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ThrowIfFullElsePickup();
        }
    }

    void ThrowIfFullElsePickup()
    {
        if (item == null)
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, itemPickupRange, itemLayerMask);

            if (hitCollider != null)
            {
                item = hitCollider.gameObject;
                ItemController itemController = hitCollider.gameObject.GetComponent<ItemController>();
                hitCollider.enabled = false;

                itemController.rb2d.simulated = false;

                item.transform.parent = itemHolder.transform;
                item.transform.localPosition = itemController.pickedPosition;
                item.transform.localEulerAngles = itemController.pickedRotation;
            }
        }
        else
        {
            StartCoroutine(ThrowItem());
        }
    }

    IEnumerator ThrowItem()
    {
        Rigidbody2D itemRb2D = item.GetComponent<Rigidbody2D>();
        float currentThrowPower = throwPower;

        item.transform.parent = null;
        itemRb2D.simulated = true;
        item.GetComponent<Collider2D>().enabled = true;

        itemRb2D.velocity = item.transform.up * currentThrowPower;
        while (itemRb2D.velocity != Vector2.zero)
        {
            yield return new WaitForSeconds(0.01f);

            currentThrowPower -= Time.deltaTime * 10;
            if (currentThrowPower < 0)
            {
                currentThrowPower = 0;
            }

            itemRb2D.velocity = item.transform.up * currentThrowPower;
        }

        item = null;
    }
}
