using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private const string KEY_STAGE_COMPLETED = "StageCompleted";
    private const string KEY_HIGH_SCORE = "HighScore";
    private const string KEY_GAME_DIFFICULTY = "GameDifficulty";
    private const string KEY_MUSIC_VOLUME = "MusicVolume";

    public static int StageCompleted {
        get {return PlayerPrefs.GetInt(KEY_STAGE_COMPLETED, 1);}
        set {
            PlayerPrefs.SetInt(KEY_STAGE_COMPLETED, value);
            PlayerPrefs.Save();
        }
    }
    public static int HighScore {
        get {return PlayerPrefs.GetInt(KEY_HIGH_SCORE, 0);}
        set {
            PlayerPrefs.SetInt(KEY_HIGH_SCORE, value);
            PlayerPrefs.Save();
        }
    }

    //Lưu độ khó hiện tại
    public static string GameDifficulty {
        get {return PlayerPrefs.GetString(KEY_GAME_DIFFICULTY, "Easy");}
        set {
            PlayerPrefs.SetString(KEY_GAME_DIFFICULTY, value);
            PlayerPrefs.Save();
        }
    }

    public static float MusicVolume {
        get {return PlayerPrefs.GetFloat(KEY_MUSIC_VOLUME, 1f);}
        set {
            PlayerPrefs.SetFloat(KEY_MUSIC_VOLUME, value);
            PlayerPrefs.Save();
        }
    }
}
