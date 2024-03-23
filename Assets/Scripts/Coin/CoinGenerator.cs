using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private int coinsAmount;
    [SerializeField] GameObject coinPrefab;

    [SerializeField] private int minCoin;
    [SerializeField] private int maxCoin;

    [SerializeField] private float changeToSpawn;
    private void Start() {
        coinsAmount = Random.Range(minCoin, maxCoin);
        int additionalOffset = coinsAmount / 2;
    
        for (int i = 0; i < coinsAmount; i++){
            bool canSpawn = changeToSpawn > Random.Range(0, 100);
            Vector3 offset = new Vector3(i - additionalOffset, 0);
            if(canSpawn)
                Instantiate(coinPrefab, transform.position + offset, Quaternion.identity, transform);
        }
    }
}
