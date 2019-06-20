using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickVFX : MonoBehaviour
{
    public int m_mouseButton = 0;

    [Header("For Timeline")]
    public bool m_Trigger;

    public bool Trigger { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(m_mouseButton)) {

            SendToReceiver();
        }

        if (Trigger != m_Trigger) {
            Trigger = m_Trigger;

            if (Trigger) {
                SendToReceiver();
            }
        }
    }

    public void SendToReceiver() {
        var objs = GameObject.FindObjectsOfType<ClickVFXReceiver>();

        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].TurnOnVFX();
        }
    }
}
