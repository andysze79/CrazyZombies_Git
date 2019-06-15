using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform m_Target;

    public Vector3 offset { get; set; }
    public Vector3 PosPre { get; set; }

    public void OnEnable()
    {
        offset = transform.position - m_Target.position;

    }
    
    public void Update()
    {
        if (m_Target.position != PosPre) {
            var moveXfer = transform.position;
            moveXfer.x = m_Target.position.x + offset.x;
            moveXfer.z = m_Target.position.z + offset.z;

            transform.position = moveXfer;

            PosPre = transform.position;
        }
    }

}
