using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptiveScript : MonoBehaviour
{
    public float influenceTimer = 0f;
    public float requiredTime = 6f;
    public float chance;
    public bool stillCaptive = true;
    public bool ally = false;
    public bool remainCaptive = false;
    public bool enemy = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (influenceTimer > 0 && stillCaptive) 
        {
            chance = Random.Range(0f, 100f);
            influenceTimer -= Time.deltaTime;
        }

        if (influenceTimer > 5)
        {
            stillCaptive = false;
            if (chance > 50) ally = true;
            if (chance > 25 && chance < 50) remainCaptive = true;
            if (chance > 0 && chance < 25) enemy = true;
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if ((other.CompareTag("Aura")) && stillCaptive)
        {
            if (influenceTimer < requiredTime) influenceTimer += Time.deltaTime;
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if ((other.CompareTag("Aura")))
    //         if (influenceTimer > 0) influenceTimer -= Time.deltaTime;
    // }
}
