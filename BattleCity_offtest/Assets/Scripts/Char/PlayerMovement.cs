using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Movement
{
    float h, v, hj, vj;
    Animator anim;
    Rigidbody2D rb2d;
    public Joystick joystick;
    // public PhotonView photonView;
    public Button fireButton;
    WeaponController wc;
    public int defaultSpeed = 5;

    void Awake()
    {
        if (joystick == null) {
            joystick = FindObjectOfType<Joystick>();
            if (joystick == null) Debug.Log("Không thấy joystick trong scene!");
        }  
        if (fireButton == null) {
            // fireButton = FindObjectOfType<Button>();
            fireButton = GameObject.Find("FireButton").GetComponent<Button>();
            if (fireButton == null) Debug.LogError("không thấy nút bắn!");
        }    
    }
    void Start () {
        //wc của player nằm trong gunport
        wc = GetComponentInChildren<WeaponController>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        fireButton.onClick.AddListener(() => wc.Fire());
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

        //Di chuyển = Joystick 
        else if (hj != 0 && !isMoving) {
            StartCoroutine(MoveHorizontal(hj, rb2d));
            anim.SetBool("isRunning", true);
        }
        else if (vj != 0 && !isMoving) {
            StartCoroutine(MoveVertical(vj, rb2d));
            anim.SetBool("isRunning", true);
        }
        
        anim.SetBool("isRunning", false);
    }
    void Update () {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        hj = joystick.Horizontal;
        vj = joystick.Vertical;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            wc.Fire();
        }
    }
}
