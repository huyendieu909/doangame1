using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2Movement : Movement
{
    float h, v;
    Animator anim;
    Rigidbody2D rb2d;
    WeaponController wc;
    public int defaultSpeed = 5;

    void Start () {
        //wc của player nằm trong gunport
        wc = GetComponentInChildren<WeaponController>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        
        if (h != 0 && !isMoving) {
            StartCoroutine(MoveHorizontal(h, rb2d));
            anim.SetBool("isRunning", true);
        }
        else if (v != 0 && !isMoving) {
            StartCoroutine(MoveVertical(v, rb2d));
            anim.SetBool("isRunning", true);
        }
        
        anim.SetBool("isRunning", false);
    }
    void Update () {

        h = 0;
        v = 0;

        if (Input.GetKey(KeyCode.J)){ h = -1; v = 0;}
        if (Input.GetKey(KeyCode.L)){ h = 1; v = 0;}
        if (Input.GetKey(KeyCode.K)){ v = -1; h = 0;}
        if (Input.GetKey(KeyCode.I)){ v = 1; h = 0;}

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            wc.Fire();
        }
    }
}
