using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXOnHit : MonoBehaviour
{
    public GameObject m_HitVFX;

    void Awake()
    {
        
    }

    public /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        m_HitVFX.transform.SetParent(transform.parent);
        m_HitVFX.SetActive(true);
    }
}
