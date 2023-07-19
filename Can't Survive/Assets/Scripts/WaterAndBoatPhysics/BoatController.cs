using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] float maxEnginePower;
    [SerializeField] float minEnginePower;
    [SerializeField] float acceleration;
    [SerializeField] float deceleration;
    [SerializeField] float brakePower;
    [SerializeField] float maxRotateSpeed;
    [SerializeField] float minRotateSpeed;
    float rotateSpeed;
    float enginePower;

    Rigidbody rb;
    float xInput, yInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BoatControl();
    }

    void BoatControl()
    {
        //get inputs
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxis("Vertical");

        bool isMoving = rb.velocity.z > 0.5f || rb.velocity.x > 0.5f || rb.velocity.z < 0.5f || rb.velocity.x < 0.5f;

        if (yInput > 0) MoveBoat(acceleration);
        else if (yInput == 0) StopBoat(deceleration);
        else if(yInput < 0) StopBoat(brakePower);
        

        if (xInput < 0 && isMoving)
        {
            Quaternion rot = Quaternion.LookRotation(-transform.right, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotateSpeed * Time.fixedDeltaTime);
        }else if (xInput > 0 && isMoving)
        {
            Quaternion rot = Quaternion.LookRotation(transform.right, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotateSpeed * Time.fixedDeltaTime);
        }
    }

    void MoveBoat(float force)
    {
        enginePower = Mathf.Clamp(enginePower, minEnginePower, maxEnginePower);
        enginePower += force;
        rotateSpeed = Mathf.Clamp(rotateSpeed, minRotateSpeed, maxRotateSpeed);
        rotateSpeed += force / 4;
        rb.AddForce(500f * enginePower * Time.fixedDeltaTime * transform.forward, ForceMode.Force);
    }

    void StopBoat(float force)
    {
        enginePower = Mathf.Clamp(enginePower, 0, maxEnginePower);
        enginePower -= force;
        rotateSpeed = Mathf.Clamp(rotateSpeed, 0, maxRotateSpeed);
        rotateSpeed -= force / 4;
        rb.AddForce(500f * enginePower * Time.fixedDeltaTime * transform.forward, ForceMode.Force);
    }
}
