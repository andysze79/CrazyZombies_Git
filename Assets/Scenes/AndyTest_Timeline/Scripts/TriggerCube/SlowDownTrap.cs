using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SlowDownTrap : MonoBehaviour
{
    [SerializeField]private string m_AffectLayer = "RedCollider";

    public float m_SlowDownSpeed;

    public float m_SpeedOriginal { get; set; }
    

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(m_AffectLayer))
        {
            if (other.transform.GetComponentInParent<NavMeshAgent>() != null)
            {
                m_SpeedOriginal = other.transform.GetComponentInParent<NavMeshAgent>().speed;
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(m_AffectLayer))
        {
            if (other.transform.GetComponentInParent<NavMeshAgent>() != null) {
                other.transform.GetComponentInParent<NavMeshAgent>().speed = m_SlowDownSpeed;
            }
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(m_AffectLayer))
        {
            if (other.transform.GetComponentInParent<NavMeshAgent>() != null)
            {
                other.transform.GetComponentInParent<NavMeshAgent>().speed = m_SpeedOriginal;
            }
        }
    }


}
