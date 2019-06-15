using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSequencer : MonoBehaviour
{
    public struct CloseEvent {
        public GameObject m_Obj;
        public bool m_OnOff;
        public float m_DelayToNextEvent;
    }

    public List<CloseEvent> m_Events = new List<CloseEvent>();

    public void Awake()
    {
        
    }


}
