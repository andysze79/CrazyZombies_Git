using UnityEngine;
using BaseAssets.Tools;

namespace BaseAssets.AI
{
    public class State_Idle : State
    {
        // State Functions =========================================================================================================
        protected override void RunOnce()
        {
            base.RunOnce();

            IfOriginDoesntExistSpawnOne();
        }

        public override void EnterState()
        {
            Reference.agent.isStopped = true;
            AIStateHelperMethods.PlayAnimation(Reference.animator, "Idle");
        }

        public override void UpdateState()
        {
            AIStateHelperMethods.CheckAttackerList(Data);
            CheckIfShouldDie();
        }

        public override void ExitState()
        {

        }
        // =========================================================================================================================

        // CHAIN ===============
        private void CheckIfShouldDie()
        {
            if (Data.CurrentHealth <= 0f)
            {
                Owner.ChangeState(AIStateKeeper.States.Death);
                return;
            }

            FindAndMoveToEnemy();
        }

        public void FindAndMoveToEnemy()
        {
            AIStateHelperMethods.FindEnemy(Data, Reference, Owner);

            IfOriginIsFarAndThereIsNoEnemyGoToOrigin();
        }

        private void IfOriginIsFarAndThereIsNoEnemyGoToOrigin()
        {
            if (Data.origin == null) return;

            if (Vector3.Distance(new Vector3(Data.origin.position.x, transform.position.y, Data.origin.position.z), transform.position) > 0.5f && Data.enemy == null)
            {
                Data.currentMoveTo = Data.origin;
                Data.currentMoveTo.position = new Vector3(Data.currentMoveTo.position.x, transform.position.y, Data.currentMoveTo.position.z);

                AIStateHelperMethods.SetAgentPath(transform.position, Data.currentMoveTo.position, Reference.agent);
                Owner.ChangeState(AIStateKeeper.States.Move);
                return;
            }

            TakeOriginRotation();
        }

        private void TakeOriginRotation()
        {
            if (!Data.origin) return;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Data.origin.rotation, Time.deltaTime * 200f);
        }
        // CHAIN ===============

        private void IfOriginDoesntExistSpawnOne() // Called Once OnEnter
        {
            if (Data.origin == null)
            {
                Transform nullOrigin = new GameObject().transform;
                nullOrigin.name = transform.name + "Origin";
                nullOrigin.position = transform.position;
                nullOrigin.rotation = transform.rotation;
                Data.origin = nullOrigin;

                IfOriginDoesntExistSpawnOne();
            }
            else
            {
                Data.currentMoveTo = Data.origin;
                Data.currentMoveTo.position = new Vector3(Data.currentMoveTo.position.x, transform.position.y, Data.currentMoveTo.position.z);

                AIStateHelperMethods.SetAgentPath(transform.position, Data.currentMoveTo.position, Reference.agent);
                Owner.ChangeState(AIStateKeeper.States.Move);
            }
        }
    }
}