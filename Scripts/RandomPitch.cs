using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitch : MonoBehaviour
{
    public float minPitch;
    public float maxPitch;

    private void Start()
    {
        GetComponent<AudioSource>().pitch = Random.Range(minPitch, maxPitch);
    }
}
