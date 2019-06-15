using UnityEngine;

public abstract class State : MonoBehaviour
{
    public AIStateKeeper.States ThisState;

    private bool RanOnce = false;

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();

    protected AIStateManager Owner = null;
    protected AIReferenceKeeper Reference = null;
    protected AIDataHolder Data = null;

    private void Awake()
    {
        Owner = transform.GetComponent<AIStateManager>();
        Reference = transform.GetComponent<AIReferenceKeeper>();
        Data = transform.GetComponent<AIDataHolder>();
        Owner.onEnterState  += CanEnterState;
        Owner.onUpdateState += CanUpdateState;
        Owner.onExitState   += CanExitState;
    }

    protected void GoBackToPreviousState()
    {
        Owner.ChangeStateToPreviousState();
    }

    private void CanEnterState()
    {
        if(IsActive())
        {
            if (RanOnce == false) RunOnce();
            EnterState();
        }
    }

    private void CanUpdateState()
    {
        if(IsActive())
        {
            UpdateState();
        }
    }

    private void CanExitState()
    {
        if(IsActive())
        {
            ExitState();
        }
    }

    protected virtual void RunOnce()
    {
        RanOnce = true;
    }

    private bool IsActive()
    {
        if (Owner.ActiveState == ThisState)
        {
            return true;
        }

        return false;
    }
}
