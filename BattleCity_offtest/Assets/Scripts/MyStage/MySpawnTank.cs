using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySpawnTank : MonoBehaviour
{
    GameObject[] tanks;
    GameObject tank;
    [SerializeField]
    bool isPlayer;
    [SerializeField]
    GameObject smallTank, fastTank, bigTank, armoredTank;
    Transform enemyHolder;

    enum tankType
    {
        smallTank, fastTank, bigTank, armoredTank
    };
    private void Start()
    {
        enemyHolder = GameObject.Find("EnemyHolder").transform;
        if (isPlayer)
        {
            tanks = new GameObject[1] { smallTank };
        }
        else
        {
            tanks = new GameObject[4] { smallTank, fastTank, bigTank, armoredTank };
        }
    }
    public void StartSpawning(){
        if (!isPlayer)
        {
            List<int> tankToSpawn = new List<int>();
            tankToSpawn.Clear();
            tankToSpawn.Add((int)tankType.smallTank);
            tankToSpawn.Add((int)tankType.fastTank);
            tankToSpawn.Add((int)tankType.bigTank);
            tankToSpawn.Add((int)tankType.armoredTank);
            int tankID = tankToSpawn[Random.Range(0, tankToSpawn.Count)];
            tank = Instantiate(tanks[tankID], transform.position, transform.rotation);
            tank.transform.SetParent(enemyHolder);

            // MyGamePlayManager GPM = GameObject.Find("StageManager").GetComponent<MyGamePlayManager>();

        }
        else
        {
            tank = Instantiate(tanks[0], transform.position, transform.rotation);
        }
    }
    public void SpawnNewTank()
    {
        if (tank != null) {
            tank.SetActive(true);
            if (EnemyMovement.freezing == true)
            {
                tank.SetActive(false);
                tank.GetComponent<EnemyMovement>().ToFreezeTank();
                tank.GetComponent<EnemyMovement>().enabled = false;
                tank.SetActive(true);
            }
        } 
    }
}
