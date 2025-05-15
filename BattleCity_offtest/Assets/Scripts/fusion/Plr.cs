using System.Diagnostics;
using Fusion;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Plr : NetworkBehaviour
{
    float h, v, hj, vj;
    Animator anim;
    Rigidbody2D rb2d;
    public Joystick joystick;
    // public PhotonView photonView;
    public Button fireButton;
    WeaponControllerNet wc;

    void Awake()
    {
        if (joystick == null) {
            joystick = FindObjectOfType<Joystick>();
            if (joystick == null) print("Không thấy joystick trong scene!");
        }  
        if (fireButton == null) {
            fireButton = FindObjectOfType<Button>();
            if (fireButton == null) print("không thấy nút bắn!");
        }  
        //wc của player nằm trong gunport
        wc = GetComponentInChildren<WeaponControllerNet>();          
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    public override void Spawned()
    {
        base.Spawned();
        print("Spwawn");
    }

  public override void FixedUpdateNetwork()
  {
    if (GetInput(out NetworkInputData data)) {
        data.direction.Normalize();
        h = data.direction.x;
        v = data.direction.y;
        if (data.fire) wc.Fire();
    }

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
    public int speed = 5;
    protected bool isMoving = false;

    protected IEnumerator MoveHorizontal(float movementHorizontal, Rigidbody2D rb2d)
    {
        isMoving = true;

        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        Quaternion rotation = Quaternion.Euler(0, 0, -movementHorizontal * 90f);
        transform.rotation = rotation;
        
        float movementProgress = 0f;
        Vector2 movement, endPos;

        while (movementProgress < Mathf.Abs(movementHorizontal))
        {
            movementProgress += speed * Runner.DeltaTime;
            movementProgress = Mathf.Clamp(movementProgress, 0f, 1f);
            movement = new Vector2(speed * Runner.DeltaTime * movementHorizontal, 0f);
            endPos = rb2d.position + movement;

            if (movementProgress == 1) endPos = new Vector2(Mathf.Round(endPos.x), endPos.y);
            rb2d.MovePosition(endPos);
            
            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
    }

    protected IEnumerator MoveVertical(float movementVertical, Rigidbody2D rb2d)
    {
        isMoving = true;

        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));

        Quaternion rotation;

        if (movementVertical < 0)
        {
            rotation = Quaternion.Euler(0, 0, movementVertical * 180f);
        }
        else
        {
            rotation = Quaternion.Euler(0, 0, 0);
        }
        transform.rotation = rotation;

        float movementProgress = 0f;       
        Vector2 endPos, movement;
 
        while (movementProgress < Mathf.Abs(movementVertical))
        {
            
            movementProgress += speed * Runner.DeltaTime;
            movementProgress = Mathf.Clamp(movementProgress, 0f, 1f);

            movement = new Vector2(0f, speed * Runner.DeltaTime * movementVertical);
            endPos = rb2d.position + movement;
            
            if (movementProgress == 1) endPos = new Vector2(endPos.x, Mathf.Round(endPos.y));
            rb2d.MovePosition(endPos);
            yield return new WaitForFixedUpdate();
            
        }

        isMoving = false;

    }

}