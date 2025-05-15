using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DifficultyMenu : MonoBehaviour
{
    public Button difficultyChangeButton;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button exitButton;
    public GameObject difficultyChangeMenu;
    public TextMeshProUGUI difficultyText;
    private bool isDisplayMenu = false;


    void Start()
    {
        difficultyChangeMenu.SetActive(false);
        // Gán sự kiện cho các nút
        difficultyChangeButton.onClick.AddListener(() => ToggleMenu());
        easyButton.onClick.AddListener(() => OnDifficultySelected("Easy"));
        mediumButton.onClick.AddListener(() => OnDifficultySelected("Medium"));
        hardButton.onClick.AddListener(() => OnDifficultySelected("Hard"));
        exitButton.onClick.AddListener(() => LoadMainMenu());
    }
    void Update()
    {
        difficultyText.text = PlayerPrefsManager.GameDifficulty;
    }

    //Ẳn hiện menu chọn độ khó
    public void ToggleMenu()
    {
        isDisplayMenu = !isDisplayMenu;
        if (isDisplayMenu)
            DisplayDifficultyChangeMenu();
        else
            HideDifficultyChangeMenu();
    }
    void DisplayDifficultyChangeMenu() {
        difficultyChangeMenu.SetActive(true);
    }
    void HideDifficultyChangeMenu() {
        difficultyChangeMenu.SetActive(false);
    }

    // Khi người chơi chọn độ khó
    void OnDifficultySelected(string difficulty)
    {
        // Lưu độ khó vào PlayerPrefs
        PlayerPrefsManager.GameDifficulty = difficulty;
        Debug.Log("Độ khó được chọn: " + difficulty);
        ToggleMenu();  
    }

    void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
