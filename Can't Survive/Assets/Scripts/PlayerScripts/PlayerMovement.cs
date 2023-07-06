using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rifleWalkSpeed;
    [SerializeField] float rifleRunSpeed;
    [SerializeField] float pistolWalkSpeed;
    [SerializeField] float pistolRunSpeed;
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
    PickUpSystem pickUp;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        pickUp = GetComponent<PickUpSystem>();
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
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        ChangeSpeed();

        //move player
        Vector3 movement = new (xInput, 0, yInput);
        movement.Normalize();

        if (IsGrounded())
        {
            playerRb.AddForce(500f * speed * Time.fixedDeltaTime * movement, ForceMode.Force);
        }
        else
        {
            playerRb.AddForce(500f * airMultiplier * speed * Time.fixedDeltaTime * movement, ForceMode.Force);
        }

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
        if(pickUp.hasRifle && pickUp.hasItemInHand)
        {
            if (Input.GetKey(runKey)) speed = rifleRunSpeed;
            else speed = rifleWalkSpeed;
        }
        else if(pickUp.hasPistol && pickUp.hasItemInHand)
        {
            if (Input.GetKey(runKey)) speed = pistolRunSpeed;
            else speed = pistolWalkSpeed;
        }
        else
        {
            if (Input.GetKey(runKey)) speed = runSpeed;
            else speed = walkSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - rayDistance, transform.position.z));
    }
}
