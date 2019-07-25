using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float motorForce;
    public float breakForce;
    public float maxWheelAngle;

    //Les colliders des 4 roues
    public WheelCollider FR_wheel;//front right
    public WheelCollider FL_wheel;//front left
    public WheelCollider RR_wheel;//rear right
    public WheelCollider RL_wheel;//rear left

    //Les tranforms des roues de devant
    public Transform FR_wheel_body;
    public Transform FL_wheel_body;

    float v = 0f;
    float angle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        angle = maxWheelAngle * Input.GetAxis("Horizontal");
        v = motorForce * Input.GetAxis("Vertical");

        RR_wheel.motorTorque = v;
        RL_wheel.motorTorque = v;

        FR_wheel.steerAngle = angle;
        FR_wheel_body.localRotation = Quaternion.Euler(new Vector3(0,angle,90));
        FL_wheel.steerAngle = angle;
        FL_wheel_body.localRotation = Quaternion.Euler(new Vector3(0, angle,90));


        if (Input.GetKey(KeyCode.Space) || Input.GetAxis("Vertical") == 0)
        {
            RR_wheel.brakeTorque = breakForce;
            RL_wheel.brakeTorque = breakForce;
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetAxis("Vertical") != 0)
        {
            RR_wheel.brakeTorque = 0;
            RL_wheel.brakeTorque = 0;
        }


    }
}
