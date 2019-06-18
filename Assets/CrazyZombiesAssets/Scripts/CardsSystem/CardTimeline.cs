using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTimeline : MonoBehaviour
{
    public CardItem m_ControlThisCard;

    public void Update() {
        if (CardsManager.Instance.CurrentDragEvent != CardsManager.Instance.m_CurrentDragEvent) {
            if (CardsManager.Instance.m_CurrentDragEvent == DragEvent.Start) {
                m_ControlThisCard.OnDragStart();
            }
            if (CardsManager.Instance.m_CurrentDragEvent == DragEvent.End) {
                m_ControlThisCard.OnDragEnd();
            }
        }
        if (CardsManager.Instance.m_CurrentDragEvent == DragEvent.Update) {
            m_ControlThisCard.DragUpdateForTimeline(transform);
        }
    }

}
