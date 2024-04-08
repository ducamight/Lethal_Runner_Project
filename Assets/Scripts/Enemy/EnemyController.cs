using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 5f;

    private Vector2 startPos;
    [SerializeField] private Rigidbody2D rb;

    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (transform.position.x >= startPos.x + moveDistance)
            {
                Flip();
            }
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            if (transform.position.x <= startPos.x - moveDistance)
            {
                Flip();
            }
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
        Vector3 enemyScale = transform.localScale;
        enemyScale.x *= -1;
        transform.localScale = enemyScale;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<Player>().Damage();
        }
    }
}
