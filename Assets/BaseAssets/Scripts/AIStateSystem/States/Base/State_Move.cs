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
                    //AIStateHelperMethods.SetAgentPath(transform.position, Data.currentMoveTo.position, Reference.agent);
                    Reference.agent.SetDestination(Data.currentMoveTo.position);
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

            if(distanceToOrigin > Data.maxMovementDistance)
            {
                Gizmos.color = Color.red;
                AIStateHelperMethods.DrawString(AIStateHelperMethods.Remap(Reference.agent.speed, Data.minSpeed, Data.maxSpeed, 0f, 100f).ToString("0.0"), Vector3.Lerp(transform.position, Data.currentMoveTo.transform.position, 0f), Color.red);
                //AIStateHelperMethods.DrawString(distanceToOrigin.ToString("0.0") + "D", Vector3.Lerp(transform.position, Data.currentMoveTo.transform.position, 0f), Color.red);
                //AIStateHelperMethods.DrawString(Reference.agent.speed.ToString("0.0") + "S", Vector3.Lerp(transform.position, Data.currentMoveTo.transform.position, 1f), Color.red);
            }
            else if(distanceToOrigin < Data.minMovementDistance)
            {
                Gizmos.color = Color.red;
                AIStateHelperMethods.DrawString(distanceToOrigin.ToString("0.0") + "D", Vector3.Lerp(transform.position, Data.currentMoveTo.transform.position, 0f), Color.red);
                AIStateHelperMethods.DrawString(Reference.agent.speed.ToString("0.0") + "S", Vector3.Lerp(transform.position, Data.currentMoveTo.transform.position, 1f), Color.red);
            }
            else
            {
                Gizmos.color = Color.green;
                AIStateHelperMethods.DrawString(distanceToOrigin.ToString("0.0") + "D", Vector3.Lerp(transform.position, Data.currentMoveTo.transform.position, 0f), Color.green);
                AIStateHelperMethods.DrawString(Reference.agent.speed.ToString("0.0") + "S", Vector3.Lerp(transform.position, Data.currentMoveTo.transform.position, 1f), Color.green);
            }

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