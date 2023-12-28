using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public int numShots = 5;
    [SerializeField] public float rotationAngle = 15f;
    [SerializeField] private Transform projectile;
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform idleLocation;
    [SerializeField] private Transform secondIdleLocation;
    private Transform nextLocation;
    [SerializeField] public float sightRange = 5f;
    private bool playerDetected;
    void Start()
    {
        gameObject.GetComponent<AIDestinationSetter>().target = idleLocation;
    }

    // Update is called once per frame
    void Update()
    {  

    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Player Projectile")) && !playerDetected)
        {
            playerDetected = true;
            InvokeRepeating("ShootBullet", 1f, 1f);
            gameObject.GetComponent<AIDestinationSetter>().target = enemy;
        }
    }

    void ShootBullet() {
        Transform projectileTransform = Instantiate(projectile, transform.position, Quaternion.identity);
        Vector3 shootDirection = (enemy.position - transform.position).normalized;
        projectileTransform.GetComponent<Projectile>().Setup(shootDirection);

        for (int i = 1; i < numShots; i++){
            Transform projTransform = Instantiate(projectile, transform.position, Quaternion.identity);
            
            float newAngle = rotationAngle * i;
            Quaternion rotationQuaternion = Quaternion.Euler(0f, newAngle, 0f);
            Vector3 rotatedVector = rotationQuaternion * shootDirection;
            
            projTransform.GetComponent<Projectile>().Setup(rotatedVector);
        }

        for (int i = 1; i < numShots; i++){
            Transform projTransform = Instantiate(projectile, transform.position, Quaternion.identity);
            
            float newAngle = -rotationAngle * i;
            Quaternion rotationQuaternion = Quaternion.Euler(0f, newAngle, 0f);
            Vector3 rotatedVector = rotationQuaternion * shootDirection;

            
            projTransform.GetComponent<Projectile>().Setup(rotatedVector);
        }
    }

}
