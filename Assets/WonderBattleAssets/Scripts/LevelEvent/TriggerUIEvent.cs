using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerUIEvent : MonoBehaviour
{
    [SerializeField] private string m_TargetTag = "Player";
    [SerializeField] private float m_Damage = 30;


    public void Update()
    {
        if (Input.GetMouseButtonUp(1)) {
            UIManager.Instance.UpdateGateHealth(m_Damage);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == m_TargetTag)
        {
            UIManager.Instance.UpdateGateHealth(m_Damage);
        }
    }
}
