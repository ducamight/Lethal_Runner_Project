using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpKill : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("WeakPoint")){
            Destroy(other.gameObject);
            Debug.Log("WeakPoint");
        }
    }
}
