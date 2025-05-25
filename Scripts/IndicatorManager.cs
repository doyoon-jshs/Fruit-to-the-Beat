using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    public Transform crosshairHolder;
    private float smoothTime = 0.08f; // Time to smooth to the target
    private float rotateAngle = 35;

    private LevelManager levelManager;
    private CameraController cameraController;

    private int i = 0;
    private float targetRotation;
    private float fruitRotation = 0;
    private float lerpRotation;
    private float rotationVelocity; // Velocity for SmoothDamp

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
        // Smoothly interpolate lerpRotation towards targetRotation
        lerpRotation = Mathf.SmoothDamp(lerpRotation, targetRotation, ref rotationVelocity, smoothTime);

        crosshairHolder.eulerAngles = new Vector3(0, 0, lerpRotation);

        if (!levelManager.isPlaying)
        {
            targetRotation = 0;
        }
        else if (cameraController.targetRotation > fruitRotation)
        {
            targetRotation = rotateAngle;
        }
        else if (cameraController.targetRotation < fruitRotation)
        {
            targetRotation = -rotateAngle;
        }
        else
        {
            targetRotation = 0;
        }
    }

    public void Next()
    {
        if (i + 1 < levelManager.level.spawnDatas.Length)
        {
            fruitRotation += levelManager.level.spawnDatas[i + 1].rotation * 15;
        }
        i++;
    }

    public void Reset()
    {
        fruitRotation = cameraController.targetRotation;
        i = 0;
    }
}
