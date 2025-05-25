using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FruitOnDestroy : MonoBehaviour
{
    private float explodeForce = 4f;
    private float torque = 15f;
    public GameObject[] onDestroyStuff;

    public void FruitEffect()
    {
        transform.DOKill();
        foreach (var fruit in onDestroyStuff)
        {
            GameObject thingy = Instantiate(fruit, fruit.transform.position, Quaternion.identity);
            thingy.transform.localScale = Vector3.one * thingy.transform.localScale.x * transform.localScale.x;
            thingy.SetActive(true);
            if (thingy.GetComponent<Rigidbody>() != null)
            {
                Rigidbody rb = thingy.GetComponent<Rigidbody>();
                rb.AddForce(Random.onUnitSphere * explodeForce, ForceMode.Impulse);
                rb.AddTorque(Random.insideUnitSphere * torque, ForceMode.Impulse);
                Vector3 direction = (transform.position - Camera.main.transform.position).normalized;
                rb.AddForce(direction * Random.Range(2f, 6f), ForceMode.Impulse); 
            }
        }
    }
}
