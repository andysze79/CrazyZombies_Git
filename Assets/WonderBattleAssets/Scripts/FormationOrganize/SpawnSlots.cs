using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSlots : MonoBehaviour
{
    public List<Transform> m_Slots = new List<Transform>();

    public void Awake()
    {
        var slots = GetComponentsInChildren<Transform>();

        for (int i = 0; i < slots.Length; i++)
        {
            // Make sure itself doesn't include in slots
            if (slots[i] != transform && slots[i].GetComponent<MeshRenderer>() != null)
            {             
                m_Slots.Add(slots[i]);
            }
        }
    }
}
