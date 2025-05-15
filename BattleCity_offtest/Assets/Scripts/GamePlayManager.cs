using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.Linq;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField]
    Image topCurtain, bottomCurtain, blackCurtain;
    [SerializeField]
    Text stageNumberText, gameOverText;
    Tilemap waterTilemap, steelTilemap;

    GameObject[] spawnPoints, spawnPlayerPoints, spawnPlayer2Points;
    public Transform spawnLocation, playerSpawnLocation, player2SpawnLocation;
    bool stageStart = false, stageStart2 = false;

    public Transform eagle;
    bool tankReserveEmpty = false;

    [SerializeField]
    Transform tankReservePanel;
    [SerializeField]
    Text playerLivesText, stageNumber;
    GameObject tankImage;

    [SerializeField]
    GameObject[] bonusCrates;
    Transform enemyHolder;


    void Start () {
        //Khởi tạo Location cho quái spawn
        Instantiate(spawnLocation, new Vector3(-12,12,0), Quaternion.identity);
        Instantiate(spawnLocation, new Vector3(0,12,0), Quaternion.identity);
        Instantiate(spawnLocation, new Vector3(12,12,0), Quaternion.identity);
        
        //Khởi tạo location cho eagle spawn 
        Instantiate(eagle, new Vector3(0,-12,0), Quaternion.identity);

        //Khởi tạo Location cho player spawn
        Vector3 player1SpawnPosition = new Vector3(-4,-12,0);
        Instantiate(playerSpawnLocation, player1SpawnPosition, Quaternion.identity);
        //Spawn người chơi thứ 2
        if (MasterTracker.multiplayer > 1) {
            Vector3 player2SpawnPosition = new Vector3(3, -12, 0);
            Instantiate(player2SpawnLocation,player2SpawnPosition, Quaternion.identity);
        }


        stageStart = true;
        spawnPlayerPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoint");
        if (MasterTracker.multiplayer > 1) 
        {
            stageStart2 = true;
            spawnPlayer2Points = GameObject.FindGameObjectsWithTag("Player2SpawnPoint");
        }
        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
        stageNumberText.text = "STAGE " + MasterTracker.stageNumber.ToString();

        //Tìm tilemap dạng nước và thép để bonus tạo ra né vị trí của nó
        steelTilemap = GameObject.Find("Steel").GetComponent<Tilemap>();
	    waterTilemap = GameObject.Find("Water").GetComponent<Tilemap>();

        UpdateTankReserve();
        UpdatePlayerLives();
        UpdateStageNumber();        
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
        if (MasterTracker.multiplayer > 1) SpawnPlayer2();
    }
    IEnumerator SpawnEnemiesCoroutine()
    {
        while (!tankReserveEmpty)
        {
            int currentCount = GameObject.FindGameObjectsWithTag("Small").Length +
                               GameObject.FindGameObjectsWithTag("Big").Length +
                               GameObject.FindGameObjectsWithTag("Fast").Length +
                               GameObject.FindGameObjectsWithTag("Armored").Length;

            // Nếu số quái hiện tại nhỏ hơn maxEnemyCount thì spawn thêm quái
            if (currentCount < 5)
            {
                SpawnEnemy();
            }
            // Đợi spawnInterval giây trước khi kiểm tra lại
            yield return new WaitForSeconds(1);
        }
    }
    public IEnumerator GameOver()
    {
        while (gameOverText.rectTransform.localPosition.y < 0)
        {
            gameOverText.rectTransform.localPosition = new Vector3(gameOverText.rectTransform.localPosition.x, gameOverText.rectTransform.localPosition.y + 120f * Time.deltaTime, gameOverText.rectTransform.localPosition.z);
            yield return null;
        }
        MasterTracker.stageCleared = false;
        LevelCompleted();
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
        if (StageManager.smallTanks + StageManager.fastTanks + StageManager.bigTanks + StageManager.armoredTanks > 0)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Animator anime = spawnPoints[spawnPointIndex].GetComponent<Animator>();
            anime.SetTrigger("Spawning");
        }
        else
        {
            CancelInvoke();
            tankReserveEmpty = true;
        }
    }
    public void SpawnPlayer()
    {
        if (MasterTracker.playerLives > 0)
        {
            if (!stageStart)
            {
            MasterTracker.playerLives--;
            UpdatePlayerLives();
            }
            stageStart = false;
            Animator anime = spawnPlayerPoints[0].GetComponent<Animator>();
            anime.SetTrigger("Spawning");
            
        }
        if (MasterTracker.multiplayer == 1 && MasterTracker.playerLives <= 0) {
            StartCoroutine(GameOver());
        }
        // else
        if (MasterTracker.playerLives <= 0 && MasterTracker.player2Lives <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    public void SpawnPlayer2() {
        if (MasterTracker.player2Lives > 0) {
            if (!stageStart2)
            {
            MasterTracker.player2Lives--;
            // UpdatePlayerLives();
            }
            stageStart2 = false;
            if (spawnPlayer2Points.Length != 0) {
                Animator anime = spawnPlayer2Points[0].GetComponent<Animator>();
                anime.SetTrigger("Spawning");
            }
        }
        if (MasterTracker.playerLives <= 0 && MasterTracker.player2Lives <= 0)
        {
            StartCoroutine(GameOver());
        }
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
}
