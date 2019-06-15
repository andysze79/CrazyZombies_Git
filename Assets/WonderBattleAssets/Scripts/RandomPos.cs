using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPos : MonoBehaviour
{
    public Transform[] m_cubes;

    public void Awake()
    {
        m_cubes = GetComponentsInChildren<Transform>();
    }

    public void OnEnable()
    {
        foreach (var obj in m_cubes)
        {
            var rot = obj.eulerAngles;
            var angle = Random.Range(0,360);

            rot.y = angle;

            obj.eulerAngles = rot;
        }
    }
}
