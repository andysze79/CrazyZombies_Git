using UnityEngine;

namespace BaseAssets.AI
{
    public class State_Move_Zombie : State_Move
    {
        // State Functions =========================================================================================================
        protected override void RunOnce()
        {
            base.RunOnce();
        }

        public override void EnterState()
        {
            Reference.agent.isStopped = false;
            AIStateHelperMethods.PlayAnimation(Reference.animator, "Run");
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }
        // =========================================================================================================================   

        // CHAIN ===============
        protected override void FindAndMoveToEnemy()
        {
            IfOriginIsFarAndThereIsNoEnemyGoToOrigin();
        }

        protected override void IfOriginIsFarAndThereIsNoEnemyGoToOrigin()
        {
            if (Data.origin == null) return;

            ReachedDestination();
        }

        protected override void ReachedDestination()
        {
            if (AIStateHelperMethods.HasReachedDestination(Reference.agent))
            {
                //Data.currentMoveTo = null;
                //Owner.ChangeState(AIStateKeeper.States.Idle);
            }
        }
        // CHAIN ===============

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            if (Owner.ActiveState != AIStateKeeper.States.Move) return;

            Gizmos.color = Color.green;
            if (Reference.agent.path.corners.Length > 0)
            {
                for (var i = 1; i < Reference.agent.path.corners.Length; i++)
                {
                    Gizmos.DrawLine(Reference.agent.path.corners[i - 1], Reference.agent.path.corners[i]);
                }
            }
        }
    }
}