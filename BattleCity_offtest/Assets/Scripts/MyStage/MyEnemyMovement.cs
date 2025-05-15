using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public class MyEnemyMovement : Movement
{
    Rigidbody2D rb2d;
    float h, v;
    [SerializeField]
    LayerMask blockingLayer;
    WeaponController wc;
    enum Direction { Up, Down, Left, Right };

    //For AIIIIII
    public bool isAI = false;
    public GameObject target;
    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool reachTheEndOfPath = false;
    Seeker seeker;
    public static bool freezing= false;
    public string targetTraceTag = "";
    public string targetTraceTag2 = "";

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (!isAI) RandomDirection();
        wc = GetComponentInChildren<WeaponController>();
        Invoke("FireWhenWanted", Random.Range(1f, 3f));

        if (isAI){
            seeker = GetComponent<Seeker>();
            InvokeRepeating("UpdatePath", 0f, .5f) ;
            var playerToTrace = GameObject.FindGameObjectsWithTag("PlayerTank");
            target = playerToTrace[Random.Range(0, playerToTrace.Count())] ;
            // AIDirection();
            InvokeRepeating("ReScan", 1f, 5f);
            
        } 
    }

    void UpdatePath() {
        if (seeker.IsDone()) seeker.StartPath(rb2d.position, target.transform.position, OnPathComplete);
    }
    void ReScan() {
        AstarPath.active.Scan();
    }
    void OnPathComplete(Path p) {
        if (!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    public void RandomDirection()
    {
        CancelInvoke("RandomDirection");
        List<Direction> lottery = new List<Direction>();
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, 1), blockingLayer))
        {
            lottery.Add(Direction.Right);
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, -1), blockingLayer))
        {
            lottery.Add(Direction.Left);
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(1, 0), blockingLayer))
        {
            lottery.Add(Direction.Up);
        }
        if (!Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(-1, 0), blockingLayer))
        {
            lottery.Add(Direction.Down);
        }
        Direction selection = lottery[Random.Range(0, lottery.Count)];
        if (selection == Direction.Up)
        {
            v = 1;
            h = 0;
        }
        if (selection == Direction.Down)
        {
            v = -1;
            h = 0;
        }
        if (selection == Direction.Right)
        {
            v = 0;
            h = 1;
        }
        if (selection == Direction.Left)
        {
            v = 0;
            h = -1;
        }
        Invoke("RandomDirection", Random.Range(2, 3));
    }
    public void AIDirection() {
        CancelInvoke("AIDirection");
        var playerToTrace = GameObject.FindGameObjectsWithTag(targetTraceTag);
        // if (target == null) 
        if (playerToTrace == null) target = GameObject.FindGameObjectWithTag(targetTraceTag2); 
        else target = playerToTrace[Random.Range(0, playerToTrace.Count())];
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count) {
            reachTheEndOfPath = true;
            return;
        } else {
            reachTheEndOfPath = false;
        }

        // Lấy vị trí đích của waypoint hiện tại
        Vector2 targetPos = (Vector2)path.vectorPath[currentWaypoint];
        Vector2 diff = targetPos - rb2d.position;

        // Nếu đã gần waypoint, chuyển sang waypoint tiếp theo
        if (diff.magnitude < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }

        // Xác định hướng ưu tiên: chọn trục có hiệu số lớn hơn
        Vector2 primaryDir = Vector2.zero;
        Vector2 secondaryDir = Vector2.zero;

        if (Mathf.Abs(diff.x) >= Mathf.Abs(diff.y)) {
            // Ưu tiên di chuyển theo trục X
            primaryDir = diff.x > 0 ? Vector2.right : Vector2.left;
            secondaryDir = diff.y > 0 ? Vector2.up : Vector2.down;
        } else {
            // Ưu tiên di chuyển theo trục Y
            primaryDir = diff.y > 0 ? Vector2.up : Vector2.down;
            secondaryDir = diff.x > 0 ? Vector2.right : Vector2.left;
        }

        // Kiểm tra xem hướng chính có bị chặn không (IsPathBlocked là hàm bạn tự định nghĩa)
        Vector2 finalDirection = primaryDir;
        if (IsPathBlocked(primaryDir)) {
            // Nếu hướng chính bị chặn, thử chuyển sang hướng phụ
            if (!IsPathBlocked(secondaryDir))
                finalDirection = secondaryDir;
            // Nếu cả hai hướng đều bị chặn, có thể đặt finalDirection = Vector2.zero hoặc xử lý khác
        }

        // Gán các giá trị h, v dựa trên finalDirection
        if (finalDirection == Vector2.right)
        {
            h = 1;
            v = 0;
        }
        else if (finalDirection == Vector2.left)
        {
            h = -1;
            v = 0;
        }
        else if (finalDirection == Vector2.up)
        {
            h = 0;
            v = 1;
        }
        else if (finalDirection == Vector2.down)
        {
            h = 0;
            v = -1;
        }
        else
        {
            h = 0;
            v = 0;
        }

        Invoke("AIDirection", 1);
    }
    bool IsPathBlocked(Vector2 direction)
    {
        // Kiểm tra va chạm trong khoảng cách nhất định (ví dụ: 0.5 đơn vị)
        float checkDistance = 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(rb2d.position, direction, checkDistance);
        return (hit.collider != null);
    }


    void FireWhenWanted()
    {
        // print("FireWhenWanted");
        wc.Fire();
        Invoke("FireWhenWanted", Random.Range(1f, 3f));
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // if (!isAI) RandomDirection();
        RandomDirection();

    }
    void FixedUpdate()
    {
        if (isAI) {
            AIDirection();
        } 

        if (v != 0 && isMoving == false) StartCoroutine(MoveVertical(v, rb2d));
        else if (h != 0 && isMoving == false) StartCoroutine(MoveHorizontal(h, rb2d));        
    }

    public void ToFreezeTank()
    {
        CancelInvoke();
        StopAllCoroutines();
    }
    public void ToUnfreezeTank()
    {
        isMoving = false;
        RandomDirection();
        Invoke("FireWhenWanted", Random.Range(0.5f, 1));     
    }

}
