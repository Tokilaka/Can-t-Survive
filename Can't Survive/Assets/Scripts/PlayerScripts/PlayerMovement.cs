using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float groundDrag = 5f;
    [SerializeField] float airMultiplier = 0.4f;
    float speed;

    [Header("KeyBinds")]
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;

    [Header("GroundCheck")]
    [SerializeField] float rayDistance;
    [SerializeField] LayerMask groundLayer;

    float xInput, yInput;
    Rigidbody playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (IsGrounded()) playerRb.drag = groundDrag;
        else playerRb.drag = 0;

        if (Input.GetKeyDown(jumpKey) && IsGrounded()) Jump();

    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        //get inputs
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        ChangeSpeed();
        //get dir
        Vector3 movement = new (xInput, 0, yInput);
        movement.Normalize();

        //move player
        if (IsGrounded()) playerRb.AddForce(500f * speed * Time.fixedDeltaTime * movement, ForceMode.Force);
        else playerRb.AddForce(500f * airMultiplier * speed * Time.fixedDeltaTime * movement, ForceMode.Force);


        //rotate player when moving
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * 100f * Time.deltaTime);
        }
    }

    public bool IsGrounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down, rayDistance, groundLayer)) return true;
        else return false;
    }

    void Jump()
    {
        playerRb.velocity = new(playerRb.velocity.x, 0f, playerRb.velocity.z);

        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void ChangeSpeed()
    {
        if (Input.GetKey(runKey)) speed = runSpeed;
        else speed = walkSpeed;
    }

    //debuger for ground check
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - rayDistance, transform.position.z));
    //}
}
