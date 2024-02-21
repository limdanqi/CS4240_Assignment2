using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotHitScript : MonoBehaviour
{
    Rigidbody rb;
    public GameObject bigFlower;

    private static int pointCount = 0;

    public AudioSource audioPlayer;

    void Start()
    {
        resetCount();
        rb = GetComponent<Rigidbody>();
        // bigFlower = GameObject.FindWithTag("Big"); 
        bigFlower.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball") && !bigFlower.activeSelf)
        {
            pointCount++;
            bigFlower.SetActive(true);
            audioPlayer.Play();
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
