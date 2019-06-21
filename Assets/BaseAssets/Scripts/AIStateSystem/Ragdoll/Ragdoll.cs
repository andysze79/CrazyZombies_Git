using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour 
{
    private void Start()
    {
        EnableRagdoll();
    }

    public void EnableRagdoll()
    {
        List<Rigidbody> rigidbodies = new List<Rigidbody>();

        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            if(rigidbodies.Contains(rb) == false)
            {
                rigidbodies.Add(rb);
            }
        }

        if (rigidbodies == null) return;

        foreach (var rb in rigidbodies)
        {
            if (rb == null) continue;

            rb.velocity = Vector3.zero;
            rb.useGravity = true;
            rb.isKinematic = false;

            Collider collider = rb.GetComponent<Collider>();

            if (collider)
            {
                collider.enabled = true;
                collider.isTrigger = false;
            }
        }
    }
}