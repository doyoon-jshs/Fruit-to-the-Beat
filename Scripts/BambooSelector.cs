using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BambooSelector : MonoBehaviour
{
    public Transform holder;
    public GameLevel[] gameLevel;
    public GameObject bambooPrefab;
    public int bambooNum = 5;
    public float gap = 2.0f;
    public float posY;
    public float targetPosY;
    public float posYLerpSpeed = 4;
    public bool canScroll = true;
    public int aheadUnlockNum = 3;

    [HideInInspector]
    public float underGroundY;

    public float scrollValue = 0;
    public float scrollSpeed = 5f; // 기본 속도
    public float speedMultiplier = 10f; // 속도 조정용 멀티플라이어
    public float smoothTime = 0.1f;

    public float targetScrollValue = 0f;
    private float velocity = 0f;
    private float maxScrollValue;

    private void Start()
    {
        
        scrollValue = DataManager.instance.gameData.scrollValue;
        targetScrollValue = DataManager.instance.gameData.scrollValue;
        int count = 0;
        for (int i = 0; i < DataManager.instance.gameData.unlockDatas.Length; i++)
        {
            if (DataManager.instance.gameData.unlockDatas[i])
            {
                count++;
            }
        }
        if (DataManager.instance.gameData.unlockDatas[1] == false)
        {
            bambooNum = 2;
        }
        else if (count + aheadUnlockNum > gameLevel.Length)
        {
            bambooNum = gameLevel.Length;
        }
        else
        {
            bambooNum = count + aheadUnlockNum;
        }
        underGroundY = targetPosY;
        GenerateBamboo();
    }

    private void Update()
    {
        float horizontalInput = -Input.GetAxisRaw("Horizontal");
        targetScrollValue += horizontalInput * scrollSpeed * speedMultiplier * Time.deltaTime;

        targetScrollValue = Mathf.Clamp(targetScrollValue, -maxScrollValue, 0);
        transform.position = new Vector3(scrollValue, transform.position.y, transform.position.z);
        posY = Mathf.Lerp(posY, targetPosY, posYLerpSpeed * Time.deltaTime);
        
        if (targetPosY == 0 && canScroll)
        {
            Scroll();
        }
        
        DataManager.instance.gameData.scrollValue = targetScrollValue;
    }

    private void Scroll()
    {
        // Mouse ScrollWheel을 통한 스크롤 처리
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        targetScrollValue += scrollInput * scrollSpeed;

        // ScrollValue를 gap의 배수로 고정
        float closestGap = Mathf.Round(targetScrollValue / gap) * gap;
        scrollValue = Mathf.SmoothDamp(scrollValue, closestGap, ref velocity, smoothTime);
    }

    private void GenerateBamboo()
    {
        Vector3 startPosition = transform.position;

        for (int i = -1; i < bambooNum; i++)
        {
            Vector3 position = startPosition + new Vector3(i * gap, 0, 0);
            
            GameObject newBamboo = Instantiate(bambooPrefab, position, Quaternion.identity, holder);
            if (i == -1)
            {
                Destroy(newBamboo);
                // newBamboo.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Back";
                // newBamboo.transform.GetChild(2).GetComponent<TextMeshPro>().text = "";
                // newBamboo.name = "Level Select Back";
                // newBamboo.tag = "Main Menu Bamboo";
            }
            else if (gameLevel.Length > i)
            {
                if (DataManager.instance.gameData.unlockDatas[gameLevel[i].id] == true)
                {
                    newBamboo.GetComponent<BambooMover>().isUnlocked = true;
                }
                newBamboo.transform.GetChild(0).GetComponent<TextMeshPro>().text = gameLevel[i].levelName;
                newBamboo.transform.GetChild(2).GetComponent<TextMeshPro>().text = DataManager.instance.gameData.percentages[gameLevel[i].id].ToString() + "%";
                newBamboo.name = i.ToString();
            }
            else
            {
                newBamboo.transform.GetChild(0).GetComponent<TextMeshPro>().text = "NAME";
            }

            if (i == bambooNum - 1)
            {
                maxScrollValue = position.x;
            }
        }
    }
}
