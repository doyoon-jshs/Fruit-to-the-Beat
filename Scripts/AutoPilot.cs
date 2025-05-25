using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoPilot : MonoBehaviour
{
    public PlayerCam playerCam;
    public LevelManager levelManager;
    public HitController hitController;
    public Ease rotateEase;

    private int i = 0;
    public bool isPlaying = false;
    private float angle = 0;
    private float rotationOffet;

    public void StartAutoPilot()
    {
        isPlaying = true;
        playerCam.enabled = false;
        rotationOffet = Camera.main.transform.eulerAngles.y;
    }

    private void Update()
    {
        if (isPlaying)
        {
            //playerCam.transform.eulerAngles = new Vector3(0, playerCam.transform.eulerAngles.y, playerCam.transform.eulerAngles.z);
            float perfectHitTime = BpmToSecond(levelManager.level.spawnDatas[i].beatTime - 1 + SecondToBpm(levelManager.level.spawnDatas[0].durationSec));
            if (levelManager.secondCounter >= perfectHitTime)
            {
                if (levelManager.autoBeat)
                {
                    hitController.Hit();
                }
                if (i + 1 < levelManager.level.spawnDatas.Length)
                {
                    //angle += levelManager.level.spawnDatas[i + 1].angle;
                    float a = BpmToSecond(levelManager.level.spawnDatas[i].beatTime);
                    float b = BpmToSecond(levelManager.level.spawnDatas[i + 1].beatTime);
                    if (levelManager.autoAim)
                    {
                        playerCam.transform.DORotate(Vector3.up * (angle + rotationOffet), b-a).SetEase(rotateEase);
                    }
                    i++;
                }
            }
        }
    }

    float BpmToSecond(float bpm)
    {
        return bpm * 60 / levelManager.level.bpm;
    }

    float SecondToBpm(float sec)
    {
        return sec / 60 * levelManager.level.bpm;
    }
}
