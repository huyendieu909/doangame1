using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet") || collision.gameObject.CompareTag("PlayerBullet"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(true);
            GamePlayManager GPM = GameObject.Find("StageManager").GetComponent<GamePlayManager>();
            StartCoroutine(GPM.GameOver());
        }

    }
}
