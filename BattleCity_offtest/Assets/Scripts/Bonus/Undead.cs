using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undead : PowerUps
{
    protected override void Start()
    {
        base.Start();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Animator>().SetTrigger("invincible");
        Destroy(this.gameObject);
    }
}
