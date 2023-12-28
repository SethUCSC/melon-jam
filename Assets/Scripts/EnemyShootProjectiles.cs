using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootProjectiles : MonoBehaviour
{
    [SerializeField] public int numShots = 5;
    [SerializeField] public float rotationAngle = 15f;
    [SerializeField] private Transform projectile;
    [SerializeField] private Transform enemy;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ShootBullet", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
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
