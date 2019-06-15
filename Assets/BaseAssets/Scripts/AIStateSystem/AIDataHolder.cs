using System.Collections.Generic;
using UnityEngine;

public class AIDataHolder : MonoBehaviour
{
    [Header("Tool Settings")]
    public bool showGizmoOnSelect = true;
    [HideInInspector] public bool gizmoInPlayMode = false;

    public Color gizmoColor1 = Color.white;
    public Color gizmoColor2 = Color.red;

    [Header("Common Class Settings")]
    public TroopType troopType = TroopType.Infantry;
    public enum TroopType { Infantry, Cavalry, Archer, None }
    public LayerMask searchLayerMask = 0;
    public FightingMode fightingMode = FightingMode.Passive;
    public enum FightingMode { Passive, Aggressive, PassiveAggressive, Neutral }
    public bool PrioritizeNotTargeted = true;
    public bool IsInvulnerable = false;
    public bool IsTargeted = false;
    public float MaximumHealth = 100f;
    public float CurrentHealth = 100f;
    public float Damage = 10f;
    public float AttackDistance = 40f;
    public float SearchRadius = 10f;
    public float SinkDelay = 2f;
    public float SinkSpeed = 1f;
    public Vector3 searchPosition = new Vector3();
    public Transform origin = null;
    public Transform currentMoveTo = null;
    public AIDataHolder enemy = null;
    public List<AIDataHolder> attackerList = new List<AIDataHolder>();

    [Header("Archer Class Settings")]
    public bool homingProjectile = true;
    public float projectileSpeedInSeconds = 1f;
    public float projectileTrajectoryHeight = 1f;
    public float projectileTargetOffsetY = 0.5f;
    public GameObject arrowPrefab = null;
    public Vector3 projectileSpawnOffset = new Vector3();

    private AIStateManager Owner = null;

    [HideInInspector] public SpawnManager Spawner = null;
    [HideInInspector] public State_Idle Idle = null;
    [HideInInspector] public State_Move Move = null;
    [HideInInspector] public State_Attack Attack = null;
    [HideInInspector] public State_Death Death = null;

    private void Awake() 
    {
        Owner = GetComponent<AIStateManager>();
        Idle = GetComponent<State_Idle>();
        Move = GetComponent<State_Move>();
        Attack = GetComponent<State_Attack>();
        Death = GetComponent<State_Death>();

        CurrentHealth = MaximumHealth;
    }

    // External Transitions
    public void KillAI()
    {
        Owner.ChangeState(AIStateKeeper.States.Death);
    }

    public void SetupAI(SpawnManager _Spawner, Transform _newOrigin, bool _changeState = true)
    {
        origin = _newOrigin;
        Spawner = _Spawner;

        if (_changeState)
        {
            if(Owner == null)
            {
                Owner = GetComponent<AIStateManager>();
            }

            Owner.ChangeState(AIStateKeeper.States.Move);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && !gizmoInPlayMode) return;

        if (showGizmoOnSelect) return;
        DrawGizmos();
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying && !gizmoInPlayMode) return;

        if (!showGizmoOnSelect) return;
        DrawGizmos();
    }

    private void DrawGizmos()
    {
        UnityEditor.Handles.color = gizmoColor1;
        UnityEditor.Handles.DrawWireDisc(transform.position + searchPosition, Vector3.up, SearchRadius);
        UnityEditor.Handles.color = gizmoColor2;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, AttackDistance);
    }

    public void SetFightingMode(AIDataHolder.FightingMode _newFightingMode)
    {
        fightingMode = _newFightingMode;
    }

    public void DrawButtons()
    {
        GUILayout.BeginHorizontal("Box");
        if (GUILayout.Button("Enter Idle"))
        {
            if (!Application.isPlaying) return;

            Owner.ChangeState(AIStateKeeper.States.Idle);
        }

        if (GUILayout.Button("Enter Move"))
        {
            if (!Application.isPlaying) return;

            Owner.ChangeState(AIStateKeeper.States.Move);
        }

        if (GUILayout.Button("Enter Attack"))
        {
            if (!Application.isPlaying) return;

            Owner.ChangeState(AIStateKeeper.States.Attack);
        }

        if (GUILayout.Button("Enter Death"))
        {
            if (!Application.isPlaying) return;

            Owner.ChangeState(AIStateKeeper.States.Death);
        }
        GUILayout.EndHorizontal();
    }
}