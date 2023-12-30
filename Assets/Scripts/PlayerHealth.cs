using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 10f;
    public bool enemyHit = false;
    public bool isPlayer;
    public bool playerDead = false;

    
    public Image healthBar;
    public float damage = 1;
    // Start is called before the first frame update
    [Header("Health Variables")]
    public bool canDamage = true;
    public bool isHit = false;
    public bool hasSparked = false;
    public float maxHealth;
    public float damage_reduction = 10f;
    public float totalDamage = 10f;
    public float invul_dur = 2f;
    public Vector2 offset;
    public GameObject gameOver;
    // public ParticleSystem spark;


    // public BoxCollider2D playerCollider;
    // public SpriteRenderer impact;
    public static event Action OnPlayerDamaged;

    void Update()
    {
        if (isPlayer && health <= 0)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            playerDead = true;
            gameOver.SetActive(true);
        }
        else if (!isPlayer && health <= 0) 
        {
            CharmManager charm = GameObject.FindWithTag("Player").GetComponent<CharmManager>();
            charm.charmScore ++;
            Destroy(gameObject);
        }
    }

    public void Restart()
    {
        // gameOver.SetActive(false);
        health = 20f;
        healthBar.fillAmount = 10f;
        Time.timeScale = 1f;
    }

    public IEnumerator HitPing()
    {
        isHit = true;
        Debug.Log("Damaged!!!");
        yield return new WaitForSeconds(0.15f);
        isHit = false;
    }

    public IEnumerator Damaged()
    {
        canDamage = false;
        health -= damage;
        healthBar.fillAmount = health / 10f;
    
        yield return new WaitForSeconds(0.15f);
        canDamage = true;
    }

    public void TakeDamage(float amount){
        health -= amount;
        OnPlayerDamaged?.Invoke();
    }
}
