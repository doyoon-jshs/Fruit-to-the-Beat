using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingManager : MonoBehaviour
{
    public float volume = 0;
    //public TMP_InputField sensitivityInput;
    //public KeyCode selectedKey;
    public GameObject clickSound;
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public TMP_Text rightTxt;
    public TMP_Text leftTxt;
    //public TMP_Text calibrationTxt;
    public TMP_InputField calibrationInput;

    private KeyCode selectedKey;

    public void Start()
    {
        volume = DataManager.instance.gameData.volume;
        UpdateUI();
    }

    public void UpdateUI()
    {
        volumeSlider.value = volume;
        rightTxt.text = DataManager.instance.gameData.rightKey.ToString();
        leftTxt.text = DataManager.instance.gameData.leftKey.ToString();
        calibrationInput.text = DataManager.instance.gameData.calibration.ToString("F0") + "ms";
        //calibrationTxt.text = DataManager.instance.gameData.calibration.ToString("F0") + "ms";
    }

    public void ButtonClickSound()
    {
        if (Time.timeSinceLevelLoad < 1)
        {
            return;
        }
        Instantiate(clickSound);
    }
    public void SetCalibration()
    {
        if (int.Parse(calibrationInput.text) > 1000)
        {
            ResetCalibration();
        }
        DataManager.instance.gameData.calibration = int.Parse(calibrationInput.text);
        calibrationInput.text = calibrationInput.text + "ms";
    }
    public void ResetCalibration()
    {
        DataManager.instance.gameData.calibration = 0;
        calibrationInput.text = "0";
    }

    public void SetVolume(float sliderVolume)
    {
        volume = sliderVolume;
        DataManager.instance.gameData.volume = volume;
        audioMixer.SetFloat("volume", DataManager.instance.gameData.volume);
    }

    public void TurnRightInput()
    {
        rightTxt.text = ".....";
        
        // 추가적인 초기화 작업이 필요할 경우 여기에 작성
        StartCoroutine(WaitForKeyPress("right"));
    }
    public void TurnLeftInput()
    {
        leftTxt.text = ".....";
        
        // 추가적인 초기화 작업이 필요할 경우 여기에 작성
        StartCoroutine(WaitForKeyPress("left"));
    }

    private IEnumerator WaitForKeyPress(string type)
    {
        // 마우스 버튼이나 키보드를 기다립니다.
        while (true)
        {
            // 모든 키를 감지 (A-Z, 0-9, 특수 문자, 제어 키 포함)
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key) && key != KeyCode.None)
                {
                    selectedKey = key;
                    if (type == "right")
                    {
                        DataManager.instance.gameData.rightKey = selectedKey;
                        rightTxt.text = selectedKey.ToString();
                    }
                    if (type == "left")
                    {
                        DataManager.instance.gameData.leftKey = selectedKey;
                        leftTxt.text = selectedKey.ToString();
                    }
                    yield break; // 입력이 감지되면 코루틴 종료
                }
            }

            yield return null; // 다음 프레임까지 대기
        }
    }
}
