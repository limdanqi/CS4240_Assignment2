using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBall : MonoBehaviour
{
    private Vector3 originalPosition;
    private Rigidbody rb;
    private bool isOriginal;
    void Awake()
    {
        originalPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        isOriginal = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Pot"))
        {
            Destroy(gameObject);
        }
        
    }

    public void SetAsClone()
    {
        isOriginal = false;
    }
}
