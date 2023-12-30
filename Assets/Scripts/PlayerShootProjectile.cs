using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{
    public Camera cam;
    public PauseScript pause;
    public AudioClip blastClip;
	public AudioSource blast;
    public float bulletSpeed = 10;
    public float fireRate = 0.2f;
    public bool canFire = true;
    [SerializeField] public int numShots = 5;
    [SerializeField] public float rotationAngle = 15f;
    [SerializeField] private Vector3 shootOffset = new Vector3(0, 1, 0);
    [SerializeField] private Transform projectile;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canFire && !pause.paused) // 0 corresponds to the left mouse button
        {
            canFire = false;
            StartCoroutine(ShootBullet());
        }

        if (pause.health.playerDead) gameObject.GetComponent<PlayerShootProjectile>().enabled = false;
    }

    public IEnumerator ShootBullet() {
        float x = Screen.width / 2;
        float y = Screen.height / 2;
                    
        Transform projectileTransform = Instantiate(projectile, transform.position + shootOffset, Quaternion.identity);
        var ray = cam.ScreenPointToRay(new Vector3(x, y, 0));
        projectileTransform.GetComponent<Projectile>().Setup(ray.direction * bulletSpeed, transform);
        blast.PlayOneShot(blastClip, 1f);
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}
