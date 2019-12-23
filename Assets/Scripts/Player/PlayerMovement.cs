using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAppearance playerAppearance;

    public float movementSpeed = 0.007f; // 0.007 combined with runMultiplier at 3 I find is a good combination
    public float speedMultiplier = 1;
    public float runMultiplier = 3; // 3 I find good

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

        SetMovingState();
        SetPlayerRotation();
        SetMaxMovementSpeedSumTo1();

        movementX *= speedMultiplier;
        movementY *= speedMultiplier;
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

    void SetMovingState()
    {
        if (movementX == 0 && movementY == 0)
        {
            isWalking = false;
            isRunning = false;
            speedMultiplier = 1;

            playerAppearance.SetAnimationState("IsIdle");
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            isWalking = false;
            speedMultiplier = runMultiplier;

            playerAppearance.SetAnimationState("IsRunning");
        }
        else
        {
            isWalking = true;
            isRunning = false;
            speedMultiplier = 1;

            playerAppearance.SetAnimationState("IsWalking");
        }
    }
}
