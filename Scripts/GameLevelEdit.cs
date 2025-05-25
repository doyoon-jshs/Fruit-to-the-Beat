using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelEdit : MonoBehaviour
{
    public GameLevel gameLevel;

    public float fallDuration = 0.7f;
    public float minimumGap = 1;
    public float longGap = 4;

    public bool doDuration = false;
    public bool doRotation = false;

    private void Start()
    {
        if (doDuration)
        {
            SetSpawnDataDurations(gameLevel);
        }
        if (doRotation)
        {
            UpdateRotations(gameLevel);
        }
    }

    private void SetSpawnDataDurations(GameLevel gameLevel)
    {
        if (gameLevel.spawnDatas != null)
        {
            foreach (var spawnData in gameLevel.spawnDatas)
            {
                spawnData.durationSec = fallDuration; // sharedDuration 값을 durationSec에 할당
            }
        }
    }

    private void UpdateRotations(GameLevel gameLevel)
    {
        if (gameLevel.spawnDatas != null && gameLevel.spawnDatas.Length > 1)
        {
            for (int i = 1; i < gameLevel.spawnDatas.Length; i++)
            {
                float gap = gameLevel.spawnDatas[i].beatTime - gameLevel.spawnDatas[i - 1].beatTime;

                if (gap > longGap)
                {
                    // gap이 longGap보다 크면 rotation을 1, 0, 또는 -1로 무작위 변경
                    gameLevel.spawnDatas[i].rotation = Random.Range(-2, 3); // -1, 0, 1 중 하나
                }
                else if (gap > minimumGap)
                {
                    // gap이 minimumGap보다 크면 rotation을 1, 0, 또는 -1로 무작위 변경
                    gameLevel.spawnDatas[i].rotation = Random.Range(-1, 2);
                }
                else
                {
                    gameLevel.spawnDatas[i].rotation = 0;
                }
            }
        }
    }
}
