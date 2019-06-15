using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField]private Transform m_target;
    [SerializeField]private float m_SpeedMultiplier;

    // Call it in Update
    public void Look() {

        var rot = Quaternion.LookRotation(m_target.position - transform.position);
        var rotOrigin = transform.rotation;

        transform.rotation = Quaternion.Lerp(rotOrigin, rot, Time.deltaTime * m_SpeedMultiplier);
    }
    
}
