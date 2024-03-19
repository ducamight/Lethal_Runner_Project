using System.Collections;
using System.Collections.Generic;
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

    [Header("Collision Infor")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundDistance;
    private bool isGrounded;
    private bool isRunning;
    private bool canDoubleJump;
    void Start()
    {
        rb = rb.GetComponent<Rigidbody2D>();
        anim = anim.GetComponent<Animator>();
    }
    void Update()
    {
        AnimatorController();

        if(playerUnlocked)
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        GroundCheck();
        CheckInput();
    }

    private void AnimatorController(){
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }
    private void GroundCheck(){
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, whatIsGround);
    }

    private void CheckInput(){
        if(Input.GetMouseButtonDown(0)){
            playerUnlocked = true;
        }if(Input.GetKeyDown(KeyCode.Space)){
            Jump();
        }
    }

    private void Jump(){
        if(isGrounded){
            Debug.Log("Jump");
            canDoubleJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }else if(canDoubleJump){
            Debug.Log("Double Jump");
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
    }
}
