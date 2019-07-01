using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickVFXReceiver : MonoBehaviour
{
    public GameObject m_VFX;

    public void Awake()
    {
        ClickVFX.Instance.AddListener(this);
    }

    public void TurnOnVFX() {
        if(m_VFX != null)
        m_VFX.SetActive(true);
    }
    public void TurnOffVFX()
    {
        if(m_VFX != null)
        m_VFX.SetActive(false);
    }

    public void OnDestroy()
    {
        ClickVFX.Instance.RemoveListener(this);
    }
}
