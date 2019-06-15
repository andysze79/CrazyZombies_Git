using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTrigger : MonoBehaviour
{

    [SerializeField]private string m_TargetTag = "Player";
    [SerializeField]private VFXPlayer m_VFXPlayer;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == m_TargetTag)
        {
            //Debug.Log(other.name);
            m_VFXPlayer.PlayVFX();
        }
    }

}
