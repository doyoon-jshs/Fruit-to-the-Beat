using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVoid : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < -1)
        {
            Destroy(gameObject);
        }
    }
}
