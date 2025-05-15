using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;  
    public Button pauseButton;     
    public Button resumeButton;   
    public Button quitButton;      

    private bool isPaused = false;

    void Start()
    {
        // Ẩn menu khi bắt đầu
        pauseMenu.SetActive(false);

        // Gán sự kiện cho các button
        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(TogglePause);
        quitButton.onClick.AddListener(QuitToMainMenu);
    }

    // Toggle giữa Pause và Resume
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    void PauseGame()
    {
        Time.timeScale = 0f;          
        pauseMenu.SetActive(true);    
  
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);   
        Time.timeScale = 1f;        
    }

    void QuitToMainMenu()
    {
        Time.timeScale = 1f;  
        MasterTracker.playerLives = 3;
        MasterTracker.player2Lives = 3;        
        SceneManager.LoadScene("MainMenu"); 
    }
}
