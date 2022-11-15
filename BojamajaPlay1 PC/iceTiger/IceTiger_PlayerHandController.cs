using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTiger_PlayerHandController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collision.collider.gameObject.layer: " + collision.collider.gameObject.layer);
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("IceTigers"))
        {
            //Debug.Log("contact.normal : " + contact.normal);
            //ContactPoint contact = collision.contacts[0];   // contacts[0] first collision, 1,2,3... 
            //Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            //Vector3 pos = contact.point;
            //Instantiate(explosionPrefab, pos, rot);
            //Debug.Log("contact.point : " + pos);
            //Debug.Log("contact.thisCollider : " + contact.thisCollider);
            //Debug.Log("contact.otherCollider : " + contact.otherCollider);
            //Debug.Log("contact.normal : " + contact.normal);
            //Debug.Log("impulse : " + collision.impulse);
            //Debug.Log("relativeVelocity : " + collision.relativeVelocity);
            if (collision.relativeVelocity.y > 1f)
            {
                collision.collider.gameObject.transform.GetComponent<IceTiger>().OnDown();
            }
            
        }
    }
}
