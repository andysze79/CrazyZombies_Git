using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class StartButton : MonoBehaviour
{
    public PlayableDirector m_TimeLinePlayer { get { return GameObject.FindGameObjectWithTag("Timeline").GetComponent<PlayableDirector>(); } }
    public TimelineAsset m_Timeline;
        
    public void OnClick() {
        UIObserver.Instance.m_NextState = GameState.Battle;
        m_TimeLinePlayer.Play(m_Timeline);
    }
}
