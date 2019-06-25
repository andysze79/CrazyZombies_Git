using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetInSquad : MonoBehaviour
{
    public float m_DelayForReset;

   
    public float OriginalAvoidence { get; set; }
    public NavMeshAgent Agent { get { return GetComponent<NavMeshAgent>(); } }

    public void Awake()
    {
        OriginalAvoidence = Agent.radius;
    }

    public void OnEnable()
    {
        StartCoroutine(Delay());
    }

    public IEnumerator Delay() {
        Agent.radius = 0.2f;

        yield return new WaitForSeconds(m_DelayForReset);

        Agent.radius = OriginalAvoidence;
    }

}
