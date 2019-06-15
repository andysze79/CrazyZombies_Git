using UnityEngine;

public class State_Idle_Zombie : State_Idle 
{
    // State Functions =========================================================================================================
    protected override void RunOnce()
    {
        base.RunOnce();
    }

    public override void EnterState()
    {
        Owner.ChangeState(AIStateKeeper.States.Move);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
    // =========================================================================================================================    
}