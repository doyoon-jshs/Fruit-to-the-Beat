using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelFade : MonoBehaviour
{
    public float duration;
    public bool fadeOnStart = false;
    public Ease ease;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        if (fadeOnStart)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
            DoFade();
        }
    }

    public void DoFade()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        image.DOFade(0, duration).SetEase(ease);
    }

    public void DoAppear()
    {
        image.DOFade(1, duration).SetEase(ease);
    }
}
