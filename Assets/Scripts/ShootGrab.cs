using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootGrab : MonoBehaviour
{
    public OVRInput.Controller Controller;

    public string grabButtonName;
    public float shootForce;
    public float grabRadius;
    public LayerMask grabMask;

    private GameObject currGrabbedObject;
    private bool isGrabbing;
    private bool wasPressed; // Tracks if the button was pressed in the previous frame
    private bool toggleGrab; // Tracks the toggle state of grabbing
    private int ammoCount;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis(grabButtonName) > 0 && !wasPressed)
        {
            toggleGrab = !toggleGrab;
            if (toggleGrab && !isGrabbing)
            {
                GrabObject();
                ammoCount = 3;
            }
            else if (isGrabbing)
            {
                if (ammoCount > 1)
                {
                    ShootObject();
                    ammoCount = ammoCount - 1;
                }
                else
                {
                    DropObject();
                }
            }
        }
        wasPressed = Input.GetAxis(grabButtonName) > 0;
    }

    // For debugging
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grabRadius);
    }

    void GrabObject()
    {
        RaycastHit[] hits;

        hits = Physics.SphereCastAll(transform.position, grabRadius, transform.forward, 100.0f, grabMask);

        if (hits.Length > 0)
        {
            isGrabbing = true;

            int closestHit = 0;

            for (int i = 0; i < hits.Length; i++)
            {
                if ((hits[i]).distance < hits[closestHit].distance)
                {
                    closestHit = i;
                }
            }

            currGrabbedObject = hits[closestHit].transform.gameObject; // grab the closest object
            currGrabbedObject.GetComponent<Rigidbody>().isKinematic = true; // the grabbed object should not have gravity

            // grab object will follow our hands
            currGrabbedObject.transform.position = transform.position;
            currGrabbedObject.transform.parent = transform;
        }
    }

    void DropObject()
    {
        isGrabbing = false;
        if (currGrabbedObject != null)
        {
            currGrabbedObject.transform.parent = null;
            currGrabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            Vector3 shootDirection = transform.forward;
            Debug.Log(shootDirection);
            currGrabbedObject.GetComponent<Rigidbody>().AddForce(shootDirection * shootForce, ForceMode.Impulse);
        }
    }

    void ShootObject()
    {
        GameObject clone = Instantiate(currGrabbedObject, currGrabbedObject.transform.position, currGrabbedObject.transform.rotation);
        clone.GetComponent<RespawnBall>().SetAsClone();
        clone.transform.parent = null;
        clone.GetComponent<Rigidbody>().isKinematic = false;
        Vector3 shootDirection = transform.forward;
        Debug.Log(shootDirection);
        clone.GetComponent<Rigidbody>().AddForce(shootDirection * shootForce, ForceMode.Impulse);
    }
}
