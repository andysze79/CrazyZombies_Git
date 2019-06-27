using UnityEngine;
using BaseAssets.Tools;

namespace BaseAssets.AI
{
    public class State_Move : State
    {
        private float currentSpeed;
        private Vector3 previousPosition;
        private float distanceToOrigin = 0f;

        // State Functions =========================================================================================================
        protected override void RunOnce()
        {
            base.RunOnce();
        }

        public override void EnterState()
        {
            Reference.agent.isStopped = false;
            AIStateHelperMethods.PlayAnimation(Reference.animator, "Run");

            if (Data.currentMoveTo)
            {
                if (Data.useSetDestination)
                    Reference.agent.SetDestination(Data.currentMoveTo.position);
                else
                    AIStateHelperMethods.SetAgentPath(transform.position, Data.currentMoveTo.position, Reference.agent);
            }
        }

        public override void UpdateState()
        {
            AIStateHelperMethods.CheckAttackerList(Data);
            AIStateHelperMethods.SetAnimationParameters(Reference.animator, "MovementSpeed", Reference.agent.velocity.magnitude / Reference.agent.speed);

            CheckIfShouldDie();
        }

        public override void ExitState()
        {

        }
        // ========================================================================================================================= 

        // CHAIN ===============
        protected void CheckIfShouldDie()
        {
            if (Data.CurrentHealth <= 0f)
            {
                Owner.ChangeState(AIStateKeeper.States.Death);
                return;
            }

            CheckIfDestinationExists();
        }

        private void CheckIfDestinationExists()
        {
            if (!Data.currentMoveTo)
            {
                Owner.ChangeState(AIStateKeeper.States.Idle);
                return;
            }

            FindAndMoveToEnemy();
        }

        protected virtual void FindAndMoveToEnemy()
        {
            AIStateHelperMethods.FindEnemy(Data, Reference, Owner);

            IfOriginIsFarAndThereIsNoEnemyGoToOrigin();
        }

        protected virtual void IfOriginIsFarAndThereIsNoEnemyGoToOrigin()
        {
            if (Data.origin != null)
            {
                distanceToOrigin = Vector3.Distance(new Vector3(Data.origin.position.x, transform.position.y, Data.origin.position.z), transform.position);
                Reference.agent.speed = AIStateHelperMethods.GetAgentSpeedBasedOnDistance(Data, distanceToOrigin);

                if (distanceToOrigin > 0.5f && Data.enemy == null)
                {
                    Data.currentMoveTo = Data.origin;
                    if (Data.useSetDestination)
                        Reference.agent.SetDestination(Data.currentMoveTo.position);
                    else
                        AIStateHelperMethods.SetAgentPath(transform.position, Data.currentMoveTo.position, Reference.agent);
                    return;
                }
            }

            ReachedDestination();
        }

        protected virtual void ReachedDestination()
        {
            if (AIStateHelperMethods.HasReachedDestination(Reference.agent))
            {
                if (Data.enemy)
                {
                    Data.currentMoveTo = null;
                    Owner.ChangeState(AIStateKeeper.States.Attack);
                    return;
                }
                else
                {
                    Data.currentMoveTo = null;
                    Owner.ChangeState(AIStateKeeper.States.Idle);
                    return;
                }
            }
        }
        // CHAIN ===============

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            if (Owner.ActiveState != AIStateKeeper.States.Move) return;

            if (Data.currentMoveTo == null) return;

            AIStateHelperMethods.DrawString("+" + AIStateHelperMethods.Remap(Reference.agent.speed, Data.minPossibleSpeed, Data.maxPossibleSpeed, 0f, 100f).ToString("0") + "%", Vector3.Lerp(transform.position, Data.currentMoveTo.transform.position, 0f), 
                                            Color.Lerp(Color.green, Color.red, AIStateHelperMethods.Remap(Reference.agent.speed, Data.minPossibleSpeed, Data.maxPossibleSpeed, 0f, 1f)));

            Gizmos.color = Color.Lerp(Color.green, Color.red, AIStateHelperMethods.Remap(Reference.agent.speed, Data.minPossibleSpeed, Data.maxPossibleSpeed, 0f, 1f));

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