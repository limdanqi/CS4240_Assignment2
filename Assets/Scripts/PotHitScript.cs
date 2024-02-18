using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotHitScript : MonoBehaviour
{
    Rigidbody rb;
    private static int pointCount = 0;
    void Start()
    {
        resetCount();
        rb = GetComponent<Rigidbody>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            pointCount++;
            Destroy(this.gameObject);
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
