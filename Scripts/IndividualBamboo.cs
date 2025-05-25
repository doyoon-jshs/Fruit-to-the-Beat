using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualBamboo : MonoBehaviour
{
    public float targetPosY;
    public float lerpSpeed = 4;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, targetPosY, lerpSpeed * Time.deltaTime), transform.position.z);
    }
}
