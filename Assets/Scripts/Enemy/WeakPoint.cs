using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("WeakPoint")){
            Debug.Log("WeeakPoint");
            Destroy(other.gameObject);
        }
    }
}
