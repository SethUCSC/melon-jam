using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public int numShots = 5;
    [SerializeField] public float rotationAngle = 15f;
    [SerializeField] private Vector3 shootOffset = new Vector3(0, 1, 0);
    [SerializeField] private Transform projectile;
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform idleLocation;
    [SerializeField] private Transform secondIdleLocation;
    private Transform nextLocation;
    private bool playerDetected;
    private bool isShooting;
    IAstarAI ai;
    void Start()
    {
        nextLocation = idleLocation;
        ai = GetComponent<IAstarAI>();
        ai.destination = nextLocation.position;
        ai.SearchPath();
    }

    // Update is called once per frame
    void Update()
    {  
        RaycastHit hit;
        if (enemy != null && playerDetected) {
            Vector3 direct = enemy.position - transform.position;
            if (Physics.Raycast(transform.position, direct, out hit))
            {
                if (hit.collider.CompareTag("Obstacle") && isShooting)
                {
                    CancelInvoke("ShootBullet");
                    isShooting = false;
                }
                else if (hit.collider.CompareTag("Obstacle") != true && !isShooting)
                {
                    InvokeRepeating("ShootBullet", 0.1f, 1f);
                    isShooting = true;
                }
            }
            ai.destination = enemy.position;
            ai.SearchPath(); 
        }
        else if (ai.remainingDistance < 0.12) {
            if (nextLocation == idleLocation) {
                nextLocation = secondIdleLocation;
            } else {
                nextLocation = idleLocation;
            }
            ai.destination = nextLocation.position;
            ai.SearchPath(); 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player") || other.CompareTag("Player Projectile")) && !playerDetected)
        {
            playerDetected = true;
            if (!isShooting) {
                InvokeRepeating("ShootBullet", 1f, 1f);
                isShooting = true;
            }
        }
    }

    void ShootBullet() {
        if (enemy != null) {
            Transform projectileTransform = Instantiate(projectile, transform.position+shootOffset, Quaternion.identity);
            Vector3 shootDirection = (enemy.position - transform.position).normalized;
            projectileTransform.GetComponent<Projectile>().Setup(shootDirection, transform);

            for (int i = 1; i < numShots; i++){
                Transform projTransform = Instantiate(projectile, transform.position+shootOffset, Quaternion.identity);
                
                float newAngle = rotationAngle * i;
                Quaternion rotationQuaternion = Quaternion.Euler(0f, newAngle, 0f);
                Vector3 rotatedVector = rotationQuaternion * shootDirection;
                
                projTransform.GetComponent<Projectile>().Setup(rotatedVector, transform);
            }

            for (int i = 1; i < numShots; i++){
                Transform projTransform = Instantiate(projectile, transform.position+shootOffset, Quaternion.identity);
                
                float newAngle = -rotationAngle * i;
                Quaternion rotationQuaternion = Quaternion.Euler(0f, newAngle, 0f);
                Vector3 rotatedVector = rotationQuaternion * shootDirection;

                
                projTransform.GetComponent<Projectile>().Setup(rotatedVector, transform);
            }
        }
    }

}
