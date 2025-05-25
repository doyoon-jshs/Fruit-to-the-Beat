using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class LevelManager : MonoBehaviour
{
    [Header("Reference")]
    public GameLevel level;
    public Transform spawnPosHolder;
    public Transform spawnPos;
    public GameObject[] fruitPrefabs;
    public CrossHairManager crossHairManager;
    public UpDownBamboo gameOverAnim;
    public UpDownBamboo startAnim;
    public RatingManager ratingManager;
    public GameObject sword;
    public HitController hitController;
    public CameraController cameraController;
    public GameObject fireworks;
    public TextMeshProUGUI info;
    private IndicatorManager indicatorManager;
    [Header("Option")]
    public bool autoAim = false;
    public bool autoBeat = false;
    public float targetPosY;
    public Ease fallEase;
    public AudioMixerGroup mixer;

    [HideInInspector]
    public float beatCounter;
    [HideInInspector]
    public float secondCounter;
    public float startTime;
    private int i = 0;
    public bool isComplete = false;
    public bool isPlaying = false;
    public bool isGameOver = false;
    private AudioSource soundTrack;
    private float rotationOffet;
    private GameObject fruitPrefab;
    private int currentAngle = 0;

    public float swordThrowForce = 10;

    private void Awake()
    {
        if (DataManager.instance != null)
        {
            level = DataManager.instance.gameLevel;
        }
        indicatorManager = GetComponent<IndicatorManager>();
    }

    private void Update()
    {
        if (!isComplete && isPlaying && level.spawnDatas.Length != i)
        {
            if (level.spawnDatas[i].beatTime - 1 - SecondToBpm(level.spawnDatas[i].durationSec) <= beatCounter - SecondToBpm(level.spawnDatas[0].durationSec))
            {
                crossHairManager.SpawnCrosshair(level.spawnDatas[i].durationSec, i);
                Spawn();
            }
        }
        if (Input.GetMouseButtonDown(0)) {
            //Debug.Log(SecondToBpm(Time.time - startTime));
        }
        if (isPlaying)
        {
            beatCounter += Time.deltaTime / 60 * level.bpm;
            secondCounter += Time.deltaTime;
        }
    }
    private void Spawn()
    {
        RandomFruit();
        currentAngle += level.spawnDatas[i].rotation;
        spawnPosHolder.eulerAngles = new Vector3(0, currentAngle * 15 + rotationOffet, 0);
        GameObject newFruit = Instantiate(fruitPrefab, spawnPos.position, Quaternion.identity);
        newFruit.GetComponent<FruitDrop>().DropStart(targetPosY, level, i, fallEase);
        newFruit.name = i.ToString();
        
        i++;
    }
    public void GameOver()
    {
        hitController.ResetHitCounter();
        isGameOver = true;
        ratingManager.EvaluatePlayer();
        ThrowSword();
        gameOverAnim.GoUp();
        isPlaying = false;
        soundTrack.DOFade(0, 1);
        Destroy(soundTrack.gameObject, 1);
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");

        foreach (GameObject fruit in fruits)
        {
            Destroy(fruit);
        }
        GameObject[] crosshairs = GameObject.FindGameObjectsWithTag("Crosshair");

        foreach (GameObject crosshair in crosshairs)
        {
            crosshair.GetComponent<Image>().DOFade(0, 0.3f).OnComplete(() => { Destroy(crosshair); });
        }
    }

    public void StartGame()
    {
        if (isPlaying || DataManager.instance.isCalibration)
        {
            return;
        }
        ResetSettings();
        isPlaying = true;
        isGameOver = false;
        //info.DOFade(0, 0.1f);
        hitController.UpdateHitCounter();
        fireworks.SetActive(false);
        sword.SetActive(true);
        currentAngle = 0;
        startAnim.GoDown();

        rotationOffet = cameraController.targetRotation;
        indicatorManager.Reset();
        GameObject newSoundTrackObj = new GameObject();
        newSoundTrackObj.AddComponent<AudioSource>();
        soundTrack = newSoundTrackObj.GetComponent<AudioSource>();
        soundTrack.clip = level.soundTrack;
        soundTrack.outputAudioMixerGroup = mixer;
        isComplete = false;
        //soundTrack.PlayDelayed(level.spawnDatas[0].durationSec);
        soundTrack.PlayDelayed(level.spawnDatas[0].durationSec - DataManager.instance.gameData.calibration / 1000);
        //Invoke("InvokeStartGame", level.spawnDatas[0].durationSec - DataManager.instance.gameData.calibration / 1000);
    }

    private void InvokeStartGame()
    {
        //soundTrack.Play();
    }

    public void LevelComplete()
    {
        soundTrack.DOFade(0, 1);
        Destroy(soundTrack.gameObject, 1);
        ratingManager.EvaluatePlayer();
        fireworks.SetActive(true);
        ThrowSword();
        isComplete = true;
        ResetSettings();
        gameOverAnim.GoUp();
    }

    private void ResetSettings()
    {
        ratingManager.gapSum = 0;
        ratingManager.hitCounter = 0;
        beatCounter = 0;
        isPlaying = false;
        secondCounter = 0;
        i = 0;
    }

    private void RandomFruit()
    {
        fruitPrefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
    }

    float SecondToBpm(float sec)
    {
        return sec / 60 * level.bpm;
    }

    private void ThrowSword()
    {
        GameObject newSword = Instantiate(sword, sword.transform.position, sword.transform.rotation);
        sword.SetActive(false);
        newSword.AddComponent<Rigidbody>().AddForce((Camera.main.transform.forward + Vector3.up * 0.7f) * swordThrowForce, ForceMode.Impulse);
        newSword.GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * 20000, ForceMode.Impulse);
        Destroy(newSword, 10);
    }

    public void UpdatePercentage()
    {
        if (DataManager.instance.gameData.percentages[DataManager.instance.levelId] != 100 && ratingManager.percentage == 100)
        {
            DataManager.instance.gameData.key += level.rewardKey;
        }
        if (ratingManager.percentage > DataManager.instance.gameData.percentages[DataManager.instance.levelId])
        {
            DataManager.instance.gameData.percentages[DataManager.instance.levelId] = Mathf.RoundToInt(ratingManager.percentage);
        }
    }
}
