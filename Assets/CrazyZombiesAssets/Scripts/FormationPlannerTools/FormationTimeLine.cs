using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BaseAssets.AI;

public class FormationTimeLine : MonoBehaviour
{

    public enum Animation {
        Default,Idle,Run,Attack1,Death
    }

    public Animation m_CurrentAnimation;
    public bool m_EnableAI;

    public Animation CurrentAnimation { get; set; }
    public bool EnableAI { get; set; }

    public void OnEnable()
    {
        if (!m_EnableAI) {
            EnableAI = m_EnableAI;
            DisableAI();
        }
    }

    public void Update()
    {
        if (CurrentAnimation != m_CurrentAnimation) {

            CurrentAnimation = m_CurrentAnimation;

            if (CurrentAnimation != Animation.Default)
            {
                Debug.Log(CurrentAnimation.ToString());
                UpdateAnimation();
            }
        }
        if (EnableAI != m_EnableAI) {
            EnableAI = m_EnableAI;

            DisableAI();
        }
    }

    public Animator[] GetTroopsCollider() {

        return transform.GetComponentsInChildren<Animator>();
    }

    public AIDataHolder[] GetTroops()
    {
        return transform.GetComponentsInChildren<AIDataHolder>();
    }

    public void DisableAI() {
        var troops = GetTroops();

        foreach (var item in troops)
        {
            if(item.GetComponent<NavMeshAgent>() != null)
            item.GetComponent<NavMeshAgent>().enabled = EnableAI;
        }
    }

    public void UpdateAnimation() {

        var troops = GetTroopsCollider();

        if (troops != null)
        {
            foreach (var item in troops)
            {
                item.SetTrigger(CurrentAnimation.ToString());
            }
        }
    }
}
