using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionToData : MonoBehaviour
{
    public GameLevel gameLevel;
    public float fallDuration = 1;

    private void Start()
    {
        gameLevel.spawnDatas = new GameLevel.SpawnData[0];
    }

    public void BeatAdd(float beatTime)
    {
        GameLevel.SpawnData newSpawnData = new GameLevel.SpawnData();
        newSpawnData.beatTime = beatTime + 1;
        newSpawnData.durationSec = fallDuration;
        AddElement(newSpawnData);
    }

    void AddElement(GameLevel.SpawnData newElement)
    {
        GameLevel.SpawnData[] newArray = new GameLevel.SpawnData[gameLevel.spawnDatas.Length + 1];
        for (int i = 0; i < gameLevel.spawnDatas.Length; i++)
        {
            newArray[i] = gameLevel.spawnDatas[i];
        }
        newArray[newArray.Length - 1] = newElement;
        gameLevel.spawnDatas = newArray;
    }
}
