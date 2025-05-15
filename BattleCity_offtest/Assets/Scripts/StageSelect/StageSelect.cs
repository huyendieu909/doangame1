using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    // Gán LevelButtonPrefab từ Inspector
    public GameObject levelButtonPrefab, levelButtonNotUnlockPrefab, errorBox, backButton; 
    // Container chứa các nút (Panel có Grid Layout Group)
    public Transform buttonContainer;    
    // Tổng số level muốn hiển thị
    public int totalLevels = 12;         

    void Start()
    {

        // Xóa các nút cũ (nếu có)
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Tạo động các nút level
        for (int i = 1; i <= totalLevels; i++)
        {
            // if (i == 1 || i <= PlayerPrefs.GetInt("StageCompleted")){
            if (i == 1 || i <= PlayerPrefsManager.StageCompleted){
                GameObject btn = Instantiate(levelButtonPrefab, buttonContainer);
                // Gán tên cho nút
                btn.name = "ButtonStage_" + i;
                
                // Nếu prefab có component Text trong con (ví dụ, ở con đầu tiên):
                TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null)
                {
                    btnText.text = i.ToString();
                }

                // Gán sự kiện cho nút để chuyển sang level tương ứng
                // Lưu cục bộ biến i cho lambda
                int levelIndex = i; 
                btn.GetComponent<Button>().onClick.AddListener(() => OnLevelButtonClicked(levelIndex));
            }
            else {
                GameObject btn = Instantiate(levelButtonNotUnlockPrefab, buttonContainer);
                // Gán tên cho nút
                btn.name = "ButtonStage_" + i;
                
                // Nếu prefab có component Text trong con (ví dụ, ở con đầu tiên):
                TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
                if (btnText != null)
                {
                    btnText.text = i.ToString();
                }

                btn.GetComponent<Button>().onClick.AddListener(() => ShowErrorBox());
            }
        }

        backButton.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
    }

    void OnLevelButtonClicked(int level)
    {
        Debug.Log("Chọn Level: " + level);
        // Lưu level vừa chọn (nếu cần) và chuyển sang scene tương ứng.
        string sceneName = "Stage" + level;
        SceneManager.LoadScene(sceneName);
    }

    void ShowErrorBox() {
        errorBox.SetActive(true);
        Invoke("HideErrorBox", 3);
    }
    void HideErrorBox(){
        errorBox.SetActive(false);
    }
}
