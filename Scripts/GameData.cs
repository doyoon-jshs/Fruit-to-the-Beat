using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public int key;
    public int[] percentages = new int[500]; // 초기값을 위해 배열 크기 설정
    public bool[] unlockDatas = new bool[500];
    public float volume = 0.0f; // 기본 볼륨
    public float scrollValue = 0.0f; // 기본 스크롤 값
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;
    public string currentLanguage = "en";
    public bool isFullscreen = false; // 전체 화면 모드
    public float calibration = 0;
}