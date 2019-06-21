using UnityEngine;
using UnityEngine.AI;

namespace BaseAssets.AI
{
    public class AIReferenceKeeper : MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent = null;
        [HideInInspector] public Animator animator = null;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
        }
    }
}
