using UnityEngine;

public class State_Move_Zombie : State_Move
{
    // State Functions =========================================================================================================
    protected override void RunOnce()
    {
        base.RunOnce();
    }

    public override void EnterState()
    {
        base.EnterState();
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
            Data.currentMoveTo = null;
            Owner.ChangeState(AIStateKeeper.States.Idle);
        }
    }
}