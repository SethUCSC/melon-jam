using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsShake : MonoBehaviour
{
    public CameraShake cam;
    public bool canShake = true;
    public bool isNearPlayer = false;

    void Update()
    {
        if (isNearPlayer)
        {
            if(canShake) StartCoroutine(ShakeCamera());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player"))) isNearPlayer = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Player"))) isNearPlayer = false;
    }

    public IEnumerator ShakeCamera()
    {
        canShake = false;
        StartCoroutine(cam.Shake());
        yield return new WaitForSeconds(0.45f);
        canShake = true;
    }
}
