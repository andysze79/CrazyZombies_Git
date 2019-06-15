using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField]private string m_TargetLayer;
    [SerializeField]private GameObject m_ControlThisFormation;
    [SerializeField]private int m_SuicideAfterKillThisAmount;
    [SerializeField]private float m_DelayDestroyTime = 0;

    public int CurrentKill { get; set; }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(m_TargetLayer) && other.transform != transform.parent) {
            Destroy(other.transform.parent.gameObject, m_DelayDestroyTime);
            ++CurrentKill;
            if (CurrentKill >= m_SuicideAfterKillThisAmount) {
                if(m_ControlThisFormation != null)
                m_ControlThisFormation.SetActive(false);
            }
        }
    }



}
