using UnityEngine;

public class FertilizerStickScript : MonoBehaviour
{
    public float stickiness = 10f;

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // Apply a force to stick the object to the surface
            GetComponent<Rigidbody>().AddForce(contact.normal * stickiness);
        }
    }
}
