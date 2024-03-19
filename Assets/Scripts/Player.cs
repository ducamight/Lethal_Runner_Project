using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Movement Infor")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool playerUnlocked;
    private bool canDoubleJump;

    [Header("Slide Infor")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideCooldown;
    private float slideTimeCounter;
    private float slideCooldownCounter;
    private bool isSliding;

    [Header("Ledge Info")]
    public bool ledgeDetected;
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;
    private bool canGrabLedge = true;
    private bool canClimb;

    [Header("Collision Infor")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundDistance;
    [SerializeField] private  float ceillingCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    private bool wallCheckDetected;
    private bool isGrounded;
    private bool isRunning;
    private bool ceillingDetected;

    void Start()
    {
        rb = rb.GetComponent<Rigidbody2D>();
        anim = anim.GetComponent<Animator>();
    }
    void Update()
    {
        CheckCollision();
        AnimatorController();

        slideTimeCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if(playerUnlocked)
            Movement();

        CheckForLedge();
        CheckInput();
        SlideCheck();

        if(isGrounded)
            canDoubleJump = true;
    }

    
    

    private void AnimatorController(){
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimp", canClimb);
    }

    //MOVEMENT
    private void Movement(){
        if(wallCheckDetected)
            return;
        if(isSliding)
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void Jump(){
        if(isSliding)
            return;

        if(isGrounded){
            Debug.Log("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }else if(canDoubleJump){
            Debug.Log("Double Jump");
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
    }

    private void Slide(){
        if(rb.velocity.x != 0 && slideCooldownCounter < 0){
            Debug.Log("Slide");
            isSliding = true;
            slideTimeCounter = slideTime;
            slideCooldownCounter = slideCooldown;
        }
    }


    //CHECK
    private void CheckInput(){
        if(Input.GetMouseButtonDown(0)){
            playerUnlocked = true;
        }if(Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }if(Input.GetKeyDown(KeyCode.LeftShift))
            Slide();
    }
    private void SlideCheck(){
        if(slideTimeCounter < 0 && !ceillingDetected)
            isSliding = false;
    }
    private void CheckCollision(){
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, whatIsGround);
        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingCheckDistance, whatIsGround);
        wallCheckDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }
    private void CheckForLedge(){
        if(ledgeDetected && canGrabLedge){
            canGrabLedge = false;
            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;
            canClimb = true;
        }if(canClimb){
            transform.position = climbBegunPosition;
        }
    }

    //GIZMOS
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
