using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseAssets.AI;

public class AttackTrigger : MonoBehaviour
{
    [SerializeField] private string m_TargetLayer;
    [SerializeField] private GameObject m_ControlThisFormation;
    [SerializeField] private Transform m_FaceThisObj;
    [SerializeField] private int m_SuicideAfterKillThisAmount;
    [SerializeField] private float m_DelayDestroyTime = 0;
    [SerializeField] private int m_VFXBufferSize;
    [SerializeField] private GameObject m_AttackVFX;

    public List<GameObject> m_AttackVFXs = new List<GameObject>();
    public List<Collider> m_WaitForDamage = new List<Collider>();
    
    public int CurrentKill { get; set; }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(m_TargetLayer) && other.transform != transform.parent) {

            m_WaitForDamage.Add(other);
            StartCoroutine(DealDamage(other));

            ++CurrentKill;
            if (CurrentKill >= m_SuicideAfterKillThisAmount) {
                if(m_ControlThisFormation != null)
                m_ControlThisFormation.SetActive(false);
            }
        }
    }

    public IEnumerator DealDamage(Collider other) {

        GameObject VFX = null;

        if (m_FaceThisObj != null)
        other.transform.parent.GetComponent<AIDataHolder>().origin = m_FaceThisObj;

        if (m_AttackVFX != null)
        {
            if (m_AttackVFXs.Count < m_VFXBufferSize)
            {
                VFX = Instantiate(m_AttackVFX, other.transform);
                m_AttackVFXs.Add(VFX);
            }
            else
            {
                for (int i = 0; i < m_AttackVFXs.Count; i++)
                {
                    if (!m_AttackVFXs[i].activeSelf && VFX == null)
                    {
                        VFX = m_AttackVFXs[i];
                        VFX.transform.SetParent(other.transform);
                        VFX.transform.localPosition = Vector3.zero;
                        VFX.transform.localScale = m_AttackVFX.transform.localScale;
                        VFX.SetActive(true);
                    }
                }
            }
        }

        yield return new WaitForSeconds(m_DelayDestroyTime);

        if (VFX != null) {
            VFX.transform.SetParent(transform);
            VFX.SetActive(false);
        }

        if (other != null)
        {
            m_WaitForDamage.Remove(other);
            other.transform.parent.GetComponent<AIDataHolder>().origin = null;
            other.transform.parent.GetComponent<AIDataHolder>().CurrentHealth -= float.PositiveInfinity;
        }

    }

    private void OnDisable()
    {
        foreach (var item in m_WaitForDamage)
        {
            if (item != null) {
                item.transform.parent.GetComponent<AIDataHolder>().origin = null;
                item.transform.parent.GetComponent<AIDataHolder>().CurrentHealth -= float.PositiveInfinity;
            }
        }

        m_WaitForDamage.Clear();
    }

}
