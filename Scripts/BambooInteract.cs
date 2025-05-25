using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooInteract : MonoBehaviour
{
    public int n;
    private float lerpSpeed = 15;
    private int isPointed = 0;

    private float scaleMultiplier = 1.05f;
    private Vector3 targetScale;

    private int isPointedLast = 0;
    private Vector3 startScale;
    private Vector3 startLocalPos;
    private float seed;

    private void Start()
    {
        seed = Random.Range(-100f, 100f);
        startScale = transform.localScale;
        startLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (gameObject.tag == "Main Menu Bamboo")
        {
            transform.localPosition = startLocalPos + Vector3.up * Mathf.PerlinNoise1D(Time.time * 0.9f + n * 0.1f) * 0.05f;
        }
        if (isPointed > 0) //when highlighted
        {
            targetScale = startScale * scaleMultiplier;
        }
        else //when not highlighted
        {
            targetScale = startScale;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpSpeed * Time.deltaTime);
        if (isPointed == isPointedLast)
        {
            isPointed = 0;
        }
        isPointedLast = isPointed;
    }

    public void SetPointed()
    {
        isPointed += 1;
    }
}
