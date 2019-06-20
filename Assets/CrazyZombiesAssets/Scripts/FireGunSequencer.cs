using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireGunSequencer : MonoBehaviour
{
    [System.Serializable]
    public struct Event {
        public string Name;
        public GameObject[] Objects;
        public float Duration;
    }
    [System.Serializable]
    public struct UIEvent
    {
        public string Name;
        public int Index;
        public Image ProgressBar;
        public float From;
        public float To;
    }
    //public GameObject m_Canvas;
    //public GameObject m_Mesh;
    //public GameObject m_CrumbledMesh;

    //public float m_BuildingTime;
    //public float m_ActiveTime;
    //public float m_DeactiveTime;

    public List<Event> m_Events = new List<Event>();
    public List<UIEvent> m_UIEvents = new List<UIEvent>();

    //public Image m_BuildingProgressBar;
    //public Image m_LifeProgressBar;
    
    public void OnEnable() {
        Initialize();
        StartCoroutine(Sequence());
    }

    public void Initialize() {
        foreach (var item in m_Events)
        {
            foreach (var obj in item.Objects)
            {
                obj.SetActive(false);
            }
        }
        foreach (var item in m_UIEvents)
        {
            item.ProgressBar.enabled = false;
        }
    }

    public IEnumerator Sequence() {
        for (int i = 0; i < m_Events.Count; i++)
        {
            if (i < m_UIEvents.Count) {
                for (int j = 0; j < m_UIEvents.Count; j++)
                {
                    if (m_UIEvents[i].Index == i) {
                        m_UIEvents[j].ProgressBar.enabled = true;
                        StartCoroutine(UpdateUI(m_UIEvents[j], m_Events[i].Duration));
                    }
                }
            }

            SwitchObject(m_Events[i].Objects, true);

            yield return new WaitForSeconds(m_Events[i].Duration);

            SwitchObject(m_Events[i].Objects, false);
        }
    }

    public void SwitchObject(GameObject[] objs, bool value) {
        for (int i = 0; i < objs.Length; i++)
        {
            objs[i].SetActive(value);
        }
    }
    
    public IEnumerator UpdateUI(UIEvent barEvent, float duration) {
        var startTime = Time.time;
        var endTime = duration;
        var from = barEvent.From;
        var to = barEvent.To;
       
        while (Time.time - startTime < endTime) {
            barEvent.ProgressBar.fillAmount = Mathf.Lerp(from, to, (Time.time - startTime) / endTime);
            yield return null;
        }

    }
}
