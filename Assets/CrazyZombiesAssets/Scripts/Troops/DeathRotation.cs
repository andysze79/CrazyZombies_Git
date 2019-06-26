using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseAssets.AI;

public class DeathRotation : MonoBehaviour
{
    public float m_MaxRotation = 30f;

    public AIStateManager m_AIStateManager { get { return GetComponent<AIStateManager>(); } }

    public AIStateKeeper.States CurrentState { get; set; }

    public void Update()
    {
        if (CurrentState != m_AIStateManager.ActiveState)
        {
            CurrentState = m_AIStateManager.ActiveState;

            if (m_AIStateManager.ActiveState == AIStateKeeper.States.Death)
            {
                StartCoroutine(Turn());
            }
        }
    }
    public IEnumerator Turn() {
        var startTime = Time.time;
        var endTime = 0.3f;
        var start = transform.rotation;
        var end = Quaternion.Euler(0, Random.Range(-m_MaxRotation, m_MaxRotation), 0);

        while (Time.time - startTime < endTime)
        {
            transform.rotation = Quaternion.Lerp(start, end, (Time.time - startTime) / endTime);
            yield return null;
        }
    }
}
