using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lifetime = 7f;
    public bool isPlayerProjectile;
    private Vector3 shootDirection;

    public void Setup(Vector3 shootDirection) {
        this.shootDirection = shootDirection;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += shootDirection * moveSpeed * Time.deltaTime; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Enemy") && isPlayerProjectile))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(playerHealth.Damaged());
            Destroy(gameObject);
        }
        else if ((other.CompareTag("Player") && !isPlayerProjectile))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(playerHealth.Damaged());
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle")) Destroy(gameObject);
    }
}
