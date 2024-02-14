using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour
{
    public OVRInput.Controller Controller;

    public string teleportButtonName;
    public LayerMask teleportMask;
    public GameObject objectToBeTeleported;
    public float maxDistance = 100.0f;
    public float teleportArcWidthMultiplier;

    private bool isPressingTeleport = false;
    private RaycastHit rayHitInfo;
    private LineRenderer teleportArc;

    private int numPoints = 2;

    private void Start()
    {
        teleportArc = gameObject.AddComponent<LineRenderer>();
        teleportArc.material = new Material(Shader.Find("Sprites/Default"));
        teleportArc.widthMultiplier = teleportArcWidthMultiplier;
        teleportArc.positionCount = numPoints;
        teleportArc.startColor = Color.blue;
        teleportArc.endColor = Color.blue;
        teleportArc.useWorldSpace = false;
        teleportArc.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if you hold teleport button
        if (!isPressingTeleport && Input.GetAxis(teleportButtonName) == 1)
        {
            Debug.Log("pressed teleport");
            isPressingTeleport = true;
            ShowTeleportPreview();
        }

        // if you release teleport button
        if (isPressingTeleport && Input.GetAxis(teleportButtonName) < 1)
        {
            Debug.Log("released teleport");
            isPressingTeleport = false;
            Teleport();
        }
    }



    void ShowTeleportPreview()
    {
        bool hit = Physics.Raycast(transform.position, transform.forward, out rayHitInfo, maxDistance, teleportMask);

        if (hit)
        {
            // show blue preview
            Debug.Log("Did Hit");
            Debug.Log("transform forward " + transform.forward);
            teleportArc.enabled = true;
            teleportArc.SetPositions(GetTeleportArcPoints());


        }
        else
        {
            teleportArc.enabled = false;
        }
        //else
        //{
        //    // show red preview
        //    Debug.Log("Did not Hit");
        //    teleportArc.startColor = Color.red;
        //    teleportArc.endColor = Color.red;
        //}




    }

    Vector3[] GetTeleportArcPoints()
    {
        Vector3[] res = new Vector3[numPoints];

        res[0] = transform.position;
        res[1] = transform.position + transform.forward * rayHitInfo.distance;

        Debug.Log("point 0 " + res[0]);
        Debug.Log("point 1 " + res[1]);

        return res;
    }

    void Teleport()
    {
        if (rayHitInfo.collider != null)
        {
            objectToBeTeleported.transform.position = transform.position + transform.forward * rayHitInfo.distance;
            Debug.Log("Teleported " + objectToBeTeleported.transform.position);
        }

    }


}
