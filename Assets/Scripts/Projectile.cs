using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lifetime = 7f;
    public bool isPlayerProjectile;
    public bool isAllyProjectile;
    private Vector3 shootDirection;
    public Transform shooter;

    public void Setup(Vector3 shootDirection, Transform shooter) {
        this.shootDirection = shootDirection;
        this.shooter = shooter;
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
        Debug.Log(other);
        if ((other.CompareTag("Enemy") && isPlayerProjectile))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(playerHealth.Damaged());
            Destroy(gameObject);
        }
        else if ((other.CompareTag("Enemy") && isAllyProjectile))
        {
            PlayerHealth enemyHealth = other.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(enemyHealth.Damaged());
            Destroy(gameObject);
        }
        else if ((other.CompareTag("Player") && !isPlayerProjectile))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(playerHealth.Damaged());
            Destroy(gameObject);
        }
        else if ((other.CompareTag("Ally") && !isPlayerProjectile && !isAllyProjectile))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(playerHealth.Damaged());
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle")) Destroy(gameObject);
        else if (other.CompareTag("Cage")) Destroy(gameObject);
    }
}
