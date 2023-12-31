using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAllyController : MonoBehaviour
{
    // gun variables
    [SerializeField] public int numShots = 5;
    [SerializeField] public float rotationAngle = 15f;
    [SerializeField] private Vector3 shootOffset = new Vector3(0, 1, 0);
    [SerializeField] private Transform projectile;
    private Transform target = null;
    private bool isShooting = false;
    private bool enemyDetected = false;
    private Animator animator;
    
    

    private CaptiveScript captiveScript;

    // ai variables
    public float pathingRadius = 20f;
    IAstarAI ai;

    void Start()
    {
            animator = GetComponent<Animator>();
            captiveScript = GetComponent<CaptiveScript>();
            ai = GetComponent<IAstarAI>();
    }

    Vector3 PickRandomPoint () {
        var point = Random.insideUnitSphere * pathingRadius;

        point.y = 0;
        point += ai.position;
        return point;
    }

    // Update is called once per frame
    void Update()
    {     
        RaycastHit hit;
        if (captiveScript.enemyAlly) {
            if (target != null && enemyDetected) {
                Vector3 direct = target.position - transform.position;
                if (Physics.Raycast(transform.position, direct, out hit)) {
                    if (hit.collider.CompareTag("Obstacle") && isShooting)
                    {
                        CancelInvoke("ShootBullet");
                        isShooting = false;
                    }
                    else if (hit.collider.CompareTag("Obstacle") != true && !isShooting)
                    {
                        Debug.Log(hit.collider.CompareTag("Obstacle"));
                        InvokeRepeating("ShootBullet", 2f, 1f);
                        isShooting = true;
                    }
                }
                ai.destination = target.position;
                ai.SearchPath();    
                animator.SetBool("isWalking", true);
            }
            // Idle state - chooses a random point and goes towards it
            else if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath) && target == null) {
                ai.destination = PickRandomPoint();
                ai.SearchPath();
                animator.SetBool("isWalking", true);
            }
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
        if (target != null) {
            Transform projectileTransform = Instantiate(projectile, transform.position+shootOffset, Quaternion.identity);
            Vector3 shootDirection = (target.position - transform.position).normalized;
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
