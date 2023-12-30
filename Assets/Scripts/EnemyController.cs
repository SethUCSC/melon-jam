using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float fireRate = 1f;
    [SerializeField] public float fireDelay = 1f;
    [SerializeField] public int numShots = 5;
    [SerializeField] public int numVerticalStacks = 1;
    [SerializeField] public Vector3 verticalStackOffset = new Vector3(0, 1, 0);
    [SerializeField] public float rotationAngle = 15f;
    [SerializeField] public Vector3 shootOffset = new Vector3(0, 1, 0);
    [SerializeField] private Transform projectile;
    [SerializeField] private Transform idleLocation;
    [SerializeField] private Transform secondIdleLocation;
    private Transform nextLocation;
    private Transform target = null;
    private bool isShooting = false;
    private bool enemyDetected = false;
    private float oscillatingAngle;
    private int counter = -10;
    private Animator animator;
    IAstarAI ai;
    void Start()
    {
        animator = GetComponent<Animator>();
        nextLocation = idleLocation;
        ai = GetComponent<IAstarAI>();
        ai.destination = nextLocation.position;
        ai.SearchPath();
    }

    // Update is called once per frame
    void Update()
    {  
        
        RaycastHit hit;
        if (target != null && enemyDetected) {
            Vector3 direct = target.position - transform.position;
            if (Physics.Raycast(transform.position, direct, out hit))
            {
                if (hit.collider.CompareTag("Obstacle") && isShooting)
                {
                    CancelInvoke("ShootBullet");
                    isShooting = false;
                }
                else if (hit.collider.CompareTag("Obstacle") != true && !isShooting)
                {
                    InvokeRepeating("ShootBullet", fireDelay, fireRate);
                    isShooting = true;
                }
            }
            ai.destination = target.position;
            ai.SearchPath();
            animator.SetBool("isWalking", true);
        }
        // idle state - ai paths between two points
        else if (ai.remainingDistance < 0.12 && secondIdleLocation != null) {
            if (nextLocation == idleLocation) {
                nextLocation = secondIdleLocation;
            } else {
                nextLocation = idleLocation;
            }
            ai.destination = nextLocation.position;
            ai.SearchPath(); 
            animator.SetBool("isWalking", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ally"))
        {
            enemyDetected = true;
            target = other.transform;
        }
        else if (other.CompareTag("Player Projectile") || other.CompareTag("Ally Projectile"))
        {
            enemyDetected = true;
            target = other.GetComponent<Projectile>().shooter;
        }
    }

    void ShootBullet() {
        float wave = counter * 8;
        if (target != null) {
            for (int j = 0; j < numVerticalStacks; j++) {
                Vector3 firingPosition = transform.position + shootOffset + verticalStackOffset*j;
                Transform projectileTransform = Instantiate(projectile, firingPosition, Quaternion.identity);
                Vector3 shootDirection = (target.position - transform.position).normalized;

                Quaternion rotationQuaternion = Quaternion.Euler(0f, wave, 0f);
                Vector3 rotatedVector = rotationQuaternion * shootDirection;
                projectileTransform.GetComponent<Projectile>().Setup(rotatedVector, transform);

                for (int i = 1; i < numShots; i++){
                    Transform projTransform = Instantiate(projectile, firingPosition, Quaternion.identity);
                    
                    float newAngle = wave + rotationAngle * i;
                    rotationQuaternion = Quaternion.Euler(0f, newAngle, 0f);
                    rotatedVector = rotationQuaternion * shootDirection;
                    
                    projTransform.GetComponent<Projectile>().Setup(rotatedVector, transform);
                }

                for (int i = 1; i < numShots; i++){
                    Transform projTransform = Instantiate(projectile, firingPosition, Quaternion.identity);
                    
                    float newAngle = -rotationAngle * i + wave;
                    rotationQuaternion = Quaternion.Euler(0f, newAngle, 0f);
                    rotatedVector = rotationQuaternion * shootDirection;

                    
                    projTransform.GetComponent<Projectile>().Setup(rotatedVector, transform);
                }
            }
            if (counter < 10) {
                counter++;
            } else {
                counter = -10;
            }
        }
    }

}
