using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferenciatePos : MonoBehaviour
{
    
    public bool m_X;
    public float m_MinX;
    public float m_MaxX;
    public bool m_Y;
    public float m_MinY;
    public float m_MaxY;
    public bool m_Z;
    public float m_MinZ;
    public float m_MaxZ;

    private void OnEnable()
    {
        var pos = transform.position;
        if (m_X)
            pos.x += Random.Range(m_MinX, m_MaxX);
        if (m_Y)
            pos.y += Random.Range(m_MinY, m_MaxY);
        if (m_Z)
            pos.z += Random.Range(m_MinZ, m_MaxZ);

        transform.position = pos;
    }
}
