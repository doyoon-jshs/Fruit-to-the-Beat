using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float afterTime = 5f;

    private void Start()
    {
        Destroy(gameObject, afterTime);
    }
}
