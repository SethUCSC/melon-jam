using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBreak : MonoBehaviour
{
    public float cageHealth = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cageHealth <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player Projectile")))
        {
            cageHealth --;
        }
    }
}
