using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SimulationMode
{
    Manual,
    TimeLine
}
/// <summary>
/// For Timeline
/// </summary>
public enum DragEvent
{
    Start, Update, End
}
public class CardsManager : MonoBehaviour
{
    public float m_TransitionSpeed = 10f;
    public Collider CardField;
    public List<CardItem> m_Listener = new List<CardItem>();

    [Header("For Timeline")]
    public SimulationMode m_SimulationMode = SimulationMode.Manual;
    public DragEvent m_CurrentDragEvent;
    public DragEvent CurrentDragEvent { get; set; }

    public static CardsManager _instance;

    public static CardsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<CardsManager>();
                //Debug.Log(_instance.name);
            }

            return _instance;
        }
    }



    

    

    public void AddListener(CardItem card) {
        m_Listener.Add(card);
    }
    public void RemoveListener(CardItem card) {
        m_Listener.Remove(card);
    }

}
