using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatch : PowerUps
{
    GameObject[] enemies;
    GameObject enemyHolder;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GamePlayManager GPM = GameObject.Find("StageManager").GetComponent<GamePlayManager>();
        GPM.ActivateFreeze();
        Destroy(this.gameObject);
    }
}
