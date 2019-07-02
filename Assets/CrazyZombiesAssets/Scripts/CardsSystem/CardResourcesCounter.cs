﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CardItem))]
public class CardResourcesCounter : MonoBehaviour
{
    public float m_RequiredResources = 10;
    public TextUpdater m_Text;

    public void OnEnable()
    {
        m_Text.ChangeText(m_RequiredResources);
    }

    public void UseCard() {
        ObjectCounter.Instance.UseResources(m_RequiredResources);
    }
}
