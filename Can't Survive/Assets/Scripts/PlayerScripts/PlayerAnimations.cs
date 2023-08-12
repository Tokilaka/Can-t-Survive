using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    PlayerMovement player;
    [HideInInspector] public Animator animator;
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
        bool shooting = Input.GetKey(KeyCode.Mouse0);
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
        //shoot
        if (shooting && grounded) animator.SetBool("shooting", true);
        else animator.SetBool("shooting", false);


        velocity = Mathf.Clamp(velocity, 0f, 1f);

        animator.SetFloat("Default", velocity);
    }

    public void ChangeAnimation(Item recivedItem)
    {
        switch (recivedItem.animState)
        {
            case AnimState.Default:
                animator.SetBool("hasRifle", false);
                animator.SetBool("hasPistol", false);
                break;
            case AnimState.Pistol:
                animator.SetBool("hasPistol", true);
                animator.SetBool("hasRifle", false);
                break;
            case AnimState.Rifle:
                animator.SetBool("hasRifle", true);
                animator.SetBool("hasPistol", false);
                break;
            default:
                animator.SetBool("hasRifle", false);
                animator.SetBool("hasPistol", false);
                break;

        }
    }
}
