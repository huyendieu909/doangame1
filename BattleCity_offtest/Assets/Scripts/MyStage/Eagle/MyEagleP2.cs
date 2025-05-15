using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEagleP2 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("PlayerBullet"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
            MyGamePlayManager GPM = GameObject.Find("StageManager").GetComponent<MyGamePlayManager>();
            StartCoroutine(GPM.GameOverP1Win());
        }

    }
}
