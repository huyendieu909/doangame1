using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TallyScore : MonoBehaviour
{
    [SerializeField]
    Text hiScoreText, stageText, playerScoreText, smallTankScoreText, fastTankScoreText, bigTankScoreText, armoredTankScoreText, smallTanksDestroyed, fastTanksDestroyed, bigTanksDestroyed, armoredTanksDestroyed, totalTanksDestroyed;
    int smallTankScore, fastTankScore, bigTankScore, armoredTankScore;
    MasterTracker masterTracker;
    int smallTankPointsWorth, fastTankPointsWorth, bigTankPointsWorth, armoredTankPointsWorth;
    // Use this for initialization
    void Start () {
        masterTracker = GameObject.Find("MasterTracker").GetComponent<MasterTracker>();
        smallTankPointsWorth = masterTracker.smallTankPointsWorth;
        fastTankPointsWorth = masterTracker.fastTankPointsWorth;
        bigTankPointsWorth = masterTracker.bigTankPointsWorth;
        armoredTankPointsWorth = masterTracker.armoredTankPointsWorth;
        stageText.text = "STAGE " + MasterTracker.stageNumber;
        playerScoreText.text = MasterTracker.playerScore.ToString();
        StartCoroutine(UpdateTankPoints());
    }
    IEnumerator UpdateTankPoints()
    {
        for (int i = 0; i <= MasterTracker.smallTanksDestroyed; i++)
        {
            smallTankScore = smallTankPointsWorth * i;
            smallTankScoreText.text = smallTankScore.ToString();
            smallTanksDestroyed.text = i.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 0; i <= MasterTracker.fastTanksDestroyed; i++)
        {
            fastTankScore = fastTankPointsWorth * i;
            fastTankScoreText.text = fastTankScore.ToString();
            fastTanksDestroyed.text = i.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 0; i <= MasterTracker.bigTanksDestroyed; i++)
        {
            bigTankScore = bigTankPointsWorth * i;
            bigTankScoreText.text = bigTankScore.ToString();
            bigTanksDestroyed.text = i.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        for (int i = 0; i <= MasterTracker.armoredTanksDestroyed; i++)
        {
            armoredTankScore = armoredTankPointsWorth * i;
            armoredTankScoreText.text = armoredTankScore.ToString();
            armoredTanksDestroyed.text = i.ToString();
            yield return new WaitForSeconds(0.2f);
        }
        totalTanksDestroyed.text = (MasterTracker.smallTanksDestroyed + MasterTracker.fastTanksDestroyed + MasterTracker.bigTanksDestroyed + MasterTracker.armoredTanksDestroyed).ToString();
        MasterTracker.playerScore += (smallTankScore + fastTankScore + bigTankScore + armoredTankScore);
        int currentHiScore = PlayerPrefsManager.HighScore;
        PlayerPrefsManager.HighScore = Mathf.Max(currentHiScore, MasterTracker.playerScore);
        hiScoreText.text = PlayerPrefsManager.HighScore.ToString();
        playerScoreText.text = MasterTracker.playerScore.ToString();
        yield return new WaitForSeconds(5f);
        if (MasterTracker.stageCleared)
        {
            ClearStatistics();
            int nextStage = PlayerPrefsManager.StageCompleted+1;
            if (MasterTracker.stageNumber + 1 <= 12) {
                PlayerPrefsManager.StageCompleted = nextStage;
                SceneManager.LoadScene("Stage" + (MasterTracker.stageNumber + 1));
            }
            else {
                SceneManager.LoadScene("WinScreen");
            }
        }
        else
        {
            ClearStatistics();
            //Load GameOver Scene
            SceneManager.LoadScene("GameOver");
        }
    }
    void ClearStatistics()
    {
        MasterTracker.smallTanksDestroyed = 0;
        MasterTracker.fastTanksDestroyed = 0;
        MasterTracker.bigTanksDestroyed = 0;
        MasterTracker.armoredTanksDestroyed = 0;
        MasterTracker.stageCleared = false;
    }
}
