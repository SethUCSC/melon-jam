using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class CaptiveScript : MonoBehaviour
{
    public Image influenceBar;
    public CharmManager charm;
    public float influenceTimer = 0f;
    public float requiredTime = 6f;
    public int chance;
    public bool stillCaptive = true;
    public bool ally = false;
    public bool remainCaptive = false;
    public bool enemy = false;
    public float pathingRadius = 20;
    private bool enemyDetected;
    private Transform target;
    [SerializeField] public int numShots = 5;
    [SerializeField] public float rotationAngle = 15f;
    [SerializeField] private Transform projectile;

    IAstarAI ai;

    void Start()
    {
        ai = GetComponent<IAstarAI>();   
        chance = Random.Range(0f, 100f);
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
        
        if (influenceTimer > 0 && stillCaptive) 
        {
            influenceTimer -= Time.deltaTime;
            influenceBar.fillAmount = influenceTimer / 5f;
        }

        if (influenceTimer > 5)
        {
            stillCaptive = false;
            ally = true;
            if (!ai.pathPending && (ai.reachedEndOfPath || !ai.hasPath)) {
                ai.destination = PickRandomPoint();
                ai.SearchPath();
            }
            // if (chance > 50 - (charm.allyCount * 2)) ally = true;
            // if (chance > 25 - charm.allyCount && chance < 50 - (charm.allyCount * 2)) remainCaptive = true;
            // if (chance > 0 && chance < 25 - charm.allyCount) enemy = true;
        }
        
    }
    
    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Aura")) && stillCaptive)
        {
            if (influenceTimer < requiredTime) influenceTimer += Time.deltaTime;
            influenceBar.fillAmount = influenceTimer / 5f;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("Enemy Projectile")) && !enemyDetected && ally)
        {
            enemyDetected = true;
            target = other.transform;
            InvokeRepeating("ShootBullet", 1f, 1f);
            ai.destination = target.position;
        }
    }

    void ShootBullet() {
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

    // private void OnTriggerExit(Collider other)
    // {
    //     if ((other.CompareTag("Aura")))
    //         if (influenceTimer > 0) influenceTimer -= Time.deltaTime;
    // }
}
