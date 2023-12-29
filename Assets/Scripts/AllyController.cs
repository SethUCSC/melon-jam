using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AllyController : MonoBehaviour
{
    // gun variables
    [SerializeField] public int numShots = 5;
    [SerializeField] public float rotationAngle = 15f;
    [SerializeField] private Transform projectile;
    private Transform target = null;
    private bool isShooting = false;

    private CaptiveScript captiveScript;

    // ai variables
    public float pathingRadius = 20f;
    IAstarAI ai;

    void Start()
    {
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
        if (captiveScript.ally) {
            if (target != null && !isShooting) {
                ai.destination = target.position;
                ai.SearchPath();
                InvokeRepeating("ShootBullet", 1f, 1f);
                isShooting = true;
            }
            else if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath) && target == null) {
                ai.destination = PickRandomPoint();
                ai.SearchPath();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Enemy")))
        {
            target = other.transform;
        }
    }

    void ShootBullet() {
        if (target != null) {
            Transform projectileTransform = Instantiate(projectile, transform.position, Quaternion.identity);
            Vector3 shootDirection = (target.position - transform.position).normalized;
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
}
