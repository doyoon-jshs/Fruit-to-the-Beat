using UnityEngine;

[CreateAssetMenu(fileName = "GameLevel", menuName = "GameLevel", order = 0)]
public class GameLevel : ScriptableObject
{
    public int id;
    public string levelName;
    public string difficulty;
    public string genre;
    public float bpm = 60;
    public int rewardKey = 1;
    public ThemeManager.ThemeType theme;
    public SpawnData[] spawnDatas;
    public AudioClip soundTrack;

    [System.Serializable]
    public class SpawnData
    {
        public float beatTime;
        public int rotation;
        public float durationSec;
    }
}