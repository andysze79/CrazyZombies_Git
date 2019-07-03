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
    [System.Serializable]
    public struct VFXEvent
    {
        public string Name;
        public GameObject Effect;
        public Transform Position;
        public int Index;
        public int Min;
        public int Max;
        public float DealayStart;
        public float Percisement;
    }

    public List<Event> m_Events = new List<Event>();
    public List<UIEvent> m_UIEvents = new List<UIEvent>();
    public List<VFXEvent> m_VFXEvents = new List<VFXEvent>();
    
    List<GameObject> Clones = new List<GameObject>();

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
            for (int j = 0; j < m_UIEvents.Count; j++)
            {
                if (m_UIEvents[j].Index == i)
                {
                    m_UIEvents[j].ProgressBar.enabled = true;
                    StartCoroutine(UpdateUI(m_UIEvents[j], m_Events[i].Duration));
                }
            }

            for (int j = 0; j < m_VFXEvents.Count; j++)
            {
                if (m_VFXEvents[j].Index == i)
                {
                    StartCoroutine(UpdateVFX(m_VFXEvents[j], m_Events[i].Duration)); ;
                }
                else {
                    if (Clones.Count != 0)
                    {
                        foreach (var item in Clones)
                        {
                            Destroy(item);
                        }
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

    public IEnumerator UpdateVFX(VFXEvent vfxEvent, float duration) {
        var startTime = Time.time;
        var endTime = duration;
        var from = vfxEvent.Min;
        var to = vfxEvent.Max;
        var step = endTime / (to - from);

        while (Time.time - startTime < endTime)
        {
            if ((Time.time - startTime) > vfxEvent.DealayStart && (Time.time - startTime - vfxEvent.DealayStart) % step < vfxEvent.Percisement) {

                if (Clones.Count < to)
                    Clones.Add(Instantiate(vfxEvent.Effect, vfxEvent.Position));
            }
            yield return null;
        }
    }
}
