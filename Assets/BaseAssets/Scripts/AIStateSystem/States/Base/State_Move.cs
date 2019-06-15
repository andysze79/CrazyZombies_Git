using UnityEngine;

public class State_Move : State 
{
    private float currentSpeed;
    private Vector3 previousPosition;

    // State Functions =========================================================================================================
    protected override void RunOnce()
    {
        base.RunOnce();
    }

    public override void EnterState()
    {
        Reference.agent.isStopped = false;
        AIStateHelperMethods.PlayAnimation(Reference.animator, "Run");

        if(Data.currentMoveTo)
        {
            Reference.agent.SetDestination(Data.currentMoveTo.position);
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

        CheckIfShouldChangeAnimation();
    }

    private void CheckIfShouldChangeAnimation()
    {
        Vector3 curMove = transform.position - previousPosition;
        currentSpeed = curMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;

        if (currentSpeed < 2f)
        {
            AIStateHelperMethods.PlayAnimation(Reference.animator, "Idle");
        }
        else
        {
            AIStateHelperMethods.PlayAnimation(Reference.animator, "Run");
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
        AIDataHolder enemy = null;

        if (Data.Spawner && Data.Spawner.targetingType == SpawnManager.TargetingType.BoxCast)
        {
            if (Time.frameCount % 20 == 0)
                enemy = AIStateHelperMethods.FindEnemyInBox(Data, Data.Spawner.transform.position + Data.Spawner.targetAreaPosition, Data.Spawner.targetAreaDimension, 1000f, Data.searchLayerMask, Data.PrioritizeNotTargeted);
        }
        else
        {
            if (Time.frameCount % 20 == 0)
                enemy = AIStateHelperMethods.FindEnemyInCircle(Data, Data.searchPosition, Data.SearchRadius, Vector3.up, 1000f, Data.searchLayerMask, Data.PrioritizeNotTargeted);
        }

        if (enemy)
        {
            Data.enemy = enemy;
            Data.currentMoveTo = enemy.transform;
            Reference.agent.SetDestination(Data.currentMoveTo.position);

            if (Vector3.Distance(enemy.transform.position, transform.position) <= Data.AttackDistance)
            {
                Owner.ChangeState(AIStateKeeper.States.Attack);
                return;
            }
        }

        IfOriginIsFarAndThereIsNoEnemyGoToOrigin();
    }

    protected virtual void IfOriginIsFarAndThereIsNoEnemyGoToOrigin()
    {
        if (Data.origin == null) return;

        if (Vector3.Distance(new Vector3(Data.origin.position.x, transform.position.y, Data.origin.position.z), transform.position) > 0.5f && Data.enemy == null)
        {
            Data.currentMoveTo = Data.origin;
            Reference.agent.SetDestination(Data.currentMoveTo.position);
            return;
        }

        ReachedDestination();
    }

    protected virtual void ReachedDestination()
    {
        if (AIStateHelperMethods.HasReachedDestination(Reference.agent))
        {
            if(Data.enemy)
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

        Gizmos.color = Color.green;
        if(Reference.agent.path.corners.Length > 0)
        {
            for (var i = 1; i < Reference.agent.path.corners.Length; i++)
            {
                Gizmos.DrawLine(Reference.agent.path.corners[i - 1], Reference.agent.path.corners[i]);
            }
        }
    }
}