using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Profiling.Memory;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    // Capture progress: từ 0 đến requiredProgress
    public float captureProgress { get; set; }
    // Trạng thái flag đã bị chiếm hay chưa
    public bool isCaptured { get; set; }
    [Header("Flag Settings")]  
    [Tooltip("Tên tag của tank đối phương đến chiếm. P1 là PlayerTank, P2 là EnemyTank")]
    public string teamName = "";  
    // Ngưỡng để capture thành công
    public float requiredProgress = 100f;
    // Tốc độ tăng progress (điểm/giây)
    public float captureRate = 20f;
    private int enemyCount = 0;
    
    [Header("Optional UI")]
    // Hiển thị tiến trình capture bằng UI
    public TextMeshProUGUI progressText;

    void Start()
    {
        captureProgress = 0f;
        isCaptured = false;
    }
    void Update()
    {
        if (enemyCount > 0 && !isCaptured) {
            captureProgress += captureRate * Time.deltaTime;
            if (captureProgress >= requiredProgress) {
                captureProgress = requiredProgress;
                CaptureFlag();
            }
        }
        else if (enemyCount == 0 && captureProgress > 0f && !isCaptured) {
            captureProgress -= captureRate * Time.deltaTime;
            if (captureProgress < 0f) captureProgress = 0f;
        }
        if (progressText != null) {
            progressText.text = $"{captureProgress:F0} / {requiredProgress}";
            if (captureProgress == 0 || captureProgress == 100) progressText.text = "";
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(teamName)){
            enemyCount++;
        }       
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(teamName)){
            enemyCount = Mathf.Max(0, enemyCount-1);
        }
    }
    
    void CaptureFlag() {
        isCaptured = true;
        //đổi ảnh cờ
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(true);
        //cập nhật số lượng cờ đã captured
        var mGPM = GameObject.Find("StageManager").GetComponent<MyGamePlayManager>();
        if (teamName == "EnemyTank") {
            mGPM.P2CapturedAFlag();
        }
        else {
            mGPM.P1CapturedAFlag();
        }
        Debug.Log($"{teamName} Capture Flag!");
    }
}
