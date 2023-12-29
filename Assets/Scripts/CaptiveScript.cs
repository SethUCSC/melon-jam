using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Update is called once per frame
    void Update()
    {
        
        if (influenceTimer > 0 && stillCaptive) 
        {
            chance = Random.Range(0, 100);
            influenceTimer -= Time.deltaTime;
            influenceBar.fillAmount = influenceTimer / 5f;
        }

        if (influenceTimer > 5)
        {
            stillCaptive = false;
            if (chance > 50 - (charm.allyCount * 2)) ally = true;
            if (chance > 25 - charm.allyCount && chance < 50 - (charm.allyCount * 2)) remainCaptive = true;
            if (chance > 0 && chance < 25 - charm.allyCount) enemy = true;
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
}
