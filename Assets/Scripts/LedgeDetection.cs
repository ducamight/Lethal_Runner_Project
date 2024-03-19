using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Player player;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private bool canDetected;

    private void Update() {
        if(canDetected)
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);
            Debug.Log(player.ledgeDetected);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            canDetected = false;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            canDetected = true;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
