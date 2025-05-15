using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    GameObject projectile; //prefab load
    GameObject canonBall, canonBall2;
    Bullet canon, canon2;
    [SerializeField]
    int speed;
    public int level=1;
    
    void Start () {
        canonBall = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
        canon = canonBall.GetComponent<Bullet>();
        canon.speed = speed;
        if (level > 1) UpgradeProjectileSpeed();
        if (level > 2) GenerateSecondCanonBall();
        if (level > 3) CanonBallPowerUpgrade(); 
        canonBall.SetActive(false);        
    }

    public void Fire()
    {
        if (canonBall.activeSelf == false)
        {
            canonBall.transform.position = transform.position;
            canonBall.transform.rotation = transform.rotation;
            canonBall.SetActive(true);
        }   
        else 
        {
            if (canonBall2 != null) {
                if (canonBall2.activeSelf == false){
                    canonBall2.transform.position = transform.position;
                    canonBall2.transform.rotation = transform.rotation;
                    canonBall2.SetActive(true);
                }
            }
        }
    }


    private void OnDestroy()
    {
        if (canonBall != null) canon.DestroyProjectile();
        if (canonBall2 != null) canon2.DestroyProjectile();
    }

    //code phần đạn khi nâng cấp tank của người chơi 
    public void UpgradeProjectileSpeed()
    {
        speed = 20;
        canon.speed = speed; 
    }
    public void GenerateSecondCanonBall()
    {
        canonBall2 = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
        canon2 = canonBall2.GetComponent<Bullet>();
        canon2.speed = speed;
    }
    public void CanonBallPowerUpgrade()
    {
        if (canonBall != null) canon.destroySteel = true;
        if (canonBall2 != null) canon2.destroySteel = true;
    }
}
