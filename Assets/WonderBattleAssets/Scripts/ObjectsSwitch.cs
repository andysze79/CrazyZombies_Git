using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSwitch : MonoBehaviour
{

    [System.Serializable]
    public struct SwitchEvent {
        public string m_Name;
        public GameObject m_obj;
        public bool m_DefaultValue;
    }
    public SwitchEvent[] m_Events;
    public float m_AutoTurnOffTime = 3f;

    public Coroutine Process { get; set; }


    public void Awake()
    {
        Initiate();
    }

    /// <summary>
    ///  Call from animEvent
    /// </summary>
    /// <param name="TargetName"></param>
    public void TurnOnObj(string TargetName) {
        //StopCoroutine(Process);

        for (int i = 0; i < m_Events.Length; i++)
        {
            if(m_Events[i].m_Name == TargetName)
            m_Events[i].m_obj.SetActive(true);
        }

        //Process = StartCoroutine(DelayTurnOff());
    }

    /// <summary>
    ///  Call from animEvent
    /// </summary>
    public void TurnOffObj(string TargetName)
    {
        for (int i = 0; i < m_Events.Length; i++)
        {
            if (m_Events[i].m_Name == TargetName)
                m_Events[i].m_obj.SetActive(false);
        }
    }

    public void Initiate() {
        for (int i = 0; i < m_Events.Length; i++)
        {
            m_Events[i].m_obj.SetActive(m_Events[i].m_DefaultValue);
        }
    }

    public IEnumerator DelayTurnOff(string TargetName) {
        yield return new WaitForSeconds(m_AutoTurnOffTime);
        TurnOffObj(TargetName);
    }
}
