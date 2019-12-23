using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAppearance playerAppearance;

    public float movementSpeed = 0.05f;
    public float runMultiplier = 2;

    float movementX;
    float movementY;
    float rotationZ;

    public bool isRunning = false;
    public bool isWalking = false;

    void Start()
    {
        playerAppearance = GetComponentInChildren<PlayerAppearance>();
    }

    void Update()
    {
        movementX = Input.GetAxis("Horizontal");
        movementY = Input.GetAxis("Vertical");
        if (movementX != 0 || movementY != 0)
        {
            isWalking = true;
            playerAppearance.SetAnimationState("IsWalking");
        }
        else
        {
            isWalking = false;
            playerAppearance.SetAnimationState("IsIdle");
        }

        SetPlayerRotation();
        SetMaxMovementSpeedSumTo1();

        transform.position = new Vector2(transform.position.x + movementX * movementSpeed, transform.position.y + movementY * movementSpeed);
    }

    void SetMaxMovementSpeedSumTo1()
    {
        if (movementX > 0.5f || movementX < -0.5f)
        {
            if (movementY > 0.5f || movementY < -0.5f)
            {
                if (movementX < 0)
                {
                    movementX = -0.5f;
                }
                else
                {
                    movementX = 0.5f;
                }

                if (movementY < 0)
                {
                    movementY = -0.5f;
                }
                else
                {
                    movementY = 0.5f;
                }
            }
        }
    }

    void SetPlayerRotation()
    {
        float oneStep = 45;

        if (movementY < 0)
        {
            rotationZ = 180;

            if (movementX < 0)
            {
                rotationZ += oneStep;
            }
            else if (movementX > 0)
            {
                rotationZ -= oneStep;
            }
        }
        else if (movementY > 0)
        {
            rotationZ = 0;

            if (movementX < 0)
            {
                rotationZ -= oneStep;
            }
            else if (movementX > 0)
            {
                rotationZ += oneStep;
            }
        }
        else if (movementX < 0)
        {
            rotationZ = -oneStep * 2;
        }
        else if (movementX > 0)
        {
            rotationZ = oneStep * 2;
        }

        //Debug.Log(rotationZ);

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -rotationZ);
    }
}
