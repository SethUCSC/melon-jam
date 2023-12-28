using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 7f;
    public float speed = 5f;

    bool canDamage = true;
    Transform player;

    private void Start()
    {
        Destroy(gameObject, lifetime); //destroys after lifetime to prevent mem leaks
        player = GameObject.FindGameObjectWithTag("Player").transform;
        canDamage = true;
    }

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        var dir = player.position - transform.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
            StartCoroutine(playerHealth.Damaged());
            Destroy(gameObject);
        }
        else if (collider.CompareTag("Obstacle")) Destroy(gameObject);
    }
}
