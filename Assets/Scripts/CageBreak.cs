using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageBreak : MonoBehaviour
{
    public float cageHealth = 5f;
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        if (cageHealth <= 0) StartCoroutine(DestroyDoor());
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player Projectile")))
        {
            cageHealth --;
        }
    }

    public IEnumerator DestroyDoor()
    {
        GetComponent<Rigidbody>().mass = 0.01f;
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
