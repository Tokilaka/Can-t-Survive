using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;
    float speed;

    [Header("KeyBinds")]
    [SerializeField] KeyCode runKey = KeyCode.LeftShift;

    float xInput, yInput;
    Rigidbody playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        //change player speed 
        if (Input.GetKey(runKey)) speed = runSpeed;
        else speed = walkSpeed;

        //move player
        Vector3 movement = new (xInput, 0, yInput);
        movement.Normalize();

        playerRb.AddForce(10f * speed * movement, ForceMode.Force);

        //rotate player when moving
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * 100f * Time.deltaTime);
        }
    }
}
