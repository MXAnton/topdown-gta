using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehiclePlayers : MonoBehaviour
{
    public GameObject[] vehiclePositions;

    public void TryToSitInVehicle(GameObject player, int position)
    {
        if (vehiclePositions[position].transform.childCount == 0)
        {
            //Debug.Log("Get in vehicle");

            player.GetComponentInChildren<Animator>().enabled = false;
            player.GetComponentInChildren<SpriteRenderer>().enabled = false;

            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Collider2D>().enabled = false;

            player.transform.parent = vehiclePositions[position].transform;
            player.transform.localPosition = new Vector2(0, 0);
            player.transform.localEulerAngles = new Vector3(0, 0, 0);

            if (vehiclePositions[0].transform.childCount != 0)
            {
                GetComponent<CarController>().enabled = true;
            }
        }
        //else
        //{
        //    Debug.Log("Position is not empty");
        //}
    }
}
