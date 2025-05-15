using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
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
        var GPM = GameObject.Find("StageManager").GetComponent<GamePlayManager>();
        if (gameObject.CompareTag("PlayerTank"))
        {
            MasterTracker.playerLevel = 1;
            GPM.SpawnPlayer();
        }
        else if (gameObject.CompareTag("Player2Tank")) {
            MasterTracker.player2Level = 1;
            GPM.SpawnPlayer2();
        }
        else{
            if (!divineIntervention) {
                if (gameObject.CompareTag("Small")) MasterTracker.smallTanksDestroyed++;
                else if (gameObject.CompareTag("Fast")) MasterTracker.fastTanksDestroyed++;
                else if (gameObject.CompareTag("Big")) MasterTracker.bigTanksDestroyed++;
                else if (gameObject.CompareTag("Armored")) MasterTracker.armoredTanksDestroyed++;
            }
            if (gameObject.GetComponent<BonusTank>().IsBonusTankCheck()) GPM.GenerateBonusCrate();
        }
        Destroy(gameObject);
    }
}
