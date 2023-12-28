using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Vector3 shootDirection;

    public void Setup(Vector3 shootDirection) {
        this.shootDirection = shootDirection;
    }

    void Update()
    {
        transform.position += shootDirection * moveSpeed * Time.deltaTime; 
    }
}
