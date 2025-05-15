using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUp : PowerUps
{
    protected override void Start()
    {
        base.Start();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MasterTracker.playerLives++;
        GamePlayManager GPM = GameObject.Find("StageManager").GetComponent<GamePlayManager>();
        GPM.UpdatePlayerLives();
        Destroy(this.gameObject);
    }
}
