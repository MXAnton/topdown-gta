using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public Vector3 pickedPosition;
    public Vector3 pickedRotation;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
            
    }
}
