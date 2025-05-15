using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{   
    //Khởi tạo stage đầu tiên hoàn thành là 1 để mở stage 1 ở màn hình chọn stage
    void Start()
    {
        if (PlayerPrefs.GetInt("StageCompleted") == 0) PlayerPrefs.SetInt("StageCompleted", 1);
    } 
    public void LoadGame1 () {
        MasterTracker.multiplayer = 1;
        Debug.Log("clc");
        SceneManager.LoadScene("StageSelect");
    }
    
    public void LoadGame22 () {
        MasterTracker.multiplayer = 2;
        Debug.Log("c2");
        SceneManager.LoadScene("StageSelect");
    }

    public void LoadGame2 () {
        MasterTracker.multiplayer = 2;
        // SceneManager.LoadScene("StageSelect");
        SceneManager.LoadScene("Lobby");
    }

    public void LoadMyStage() {
        SceneManager.LoadScene("MyStage");
    }

    public void LoadSetting(){
        SceneManager.LoadScene("Setting");
    }

    public void LoadInfo() {
        SceneManager.LoadScene("Info");
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
