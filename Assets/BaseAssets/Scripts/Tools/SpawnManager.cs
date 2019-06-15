// TODO: Add hard cap to endless spawn

using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Tool Settings")]
    public bool showGizmoOnSelect                           = false;
    [HideInInspector] public bool gizmoInPlayMode           = false;
    public float gizmoScale                                 = 1f;
    public Color gizmoColor1                                = Color.cyan;
    public Color gizmoColor2                                = Color.red;
    public Color gizmoColor3                                = Color.blue;

    [Header("SETUP")]
    [Tooltip("This is the parent for all spawned troops by this spawner")]
    public Transform spawnTroopContainer                    = null;
    public GameObject troopPrefab                           = null;
    public List<FormationPlanner> formations                = new List<FormationPlanner>();

    [Header("Spawn Settings")]
    public bool spawnOnStart                                = true;
    public bool spawnAtTheSpot                              = false;
    public bool spawnInvulnerableUnits                      = false;
    public SpawnType spawnType                              = SpawnType.SpawnAllAtOnce;
    public enum SpawnType { SpawnAllAtOnce, SpawnInBurst, SpawnWithTimer, SpawnEndless }
    [Tooltip("If you are using SpawnEndless spawn type, then you need to select RandomArea")]
    public SpawnLocationType spawnLocationType              = SpawnLocationType.SpecificLocations;
    public enum SpawnLocationType { SpecificLocations, RandomArea }
    [Tooltip("Do you want units to attack enemies around them or in a specific area?")]
    public TargetingType targetingType                      = TargetingType.SphereCast;
    public enum TargetingType {SphereCast, BoxCast}
    [Tooltip("If units are moving closer to enemy in Z axis then choose X_Axis, if units are moving closer to enemy in X axis choose Z_Axis")]
    public EndlessTargetAxis endlessTargetAxis              = EndlessTargetAxis.X_Axis;
    public enum EndlessTargetAxis { X_Axis, Z_Axis }
    [Tooltip("This is the parent object to keep all waypoints, only Endless spawner requires it")]
    public Transform endlessWaypointContainer               = null;
    [Tooltip("How many troops do you want to spawn every burst?")]
    public int endlessTroopCountPerBurst                    = 1;
    [Tooltip("How long do you want to wait between each burst?")]
    public float endlessBurstDelay                          = 1;
    [Tooltip("How long do you want to wait before spawner starts spawning?")]
    public float endlessBurstDelayInitial                   = 1;
    [Tooltip("How many seconds do you want to wait before each troop kills themselves?")]
    public float endlessDestroyTime                         = 0f;
    [Tooltip("How many bursts do you want? For example: If formation supports 20 troops and burstCount is 10, then each burst will spawn 2 troops")]
    public int burstCount                                   = 1;
    [Tooltip("How long do you want to wait between each burst?")]
    public float burstDelay                                 = 1f;
    [Tooltip("How long do you want to wait before spawner starts spawning?")]
    public float burstSpawnDelayInitial                     = 0f;
    [Tooltip("How long do you want to wait before spawner starts spawning?")]
    public float spawnDelayInitial                          = 0f;
    [Tooltip("How long do you want to wait between each spawn?")]
    public float spawnDelayPerTroop                         = 0f;
    [Tooltip("The size of the target area for endless spawning")]
    public Vector3 endlessTargetAreaDimensions              = new Vector3();
    [Tooltip("The position of the target area for endless spawning, rotation is not supported. Change endlessTargetAxis for horizontal/vertical")]
    public Vector3 endlessTargetAreaPosition                = new Vector3();
    [Tooltip("Size of the spawn area for random troop spawning")]
    public Vector3 spawnAreaDimensions                      = new Vector3();
    [Tooltip("Location of the spawn area for random troop spawning")]
    public Vector3 spawnAreaPosition                        = new Vector3();
    [Tooltip("Size of the target area for units to find and attack enemies")]
    public Vector3 targetAreaDimension                      = new Vector3();
    [Tooltip("Location of the target area for units to find and attack enemies")]
    public Vector3 targetAreaPosition                       = new Vector3();

    [Tooltip("Drag all spawnLocations here and tool will take turns spawning troops, NOT COMPATIBLE with endless spawning!!!")]
    public List<GameObject> spawnLocations                  = new List<GameObject>();

    [Header("Prioritization")]
    public AIDataHolder.TroopType Priority1 = AIDataHolder.TroopType.None;
    public AIDataHolder.TroopType Priority2 = AIDataHolder.TroopType.None;
    public AIDataHolder.TroopType Priority3 = AIDataHolder.TroopType.None;
    
    [Header("Automated Spawn Settings")]
    [Tooltip("These variables provide automatic formation cycling every 'automatedTime' seconds")]
    public bool automateFormationSwitch                     = false;
    public bool loopAutomation                              = false;
    public float automatedTime                              = 0f;

    [Header("Timeline Settings")]
    [Tooltip("Setting this will apply the formation to troops. Make sure the number is correct by checking formations list")]
    public int timelineFormationIndex                       = -1;

    private int formationIndex                              = 0;
    private int activeSpawnLocation                         = 0;
    private Coroutine BurstCoroutine                        = null;
    private Coroutine TimerCoroutine                        = null;
    private Coroutine BurstEndlessCoroutine                 = null;
    private Coroutine AutomatedFormationSwitchCoroutine      = null;

    private void Start()
    {
        if(spawnTroopContainer == null)
        {
            spawnTroopContainer = new GameObject($"{transform.name}'s Troop Container").transform;
            DebugManager.SpawnContainerEmpty(gameObject);
        }

        if (spawnType == SpawnManager.SpawnType.SpawnEndless && endlessWaypointContainer == null)
        {
            endlessWaypointContainer = new GameObject($"{transform.name}'s Waypoint Container").transform;
            DebugManager.WaypointContainerEmpty(gameObject);
        }

        if(spawnOnStart)
        {
            Spawn();
        }

        if(automateFormationSwitch)
        {
            AutomatedFormationSwitchCoroutine = StartCoroutine(AutomatedFormationSwitch());
        }
    }
    
    private void Update()
    {
        TimelineFormationSwitcher();
    }

    // GIZMO ========================================================================================================================================================
    private void OnDrawGizmos()
    {
        if(Application.isPlaying && !gizmoInPlayMode) return;

        if (showGizmoOnSelect) return;
        DrawGizmo();
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying && !gizmoInPlayMode) return;

        if (!showGizmoOnSelect) return;
        DrawGizmo();
    }

    private void DrawGizmo()
    {
        if (spawnLocationType == SpawnLocationType.RandomArea)
        {
            Gizmos.color = gizmoColor1;
            Gizmos.DrawWireCube(transform.position + spawnAreaPosition, spawnAreaDimensions);

            if (spawnType == SpawnType.SpawnEndless)
            {
                Gizmos.color = gizmoColor2;
                Gizmos.DrawWireCube(transform.position + endlessTargetAreaPosition, endlessTargetAreaDimensions);
            }
        }
        else
        {
            if(spawnLocations != null && spawnLocations.Count > 0)
            {
                Gizmos.color = gizmoColor1;
                for (int i = 0; i < spawnLocations.Count; i++)
                {
                    Gizmos.DrawWireSphere(spawnLocations[i].transform.position, gizmoScale);
                }

                if (spawnType == SpawnType.SpawnEndless)
                {
                    Gizmos.color = gizmoColor2;
                    Gizmos.DrawWireCube(transform.position + endlessTargetAreaPosition, endlessTargetAreaDimensions);
                }
            }
            else
            {
                DebugManager.SpawnLocationsEmpty(gameObject);
            }
        }

        if(targetingType == TargetingType.BoxCast)
        {
            Gizmos.color = gizmoColor3;
            Gizmos.DrawWireCube(transform.position + targetAreaPosition, targetAreaDimension);
        }
    }
    // GIZMO ========================================================================================================================================================

    // FORMATION SWITCH =============================================================================================================================================
    private IEnumerator AutomatedFormationSwitch()
    {
        while(automateFormationSwitch)
        {
            yield return new WaitForSeconds(automatedTime);
            formationIndex++;

            if (loopAutomation && formationIndex == formations.Count)
            {
                formationIndex = 0;
            }
            else if(!loopAutomation && formationIndex == formations.Count)
            {
                break;
            }

            SwitchFormation(formationIndex);
        }
    }

    public void SwitchFormation(int _formationIndex)
    {
        if (formations.Count <= 1)
        {
            DebugManager.NoFormationToSwitchTo(gameObject);
            return;
        }

        if (troopPrefab == null)
        {
            DebugManager.NoTroopPrefabSet(gameObject);
            return;
        }

        if (spawnTroopContainer == null)
        {
            DebugManager.NoTroopParentSet(gameObject);
            return;
        }

        DestroyAllWaypoints();

        List<Vector3> spawnPositions = formations[_formationIndex].GetActiveList();
        List<Transform> waypoints = CreateWaypoints(spawnPositions);

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if(i < spawnTroopContainer.childCount)
            {
                AIDataHolder troop = spawnTroopContainer.GetChild(i).GetComponent<AIDataHolder>();

                troop.SetupAI(this, waypoints[i]);
                troop.fightingMode = formations[_formationIndex].fightingMode;
            }
            else
            {
                AIDataHolder troop = Instantiate(troopPrefab, spawnTroopContainer).GetComponent<AIDataHolder>();
                troop.GetComponent<NavMeshAgent>().Warp(spawnTroopContainer.GetChild(0).position);

                troop.SetupAI(this, waypoints[i]);
                troop.fightingMode = formations[_formationIndex].fightingMode;

                SetTroopInvulnerability(troop);
            }
        }
    }

    public void SwitchToNextFormation()
    {
        if (formations.Count <= 1)
        {
            DebugManager.NoFormationToSwitchTo(gameObject);
            return;
        }

        if (troopPrefab == null)
        {
            DebugManager.NoTroopPrefabSet(gameObject);
            return;
        }

        if (spawnTroopContainer == null)
        {
            DebugManager.NoTroopParentSet(gameObject);
            return;
        }

        DestroyAllWaypoints();

        formationIndex++;

        if (formationIndex >= formations.Count)
        {
            formationIndex = 0;
        }

        List<Vector3> spawnPositions = formations[formationIndex].GetActiveList();
        List<Transform> waypoints = CreateWaypoints(spawnPositions);

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            if (i < spawnTroopContainer.childCount)
            {
                AIDataHolder troop = spawnTroopContainer.GetChild(i).GetComponent<AIDataHolder>();

                troop.SetupAI(this, waypoints[i]);
                troop.fightingMode = formations[formationIndex].fightingMode;
            }
            else
            {
                AIDataHolder troop = Instantiate(troopPrefab, spawnTroopContainer).GetComponent<AIDataHolder>();
                troop.GetComponent<NavMeshAgent>().Warp(spawnTroopContainer.GetChild(0).position);

                troop.SetupAI(this, waypoints[i]);
                troop.fightingMode = formations[formationIndex].fightingMode;

                SetTroopInvulnerability(troop);
            }
        }
    }
    // FORMATION SWITCH =============================================================================================================================================

    // SPAWN TYPES ==================================================================================================================================================
    private void SpawnAllAtOnce()
    {
        if (formations.Count == 0)
        {
            DebugManager.NoFormationAtAll(gameObject);
            return;
        }

        if(formations[formationIndex] == null)
        {
            DebugManager.MissingFormation(gameObject);
            return;
        }

        if (troopPrefab == null)
        {
            DebugManager.NoTroopPrefabSet(gameObject);
            return;
        }

        if (spawnTroopContainer == null)
        {
            DebugManager.NoTroopParentSet(gameObject);
            return;
        }
        
        // TODO: Check if formations list has a formation in it even if the count is more than 0
        List<Vector3> spawnPositions = formations[formationIndex].GetActiveList();
        List<Transform> waypoints = CreateWaypoints(spawnPositions);

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            AIDataHolder troop = Instantiate(troopPrefab, spawnTroopContainer).GetComponent<AIDataHolder>();
            troop.GetComponent<NavMeshAgent>().Warp(GetTroopPosition(spawnPositions[i]));

            troop.SetupAI(this, waypoints[i]);
            troop.fightingMode = formations[formationIndex].fightingMode;

            SetTroopInvulnerability(troop);
            ActivateNextSpawnLocation();
        }
    }

    private IEnumerator SpawnInBurst()
    {
        if (formations.Count == 0)
        {
            DebugManager.NoFormationAtAll(gameObject);
            yield break;
        }

        if (formations[formationIndex] == null)
        {
            DebugManager.MissingFormation(gameObject);
            yield break;
        }

        if (troopPrefab == null)
        {
            DebugManager.NoTroopPrefabSet(gameObject);
            yield break;
        }

        if (spawnTroopContainer == null)
        {
            DebugManager.NoTroopParentSet(gameObject);
            yield break; 
        }

        List<Vector3> spawnPositions = formations[formationIndex].GetActiveList();
        List<Transform> waypoints = CreateWaypoints(spawnPositions);

        yield return new WaitForSeconds(burstSpawnDelayInitial);

        int spawnPerBurst = spawnPositions.Count / burstCount;
        int spawnIndex = 0;

        for (int i = 0; i < burstCount; i++)
        {
            for (int k = 0; k < spawnPerBurst; k++)
            {
                AIDataHolder troop = Instantiate(troopPrefab, spawnTroopContainer).GetComponent<AIDataHolder>();
                troop.GetComponent<NavMeshAgent>().Warp(GetTroopPosition(spawnPositions[spawnIndex]));

                troop.SetupAI(this, waypoints[spawnIndex]);
                troop.fightingMode = formations[formationIndex].fightingMode;

                SetTroopInvulnerability(troop);

                spawnIndex++;

                if (i == burstCount - 1 && k == spawnPerBurst - 1)
                {
                    yield return new WaitForSeconds(burstDelay);
                    ActivateNextSpawnLocation();

                    int remainingTroopCount = spawnPositions.Count - spawnTroopContainer.childCount;

                    for (int l = 0; l < remainingTroopCount; l++)
                    {
                        AIDataHolder troopRemainder = Instantiate(troopPrefab, spawnTroopContainer).GetComponent<AIDataHolder>();
                        troopRemainder.GetComponent<NavMeshAgent>().Warp(GetTroopPosition(spawnPositions[spawnIndex]));

                        troopRemainder.SetupAI(this, waypoints[spawnIndex]);
                        troopRemainder.fightingMode = formations[formationIndex].fightingMode;

                        SetTroopInvulnerability(troop);

                        spawnIndex++;
                    }
                }
            }

            yield return new WaitForSeconds(burstDelay);
            ActivateNextSpawnLocation();
        }
    }

    private IEnumerator SpawnTroopsWithTimer()
    {
        if (formations.Count == 0)
        {
            DebugManager.NoFormationAtAll(gameObject);
            yield break;
        }

        if (formations[formationIndex] == null)
        {
            DebugManager.MissingFormation(gameObject);
            yield break;
        }

        if (troopPrefab == null)
        {
            DebugManager.NoTroopPrefabSet(gameObject);
            yield break;
        }

        if (spawnTroopContainer == null)
        {
            DebugManager.NoTroopParentSet(gameObject);
            yield break;
        }
        
        List<Vector3> spawnPositions = formations[formationIndex].GetActiveList();
        List<Transform> waypoints = CreateWaypoints(spawnPositions);

        yield return new WaitForSeconds(spawnDelayInitial);

        for (int i = 0; i < spawnPositions.Count; i++)
        {
            AIDataHolder troop = Instantiate(troopPrefab, spawnTroopContainer).GetComponent<AIDataHolder>();
            troop.GetComponent<NavMeshAgent>().Warp(GetTroopPosition(spawnPositions[i]));

            troop.SetupAI(this, waypoints[i]);
            troop.fightingMode = formations[formationIndex].fightingMode;

            SetTroopInvulnerability(troop);
            ActivateNextSpawnLocation();

            yield return new WaitForSeconds(spawnDelayPerTroop);
        }
    }

    private IEnumerator SpawnInBurstEndless()
    {
        if (troopPrefab == null)
        {
            DebugManager.NoTroopPrefabSet(gameObject);
            yield break;
        }

        if (spawnTroopContainer == null)
        {
            DebugManager.NoTroopParentSet(gameObject);
            yield break;
        }

        yield return new WaitForSeconds(endlessBurstDelayInitial);

        while (spawnType == SpawnType.SpawnEndless && spawnLocationType == SpawnLocationType.RandomArea)
        {
            for (int i = 0; i < endlessTroopCountPerBurst; i++)
            {
                Transform spawnTransform = new GameObject(i.ToString()).transform;
                spawnTransform.rotation = transform.rotation;
                spawnTransform.SetParent(endlessWaypointContainer);

                AIDataHolder troop = Instantiate(troopPrefab, spawnTroopContainer).GetComponent<AIDataHolder>();
                troop.GetComponent<NavMeshAgent>().Warp(GetTroopPosition(spawnTransform.position));

                if (endlessTargetAxis == EndlessTargetAxis.X_Axis)
                {
                    spawnTransform.position = GetTargetPositionByPercentage(troop.transform.position.x);
                }
                else
                {
                    spawnTransform.position = GetTargetPositionByPercentage(troop.transform.position.z);
                }

                troop.SetupAI(this, spawnTransform);
                troop.fightingMode = AIDataHolder.FightingMode.Aggressive;

                SetTroopInvulnerability(troop);

                if (endlessDestroyTime != 0f)
                {
                    Destroy(troop.gameObject, endlessDestroyTime);
                    Destroy(spawnTransform.gameObject, endlessDestroyTime);
                }
            }

            ActivateNextSpawnLocation();
            yield return new WaitForSeconds(endlessBurstDelay);
        }

        if (spawnType != SpawnType.SpawnEndless || spawnLocationType != SpawnLocationType.RandomArea)
        {
            DebugManager.EndlessSpawnError(gameObject);
        }
    }
    // SPAWN TYPES ==================================================================================================================================================

    // GUI ==========================================================================================================================================================
    public void DrawButtons()
    {
        GUILayout.BeginHorizontal("Box");
        if (GUILayout.Button("Spawn Troops"))
        {
            if (!Application.isPlaying) return;

            StopCoroutines();
            DestroyAll();
            ResetValues();
            Spawn();
        }

        if(GUILayout.Button("Remove Troops"))
        {
            if (!Application.isPlaying) return;

            StopCoroutines();
            DestroyAll();
            ResetValues();
        }

        if (GUILayout.Button("Switch To Next Formation"))
        {
            if (!Application.isPlaying) return;

            SwitchToNextFormation();
        }
        GUILayout.EndHorizontal();
    }
    // GUI ==========================================================================================================================================================

    // HELPER METHODS ===============================================================================================================================================
    public void SetFightingMode(AIDataHolder.FightingMode _newFightingMode)
    {
        foreach (Transform spawnTroop in spawnTroopContainer)
        {
            spawnTroop.GetComponent<AIDataHolder>().fightingMode = _newFightingMode;
        }
    }

    private void TimelineFormationSwitcher()
    {
        if (timelineFormationIndex >= 0 && timelineFormationIndex < formations.Count)
        {
            SwitchFormation(timelineFormationIndex);
            timelineFormationIndex = -1;
        }
    }

    private int GetActiveSpawnLocation()
    {
        return activeSpawnLocation;
    }

    private void SetActiveSpawnLocation(int _newSpawnLocation)
    {
        activeSpawnLocation = _newSpawnLocation;
    }

    private void ActivateNextSpawnLocation()
    {
        if (GetActiveSpawnLocation() + 1 >= spawnLocations.Count)
        {
            SetActiveSpawnLocation(0);
        }
        else
        {
            SetActiveSpawnLocation(GetActiveSpawnLocation() + 1);
        }
    }

    private Vector3 GetRandomLocationInArea(Vector3 _areaPosition, Vector3 _areaDimension)
    {
        return new Vector3(transform.position.x + _areaPosition.x + Random.Range(-_areaDimension.x / 2f, _areaDimension.x / 2f),
                           transform.position.y + _areaPosition.y,
                           transform.position.z + _areaPosition.z + Random.Range(-_areaDimension.z / 2f, _areaDimension.z / 2f));
    }

    private Vector3 GetSpawnLocation()
    {
        if (spawnLocationType == SpawnLocationType.RandomArea)
        {
            return GetRandomLocationInArea(spawnAreaPosition, spawnAreaDimensions);
        }
        else
        {
            return spawnLocations[GetActiveSpawnLocation()].transform.position;
        }
    }

    private Vector3 GetTargetPositionByPercentage(float _troopPosition)
    {
        // X = ((Y - Z) / I) * 100f; To get the percentage from location
        // (((X / 100) * I) + Z) + targetMinLeft = Y; To get the location from percentage

        float spawnAreaPosition = 0f;
        float spawnAreaDimensionHalf = 0f;
        float targetAreaPosition = 0f;
        float targetAreaDimensionHalf = 0f;

        if (endlessTargetAxis == EndlessTargetAxis.X_Axis)
        {
            spawnAreaPosition = this.spawnAreaPosition.x + transform.position.x;
            spawnAreaDimensionHalf = spawnAreaDimensions.x / 2f;
            targetAreaPosition = endlessTargetAreaPosition.x + transform.position.x;
            targetAreaDimensionHalf = endlessTargetAreaDimensions.x / 2f;
        }
        else
        {
            spawnAreaPosition = this.spawnAreaPosition.z + transform.position.z;
            spawnAreaDimensionHalf = spawnAreaDimensions.z / 2f;
            targetAreaPosition = endlessTargetAreaPosition.z + transform.position.z;
            targetAreaDimensionHalf = endlessTargetAreaDimensions.z / 2f;
        }

        float minLeft = spawnAreaPosition - spawnAreaDimensionHalf;
        float maxRight = spawnAreaPosition + spawnAreaDimensionHalf;
        float zeroedRight = maxRight - minLeft;
        float zeroedPosition = _troopPosition - minLeft;
        float result = (zeroedPosition / zeroedRight) * 100f;

        float targetMinLeft = targetAreaPosition - targetAreaDimensionHalf;
        float targetMaxRight = targetAreaPosition + targetAreaDimensionHalf;
        float targetZeroedRight = targetMaxRight - targetMinLeft;
        float targetResult = (((result / 100f) * targetZeroedRight) + targetMinLeft);

        if (endlessTargetAxis == EndlessTargetAxis.X_Axis)
        {
            return new Vector3(targetResult,
                          transform.position.y + endlessTargetAreaPosition.y,
                          transform.position.z + endlessTargetAreaPosition.z + Random.Range(-endlessTargetAreaDimensions.z / 2f, endlessTargetAreaDimensions.z / 2f));
        }
        else
        {
            return new Vector3(transform.position.x + endlessTargetAreaPosition.x + Random.Range(-endlessTargetAreaDimensions.x / 2f, endlessTargetAreaDimensions.x / 2f),
                          transform.position.y + endlessTargetAreaPosition.y,
                          targetResult);
        }
    }

    private void SetTroopInvulnerability(AIDataHolder _troop)
    {
        if (spawnInvulnerableUnits)
        {
            _troop.IsInvulnerable = true;
        }
    }

    private void DestroyAll()
    {
        if(spawnType == SpawnManager.SpawnType.SpawnEndless)
        {
            foreach (Transform waypoint in endlessWaypointContainer)
            {
                Destroy(waypoint.gameObject);
            }
        }
        else
        {
            DestroyAllWaypoints();
        }

        foreach (Transform spawnTroop in spawnTroopContainer)
        {
            Destroy(spawnTroop.gameObject);
        }
    }

    private void ResetValues()
    {
        formationIndex = 0;
        activeSpawnLocation = 0;
    }

    private List<Transform> CreateWaypoints(List<Vector3> _spawnPositions)
    {
        List<Transform> waypoints = new List<Transform>();

        for (int i = 0; i < _spawnPositions.Count; i++)
        {
            Transform waypoint = new GameObject(i.ToString()).transform;
            waypoint.position = _spawnPositions[i];
            waypoint.rotation = formations[formationIndex].transform.rotation;

            if (formations[formationIndex].pathCreator)
            {
                waypoint.SetParent(formations[formationIndex].pathCreator.transform);
            }
            else if (!formations[formationIndex].pathCreator)
            {
                waypoint.SetParent(formations[formationIndex].transform);
            }

            waypoints.Add(waypoint);
        }

        return waypoints;
    }
    
    private void DestroyAllWaypoints()
    {
        if (formations[formationIndex].pathCreator)
        {
            foreach (Transform waypoint in formations[formationIndex].pathCreator.transform)
            {
                Destroy(waypoint.gameObject);
            }
        }
        else
        {
            foreach (Transform waypoint in formations[formationIndex].transform)
            {
                Destroy(waypoint.gameObject);
            }
        }
    }

    private Vector3 GetTroopPosition(Vector3 _spawnPosition)
    {
        if (spawnAtTheSpot)
        {
            return _spawnPosition;
        }
        else
        {
            return GetSpawnLocation();
        }
    }

    private void Spawn()
    {
        if (spawnType == SpawnType.SpawnAllAtOnce)
        {
            SpawnAllAtOnce();
        }
        else if (spawnType == SpawnType.SpawnInBurst)
        {
            BurstCoroutine = StartCoroutine(SpawnInBurst());
        }
        else if (spawnType == SpawnType.SpawnWithTimer)
        {
            TimerCoroutine = StartCoroutine(SpawnTroopsWithTimer());
        }
        else if (spawnType == SpawnType.SpawnEndless)
        {
            BurstEndlessCoroutine = StartCoroutine(SpawnInBurstEndless());
        }
    }

    private void StopCoroutines()
    {
        if (BurstCoroutine != null) StopCoroutine(BurstCoroutine);
        if (TimerCoroutine != null) StopCoroutine(TimerCoroutine);
        if (BurstEndlessCoroutine != null) StopCoroutine(BurstEndlessCoroutine);
        if (AutomatedFormationSwitchCoroutine != null) StopCoroutine(AutomatedFormationSwitchCoroutine);
    }
}
