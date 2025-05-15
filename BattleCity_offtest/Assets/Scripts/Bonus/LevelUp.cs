using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : PowerUps
{
    protected override void Start()
    {
        base.Start();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Player>().UpgradeTank();
        Destroy(this.gameObject);
    }
}
