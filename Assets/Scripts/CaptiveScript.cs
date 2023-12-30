using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptiveScript : MonoBehaviour
{
    // charm variables
    public Image influenceBar;
    public CharmManager charm;
    public float influenceTimer = 0f;
    public float requiredTime = 6f;

    // captive variables
    public int chance;
    public bool charmScoreIncreased = false;
    public bool stillCaptive = true;
    public bool ally = false;
    public bool remainCaptive = false;
    public bool enemyAlly = false;

    void Start()
    {
        chance = Random.Range(0, 100);
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
            if (chance >= 50 - charm.charmScore * 3) {
                if (!charmScoreIncreased)
                {
                    charmScoreIncreased = true;
                    charm.charmScore ++;
                }
                ally = true;
                gameObject.tag = "Ally";
            }
            if (chance < 50 - charm.charmScore * 3) {
                enemyAlly = true;
                gameObject.tag = "Enemy";
            }
        
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
