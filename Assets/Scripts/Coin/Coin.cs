using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null){
            GameManager.Instance.coin++;
            Destroy(gameObject);
        }
    }
}
