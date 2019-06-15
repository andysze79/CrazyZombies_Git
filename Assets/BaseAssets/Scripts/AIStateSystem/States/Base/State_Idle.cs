using UnityEngine;

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

        FindEnemyAndGetItsLocation();
    }

    public void FindEnemyAndGetItsLocation()
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
            Data.currentMoveTo = enemy.transform;
            Data.currentMoveTo.position = new Vector3(Data.currentMoveTo.position.x, transform.position.y, Data.currentMoveTo.position.z);
            Reference.agent.SetDestination(Data.currentMoveTo.position);
            Owner.ChangeState(AIStateKeeper.States.Move);
            return;
        }

        IfOriginIsFarAndThereIsNoEnemyGoToOrigin();
    }

    private void IfOriginIsFarAndThereIsNoEnemyGoToOrigin()
    {
        if (Data.origin == null) return;

        if (Vector3.Distance(new Vector3(Data.origin.position.x, transform.position.y, Data.origin.position.z), transform.position) > 0.5f)
        {
            Data.currentMoveTo = Data.origin;
            Data.currentMoveTo.position = new Vector3(Data.currentMoveTo.position.x, transform.position.y, Data.currentMoveTo.position.z);
            Reference.agent.SetDestination(Data.currentMoveTo.position);
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
        if(Data.origin == null)
        {
            Transform nullOrigin = new GameObject().transform;
            nullOrigin.name = transform.name + "Origin";
            nullOrigin.position = transform.position;
            Data.origin = nullOrigin;
        }
        else
        {
            Data.currentMoveTo = Data.origin;
            Data.currentMoveTo.position = new Vector3(Data.currentMoveTo.position.x, transform.position.y, Data.currentMoveTo.position.z);
            Reference.agent.SetDestination(Data.currentMoveTo.position);
            Owner.ChangeState(AIStateKeeper.States.Move);
        }
    }
}