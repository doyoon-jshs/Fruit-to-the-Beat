using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RatingManager : MonoBehaviour
{
    [Header("Reference")]
    public LevelManager levelManager;
    public TMP_Text percentageTxt;
    public TMP_Text accuracyTxt;
    
    public int hitCounter = 0;
    public float gapSum = 0;
    public float percentage;

    private void Update()
    {
        if (hitCounter == levelManager.level.spawnDatas.Length)
        {
            levelManager.LevelComplete();
        }
    }

    public void RateBeat(int i)
    {
        hitCounter += 1;
        float hitTime = levelManager.secondCounter;
        float perfectHitTime = BpmToSecond(levelManager.level.spawnDatas[i].beatTime - 1 + SecondToBpm(levelManager.level.spawnDatas[0].durationSec));
        //float gap = Mathf.Abs(hitTime - perfectHitTime);
        float gap = hitTime - perfectHitTime;
//        Debug.Log(gap);
        gapSum += gap;
    }

    float BpmToSecond(float bpm)
    {
        return bpm * 60 / levelManager.level.bpm;
    }

    float SecondToBpm(float sec)
    {
        return sec / 60 * levelManager.level.bpm;
    }

    public void EvaluatePlayer()
    {
        float accuracy = (gapSum / hitCounter) * 1000;
        percentage = (float)hitCounter / levelManager.level.spawnDatas.Length * 100;
        percentageTxt.text = percentage.ToString("F2") + "%";
        accuracyTxt.text = accuracy.ToString("F2") + "ms";
        levelManager.UpdatePercentage();
    }
}
