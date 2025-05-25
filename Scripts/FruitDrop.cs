using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FruitDrop : MonoBehaviour
{
    private float fallDuration = 0.5f;
    private Ease easeAfter = Ease.InQuad;
    private float rotateSpeed = 1000;
    private Vector3 rotateDirection;

    private void Start()
    {
        rotateDirection = Random.onUnitSphere;
    }

    public void DropStart(float targetPosY, GameLevel level, int i, Ease ease)
    {
        transform.DOMoveY(targetPosY, level.spawnDatas[i].durationSec).SetEase(ease).OnComplete(OnMoveComplete);
    }

    private void OnMoveComplete()
    {
        transform.DOMoveY(0, fallDuration).SetEase(easeAfter).OnComplete(OnFallGround);
    }

    private void Update()
    {
        transform.Rotate(rotateDirection * rotateSpeed * Time.deltaTime);
    }
    private void OnFallGround()
    {
        GameObject.Find("Manager").GetComponent<LevelManager>().GameOver();
        //transform.GetComponent<FruitOnDestroy>().FruitEffect();
    }
}
