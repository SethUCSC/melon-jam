using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{
    [SerializeField] public int numShots = 5;
    [SerializeField] public float rotationAngle = 15f;
    [SerializeField] private Transform projectile;
    [SerializeField] private Transform enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 corresponds to the left mouse button
        {
            // Call your custom function here
            ShootBullet();
        }
    }

    void ShootBullet() {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Spawn projectile at the shooting point
            Transform projectileTransform = Instantiate(projectile, transform.position, Quaternion.identity);
            Vector3 shootDirection = (hit.point - transform.position).normalized;  
            projectileTransform.GetComponent<Projectile>().Setup(shootDirection);
        }
    }
}
