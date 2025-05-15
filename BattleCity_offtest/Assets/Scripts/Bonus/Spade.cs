using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spade : PowerUps
{
    GameObject grid;
    protected override void Start()
    {
        base.Start();
        grid = GameObject.Find("Grid");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GridMap gridmap = grid.GetComponent<GridMap>();
        gridmap.ActivateSpadePower();
        Destroy(this.gameObject);
    }
}
