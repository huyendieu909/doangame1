using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterTracker : MonoBehaviour
{
    static MasterTracker instance = null;
    public static int multiplayer = 1;

    [SerializeField]
    int smallTankPoints = 100, fastTankPoints = 200, bigTankPoints= 300, armoredTankPoints=400;
    public int smallTankPointsWorth { get { return smallTankPoints; } }
    public int fastTankPointsWorth { get { return fastTankPoints; } }
    public int bigTankPointsWorth { get { return bigTankPoints; } }
    public int armoredTankPointsWorth { get { return armoredTankPoints; } }

    public static int smallTanksDestroyed, fastTanksDestroyed, bigTanksDestroyed, armoredTanksDestroyed;
    public static int stageNumber;
    public static int playerScore = 0;
    public static int playerLives = 3;
    public static int playerLevel = 1;
    public static int player2Lives = 3;
    public static int player2Level = 1;
    public static bool stageCleared = false;
    public static int totalStage = 12;


    private void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;    
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnDisable()
    {
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

}
