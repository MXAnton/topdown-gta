using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    private ObjectPrefs objectPrefs;
    private ItemPickup itemPickup;

    private GunController gunController;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI itemText;
    public TextMeshProUGUI ammoText;

    void Start()
    {
        objectPrefs = GetComponentInParent<ObjectPrefs>();
        itemPickup = GetComponentInParent<ItemPickup>();
    }

    void Update()
    {
        healthText.text = "" + objectPrefs.health;

        SetItemText();
        SetGunController();
        SetAmmoText();
    }

    void SetItemText()
    {
        if (itemPickup.itemName != null)
        {
            itemText.text = "" + itemPickup.itemName;

            if (itemPickup.itemName.Trim() == "")
            {
                itemText.text = "Empty hand";
            }
        }
        else
        {
            itemText.text = "Empty hand";
        }
    }

    void SetGunController()
    {
        if (itemPickup.item != null)
        {
            gunController = itemPickup.item.GetComponent<GunController>();
        }
        else
        {
            gunController = null;
        }
    }

    void SetAmmoText()
    {
        if (gunController != null)
        {
            ammoText.text = gunController.currentAmmoInClip + "/" + gunController.currentExtraAmmoAmount;
        }
        else
        {
            ammoText.text = "";
        }
    }
}
