using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Movement Vars")]
    public float movementSpeed;
    public float accelerationSpeed = 0.01f;
    public float deccelerationSpeed = 0.004f;
    public float maxMovementSpeed = 0.05f;

    public float brakeTorque = 0.5f;

    [Header("Steering Vars")]
    public float maxSteering = 1f;
    public float steeringSensivity = 0.5f;
    public float steeringMovementSpeedMultiplier = 50f;

    void Start()
    {
        
    }

    void Update()
    {
        float throttleInput = GetThrottleInput();
        float steeringInput = GetSteeringInput();

        AccelerateDeccelerate(throttleInput);
        float steeringValue = SetSteering(steeringInput);

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                transform.localEulerAngles.y,
                                                transform.localEulerAngles.z - steeringValue);
        transform.position += transform.up * movementSpeed;
    }

    void AccelerateDeccelerate(float throttle)
    {
        if (throttle > 0.1f || throttle < -0.1f)
        {
            if (throttle < -0.1f && movementSpeed > 0)
            {
                movementSpeed += brakeTorque * throttle * Time.deltaTime;
            }
            else
            {
                movementSpeed += accelerationSpeed * throttle * Time.deltaTime;
            }

            if (movementSpeed > maxMovementSpeed)
            {
                movementSpeed = maxMovementSpeed;
            }
        }
        else
        {
            if (movementSpeed > 0)
            {
                movementSpeed -= deccelerationSpeed * Time.deltaTime;

                if (movementSpeed < 0)
                {
                    movementSpeed = 0;
                }
            }
            else if (movementSpeed < 0)
            {
                movementSpeed += deccelerationSpeed * Time.deltaTime;

                if (movementSpeed > 0)
                {
                    movementSpeed = 0;
                }
            }
        }
    }

    float SetSteering(float steering)
    {
        float newSteeringValue = 0;

        if (steering != 0 && movementSpeed != 0)
        {
            newSteeringValue = (steering * steeringSensivity) / (movementSpeed * steeringMovementSpeedMultiplier);

            if (newSteeringValue < -maxSteering)
            {
                newSteeringValue = -maxSteering;
            }
            else if (newSteeringValue > maxSteering)
            {
                newSteeringValue = maxSteering;
            }
        }

        return newSteeringValue;
    }

    float GetThrottleInput()
    {
        float newThrottleInput = Input.GetAxis("Vertical");
        
        if (newThrottleInput > -0.1f && newThrottleInput < 0.1f)
        {
            newThrottleInput = 0;
        }

        return newThrottleInput;
    }

    float GetSteeringInput()
    {
        float newSteeringInput = Input.GetAxis("Horizontal");

        if (newSteeringInput > -0.1f && newSteeringInput < 0.1f)
        {
            newSteeringInput = 0;
        }

        return newSteeringInput;
    }
}
