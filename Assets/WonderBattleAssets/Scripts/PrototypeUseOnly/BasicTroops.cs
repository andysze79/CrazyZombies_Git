using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicTroops : MonoBehaviour
{
    [SerializeField] private Transform m_Home;


    public Transform Home { get { return m_Home; } set { m_Home = value; } }
    public NavMeshAgent Agent { get; set; }
    

    public void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        if (Home != null) {
            Agent.SetDestination(Home.transform.position);
        }
    }


}
