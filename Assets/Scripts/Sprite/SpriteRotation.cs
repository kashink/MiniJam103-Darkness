using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotation : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        this.player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 playerPos = this.player.transform.position - this.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, playerPos * -1, 0.1f, 0);
        newDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
