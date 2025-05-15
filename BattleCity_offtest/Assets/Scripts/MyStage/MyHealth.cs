using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHealth : MonoBehaviour
{
    [SerializeField]
    int actualHealth = 1;
    [SerializeField]
    int currentHealth;
    Animator anime;
    Rigidbody2D rb2d;
    bool divineIntervention;
    
    void Start()
    {
        SetHealth();    
        anime = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }	
    public void TakeDamage(int damage=1, bool destroyedByPowerUp=false)
    {
        divineIntervention = destroyedByPowerUp;
        currentHealth-=damage;
        if (currentHealth <= 0)
        {
            rb2d.velocity = Vector2.zero;
            anime.SetTrigger("killed");
        }
    }
    public void SetHealth()
    {
        currentHealth = actualHealth;
    }
    public void SetInvincible()
    {
        currentHealth = 1000;
        //đặt health về ban đầu. do sau khi hết animation vẫn bất tử.
        Invoke("SetHealth", 10);
    }
    void Death()
    {
        var mGPM = GameObject.Find("StageManager").GetComponent<MyGamePlayManager>();
        if (gameObject.CompareTag("PlayerTank"))
        {
            MasterTracker.playerLevel = 1;
            mGPM.SpawnPlayer();
        }
        else if (gameObject.CompareTag("EnemyTank")) {
            MasterTracker.player2Level = 1;
            mGPM.SpawnPlayer2();
        }
        else if (gameObject.CompareTag("AllyP1Tank")) {
            mGPM.SpawnAllyP1();
        }
        else if (gameObject.CompareTag("AllyP2Tank")) {
            mGPM.SpawnAllyP2();
        }
        else{
            if (!divineIntervention) {
                if (gameObject.CompareTag("Small")) MasterTracker.smallTanksDestroyed++;
                else if (gameObject.CompareTag("Fast")) MasterTracker.fastTanksDestroyed++;
                else if (gameObject.CompareTag("Big")) MasterTracker.bigTanksDestroyed++;
                else if (gameObject.CompareTag("Armored")) MasterTracker.armoredTanksDestroyed++;
            }
            // if (gameObject.GetComponent<BonusTank>().IsBonusTankCheck()) mGPM.GenerateBonusCrate();
        }
        Destroy(gameObject);
    }
}
