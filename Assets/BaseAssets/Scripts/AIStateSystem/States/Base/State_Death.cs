using UnityEngine;
using System.Collections;

namespace BaseAssets.AI
{
    public class State_Death : State
    {
        private bool canSink = false;

        // State Functions =========================================================================================================
        protected override void RunOnce()
        {
            base.RunOnce();
        }

        public override void EnterState()
        {
            if (Reference.agent && Reference.agent.enabled) Reference.agent.isStopped = true;
            AIStateHelperMethods.RemoveFromAttackerList(Data);
            AIStateHelperMethods.PlayAnimation(Reference.animator, "Death");
            StartCoroutine(SinkDelay());
            Reference.animator.gameObject.layer = 0;
        }

        public override void UpdateState()
        {
            Sink();
        }

        public override void ExitState()
        {

        }
        // =========================================================================================================================

        private IEnumerator SinkDelay()
        {
            yield return new WaitForSeconds(Data.SinkDelay);
            canSink = true;
        }

        private void Sink()
        {
            if (!canSink) return;

            Collider[] colliders = transform.GetComponentsInChildren<Collider>();

            foreach (Collider collider in colliders)
            {
                Destroy(collider);
            }

            Destroy(GetComponent<UnityEngine.AI.NavMeshAgent>());
            Destroy(gameObject, 3f);
            if (Data.origin) Destroy(Data.origin.gameObject.gameObject, 3f);

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -10f, transform.position.z), Time.deltaTime * Data.SinkSpeed);
        }
    }
}