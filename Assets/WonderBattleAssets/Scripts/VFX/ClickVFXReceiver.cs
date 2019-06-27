using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickVFXReceiver : MonoBehaviour
{
    public GameObject m_VFX;

    public void TurnOnVFX() {
        if(m_VFX != null)
        m_VFX.SetActive(true);
    }
    public void TurnOffVFX()
    {
        if(m_VFX != null)
        m_VFX.SetActive(false);
    }
}
