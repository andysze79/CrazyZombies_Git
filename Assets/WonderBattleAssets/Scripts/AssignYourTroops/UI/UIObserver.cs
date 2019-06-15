using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Prepare,
    Battle
}

public class UIObserver : MonoBehaviour
{
    public List<UISubject> m_UIs = new List<UISubject>();

    public GameState m_NextState = GameState.Prepare;

    public GameState CurrentState { get; set; }

    public static UIObserver _instance;

    public static UIObserver Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIObserver>();
                //Debug.Log(_instance.name);
            }

            return _instance;
        }
    }

    public void Awake()
    {
        //var UIs = GameObject.FindObjectsOfType<UISubject>();

        /*foreach (var obj in UIs)
        {
            m_UIs.Add(obj);
        }*/
        Notify();
    }

    public void Update()
    {
        if (CurrentState != m_NextState) {
            CurrentState = m_NextState;

            Notify();
        }
    }

    public void AddToListen(UISubject obj) {
        m_UIs.Add(obj);
    }

    public void RemoveFromListen(UISubject obj)
    {
        m_UIs.Remove(obj);
    }

    public void Notify() {
        foreach (var obj in m_UIs)
        {
            obj.OnNotify(m_NextState);
        }
    }
}
