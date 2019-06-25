using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FormationTimeLine))]
public class FormationHealth : MonoBehaviour
{
    public float m_Health;
    public float m_HealthCurrent;
    public GameObject m_AttackTrigger;

    [Header("UI")]
    public Image m_HealthBar;

    [Header("Suicide Settings")]
    public bool m_Suicide;
    public float m_ActiveTime;
    public bool m_Randomized;
    public int m_StopAtHere = 0;
    public bool m_BodiesFollowFormation = true;

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
        if (Index < Troops.Count - m_StopAtHere)
        {            
            Troops[Index].CurrentHealth -= float.PositiveInfinity;

            // Dead troop won't move with squad
            if (m_BodiesFollowFormation)
            {
                if (Troops[Index].GetComponent<ClickVFXReceiver>() != null)
                {
                    Troops[Index].GetComponent<ClickVFXReceiver>().TurnOffVFX();
                }
                Troops[Index].transform.SetParent(transform.root);
            }

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
    }

    public void UpdateHealth()
    {
        --m_HealthCurrent;

        if(m_HealthBar != null)
        m_HealthBar.fillAmount = m_HealthCurrent / m_Health;
    }
    
    public IEnumerator CheckActiveTime() {
        while (m_HealthCurrent > 0) {
            yield return new WaitForSeconds(m_ActiveTime / m_Health);
            GetDamage();
        }

        if (m_AttackTrigger != null) {
            // Clean up the squad
            Index = 0;
            Troops.Clear();

            if (m_HealthBar != null)
                m_HealthBar.transform.parent.gameObject.SetActive(false);

            m_AttackTrigger.SetActive(false);
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
