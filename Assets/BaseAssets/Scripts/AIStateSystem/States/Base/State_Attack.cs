using UnityEngine;
using BaseAssets.Tools;

namespace BaseAssets.AI
{
    public class State_Attack : State
    {
        // State Functions =========================================================================================================
        protected override void RunOnce()
        {
            base.RunOnce();
        }

        public override void EnterState()
        {
            Reference.agent.velocity = Vector3.zero;
            Reference.agent.isStopped = true;

            if (Random.value < 0.5f)
            {
                AIStateHelperMethods.PlayAnimation(Reference.animator, "Attack1");
            }
            else
            {
                AIStateHelperMethods.PlayAnimation(Reference.animator, "Attack2");
            }
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

        public void DealDamage(GameObject _go)
        {
            if (Data.enemy == null) return;
            if (Data.enemy.IsInvulnerable) return;

            int calculationMethod = -1;
            float modifier = 1f;

            if (CounterManager.Instance)
            {
                calculationMethod = CounterManager.Instance.GetCounterModifier(Data.troopType, Data.enemy, Data.Damage).Item1;
                modifier = CounterManager.Instance.GetCounterModifier(Data.troopType, Data.enemy, Data.Damage).Item2;
            }

            if (calculationMethod == 0)
            {
                Damage(Data.Damage * modifier);
            }
            else if (calculationMethod == 1)
            {
                Damage(Data.Damage / modifier);
            }
            else
            {
                Damage(Data.Damage);
            }
        }

        protected void Damage(float _damageAmount)
        {
            Data.enemy.CurrentHealth = Mathf.Clamp(Data.enemy.CurrentHealth -= _damageAmount, 0, float.MaxValue);
        }

        // CHAIN ===============
        private void CheckIfShouldDie()
        {
            if (Data.CurrentHealth <= 0f)
            {
                Owner.ChangeState(AIStateKeeper.States.Death);
                return;
            }

            CheckDistanceToEnemy();
        }

        private void CheckDistanceToEnemy()
        {
            if (AIStateHelperMethods.IsEnemyDead(Data) || AIStateHelperMethods.IsEnemyTooFar(Data, transform.position, Reference))
            {
                AIStateHelperMethods.RemoveFromAttackerList(Data);
                Data.enemy = null;
                Owner.ChangeStateIf(AIStateKeeper.States.Attack, AIStateKeeper.States.Idle);
                return;
            }

            TurnTowardsEnemy();
        }

        private void TurnTowardsEnemy()
        {
            if (!Data.enemy) return;

            Vector3 enemyDirection = Data.enemy.transform.position - transform.position;
            enemyDirection.y = 0f;
            Quaternion tempRotation = Quaternion.LookRotation(enemyDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, tempRotation, Time.deltaTime * 200f);
        }
        // CHAIN ===============
    }
}