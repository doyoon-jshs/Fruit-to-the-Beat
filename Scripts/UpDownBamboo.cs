using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownBamboo : MonoBehaviour
{
    public float targetPosY;
    public float lerpSpeed = 5;
    private float startPosY;

    private void Start()
    {
        startPosY = transform.position.y;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, targetPosY, lerpSpeed * Time.deltaTime), transform.position.z);
    }

    public void GoDown()
    {
        targetPosY = -2;
    }

    public void GoUp()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Camera.main.GetComponent<CameraController>().targetRotation, transform.eulerAngles.z);
        targetPosY = 0;
    }
}
