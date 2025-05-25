using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopUpOnStart : MonoBehaviour
{
    public float duration = 0.3f;
    public Ease ease = Ease.Linear;

    void Start()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, duration).SetEase(ease);
    }
}
