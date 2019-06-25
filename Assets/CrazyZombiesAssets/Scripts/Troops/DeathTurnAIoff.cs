using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DeathTurnAIoff : MonoBehaviour
{
    public void TurnOffAI() {
        GetComponent<NavMeshAgent>().enabled = false;
    }
}
