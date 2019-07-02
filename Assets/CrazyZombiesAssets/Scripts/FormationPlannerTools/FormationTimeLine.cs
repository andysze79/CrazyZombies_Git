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

    [Header("Animation Settings")]
    public Animation m_CurrentAnimation;
    public bool m_OffsetAnimation;
    public bool m_OffsetRandomness;
    public int m_UnitsEnablePerFrame = 1;

    [Header("AI Settings")]
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

                if(!m_OffsetRandomness)
                    StartCoroutine(UpdateAnimation());
                else
                    StartCoroutine(UpdateAnimationRandomly());
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

    public IEnumerator UpdateAnimation() {

        var troops = GetTroopsCollider();

        if (troops != null)
        {
            for (int i = 0; i < troops.Length; i++)
            {         
                troops[i].SetTrigger(CurrentAnimation.ToString());
                if (m_OffsetAnimation) {
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }

    public IEnumerator UpdateAnimationRandomly() {
        var troops = GetTroopsCollider();
        List<Animator> troopsList = new List<Animator>();

        for (int i = 0; i < troops.Length; i++)
        {
            troopsList.Add(troops[i]);
        }


        if (troops != null)
        {
            for (int i = 0; i < troops.Length; i++)
            {
                var Index = Random.Range(0, troopsList.Count);

                troopsList[Index].SetTrigger(CurrentAnimation.ToString());
                troopsList.Remove(troopsList[Index]);

                if(i % m_UnitsEnablePerFrame == 0)
                yield return new WaitForEndOfFrame();
            }
        }

    }

}
