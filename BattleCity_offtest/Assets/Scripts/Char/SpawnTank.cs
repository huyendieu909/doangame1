using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTank : MonoBehaviour
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
            if (StageManager.smallTanks > 0) tankToSpawn.Add((int)tankType.smallTank);
            if (StageManager.fastTanks > 0) tankToSpawn.Add((int)tankType.fastTank);
            if (StageManager.bigTanks > 0) tankToSpawn.Add((int)tankType.bigTank);
            if (StageManager.armoredTanks > 0) tankToSpawn.Add((int)tankType.armoredTank);
            int tankID = tankToSpawn[Random.Range(0, tankToSpawn.Count)];
            tank = Instantiate(tanks[tankID], transform.position, transform.rotation);
            tank.transform.SetParent(enemyHolder);
            if (Random.value <= StageManager.bonusCrateRate)
            {
                tank.GetComponent<BonusTank>().MakeBonusTank();
            }
            if (tankID == (int)tankType.smallTank) StageManager.smallTanks--;
            else if (tankID == (int)tankType.fastTank) StageManager.fastTanks--;
            else if (tankID == (int)tankType.bigTank) StageManager.bigTanks--;
            else if (tankID == (int)tankType.armoredTank) StageManager.armoredTanks--;
            GamePlayManager GPM = GameObject.Find("StageManager").GetComponent<GamePlayManager>();
            GPM.RemoveTankReserve();
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
