using UnityEngine;

public class WindowedModeAspect : MonoBehaviour
{
    private void Start()
    {
        // 초기 설정으로 창 크기를 비율에 맞게 설정
        SetWindowedResolution();
    }

    private void Update()
    {
        if (!DataManager.instance.gameData.isFullscreen)
        {
            // 윈도우 모드일 때 비율 유지
            SetWindowedResolution();
        }
    }

    private void SetWindowedResolution()
    {
        // 모니터의 최대 해상도를 가져옵니다.
        Resolution[] resolutions = Screen.resolutions;
        Resolution maxResolution = resolutions[resolutions.Length - 1];

        // 비율을 유지하면서 윈도우 크기를 설정합니다.
        float aspectRatio = (float)maxResolution.width / maxResolution.height;
        int windowWidth = Mathf.RoundToInt(maxResolution.height * aspectRatio);
        int windowHeight = maxResolution.height;

        // 윈도우 크기를 설정합니다.
        Screen.SetResolution(windowWidth, windowHeight, false);
    }
}
