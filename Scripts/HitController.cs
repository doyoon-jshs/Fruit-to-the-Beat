using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HitController : MonoBehaviour
{
    [Header("Hit")]
    public LayerMask whatIsFruit;
    public SwordAnimator swordAnimator;
    public float reach = 3f;
    public float hitDelay = 0.01f;
    [Header("Reference")]
    public GameObject clickSound;
    public LevelManager levelManager;
    public PanelFade panelFade;
    public UpDownBamboo gameOverAnim;
    public RatingManager ratingManager;
    public CrossHairManager crossHairManager;
    public CameraController cameraController;
    public IndicatorManager indicatorManager;
    public GameObject[] swordSound;
    public GameObject[] drumSound;
    public TMP_Text hitCounterTxt;

    private int hitCounter = 7;

    private Camera cam;
    private float lastHit = 0;

    private void Start()
    {
        cam = Camera.main;
        //hitDelay = 60 / levelManager.level.bpm / 8;
    }

    private void Update()
    {
        if (Input.anyKeyDown && Time.time - lastHit >= hitDelay && !Input.GetKeyDown(cameraController.rightKey) && !Input.GetKeyDown(cameraController.leftKey) && !Input.GetKeyDown(KeyCode.Escape))
        {
            Hit();
        }

        Point();
        if (!Input.GetKeyDown(cameraController.rightKey) && !Input.GetKeyDown(cameraController.leftKey) && !Input.GetKeyDown(KeyCode.Escape))
        {
            OperateBamboo();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMenu();
        }
    }

    public void UpdateHitCounter()
    {
        hitCounterTxt.text = hitCounter.ToString();
    }
    public void ResetHitCounter()
    {
        hitCounter = 7;
        UpdateHitCounter();
    }

    public void Hit()
    {
        //Instantiate(swordSound[Random.Range(0, swordSound.Length)], transform.position, Quaternion.identity);
        if (levelManager.isPlaying)
        {
            hitCounter --;
        }

        swordAnimator.SwingAnimate();
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reach))
        {
            if (hit.collider.gameObject.tag == "Fruit")
            {
                ResetHitCounter();
                //Debug.Log(levelManager.beatCounter - 1);
                Instantiate(drumSound[Random.Range(0, drumSound.Length)], transform.position, Quaternion.identity);
                crossHairManager.EntirePop();
                crossHairManager.BeatDestroy(int.Parse(hit.collider.gameObject.name));
                ratingManager.RateBeat(int.Parse(hit.collider.gameObject.name));
                hit.collider.gameObject.GetComponent<FruitOnDestroy>().FruitEffect();
                indicatorManager.Next();
                Destroy(hit.collider.gameObject);
            } 
        }
        if (hitCounter < 0)
        {
            levelManager.GameOver();
        }
        UpdateHitCounter();
        lastHit = Time.time;
    }

    private void Point()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Game Over Bamboo"))
            {
                hit.collider.transform.GetComponent<BambooInteract>().SetPointed();
            }
        }
        Ray ray1 = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;

        if (Physics.Raycast(ray1, out hit1))
        {
            if (hit1.collider.CompareTag("Game Over Bamboo"))
            {
                hit1.collider.transform.GetComponent<BambooInteract>().SetPointed();
            }
        }
    }

    private void OperateBamboo()
    {
        if (Input.anyKeyDown)
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Game Over Bamboo"))
                {
                    Instantiate(clickSound);
                    string name = hit.collider.gameObject.name;

                    switch (name)
                    {
                        case "Menu":
                            GoToMenu();
                            break;
                        case "Again":
                            PlayAgain();
                            break;
                        case "Start":
                            levelManager.StartGame();
                            break;
                    }
                }
            }
        }

        if (Input.anyKeyDown)
        {
            Ray ray1 = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit1;

            if (Physics.Raycast(ray1, out hit1))
            {
                if (hit1.collider.CompareTag("Game Over Bamboo"))
                {
                    Instantiate(clickSound);
                    string name = hit1.collider.gameObject.name;

                    switch (name)
                    {
                        case "Menu":
                            GoToMenu();
                            break;
                        case "Again":
                            PlayAgain();
                            break;
                        case "Start":
                            levelManager.StartGame();
                            break;
                    }
                }
            }
        }
    }
    public void GoToMenu()
    {
        panelFade.DoAppear();
        LoadMainMenu();
    }
    private void PlayAgain()
    {
        levelManager.StartGame();
        gameOverAnim.GoDown();
    }
    private void LoadMainMenu()
    {
        panelFade.DoAppear();
        Invoke("InvokeLoadScene", panelFade.duration);
    }
    private void InvokeLoadScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
