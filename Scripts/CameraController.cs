using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float swayAmount = 5;
    public float targetRotation = 0;
    private float smoothTime = 0.0297f; // Time to smooth to the target
    private float swaySmoothTime = 0.2f; // Time to smooth sway
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;

    private float lerpRotation;
    private float targetSwayRotation;
    private float lerpSwayRotation;
    private float rotationVelocity; // Velocity for SmoothDamp
    private float swayVelocity; // Velocity for SmoothDamp

    private void Start()
    {
        rightKey = DataManager.instance.gameData.rightKey;
        leftKey = DataManager.instance.gameData.leftKey;
    }

    private void Update()
    {
        // Smoothly interpolate rotation and sway
        lerpRotation = Mathf.SmoothDamp(lerpRotation, targetRotation, ref rotationVelocity, smoothTime);
        lerpSwayRotation = Mathf.SmoothDamp(lerpSwayRotation, targetSwayRotation, ref swayVelocity, swaySmoothTime);

        transform.eulerAngles = new Vector3(0, lerpRotation, lerpSwayRotation);
        MyInput();
    }

    private void RotateCam(int n)
    {
        targetRotation += n * 15;
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(rightKey))
        {
            RotateCam(1);
            targetSwayRotation = -swayAmount;
        }
        if (Input.GetKeyDown(leftKey))
        {
            RotateCam(-1);
            targetSwayRotation = swayAmount;
        }
        if (Mathf.Abs(targetRotation - lerpRotation) < 15f)
        {
            targetSwayRotation = 0;
        }
    }
}
