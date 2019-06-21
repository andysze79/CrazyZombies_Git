using UnityEngine;

namespace BaseAssets.AI
{
    public class AIStateManager : MonoBehaviour
    {
        public AIStateKeeper.States PreviousState;
        public AIStateKeeper.States ActiveState;

        private bool RunUpdate = false;

        public OnEnterState onEnterState;
        public OnUpdateState onUpdateState;
        public OnExitState onExitState;

        public delegate void OnEnterState();
        public delegate void OnUpdateState();
        public delegate void OnExitState();

        private void Start()
        {
            ChangeState(ActiveState);
        }

        public void ChangeStateToPreviousState()
        {
            ExitState();
            SetPreviousState();
            ActiveState = PreviousState;
            EnterState();
        }

        public void ChangeState(AIStateKeeper.States _newState)
        {
            ExitState();
            SetPreviousState();
            ActiveState = _newState;
            EnterState();
        }

        public void ChangeStateIf(AIStateKeeper.States _requiredState, AIStateKeeper.States _newState)
        {
            if (ActiveState != _requiredState) return;

            ExitState();
            SetPreviousState();
            ActiveState = _newState;
            EnterState();
        }

        private void SetPreviousState()
        {
            PreviousState = ActiveState;
        }

        private void EnterState()
        {
            if (onEnterState != null)
            {
                onEnterState.Invoke();
            }

            RunUpdate = true;
        }

        private void UpdateState()
        {
            if (RunUpdate == false)
            {
                return;
            }

            if (onUpdateState != null)
            {
                onUpdateState.Invoke();
            }
        }

        private void ExitState()
        {
            RunUpdate = false;

            if (onExitState != null)
            {
                onExitState.Invoke();
            }
        }

        private void Update()
        {
            UpdateState();
        }
    }
}