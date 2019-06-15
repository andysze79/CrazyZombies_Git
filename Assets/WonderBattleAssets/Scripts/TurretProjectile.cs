using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField]private float m_damage = 10;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor") {
            gameObject.SetActive(false);
            UIManager.Instance.UpdateStatueHealth(10);
        }
    }

}
