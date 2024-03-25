using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    [Header("Speed Infor")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultipler;
    [SerializeField] private float defaultSpeed;
    [Space]
    [SerializeField] private float milestoneIncrease;
    private float defaultMilestoneIncrease;
    private float speedMilestone;

    [Header("Movement Infor")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float speedUpSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool playerUnlocked;
    private bool canDoubleJump;

    [Header("Slide Infor")]
    //[SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideCooldown;
    private float slideTimeCounter;
    private float slideCooldownCounter;
    private bool isSliding;

    // [Header("Ledge Info")]
    // public bool ledgeDetected;
    // [SerializeField] private Vector2 offset1;
    // [SerializeField] private Vector2 offset2;
    // private Vector2 climbBegunPosition;
    // private Vector2 climbOverPosition;
    // private bool canGrabLedge = true;
    // private bool canClimb;

    [Header("Lading Infor")]
    [SerializeField] private float defaultGravity;
    [SerializeField] private float landingGravity;
    [SerializeField] private float heightToLanding;

    [Header("Knockback Infor")]
    [SerializeField] private Vector2 knockbackDirection;
    private bool isKnocked; 


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
    private bool isDead; 
    void Start()
    {
        rb = rb.GetComponent<Rigidbody2D>();
        anim = anim.GetComponent<Animator>();

        speedMilestone = milestoneIncrease;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncrease = milestoneIncrease;

        rb.gravityScale = defaultGravity;
    }
    void Update()
    {
        CheckCollision();
        AnimatorController();

        slideTimeCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if(isDead)
            return;

        if(isKnocked)
            return;
            
        if(playerUnlocked)
            Movement();

        SpeedController();
        //CheckForLedge();
        CheckInput();
        SlideCheck();
        Landing();

        if(isGrounded){
            canDoubleJump = true; 
            rb.gravityScale = defaultGravity;
        }
    }

#region SpeedControl
    private void SpeedReset(){
        moveSpeed = defaultSpeed;
        milestoneIncrease = defaultMilestoneIncrease;   
    }
    private void SpeedController(){
        if(moveSpeed == maxSpeed)
            return;
        
        if(transform.position.x > speedMilestone){
            speedMilestone = speedMilestone + milestoneIncrease;
            moveSpeed = moveSpeed * speedMultipler;
            milestoneIncrease = milestoneIncrease * speedMultipler;

            if(moveSpeed > maxSpeed)
                moveSpeed = maxSpeed;
        }
        if(Input.GetKeyDown(KeyCode.D))
            moveSpeed += speedUpSpeed;
        else if(Input.GetKeyUp(KeyCode.D))
            moveSpeed -= speedUpSpeed;
    }    
#endregion
    

#region AnimatorController
    private void AnimatorController(){
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("isKnocked", isKnocked);
        //anim.SetBool("canClimp", canClimb);

        if(rb.velocity.y < -20)
            anim.SetBool("canRoll", true);
    }
    
#endregion
    

#region Movement
    //MOVEMENT
    private void Movement(){
        if(wallCheckDetected){
            SpeedReset();
            return;
        }
        
        //if(isSliding)
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        // else
        //     rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
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

    private void Landing(){
        if(transform.position.y > heightToLanding){
            if(Input.GetKeyDown(KeyCode.S))
                rb.gravityScale = landingGravity;
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
    // private void LedgeClimbOver(){
    //     canClimb = false;
    //     transform.position = climbOverPosition;
    //     Invoke("AllowLedgeGrab", .1f);
    // }
    // private void AllowLedgeGrab() => canGrabLedge = true;
#endregion
    
    //Roll
    private void RollFinished()=>anim.SetBool("canRoll", false);

    //Knockback
    private void Knockback(){
        isKnocked = true;
        rb.velocity = knockbackDirection;
    }
    private void CancleKnockback() => isKnocked = false;

    private IEnumerator Die(){
        isDead = true;
        rb.velocity = knockbackDirection;
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(1f);
        rb.velocity = new Vector2(0, 0);
    }
    public void Damage(){
        if(moveSpeed >= maxSpeed)
            Knockback();
        else
            StartCoroutine(Die());
    }

    //CHECK
    private void CheckInput(){
        if(Input.GetMouseButtonDown(0))
            playerUnlocked = true;
        if(Input.GetKeyDown(KeyCode.Space))
            Jump();
        if(Input.GetKeyDown(KeyCode.LeftShift))
            Slide();
        if(Input.GetKeyDown(KeyCode.K))
            Knockback();
        if(Input.GetKeyDown(KeyCode.J) && !isDead)
            StartCoroutine(Die());
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
    // private void CheckForLedge(){
    //     if(ledgeDetected && canGrabLedge){
    //         canGrabLedge = false;
    //         Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;
    //         climbBegunPosition = ledgePosition + offset1;
    //         climbOverPosition = ledgePosition + offset2;
    //         canClimb = true;
    //     }if(canClimb){
    //         transform.position = climbBegunPosition;
    //     }
    // }

    //GIZMOS
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
