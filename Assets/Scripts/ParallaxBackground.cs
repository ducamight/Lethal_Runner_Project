using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    [SerializeField] private float parallaxEffect;
    private float lenght;
    private float xPosition;

    private void Start() {
        cam = GameObject.Find("Main Camera");

        lenght = GetComponent<SpriteRenderer>().bounds.size.x;

        xPosition = transform.position.x;
    }

    private void Update() {
        float distanceMoved = cam.transform.position.x *(1- parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if(distanceMoved > xPosition + lenght){
            xPosition = xPosition + lenght;
        }
    }
}
