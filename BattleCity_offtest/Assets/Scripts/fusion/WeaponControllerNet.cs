using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class WeaponControllerNet : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject projectilePrefab; // Prefab đạn (phải có NetworkObject)
    
    [SerializeField]
    private int speed = 10;

    public int level = 1;

    private NetworkObject primaryProjectile;
    private NetworkObject secondaryProjectile;

    private BulletNet primaryBullet;
    private BulletNet secondaryBullet;

    // Khởi tạo viên đạn khi bắt đầu (chỉ trên máy có StateAuthority)
    void Start()
    {
        if (Object.HasStateAuthority)
        {
            // Spawn primary projectile và lưu reference
            primaryProjectile = Runner.Spawn(projectilePrefab, transform.position, transform.rotation, Object.InputAuthority);
            primaryBullet = primaryProjectile.gameObject.GetComponent<BulletNet>();
            primaryBullet.speed = speed;

            // Nâng cấp nếu cần
            if (level > 1) UpgradeProjectileSpeed();
            if (level > 2) GenerateSecondProjectile();
            if (level > 3) UpgradeProjectilePower();

            // Ẩn viên đạn ngay sau spawn
            primaryProjectile.gameObject.SetActive(false);
            if (secondaryProjectile != null)
            {
                secondaryProjectile.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Gọi hàm Fire() khi bắn.
    /// Chỉ máy chủ (StateAuthority) sẽ spawn viên đạn.
    /// Client gửi input fire qua RPC (nếu cần) để host kích hoạt đạn.
    /// </summary>
    public void Fire()
    {
        // Chỉ cho phép host kích hoạt viên đạn (để đảm bảo đồng bộ)
        if (!Object.HasStateAuthority)
            return;

        if (!primaryProjectile.gameObject.activeSelf) {
            Vector3 spawnPos = transform.position;
            Quaternion spawnRot = transform.rotation;
            primaryProjectile.GetComponent<BulletNet>().RPC_ActivateBullet(spawnPos, spawnRot);
        }
        else if (secondaryProjectile != null && !secondaryProjectile.gameObject.activeSelf)
        {
            Vector3 spawnPos = transform.position;
            Quaternion spawnRot = transform.rotation;
            secondaryProjectile.GetComponent<BulletNet>().RPC_ActivateBullet(spawnPos, spawnRot);
        }
    }

    private void OnDestroy()
    {
        // Nếu có thể, hủy viên đạn khi tank bị hủy
        if (primaryProjectile != null)
        {
            primaryBullet.DestroyProjectile();
        }
        if (secondaryProjectile != null)
        {
            secondaryBullet.DestroyProjectile();
        }
    }

    // Nâng cấp tốc độ đạn
    public void UpgradeProjectileSpeed()
    {
        speed = 20;
        if (primaryBullet != null)
        {
            primaryBullet.speed = speed;
        }
        if (secondaryBullet != null)
        {
            secondaryBullet.speed = speed;
        }
    }

    // Sinh viên đạn thứ hai để có thể bắn nhanh hơn hoặc bắn liên tục
    public void GenerateSecondProjectile()
    {
        if (Object.HasStateAuthority)
        {
            // Spawn secondary projectile và lưu reference
            secondaryProjectile = Runner.Spawn(projectilePrefab, transform.position, transform.rotation, Object.InputAuthority);
            secondaryBullet = secondaryProjectile.gameObject.GetComponent<BulletNet>();
            secondaryBullet.speed = speed;
            // Ẩn secondary projectile ban đầu
            secondaryProjectile.gameObject.SetActive(false);
        }
    }

    // Nâng cấp sức mạnh của viên đạn (ví dụ, cho phép phá vỡ tường thép)
    public void UpgradeProjectilePower()
    {
        if (primaryBullet != null)
        {
            primaryBullet.destroySteel = true;
        }
        if (secondaryBullet != null)
        {
            secondaryBullet.destroySteel = true;
        }
    }
}










// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Fusion;

// public class WeaponControllerNet : NetworkBehaviour
// {
//     [SerializeField]
//     NetworkObject projectile; //prefab load
//     GameObject canonBall, canonBall2;
//     BulletNet canon, canon2;
//     [SerializeField]
//     int speed = 10;
//     public int level=1;
    
//     void Start () {
//         if (Object.HasStateAuthority) {
//             NetworkObject ob = Runner.Spawn(projectile, transform.position, transform.rotation, Object.InputAuthority);
//             // canonBall = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
//             // canonBall = GameObject.FindGameObjectWithTag("PlayerBullet");
//             canonBall = ob.gameObject;
//             canon = canonBall.GetComponent<BulletNet>();
//             canon.speed = speed;
//             if (level > 1) UpgradeProjectileSpeed();
//             if (level > 2) GenerateSecondCanonBall();
//             if (level > 3) CanonBallPowerUpgrade(); 
//             canonBall.SetActive(false);              
//         }
      
//     }

//     // [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
//     public void Fire()
//     {
//         if (canonBall.activeSelf == false)
//         {
//             canonBall.transform.position = transform.position;
//             canonBall.transform.rotation = transform.rotation;
//             canonBall.SetActive(true);
//         }   
//         else 
//         {
//             if (canonBall2 != null) {
//                 if (canonBall2.activeSelf == false){
//                     canonBall2.transform.position = transform.position;
//                     canonBall2.transform.rotation = transform.rotation;
//                     canonBall2.SetActive(true);
//                 }
//             }
//         }
//     }


//     private void OnDestroy()
//     {
//         if (canonBall != null) canon.DestroyProjectile();
//         if (canonBall2 != null) canon2.DestroyProjectile();
//     }

//     //code phần đạn khi nâng cấp tank của người chơi 
//     public void UpgradeProjectileSpeed()
//     {
//         speed = 20;
//         canon.speed = speed; 
//     }
//     public void GenerateSecondCanonBall()
//     {
//         if (Object.HasStateAuthority) {
//             Runner.Spawn(projectile, transform.position, transform.rotation);
//         }
//         // canonBall2 = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
//         canonBall2 = GameObject.FindGameObjectWithTag("PlayerBullet");
//         canon2 = canonBall2.GetComponent<BulletNet>();
//         canon2.speed = speed;
//     }
//     public void CanonBallPowerUpgrade()
//     {
//         if (canonBall != null) canon.destroySteel = true;
//         if (canonBall2 != null) canon2.destroySteel = true;
//     }
// }
