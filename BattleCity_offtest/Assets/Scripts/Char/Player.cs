using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Sprite mới cho tank khi nâng cấp. 1 và 2 là để giữ animation.
    [SerializeField]
    Sprite level2Tank1, level3Tank1, level4Tank1;
    [SerializeField]
    Sprite level2Tank2, level3Tank2, level4Tank2;
    WeaponController wc;

    public int level=1;

    void Start()
    {
        wc = GetComponentInChildren<WeaponController>();
        if (MasterTracker.playerLevel > 1)
        {
            transform.Find("Tanks_20").gameObject.GetComponent<SpriteRenderer>().sprite = level2Tank1;
            transform.Find("Tanks_48").gameObject.GetComponent<SpriteRenderer>().sprite = level2Tank2;
            wc.level = 2;
            if (MasterTracker.playerLevel > 2)
            {
                transform.Find("Tanks_20").gameObject.GetComponent<SpriteRenderer>().sprite = level3Tank1;
                transform.Find("Tanks_48").gameObject.GetComponent<SpriteRenderer>().sprite = level3Tank2;
                wc.level = 3;
                if (MasterTracker.playerLevel > 3)
                {
                    transform.Find("Tanks_20").gameObject.GetComponent<SpriteRenderer>().sprite = level4Tank1;
                    transform.Find("Tanks_48").gameObject.GetComponent<SpriteRenderer>().sprite = level4Tank2;
                    wc.level = 4;
                }
            }
        }
    }
    public void UpgradeTank()
    {
        if (level < 4)
        {
            level++;
            if (level == 2)
            {
                transform.Find("Tanks_20").gameObject.GetComponent<SpriteRenderer>().sprite = level2Tank1;
                transform.Find("Tanks_48").gameObject.GetComponent<SpriteRenderer>().sprite = level2Tank2;
                wc.UpgradeProjectileSpeed();
            }
            else if (level == 3)
            {	
                transform.Find("Tanks_20").gameObject.GetComponent<SpriteRenderer>().sprite = level3Tank1;
                transform.Find("Tanks_48").gameObject.GetComponent<SpriteRenderer>().sprite = level3Tank2;
                wc.GenerateSecondCanonBall();
            }
            else if (level == 4)
            {
                transform.Find("Tanks_20").gameObject.GetComponent<SpriteRenderer>().sprite = level4Tank1;
                transform.Find("Tanks_48").gameObject.GetComponent<SpriteRenderer>().sprite = level4Tank2;
                wc.CanonBallPowerUpgrade();
            }
        }
    }

}
