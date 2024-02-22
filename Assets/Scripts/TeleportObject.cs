using Meta.Voice.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour
{
    public GameObject objectToBeTeleported;
    public GameObject teleportSpotIndicator;
    public GameObject teleportLineIndicator;

    public OVRInput.Controller Controller;

    public string teleportButtonName;
    public LayerMask teleportMask;
    public float maxTeleportDistance = 100.0f;
    [Range(0.01f, 0.25f)][SerializeField] private float timeBetweenPoints = 0.1f;
    [Range(10, 100)][SerializeField] private int teleportLineNumPoints = 25;

    private bool isTeleporting;
    private RaycastHit rayHitInfo;
    private Vector3 teleportedLocation;

    public AudioSource teleportSound;


    // Start is called before the first frame update
    void Start()
    {
        UpdateTeleportIndicators(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if you press button
        if (Input.GetAxis(teleportButtonName) > 0)
        {
            isTeleporting = true;
            ShowTeleportPreview();
        }

        // if you let go of button
        if (isTeleporting && Input.GetAxis(teleportButtonName) == 0)
        {
            isTeleporting = false;
            Teleport();
            UpdateTeleportIndicators(false);
        }
    }

    void UpdateTeleportIndicators(bool status)
    {
        teleportSpotIndicator.SetActive(status);
        teleportLineIndicator.SetActive(status);
    }

    void ShowTeleportPreview()
    {
        // Check if there is a valid teleport location
        bool hasValidTeleportLocation = Physics.Raycast(transform.position, transform.forward, out rayHitInfo, maxTeleportDistance, teleportMask);

        if (hasValidTeleportLocation)
        {
            UpdateTeleportIndicators(true);

            teleportedLocation = transform.position + transform.forward * rayHitInfo.distance;
            teleportedLocation.y = 0;

            // update teleport spot indicator
            teleportSpotIndicator.transform.position = teleportedLocation;
            teleportSpotIndicator.transform.rotation = Quaternion.Euler(Vector3.zero);

            // update teleport line indicator
            UpdateTeleportLineIndicator(teleportLineIndicator.gameObject.GetComponent<LineRenderer>(), transform.position, teleportedLocation);
        }

    }

    void UpdateTeleportLineIndicator(LineRenderer lr, Vector3 startPos, Vector3 endPos)
    {
        lr.transform.parent = null;
        lr.positionCount = Mathf.CeilToInt(teleportLineNumPoints / timeBetweenPoints) + 1;

        /*float throwStrength = 10;
        float mass = 1;
        Vector3 startVelocity = throwStrength * transform.forward / mass;

        Vector3[] points = new Vector3[lr.positionCount];
        points[0] = startPos;
        int i = 0;
        for (float time = 0; time < teleportLineNumPoints; time += timeBetweenPoints)
        {
            i++;
            Vector3 point = startPos + time * startVelocity;
            point.y = startPos.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            points[i] = point;

        }*/
        Vector3[] points = new Vector3[lr.positionCount];

        for (int i = 0; i < lr.positionCount; i++)
        {
            float t = i / (float)(lr.positionCount - 1);
            Vector3 pointOnCurve = BezierCurve(startPos, (startPos + endPos) / 2f + Vector3.up * 1, endPos, t);
            points[i] = pointOnCurve;
        }

        lr.SetPositions(points);
    }

    Vector3 BezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector3 p = u3 * p0 + 3 * u2 * t * p1 + 3 * u * t2 * p2 + t3 * p2;

        return p;
    }

    void Teleport()
    {
        if (rayHitInfo.collider != null || rayHitInfo.collider.CompareTag("Ground"))
        {
            Debug.Log(rayHitInfo.collider);
            objectToBeTeleported.transform.position = teleportedLocation;
            Debug.Log("obj teleported to " + objectToBeTeleported.transform.position);
            teleportSound.Play();
        }
    }
}
