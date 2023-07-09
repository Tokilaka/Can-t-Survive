using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    Animator animator;
    PlayerMovement player;
    float velocity = 0.0f;
    [SerializeField] float accAndDec = 2f;
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        bool moving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;
        bool running = Input.GetKey(player.runKey);
        bool jump = Input.GetKeyDown(player.jumpKey);
        bool grounded = player.IsGrounded();

        //running
        if (moving && running && grounded)
        {
            velocity += accAndDec * Time.deltaTime;
        }
        //walking
        else if (moving && grounded)
        {
            if (velocity > 0.45f && velocity < 0.55f) velocity = 0.5f;
            else if (velocity > 0.5f) velocity -= accAndDec * Time.deltaTime;
            else velocity += accAndDec * Time.deltaTime;
        }
        //idle
        else if(grounded)
        {
            velocity -= accAndDec * Time.deltaTime;
        }
        //jump
        if (jump && grounded)
        {
            animator.SetTrigger("Jump");
        }

        velocity = Mathf.Clamp(velocity, 0f, 1f);

        animator.SetFloat("Default", velocity);
    }
}
