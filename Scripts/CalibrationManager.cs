using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine.Localization.Settings;

public class CalibrationManager : MonoBehaviour
{
    public GameObject[] disableObjects;
    public GameObject dummyFruit;
    public TMP_Text description;
    public CameraController cameraController;
    public HitController hitController;
    public AudioSource metronomeAudio; // 메트로놈 음악을 담는 AudioSource
    private float inputCount = 0;
    private float totalInputNum = 25;
    private float sumInputNum;
    private bool isComplete;

    private float startTime;


    private void Awake()
    {
        if (!DataManager.instance.isCalibration)
        {
            metronomeAudio.enabled = false;
            enabled = false;
        }
    }

    private void Start()
{
    metronomeAudio.PlayDelayed(0.7f);
    StartCoroutine(InvokeStart());
    //Invoke("InvokeStart", 0.7f);
    //Debug.Log(Time.timeSinceLevelLoad);
    foreach (GameObject obj in disableObjects)
    {
        obj.SetActive(false);
    }
    dummyFruit.SetActive(true);
    description.gameObject.SetActive(true);
    description.text = LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "calibration 1");
}

private IEnumerator InvokeStart()
{
    yield return new WaitForSeconds(0.7f);
    //metronomeAudio.Play();
    startTime = Time.time;
    //Debug.Log(Time.timeSinceLevelLoad);
}

    private void Update()
    {
        if(Input.anyKeyDown && cameraController.targetRotation % 360 == 0 && !Input.GetKeyDown(DataManager.instance.gameData.rightKey) && !Input.GetKeyDown(DataManager.instance.gameData.leftKey))
        {
            if (inputCount == 0)
            {
                inputCount++;
                description.text = "---ms" + " (" + (totalInputNum - inputCount).ToString() + ")";
            }
            else if(inputCount > 0 && inputCount < totalInputNum)
            {
                dummyFruit.transform.Rotate(new Vector3(0, 0, 45));
                sumInputNum += CalculateAccuracy();
                inputCount++;
                description.text = (CalculateAccuracy() * 1000).ToString("F0") + "ms" + " (" + (totalInputNum - inputCount).ToString() + ")";
            }
            if (isComplete)
            {
                hitController.GoToMenu();
            }
            if (inputCount >= totalInputNum)
            {
                float result = sumInputNum/totalInputNum * 1000;
                description.text = LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "calibration 2") + " " + result.ToString("F0") + "ms";
                metronomeAudio.DOFade(0, 0.5f);
                DataManager.instance.gameData.calibration = result;
                isComplete = true;
            }
        }
    }

    private float CalculateAccuracy()
    {
        float elapsedTime = Time.time - startTime;
        float nearestMultiple = Mathf.Round(elapsedTime / 0.5f) * 0.5f;
        float distance = elapsedTime - nearestMultiple; // 음수일 수도 있음
        return distance;
    }
}
