using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVFXOnDestroy : MonoBehaviour
{
    public GameObject m_VFX;

    public void OnDestroy()
    {
        if (m_VFX != null)
        {
            var hitInstance = Instantiate(m_VFX, transform.root);
            var hitPs = hitInstance.GetComponent<ParticleSystem>();

            var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy(hitInstance, hitPsParts.main.duration);
        }
    }
}
