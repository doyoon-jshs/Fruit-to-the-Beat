using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    public Toggle fullscreenToggle;

    private void Start()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            ToggleFullscreen();
        }
    }

    private void ToggleFullscreen()
    {
        bool isFullscreen = Screen.fullScreen;
        fullscreenToggle.isOn = !isFullscreen; // Toggle the UI Toggle state
        OnToggleValueChanged(!isFullscreen); // Change the screen mode
    }

    private void OnToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            // 현재 디스플레이의 최대 해상도로 전체 화면 모드로 전환
            Resolution[] resolutions = Screen.resolutions;
            Resolution maxResolution = resolutions[resolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            // 창 모드로 전환 (원하는 해상도로 변경)
            Screen.SetResolution(1280, 720, false); // 예시 해상도
        }
    }
}
