using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInOutVehicle : MonoBehaviour
{
    public float range = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckIfInVehicle();
        }
    }

    void CheckIfInVehicle()
    {
        if (transform.parent == null)
        {
            FindVehicleDoor();
        }
        else if (transform.parent.gameObject.tag == "VehicleDoor")
        {
            GetOutFromVehicle();
        }
    }

    void FindVehicleDoor()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, range);

        if (hitInfo && hitInfo.transform.gameObject.tag == "VehicleDoor")
        {
            //Debug.Log("Found a vehicle door");

            VehiclePlayers vehiclePlayers = hitInfo.transform.GetComponentInParent<VehiclePlayers>();

            if (vehiclePlayers)
            {
                vehiclePlayers.TryToSitInVehicle(gameObject, hitInfo.transform.GetComponent<VehicleDoorInfo>().vehicleDoorNumber);
            }
        }
        //else
        //{
        //    Debug.Log("Didn't find any vehicle door");
        //}
    }

    void GetOutFromVehicle()
    {
        //Debug.Log("Get out from vehicle");

        GetComponentInParent<CarController>().enabled = false;

        transform.parent = null;

        GetComponentInChildren<Animator>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;

        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}
