using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickVFXReceiver : MonoBehaviour
{
    public GameObject m_VFX;

    public void TurnOnVFX() {
        m_VFX.SetActive(true);
    }
}
