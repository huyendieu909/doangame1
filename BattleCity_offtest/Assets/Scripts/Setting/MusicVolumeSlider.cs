using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    // Tham chiếu đến Slider được gán qua Inspector
    public Slider volumeSlider;

    void Start()
    {
        // Đọc âm lượng đã lưu từ PlayerPrefs; nếu chưa có, dùng defaultVolume
        float savedVolume = PlayerPrefsManager.MusicVolume;

        // Cập nhật Slider và AudioListener với giá trị đã lưu
        volumeSlider.value = savedVolume;
        AudioListener.volume = savedVolume;

        // Thêm sự kiện khi giá trị của Slider thay đổi
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    // Hàm được gọi mỗi khi giá trị của Slider thay đổi
    void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefsManager.MusicVolume = value;
    }
}
