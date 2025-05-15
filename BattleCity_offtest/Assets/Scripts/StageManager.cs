using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    int smallTanksInThisLevel, fastTanksInThisLevel, bigTanksInThisLevel, armoredTanksInThisLevel, stageNumber;
    public static int smallTanks, fastTanks, bigTanks, armoredTanks;
    [SerializeField]
    float spawnRateInThisLevel=4, bonusCrateRateInThisLevel=0.2f;
    public static float spawnRate { get; private set; }
    public static float bonusCrateRate { get; private set; }
    private void Awake()
    {
        MasterTracker.stageNumber = stageNumber;
        InitEnemyQuantityBaseOnDifficulty();
        smallTanks = smallTanksInThisLevel;
        fastTanks = fastTanksInThisLevel;
        bigTanks = bigTanksInThisLevel;
        armoredTanks = armoredTanksInThisLevel;
        spawnRate = spawnRateInThisLevel;
        bonusCrateRate = bonusCrateRateInThisLevel;
    }

    private void InitEnemyQuantityBaseOnDifficulty() {
        switch (PlayerPrefsManager.GameDifficulty) {
            case "Easy":
                smallTanksInThisLevel = 10;
                fastTanksInThisLevel = 5;
                bigTanksInThisLevel = 3;
                armoredTanksInThisLevel = 2;
                spawnRateInThisLevel = 6;
                bonusCrateRateInThisLevel = 0.2f;
                break;
            case "Medium":
                smallTanksInThisLevel = 8;
                fastTanksInThisLevel = 6;
                bigTanksInThisLevel = 4;
                armoredTanksInThisLevel = 3;
                spawnRateInThisLevel = 5;
                bonusCrateRateInThisLevel = 0.15f;
                break;
            case "Hard": 
                smallTanksInThisLevel = 5;
                fastTanksInThisLevel = 5;
                bigTanksInThisLevel = 5;
                armoredTanksInThisLevel = 5;
                spawnRateInThisLevel = 4;
                bonusCrateRateInThisLevel = 0.1f;
                break;
        }
    }
}
