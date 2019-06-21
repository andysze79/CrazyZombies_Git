using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FormationTimeLine))]
public class FormationHealth : MonoBehaviour
{
    public float m_Health;
    public float m_HealthCurrent;
    public bool m_Suicide;
    public float m_ActiveTime;
    public bool m_Randomized;

    public bool Suicide { get; set; }
    public List<BaseAssets.AI.AIDataHolder> Troops = new List<BaseAssets.AI.AIDataHolder>();
    public int Index { get; set; }
    public FormationTimeLine FormationTimeLine { get { return GetComponent<FormationTimeLine>(); } }

    public void OnEnable()
    {
        StartCoroutine(DelayInitialize());
    }

    public void Update()
    {
        if (Suicide != m_Suicide) {
            Suicide = m_Suicide;

            if (Suicide) {
                StartCoroutine(CheckActiveTime());
            }
        }        
    }

    public void GetDamage() {
        if (Index < Troops.Count)
        {            
            Troops[Index].CurrentHealth -= float.PositiveInfinity;
            if (m_Randomized)
            {
                Troops.Remove(Troops[Index]);
                Index = Random.Range(0, Troops.Count);
            }
            else
            {
                ++Index;
            }
            UpdateHealth();
        }
        else {
            Index = 0;
            Troops.Clear();
            transform.parent.gameObject.SetActive(false);
        }
    }

    public void UpdateHealth()
    {
        m_HealthCurrent = FormationTimeLine.GetTroops().Length;
    }
    
    public IEnumerator CheckActiveTime() {
        while (m_HealthCurrent > 0) {
            yield return new WaitForSeconds(m_ActiveTime / m_Health);
            GetDamage();
        }
    }

    public IEnumerator DelayInitialize() {
        yield return new WaitForSeconds(0.1f);
        
        var troops = FormationTimeLine.GetTroops();

        foreach (var obj in troops)
        {
            Troops.Add(obj);
        }

        if (!m_Randomized)
            Index = 0;
        else
            Index = Random.Range(0, Troops.Count);

        m_Health = Troops.Count;
        m_HealthCurrent = m_Health;

        if (m_ActiveTime > 0 && m_Suicide)
        {
            StartCoroutine(CheckActiveTime());
        }
    }
}
