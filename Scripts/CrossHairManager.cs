using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Runtime.ExceptionServices;

public class CrossHairManager : MonoBehaviour
{
    public RectTransform centreDiamond;
    public RectTransform leftTargetPoint;
    public RectTransform rightTargetPoint;
    public GameObject leftPrefab;
    public GameObject rightPrefab;
    public LevelManager levelManager;
    public Ease moveEase;
    public Ease popEase;
    public Vector3 centrePopSize;
    public Vector3 entirePopSize;
    public float entirePopDuration;
    public float centrePopDuration;
    public float beatFadeDuration = 0.1f;
    public float beatAppearDuration;

    private Vector3 centreNormalSize;
    private CameraController cameraController;

    private void Start()
    {
        centreNormalSize = centreDiamond.localScale;
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
        if (Input.anyKeyDown && !Input.GetKeyDown(cameraController.rightKey) && !Input.GetKeyDown(cameraController.leftKey))
        {
            centreDiamond.DOScale(centrePopSize, centrePopDuration).SetEase(popEase).OnComplete(() => 
            {
                centreDiamond.DOScale(centreNormalSize, centrePopDuration).SetEase(popEase);
            });
        }
    }

    public void SpawnCrosshair(float duration, int i)
    {
        GameObject right = Instantiate(rightPrefab, transform);
        right.SetActive(true);
        right.name = "right " + i;
        right.GetComponent<RectTransform>().DOAnchorPos(rightTargetPoint.anchoredPosition, duration).SetEase(moveEase);
        right.GetComponent<Image>().DOFade(1, beatAppearDuration);
        GameObject left = Instantiate(leftPrefab, transform);
        left.SetActive(true);
        left.name = "left " + i;
        left.GetComponent<RectTransform>().DOAnchorPos(leftTargetPoint.anchoredPosition, duration).SetEase(moveEase);
        left.GetComponent<Image>().DOFade(1, beatAppearDuration);
    }

    public void BeatDestroy(int i)
    {
        GameObject right = GameObject.Find("right " + i);
        // Destroy(right);
        right.GetComponent<Image>().DOFade(0, beatFadeDuration).OnComplete(() => { Destroy(right); });
        GameObject left = GameObject.Find("left " + i);
        // Destroy(left);
        left.GetComponent<Image>().DOFade(0, beatFadeDuration).OnComplete(() => { Destroy(left); });
    }

    public void EntirePop()
    {
        transform.DOScale(entirePopSize, entirePopDuration).SetEase(popEase).OnComplete(() => 
        {
            transform.DOScale(Vector3.one, entirePopDuration).SetEase(popEase);
        });
    }
}
