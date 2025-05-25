using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class BambooMover : MonoBehaviour
{
    public float intensity = 1;
    public bool isUnlocked = true;
    public float lockedOffsetY = 2;
    public float unlockDuration = 0.4f;
    public float offsetY;
    private float startPos;
    private float initialPos;
    private bool isBeingUnlocked = false;
    private float keyStartPosY = 0;
    private BambooSelector bambooSelector;

    private void Start()
    {
        initialPos = transform.position.y;
        if (isUnlocked)
        {
            startPos = transform.position.y;
        }
        else
        {
            startPos = transform.position.y + lockedOffsetY;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
        }
        bambooSelector = GameObject.Find("Bamboo Selection").GetComponent<BambooSelector>();
    }

    private void Update()
    {
        float distance = Mathf.Abs(transform.position.x);
        if (!isBeingUnlocked)
        {
            transform.position = new Vector3(transform.position.x, startPos - distance * intensity + bambooSelector.posY + offsetY, transform.position.z);
        }
    }

    public void Unlock()
    {
        bambooSelector.canScroll = false;
        isBeingUnlocked = true;

        transform.GetChild(3).gameObject.SetActive(true);
        keyStartPosY = transform.GetChild(3).localPosition.y;
        float startScale = transform.GetChild(3).localScale.x;
        transform.GetChild(3).position = transform.GetChild(3).position + Vector3.up * 2f;
        transform.GetChild(3).localScale = Vector3.zero;
        transform.GetChild(3).DOLocalMoveY(keyStartPosY, unlockDuration / 2 - 0.2f);
        transform.GetChild(3).DOScale(startScale, 0.1f);

        Invoke("InvokeUnlock", unlockDuration / 2 - 0.2f);
    }
    private void InvokeUnlock()
    {
        transform.GetChild(3).DORotate(new Vector3(0, -90 , 90), 0.15f);
        Invoke("InvokeInvokeUnlock", 0.2f);
    }
    private void InvokeInvokeUnlock()
    {
        transform.DOLocalMoveY(initialPos - Mathf.Abs(transform.position.x) * intensity + bambooSelector.posY + offsetY, unlockDuration / 2);
        startPos = initialPos;

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(0).GetComponent<TextMeshPro>().alpha = 0;
        transform.GetChild(0).GetComponent<TextMeshPro>().DOFade(1, unlockDuration / 2);
        transform.GetChild(2).GetComponent<TextMeshPro>().alpha = 0;
        transform.GetChild(2).GetComponent<TextMeshPro>().DOFade(1, unlockDuration / 2);

        Invoke("InvokeInvokeInvokeUnlock", unlockDuration / 2);
    }

    private void InvokeInvokeInvokeUnlock()
    {
        transform.GetChild(3).DOLocalMoveY(keyStartPosY - 1, 1f);
        isUnlocked = true;
        bambooSelector.canScroll = true;
        isBeingUnlocked = false;
    }
}
