using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateMember : MonoBehaviour
{
    public float m_OpenDuration;
    public GateController m_Controller;// { get; set; }
    public void Awake()
    {
        m_Controller = GameObject.FindObjectOfType<GateController>();
    }
    public void OnEnable()
    {
        if (m_Controller == null)
        {
            m_Controller = GameObject.FindObjectOfType<GateController>();            
        }

        if (m_Controller != null)
        {
            m_Controller.OpenTheGate(m_OpenDuration);
        }
    }
}
