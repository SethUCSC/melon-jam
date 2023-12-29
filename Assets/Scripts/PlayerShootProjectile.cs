using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{
    public Camera cam;
    public PauseScript pause;
    public float bulletSpeed = 10;
    public bool canFire = true;
    [SerializeField] public int numShots = 5;
    [SerializeField] public float rotationAngle = 15f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform projectile;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canFire && !pause.paused) // 0 corresponds to the left mouse button
        {
            canFire = false;
            StartCoroutine(ShootBullet());
        }
    }

    public IEnumerator ShootBullet() {
        float x = Screen.width / 2;
        float y = Screen.height / 2;
                    
        Transform projectileTransform = Instantiate(projectile, transform.position + offset, Quaternion.identity);
        var ray = cam.ScreenPointToRay(new Vector3(x, y, 0));
        projectileTransform.GetComponent<Rigidbody>().velocity = ray.direction * bulletSpeed;
        yield return new WaitForSeconds(0.2f);
        canFire = true;
    }

    // void ShootBullet() {
    //     Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    //     RaycastHit hit;

    //     if (Physics.Raycast(ray, out hit))
    //     {
    //         // Spawn projectile at the shooting point
    //         Transform projectileTransform = Instantiate(projectile, transform.position + offset, Quaternion.identity);
    //         Vector3 shootDirection = (hit.point - transform.position).normalized;  
    //         projectileTransform.GetComponent<Projectile>().Setup(shootDirection);
    //     }
    // }
    
}
