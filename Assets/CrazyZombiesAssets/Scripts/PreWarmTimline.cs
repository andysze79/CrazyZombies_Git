using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PreWarmTimline : MonoBehaviour
{
    public PlayableDirector m_TimelinePlayer;

    public ObjectCounter m_ObjectCounter;

    public void Awake()
    {
        
    }

    public void Update()
    {
        if (Input.GetMouseButton(0)) {
            m_TimelinePlayer.Play();
        }
    }

    //public IEnumerator W

}
