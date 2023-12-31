using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public GameObject[] demons;

    // Update is called once per frame
    void Update()
    {
        demons = GameObject.FindGameObjectsWithTag("Enemy");
        if (demons.Length <= 3) Destroy(gameObject);
    }
}
