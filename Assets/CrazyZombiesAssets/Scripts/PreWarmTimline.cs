using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PreWarmTimline : MonoBehaviour
{
    public PlayableDirector m_TimelinePlayer;

    public ObjectCounter m_ObjectCounter;

    public int m_PrewarmAmount = 50;

    public void Awake()
    {
        StartCoroutine(WaitForPrewarm());
    }

    public void Update()
    {
        if (Input.GetMouseButton(0)) {
            m_TimelinePlayer.Play();
        }
    }

    public IEnumerator WaitForPrewarm() {
        yield return new WaitUntil(() => m_ObjectCounter.GetAmount() > m_PrewarmAmount);

        m_TimelinePlayer.Play();               
    }

}
