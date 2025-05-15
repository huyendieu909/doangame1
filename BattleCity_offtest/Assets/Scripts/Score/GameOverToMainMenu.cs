using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverToMainMenu : MonoBehaviour
{
    void Start()
    {
        Invoke("BackToMainMenu", 5);
    }

    void BackToMainMenu() {
        MasterTracker.playerLives = 3;
        MasterTracker.player2Lives = 3;
        SceneManager.LoadScene("MainMenu");
    }
}
