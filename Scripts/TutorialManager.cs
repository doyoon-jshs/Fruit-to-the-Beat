using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject startBamboo;
    public GameObject againBamboo;
    private LevelManager levelManager;
    
    private string[] steps = {
        "tutorial 1-1",
        "tutorial 1-2",
        "tutorial 1-3",
    };
    public TMP_Text tutorialText; // 튜토리얼 텍스트
    private int currentStep = 0; // 현재 단계
    private bool isAnyKeyComplete = false;
    private bool isStartedPlaying = false;

    private void Start()
    {
        levelManager = GetComponent<LevelManager>();
        if (levelManager.level.levelName != "Hello World")
        {
            enabled = false;
        }
        else
        {
            tutorialText.gameObject.SetActive(true);
            startBamboo.GetComponent<UpDownBamboo>().GoDown();
            Camera.main.GetComponent<CameraController>().enabled = false;
            UpdateTutorialText();
        }
    }


    void Update()
    {
        // 인풋을 체크
        if (Input.anyKey && !isAnyKeyComplete)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }
            isAnyKeyComplete = true;
            HandleStepAction();
        }
        else if (levelManager.isPlaying && !isStartedPlaying)
        {
            isStartedPlaying = true;
            HandleStepAction();
        }
        else if (isStartedPlaying && levelManager.isGameOver)
        {
            //levelManager.startAnim.GoUp();
            currentStep = 1;
            HandleStepAction();
        }
        else if (levelManager.isComplete)
        {
            againBamboo.SetActive(false);
            HandleStepAction();
        }
    }

    void HandleStepAction()
    {
        // 현재 단계에 따른 행동 수행
        switch (currentStep)
        {
            case 0:
                // 1단계 행동
                PerformStep1Action();
                break;
            case 1:
                // 2단계 행동
                PerformStep2Action();
                break;
            case 2:
                // 3단계 행동
                PerformStep3Action();
                break;
        }

        // 다음 단계로 진행
        NextStep();
    }

    void NextStep()
    {
        if (currentStep < steps.Length - 1)
        {
            currentStep++;
            UpdateTutorialText();
        }
        else
        {
            // 튜토리얼 종료
            tutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("String Table", "tutorial 1-3");
        }
    }

    void UpdateTutorialText()
    {
        
        //tutorialText.text = steps[currentStep];
        if (steps[currentStep] == "tutorial 1-3")
        {
            tutorialText.text = "";
            return;
        }
        tutorialText.text = LocalizationSettings.StringDatabase.GetLocalizedString("String Table", steps[currentStep]);
    }

    void PerformStep1Action()
    {
        startBamboo.GetComponent<UpDownBamboo>().GoUp();
    }

    void PerformStep2Action()
    {
        //levelManager.StartGame();
    }

    void PerformStep3Action()
    {
        
    }
}
