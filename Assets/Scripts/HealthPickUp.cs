using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    { // || (!isPlayer && other.CompareTag("Player Projectile"))
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.health += 3;
            playerHealth.healthBar.fillAmount = playerHealth.health / 10f;
            Destroy(gameObject);
        }
    }
}
