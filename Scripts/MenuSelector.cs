using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuSelector : MonoBehaviour
{
    [Header("Reference")]
    public Transform mainMenuHolder;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public BambooSelector bambooSelector;
    public PanelFade panel;
    public PanelFade settingPanel;
    public PanelFade settingHidePanel;
    public GameObject clickSound;
    public GameObject settingsHolder;
    public GameObject creditHolder;
    public SettingManager settingManager;
    public CanvasGroup levelSelectGroup;
    public TextMeshProUGUI keyCountTxt;
    public GameLevel calibrationLevel;
    [Header("Option")]
    public float underGroundY;
    public float lerpSpeed;
    public float textFadeDuration;
    public float settingAppearDuraton = 0.3f;
    public Ease settingAppearEase;

    private float targetPosY;
    private bool disableRaycast = false;
    private bool isInLevelSelect = false;
    private bool isInSetting = false;
    private bool isInCredit = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isInSetting)
        {
            ExitSetting();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && isInCredit)
        {
            ExitCredit();
        }

        keyCountTxt.text = DataManager.instance.gameData.key.ToString();
        if (Input.GetKeyDown(KeyCode.Escape) && isInLevelSelect)
        {
            LevelSelectBack();
        }
        if (!isInLevelSelect)
        {
            description.text = "";
        }
        if (Input.GetMouseButtonDown(0) && !disableRaycast)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Main Menu Bamboo"))
                {
                    Instantiate(clickSound);
                    string name = hit.collider.gameObject.name;

                    switch (name)
                    {
                        case "Start":
                            GoToLevelSelect();
                            break;
                        case "Setting":
                            GoToSetting();
                            break;
                        case "Quit":
                            QuitGame();
                            break;
                        case "Level Select Back":
                            LevelSelectBack();
                            break;
                    }
                }
                if (hit.collider.CompareTag("Level Select Bamboo"))
                {
                    Instantiate(clickSound);
                    if (hit.collider.GetComponent<BambooMover>().isUnlocked)
                    {
                        LoadLevel(int.Parse(hit.collider.name));
                    }
                    else if (bambooSelector.canScroll)
                    {
                        //check if has key
                        if (DataManager.instance.gameData.key > 0)
                        {
                            DataManager.instance.gameData.key--;
                            hit.collider.GetComponent<BambooMover>().Unlock();
                            DataManager.instance.gameData.unlockDatas[bambooSelector.gameLevel[int.Parse(hit.collider.name)].id] = true;
                        }
                    }
                }
            }
        }
        mainMenuHolder.position = new Vector3(mainMenuHolder.position.x, Mathf.Lerp(mainMenuHolder.position.y, targetPosY, lerpSpeed * Time.deltaTime), mainMenuHolder.position.z);
        Point();
    }

    public void GoToCalibration()
    {
        DataManager.instance.isCalibration = true;
        DataManager.instance.gameLevel = calibrationLevel;
        panel.DoAppear();
        Invoke("InvokeLoadScene", panel.duration);
    }

    public void CreditButton()
    {
        Invoke("GoToCredit", settingHidePanel.duration);
    }

    private void GoToCredit()
    {
        isInCredit = true;
        disableRaycast = true;

        settingPanel.DoAppear();
        Invoke("InvokeGoToCredit", settingPanel.duration);
        title.DOFade(0, textFadeDuration);
    }
    private void InvokeGoToCredit()
    {
        creditHolder.SetActive(true);
        settingHidePanel.gameObject.SetActive(true);
        settingHidePanel.DoFade();
    }
    public void ExitCredit()
    {
        isInCredit = false;
        settingHidePanel.DoAppear();
        Invoke("InvokeExitCredit", settingHidePanel.duration);
    }
    private void InvokeExitCredit()
    {
        creditHolder.SetActive(false);
        settingHidePanel.gameObject.SetActive(false);
        settingPanel.DoFade();
        title.DOFade(1, textFadeDuration);
        disableRaycast = false;
    }

    private void GoToSetting()
    {
        isInSetting = true;
        disableRaycast = true;
        
        settingPanel.DoAppear();
        Invoke("InvokeGoToSetting", settingPanel.duration);
        title.DOFade(0, textFadeDuration);
    }
    private void InvokeGoToSetting()
    {
        settingsHolder.SetActive(true);
        settingHidePanel.gameObject.SetActive(true);
        settingHidePanel.DoFade();
        settingManager.UpdateUI();
    }

    public void ExitSetting()
    {
        isInSetting = false;
        settingHidePanel.DoAppear();
        Invoke("InvokeExitSetting", settingHidePanel.duration);
    }
    private void InvokeExitSetting()
    {
        settingsHolder.SetActive(false);
        settingHidePanel.gameObject.SetActive(false);
        settingPanel.DoFade();
        title.DOFade(1, textFadeDuration);
        disableRaycast = false;
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    private void GoToLevelSelect()
    {
        DOTween.To(() => levelSelectGroup.alpha, x => levelSelectGroup.alpha = x, 1, textFadeDuration);
        isInLevelSelect = true;
        targetPosY = underGroundY;
        title.DOFade(0, textFadeDuration);
        bambooSelector.targetPosY = 0;
    }

    private void LevelSelectBack()
    {
        DOTween.To(() => levelSelectGroup.alpha, x => levelSelectGroup.alpha = x, 0, textFadeDuration);
        isInLevelSelect = false;
        targetPosY = 0;
        title.DOFade(1, textFadeDuration);
        bambooSelector.targetPosY = bambooSelector.underGroundY;
    }

    private void Point()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Main Menu Bamboo") || hit.collider.CompareTag("Level Select Bamboo"))
            {
                hit.collider.transform.GetComponent<BambooInteract>().SetPointed();
                if (hit.collider.CompareTag("Level Select Bamboo") && isInLevelSelect)
                {
                    description.text = "difficulty: " + 
                    bambooSelector.gameLevel[int.Parse(hit.collider.name)].difficulty + "\n" + "genre: " +
                    bambooSelector.gameLevel[int.Parse(hit.collider.name)].genre + "\n" + "length: " + 
                    Mathf.RoundToInt(bambooSelector.gameLevel[int.Parse(hit.collider.name)].soundTrack.length) + "s";
                }
            }
            else
            {
                if (isInLevelSelect)
                {
                    description.text = "back to menu: esc";
                }
            }
        }
        else
        {
            if (isInLevelSelect)
            {
                description.text = "back to menu: esc";
            }
        }
    }
    
    private void LoadLevel(int levelNum)
    {
        DataManager.instance.isCalibration = false;
        DataManager.instance.gameLevel = bambooSelector.gameLevel[levelNum];
        DataManager.instance.levelId = bambooSelector.gameLevel[levelNum].id;
        panel.DoAppear();
        Invoke("InvokeLoadScene", panel.duration);
    }
    private void InvokeLoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
