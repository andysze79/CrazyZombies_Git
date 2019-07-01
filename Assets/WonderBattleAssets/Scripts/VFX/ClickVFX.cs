using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickVFX : MonoBehaviour
{
    public int m_mouseButton = 0;

    [Header("For Timeline")]
    public bool m_Trigger;

    public bool Trigger { get; set; }
    public List<ClickVFXReceiver> m_Listeners = new List<ClickVFXReceiver>();

    public static ClickVFX m_Instance;
    public static ClickVFX Instance {
        get {
            if (m_Instance == null) {
                m_Instance = GameObject.FindObjectOfType<ClickVFX>();
            }
            return m_Instance;
        }
    }

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
        //var objs = GameObject.FindObjectsOfType<ClickVFXReceiver>();

        //for (int i = 0; i < objs.Length; i++)
        //{
        //    objs[i].TurnOnVFX();
        //}

        for (int i = 0; i < m_Listeners.Count; i++)
        {
            m_Listeners[i].TurnOnVFX();
        }
    }

    public void AddListener(ClickVFXReceiver target){
        m_Listeners.Add(target);
    }
    public void RemoveListener(ClickVFXReceiver target)
    {
        m_Listeners.Remove(target);
    }
}
