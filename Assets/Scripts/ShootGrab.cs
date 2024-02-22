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

    public GameObject bulletPrefeb;

    public AudioSource pickupSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis(grabButtonName) > 0 && !wasPressed)
        {
            toggleGrab = !toggleGrab;
            if (toggleGrab && !isGrabbing)
            {
                Debug.Log("grab1");
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
        Debug.Log(hits.Length);
        if (hits.Length > 0)
        {
            Debug.Log("grab2");
            isGrabbing = true;

            int closestHit = 0;

            for (int i = 0; i < hits.Length; i++)
            {
                if ((hits[i]).distance < hits[closestHit].distance)
                {
                    closestHit = i;
                }
            }
            Debug.Log(hits[closestHit].transform.CompareTag("Sack"));
            if (hits[closestHit].transform.CompareTag("Sack"))
            {
                GameObject newBullet = Instantiate(bulletPrefeb, transform.position, Quaternion.identity);

                currGrabbedObject = newBullet; // grab the closest object
                currGrabbedObject.GetComponent<Rigidbody>().isKinematic = true; // the grabbed object should not have gravity

                // grab object will follow our hands
                currGrabbedObject.transform.parent = transform.Find("AttachmentPt");
                currGrabbedObject.transform.localPosition = Vector3.zero;
            }
            pickupSound.Play();
            
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
        clone.transform.parent = transform.Find("AttachmentPt");
        clone.transform.localPosition = Vector3.zero;
        clone.GetComponent<Rigidbody>().isKinematic = false;
        Vector3 shootDirection = transform.forward;
        Debug.Log(shootDirection);
        clone.GetComponent<Rigidbody>().AddForce(shootDirection * shootForce, ForceMode.Impulse);
    }
}
