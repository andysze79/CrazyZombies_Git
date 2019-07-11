using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferenciateScale : MonoBehaviour
{
    public float m_Min;
    public float m_Max;

    private void OnEnable()
    {
        transform.localScale *= Random.Range(m_Min, m_Max);
    }
}
