using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensitivity = 200f;
    private float sensX;
    private float sensY;

    public LevelManager levelManager;

    float xRotation;
    float yRotation;

    void Start()
    {
        sensX = sensitivity;
        sensY = sensitivity;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        if (!levelManager.isPlaying)
        {      
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;
            xRotation -= mouseY;
        }

        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        if (levelManager.isPlaying)
        {
            xRotation = 0;
        }
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

    }
}
