using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotHitScript : MonoBehaviour
{
    Rigidbody rb;
    public GameObject bigFlower;

    private static int pointCount = 0;

    void Start()
    {
        resetCount();
        rb = GetComponent<Rigidbody>();
        // bigFlower = GameObject.FindWithTag("Big"); 
        bigFlower.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            pointCount++;
            bigFlower.SetActive(true);
        }
    }

    public void resetCount()
    {
        pointCount = 0;
    }

    public static int getScore()
    {
        return pointCount;
    }
}
