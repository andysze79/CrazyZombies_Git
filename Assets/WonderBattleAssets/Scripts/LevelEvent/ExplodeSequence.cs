using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeSequence : MonoBehaviour
{
    [System.Serializable]
    public struct ExplodeEvent {
        public DestructableObject m_ExplodeObj;
        public float m_delayToSink;
        public float m_delayToNext;
    }

    public List<ExplodeEvent> m_event = new List<ExplodeEvent>();
    public bool m_TriggeredOnEnable = false;
    public bool m_triggered = false;
    public string m_SFXName;

    [Header("Explode Behavior Settings")]
    public float m_ExplodeForce;
    [Range(0,1)]public float m_ExplodeRandomness;
    public Transform m_ExplodeDirection;

    public Coroutine Process { get; set; }

    public void OnEnable() {
        if (m_TriggeredOnEnable)
            m_triggered = true;
    }

    public void Update()
    {
        if (m_triggered && Process == null)
        {
            Process = StartCoroutine(Explode());
        }
        else {
            m_triggered = false;
        }
    }

    public IEnumerator Explode() {

        var dir = Vector3.one;

        if (m_ExplodeDirection != null)
        {
            dir = m_ExplodeDirection.position - transform.position;
        }

        foreach (var obj in m_event)
        {
            obj.m_ExplodeObj.m_currentState = DestructableObject.State.Destroy;
            StartCoroutine(Sink(obj));

            if (m_ExplodeDirection != null) {
                obj.m_ExplodeObj.ExplodeForce = m_ExplodeForce;
                obj.m_ExplodeObj.ExplodeForceRandomness = m_ExplodeRandomness;
                obj.m_ExplodeObj.ExplodeDirection = dir;
            }

            if (m_SFXName != "")
            SoundsManager.Instance.PlaySFX(m_SFXName);

            yield return new WaitForSeconds(obj.m_delayToNext);
        }

        Process = null;
    }

    public IEnumerator Sink(ExplodeEvent obj) {

        yield return new WaitForSeconds(obj.m_delayToSink);

        obj.m_ExplodeObj.m_currentState = DestructableObject.State.Sink;
    }

}
