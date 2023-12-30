using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lifetime = 7f;
    public ParticleSystem impact;
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
        if ((other.CompareTag("Enemy") && isPlayerProjectile))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(playerHealth.Damaged());
            impact.Play();
            StartCoroutine(DelayedDestroy());
        }
        else if ((other.CompareTag("Enemy") && isAllyProjectile))
        {
            PlayerHealth enemyHealth = other.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(enemyHealth.Damaged());
            StartCoroutine(DelayedDestroy());
        }
        else if ((other.CompareTag("Player") && !isPlayerProjectile))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(playerHealth.Damaged());
            StartCoroutine(DelayedDestroy());
        }
        else if ((other.CompareTag("Ally") && !isPlayerProjectile && !isAllyProjectile))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(playerHealth.Damaged());
            StartCoroutine(DelayedDestroy());
        }
        else if ((other.CompareTag("Enemy Projectile") && (isPlayerProjectile || isAllyProjectile)))
        {
            Destroy(other.gameObject);
            StartCoroutine(DelayedDestroy());
        }
        else if (other.CompareTag("Obstacle")) StartCoroutine(DelayedDestroy());
        else if (other.CompareTag("Cage")) StartCoroutine(DelayedDestroy());
        // else StartCoroutine(DelayedDestroy());
    }

    public IEnumerator DelayedDestroy()
    {
        impact.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
