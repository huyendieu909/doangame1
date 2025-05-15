using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Fusion;
using System.Linq;
public class BulletNet : NetworkBehaviour
{
    public bool destroySteel = false;
    [SerializeField]
    bool toBeDestroyed = false;
    // NetworkObject brickGameObject,steelGameObject;
    GameObject brickGameObject, steelGameObject;
    Tilemap tilemap;
    // public int speed = 10;
    [Networked] public int speed {get; set;}
    Rigidbody2D rb2d;
    private NetworkRunner runner;
    [SerializeField] private TilemapNetController tilemapNetController;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        tilemapNetController = GameObject.Find("Bricks").GetComponent<TilemapNetController>();
        speed = 10;
    }
    void Start () {
        runner = FindObjectOfType<NetworkRunner>();
        brickGameObject = GameObject.FindGameObjectWithTag("Brick");
        // brickGameObject = runner.GetAllNetworkObjects().FirstOrDefault(obj => obj.CompareTag("Brick"));
        // steelGameObject = GameObject.FindGameObjectWithTag("Steel");

        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb2d.velocity = Vector2.zero;
        tilemap = collision.gameObject.GetComponent<Tilemap>();
        if (collision.gameObject.GetComponent<Health>() != null){
            collision.gameObject.GetComponent<Health>().TakeDamage();
        }
        // if ((collision.gameObject == brickGameObject) || (destroySteel && collision.gameObject == steelGameObject))
        if ((collision.gameObject.CompareTag("Brick") || (destroySteel && collision.gameObject.CompareTag("Steel"))) && Object.HasStateAuthority)
        {
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                // tilemap.SetTile(tilemap.WorldToCell(hitPosition), null);
                tilemapNetController.RPC_DestroyTile(tilemap.WorldToCell(hitPosition));
            }
        }
        //keep the projectile inactive if hit anything. this will allow the projectile to be reused instead of wasting resource for garbage collector to clear it from memory
        this.gameObject.SetActive(false);
        
    }


    void OnEnable()
    {
        if (rb2d != null)
        {
            rb2d.velocity = transform.up * speed;
        }
    }

    public override void Spawned()
    {
        speed = 10;
        if (rb2d == null)
        {
            rb2d = GetComponent<Rigidbody2D>();
        }
        if (Object.HasStateAuthority) rb2d.velocity = transform.up * speed;
    }

    void OnDisable()
    {
        if (toBeDestroyed){
            // Destroy(this.gameObject);
            Runner.Despawn(this.Object);
        }
    }
    //function called from Tank to destroy the projectile when the tank is destroyed
    public void DestroyProjectile()
    {
        if (gameObject.activeSelf == false)
        {
            // Destroy(this.gameObject);
            Runner.Despawn(this.Object);
        }
        toBeDestroyed = true;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_ActivateBullet(Vector3 position, Quaternion rotation)
    {
        // Kích hoạt viên đạn và cập nhật vị trí, góc quay
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = rotation;
    }
}
