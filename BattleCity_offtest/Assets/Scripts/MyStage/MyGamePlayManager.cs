using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.Linq;

public class MyGamePlayManager : MonoBehaviour
{
    [SerializeField]
    Image topCurtain, bottomCurtain, blackCurtain;
    [SerializeField]
    Text stageNumberText, gameOverText;
    Tilemap waterTilemap, steelTilemap;

    GameObject[] spawnPoints, spawnPlayerPoints, spawnPlayer2Points, spawnAllyP1Points, spawnAllyP2Points;
    public Transform spawnLocation, playerSpawnLocation, player2SpawnLocation, allyP1SpawnLocation, allyP2SpawnLocation;
    bool stageStart = false, stageStart2 = false;

    public Transform eagleP1;
    public Transform eagleP2;
    bool tankReserveEmpty = false;

    [SerializeField]
    Transform tankReservePanel;
    [SerializeField]
    Text playerLivesText, stageNumber;
    GameObject tankImage;

    [SerializeField]
    GameObject[] bonusCrates;

    GameObject grid;
    MyGridMap gridMap;
    //enemyHolder sẽ giữ cho địch spawn ra nằm trong gameobject tên là EnemyHolder. 
    //Việc SetParent được thực hiện trong hàm StartSpawning của Script SpawnTank.
    Transform enemyHolder;
    
    public int flagP1Captured = 0;
    public int flagP2Captured = 0;


    void Start () {
        grid = GameObject.Find("Grid");
        gridMap = grid.GetComponent<MyGridMap>();
        gridMap.ChangeEagleWallToSteelP1();
        gridMap.ChangeEagleWallToSteelP2();
        
        //Khởi tạo Location cho quái neutral spawn
        Instantiate(spawnLocation, new Vector3(-3,0,0), Quaternion.identity);
        Instantiate(spawnLocation, new Vector3(1,0,0), Quaternion.identity);
        
        
        //Khởi tạo location cho eagle spawn 
        Instantiate(eagleP1, new Vector3(0,-12,0), Quaternion.identity);
        Instantiate(eagleP2, new Vector3(0, 12, 0), Quaternion.identity);

        //Khởi tạo Location cho player spawn
        Vector3 player1SpawnPosition = new Vector3(-4,-12,0);
        Instantiate(playerSpawnLocation, player1SpawnPosition, Quaternion.identity);
        Vector3 player2SpawnPosition = new Vector3(3, 12, 0);
        Instantiate(player2SpawnLocation,player2SpawnPosition, Quaternion.identity);
    
        //Khởi tạo Location cho ally spawn
        Vector3 allyP1SpawnPosition = new Vector3(3,-12,0);
        Instantiate(allyP1SpawnLocation, allyP1SpawnPosition, Quaternion.identity);
        Vector3 allyP2SpawnPosition = new Vector3(-4, 12, 0);
        Instantiate(allyP2SpawnLocation, allyP2SpawnPosition, Quaternion.identity);


        stageStart = true;
        spawnPlayerPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoint");
        spawnAllyP1Points = GameObject.FindGameObjectsWithTag("AllyP1SpawnPoint");
        stageStart2 = true;
        spawnPlayer2Points = GameObject.FindGameObjectsWithTag("Player2SpawnPoint");
        spawnAllyP2Points = GameObject.FindGameObjectsWithTag("AllyP2SpawnPoint");
        spawnPoints = GameObject.FindGameObjectsWithTag("NeutralSpawnPoint");
        stageNumberText.text = "P v P!";

        // //Tìm tilemap dạng nước và thép để bonus tạo ra né vị trí của nó
        // steelTilemap = GameObject.Find("Steel").GetComponent<Tilemap>();
	    // waterTilemap = GameObject.Find("Water").GetComponent<Tilemap>();

        // UpdateTankReserve();
        // UpdatePlayerLives();
        // UpdateStageNumber();        
        StartCoroutine(StartStage());


    }
     
    private void Update()
    {
        if (tankReserveEmpty && GameObject.FindWithTag("Small") == null && GameObject.FindWithTag("Fast") == null && GameObject.FindWithTag("Big") == null && GameObject.FindWithTag("Armored") == null)
        {
            MasterTracker.stageCleared = true;
            LevelCompleted();
        }
    }

    IEnumerator StartStage()
    {
        StartCoroutine(RevealStageNumber());
        yield return new WaitForSeconds(3);
        StartCoroutine(RevealTopStage());
        StartCoroutine(RevealBottomStage());
        yield return null;
        
        StartCoroutine(SpawnEnemiesCoroutine());
        SpawnPlayer();
        SpawnAllyP1();
        SpawnPlayer2();
        SpawnAllyP2();
    }
    IEnumerator SpawnEnemiesCoroutine()
    {
        while (!tankReserveEmpty) {
            int currentCount = GameObject.FindGameObjectsWithTag("NeutralTank").Length;

            // Nếu số quái hiện tại nhỏ hơn maxEnemyCount thì spawn thêm quái
            if (currentCount < 4)
            {
                SpawnEnemy();
            }
            // Đợi spawnInterval giây trước khi kiểm tra lại
            yield return new WaitForSeconds(3);
        }
        
    }
    public IEnumerator GameOverP1Win()
    {
        gameOverText.text = "P1 Win!";
        while (gameOverText.rectTransform.localPosition.y < 0)
        {
            gameOverText.rectTransform.localPosition = new Vector3(gameOverText.rectTransform.localPosition.x, gameOverText.rectTransform.localPosition.y + 120f * Time.deltaTime, gameOverText.rectTransform.localPosition.z);
            yield return null;
        }
        MasterTracker.stageCleared = false;
        SceneManager.LoadScene("GameOverPvP");
        // LevelCompleted();
    }
    public IEnumerator GameOverP2Win()
    {
        gameOverText.text = "P2 Win!";
        while (gameOverText.rectTransform.localPosition.y < 0)
        {
            gameOverText.rectTransform.localPosition = new Vector3(gameOverText.rectTransform.localPosition.x, gameOverText.rectTransform.localPosition.y + 120f * Time.deltaTime, gameOverText.rectTransform.localPosition.z);
            yield return null;
        }
        MasterTracker.stageCleared = false;
        SceneManager.LoadScene("GameOverPvP");
        // LevelCompleted();
    }

	IEnumerator RevealStageNumber()
    {
        while (blackCurtain.rectTransform.localScale.y > 0)
        {
            blackCurtain.rectTransform.localScale = new Vector3(1, Mathf.Clamp(blackCurtain.rectTransform.localScale.y - Time.deltaTime,0,1), 1);
            yield return null;
        }
    }
    IEnumerator RevealTopStage()
    {
        stageNumberText.enabled = false;
        while (topCurtain.rectTransform.position.y < 1250)
        {
            topCurtain.rectTransform.Translate(new Vector3(0, 500 * Time.deltaTime, 0));
            yield return null;
        }
    }
    IEnumerator RevealBottomStage()
    {
        while (bottomCurtain.rectTransform.position.y > -400)
        {
            bottomCurtain.rectTransform.Translate(new Vector3(0, -500 * Time.deltaTime, 0));
            yield return null;
        }
    }

    public void SpawnEnemy()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Animator anime = spawnPoints[spawnPointIndex].GetComponent<Animator>();
        anime.SetTrigger("Spawning");   
    }
    public void SpawnPlayer()
    {
        if (MasterTracker.playerLives > 0)
        {
            // if (!stageStart)
            // {
            // MasterTracker.playerLives--;
            // UpdatePlayerLives();
            // }
            // stageStart = false;
            Animator anime = spawnPlayerPoints[0].GetComponent<Animator>();
            anime.SetTrigger("Spawning");
            
        }

    }
    public void SpawnAllyP1()
    {
        Animator anime = spawnAllyP1Points[0].GetComponent<Animator>();
        anime.SetTrigger("Spawning");
    }

    public void SpawnPlayer2() {
        if (MasterTracker.player2Lives > 0) {
            // if (!stageStart2)
            // {
            // MasterTracker.player2Lives--;
            // // UpdatePlayerLives();
            // }
            // stageStart2 = false;
            if (spawnPlayer2Points.Length != 0) {
                Animator anime = spawnPlayer2Points[0].GetComponent<Animator>();
                anime.SetTrigger("Spawning");
            }
        }

    }
    public void SpawnAllyP2()
    {
        Animator anime = spawnAllyP2Points[0].GetComponent<Animator>();
        anime.SetTrigger("Spawning");
    }

    private void LevelCompleted()
    {
        tankReserveEmpty = false;
        GameObject playerTank = GameObject.FindGameObjectWithTag("PlayerTank");
        GameObject player2Tank = GameObject.FindGameObjectWithTag("Player2Tank");
        if (playerTank != null) 
        {
            Player player = playerTank.GetComponent<Player>();
            if (player != null) MasterTracker.playerLevel = player.level;
        }
        if (player2Tank != null) 
        {
            Player player2 = player2Tank.GetComponent<Player>();
            if (player2 != null) MasterTracker.player2Level = player2.level;
        }
        SceneManager.LoadScene("Score");
    }
    bool InvalidBonusCratePosition(Vector3 cratePosition)
    {
        return waterTilemap.GetTile(waterTilemap.WorldToCell(cratePosition)) != null || steelTilemap.GetTile(steelTilemap.WorldToCell(cratePosition)) != null;
    }
    void UpdateTankReserve()
    {
        int j;
        int numberOfTanks = StageManager.smallTanks + StageManager.fastTanks + StageManager.bigTanks + StageManager.armoredTanks;
        for (j = 0; j < numberOfTanks; j++)
        {
            tankImage = tankReservePanel.transform.GetChild(j).gameObject;
            tankImage.SetActive(true);
        }
    }
    public void RemoveTankReserve()
    {
        int numberOfTanks = StageManager.smallTanks + StageManager.fastTanks + StageManager.bigTanks + StageManager.armoredTanks;
        tankImage = tankReservePanel.transform.GetChild(numberOfTanks).gameObject;
        tankImage.SetActive(false);
    }
    public void UpdatePlayerLives()
    {
        playerLivesText.text = MasterTracker.playerLives.ToString();
    }
    void UpdateStageNumber()
    {
        stageNumber.text = MasterTracker.stageNumber.ToString();
    }

    public void GenerateBonusCrate()
    {
        GameObject bonusCrate = bonusCrates[Random.Range(0, bonusCrates.Length)];
        Vector3 cratePosition = new Vector3(Random.Range(-12, 12), Random.Range(-12, 13), 0);
        if (InvalidBonusCratePosition(cratePosition))
        {
            do
            {
                cratePosition = new Vector3(Random.Range(-12, 12), Random.Range(-12, 12), 0);
                if (!InvalidBonusCratePosition(cratePosition)) Instantiate(bonusCrate, cratePosition, Quaternion.identity);
            } while (InvalidBonusCratePosition(cratePosition));
        }
        else
        {
            Instantiate(bonusCrate, cratePosition, Quaternion.identity);
        }
    }

    public void ActivateFreeze()
    {
        StartCoroutine(FreezeActivated());
    }

    IEnumerator FreezeActivated()
    {
        EnemyMovement.freezing = true;
        enemyHolder = GameObject.Find("EnemyHolder").transform;
        for (int i = 0; i < enemyHolder.childCount; i++)
        {
            enemyHolder.GetChild(i).gameObject.SetActive(false);
            enemyHolder.GetChild(i).gameObject.GetComponent<EnemyMovement>().ToFreezeTank();
            enemyHolder.GetChild(i).gameObject.GetComponent<EnemyMovement>().enabled = false;
            enemyHolder.GetChild(i).gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(10);
        for (int i = 0; i < enemyHolder.childCount; i++)
        {
            enemyHolder.GetChild(i).gameObject.SetActive(false);
            enemyHolder.GetChild(i).gameObject.GetComponent<EnemyMovement>().enabled = true;
            enemyHolder.GetChild(i).gameObject.GetComponent<EnemyMovement>().ToUnfreezeTank();
            enemyHolder.GetChild(i).gameObject.SetActive(true);
        }
        EnemyMovement.freezing = false;
    }

    public void P1CapturedAFlag() {
        flagP1Captured++;
        if (flagP1Captured >= 2) gridMap.ChangeEagleWallToBrickP2(); 
    }
    public void P2CapturedAFlag() {
        flagP2Captured++;
        if (flagP2Captured >= 2) gridMap.ChangeEagleWallToBrickP1(); 
    }
}
