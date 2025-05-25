using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public GameData gameData;
    public GameLevel gameLevel;
    public int levelId;
    public bool isCalibration;

    private void Awake()
    {
        if (gameData == null)
    {
        Debug.LogError("GameData instance is null! Please assign it in the Inspector.");
        return;
    }
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SaveGame()
    {
        Debug.Log("Game Saved");
        string json = JsonUtility.ToJson(gameData);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    private void LoadGame()
    {
        Debug.Log("Game Loaded");
        string path = Application.persistentDataPath + "/savefile.json";

        // 처음 실행 여부 확인
        // if (!PlayerPrefs.HasKey("IsFirstRun1"))
        // {
        //     // 기본값 설정
        //     SetDefaultGameData();

        //     // 첫 실행 여부 저장
        //     PlayerPrefs.SetInt("IsFirstRun1", 1);
        //     PlayerPrefs.Save();
        //     Debug.Log("First run detected. Default values set and saved.");

        //     // 기본값 저장
        //     SaveGame(); // 초기값을 저장
        // }
        if (!System.IO.File.Exists(path))
        {
            SetDefaultGameData();
            SaveGame();
        }
        else
        {
            string json = System.IO.File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, gameData);
            Debug.Log("Game data loaded from: " + path);
        }
        // else
        // {
        //     Debug.LogWarning("Save file not found at: " + path);
        // }
    }

    private void SetDefaultGameData()
    {
        if (gameData == null)
        {
            Debug.LogError("GameData instance is null! Please assign it in the Inspector.");
            return;
        }

        // 초기값 설정
        gameData.percentages = new int[500]; // 원하는 배열 크기 설정
        for (int i = 0; i < gameData.percentages.Length; i++)
        {
            gameData.percentages[i] = 0; // 모든 값을 0으로 설정
        }
        gameData.unlockDatas = new bool[500];
        for (int i = 0; i < gameData.unlockDatas.Length; i++)
        {
            gameData.unlockDatas[i] = false;
        }
        gameData.unlockDatas[0] = true;
        gameData.volume = 0.0f;
        gameData.scrollValue = 0.0f;
        gameData.rightKey = KeyCode.RightArrow;
        gameData.leftKey = KeyCode.LeftArrow;
        gameData.key = 0;
        //gameData.currentLanguage = "en";
        gameData.isFullscreen = true; // 전체 화면 모드
        gameData.calibration = 0;
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
