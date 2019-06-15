using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(FormationMover))]
public class FormationPlanner : MonoBehaviour
{
    [Header("Tool Settings")]
    public bool showGizmoOnSelect = false;
    [HideInInspector] public bool gizmoInPlayMode = false;
    public float gizmoScale = 1f;
    public Color gizmoColor1 = Color.white;
    public Color gizmoColor2 = Color.red;
    public Mesh gizmoPreviewMesh = null;

    [Header("Formation Settings")]
    public FormationType formationType;
    public enum FormationType { Regular, Line, Manual }
    public AIDataHolder.FightingMode fightingMode = AIDataHolder.FightingMode.Aggressive;

    [Header("Regular Formation Settings")]
    public bool isMirrored = false;
    public bool isRightAligned = false;
    public float rowDistance = 1f;
    public float columnDistance = 1f;
    public float tilt = 0f;
    public List<Troops> troopFormation = new List<Troops>();

    [Header("Line Formation Settings")]
    public bool isLoop = false;
    public int lineSize = 1;
    public List<Vector3> lineCorners = new List<Vector3>();

    [Header("Manual Formation Settings")]
    public List<Vector3> manualPositions = new List<Vector3>();

    [Header("Moving Formation Settins")]
    public bool isFormationMoverActive = false;
    public bool loopMovement = false;
    public float initialDelayBeforeMove = 5f;
    public float formationMoveSpeed = 5f;

    [HideInInspector] public FormationMover formationMover = null;
    [HideInInspector] public PathCreation.PathCreator pathCreator = null;

    public List<Vector3> GetActiveList()
    {
        switch (formationType)
        {
            case FormationType.Regular:
                return RegularFormation();
            case FormationType.Line:
                return LineFormation();
            case FormationType.Manual:
                return ManualFormation();
            default:
                return null;
        }
    }

    private void Awake() 
    {
        formationMover = GetComponent<FormationMover>();
        pathCreator = GetComponentInChildren<PathCreation.PathCreator>();
    }

    private void Update() 
    {
        if(formationMover)
        {
            formationMover.SetValues(isFormationMoverActive, loopMovement, initialDelayBeforeMove, formationMoveSpeed);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && !gizmoInPlayMode) return;

        if (showGizmoOnSelect) return;
        DrawFormation();
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying && !gizmoInPlayMode) return;

        if (!showGizmoOnSelect) return;
        DrawFormation();
    }

    public void DrawFormation()
    {
        List<Vector3> troopPositions = new List<Vector3>();

        if (formationType == FormationType.Regular)
        {
            troopPositions = RegularFormation();
        }
        else if (formationType == FormationType.Line)
        {
            troopPositions = LineFormation();

            Gizmos.color = gizmoColor2;
            for (int i = 0; i < lineCorners.Count; i++)
            {
                Gizmos.DrawWireSphere(SetRotation(0, lineCorners[i]), gizmoScale * 1.5f);
            }
        }
        else if (formationType == FormationType.Manual)
        {
            troopPositions = ManualFormation();
        }

        if (troopFormation.Count == 0)
        {
            return;
        }

        Gizmos.color = gizmoColor1;
        for (int i = 0; i < troopPositions.Count; i++)
        {
            if (gizmoPreviewMesh)
            {
                Gizmos.DrawWireMesh(gizmoPreviewMesh, troopPositions[i], Quaternion.Euler(transform.rotation.x - 90f, transform.rotation.y, transform.rotation.z), Vector3.one * gizmoScale);
            }
            else
            {
                Gizmos.DrawWireSphere(troopPositions[i], gizmoScale);
            }
        }
    }

    private List<Vector3> GetTroopList()
    {
        List<Vector3> troopPositions = new List<Vector3>();

        if (formationType == FormationType.Regular)
        {
            troopPositions = RegularFormation();
        }
        else if (formationType == FormationType.Line)
        {
            troopPositions = LineFormation();
        }
        else if (formationType == FormationType.Manual)
        {
            troopPositions = ManualFormation();
        }

        return troopPositions;
    }

    private List<Vector3> RegularFormation()
    {
        if (troopFormation.Count == 0)
        {
            DebugManager.TroopFormationEmpty(gameObject);
        }

        List<Vector3> troopPositions = new List<Vector3>();

        for (int i = 0; i < troopFormation.Count; i++)
        {
            for (int k = 0; k < troopFormation[i].rowCount; k++)
            {
                if (!isRightAligned)
                {
                    float tiltModified = (k - 1) * tilt;

                    troopPositions.Add(new Vector3((k * rowDistance) + troopFormation[i].rowOffset, transform.position.y, (i * -columnDistance) + tiltModified));
                    troopPositions[troopPositions.Count - 1] = SetRotation(0, troopPositions[troopPositions.Count - 1]);

                    if (isMirrored)
                    {
                        troopPositions.Add(new Vector3(((k + 1) * rowDistance) + troopFormation[i].rowOffset, transform.position.y, (i * -columnDistance) + tiltModified));
                        troopPositions[troopPositions.Count - 1] = SetRotation(1, troopPositions[troopPositions.Count - 1]);
                    }
                }
                else
                {
                    float tiltModified = (k - 1) * tilt;

                    troopPositions.Add(new Vector3((k * rowDistance) + troopFormation[i].rowOffset, transform.position.y, (i * -columnDistance) + tiltModified));
                    troopPositions[troopPositions.Count - 1] = SetRotation(1, troopPositions[troopPositions.Count - 1]);

                    if (isMirrored)
                    {
                        troopPositions.Add(new Vector3(((k + 1) * rowDistance) + troopFormation[i].rowOffset, transform.position.y, (i * -columnDistance) + tiltModified));
                        troopPositions[troopPositions.Count - 1] = SetRotation(0, troopPositions[troopPositions.Count - 1]);
                    }
                }
            }
        }

        return troopPositions;
    }

    private List<Vector3> LineFormation()
    {
        List<Vector3> troopPositions = new List<Vector3>();

        for (int i = 0; i < lineCorners.Count; i++)
        {
            for (int k = 0; k < lineSize; k++)
            {
                if (k + 1 <= lineSize && i + 1 < lineCorners.Count)
                {
                    troopPositions.Add(Vector3.Lerp(lineCorners[i], lineCorners[i + 1], (1f / (lineSize + 1f)) * (k + 1)));
                    troopPositions[troopPositions.Count - 1] = SetRotation(0, troopPositions[troopPositions.Count - 1]);
                }
            }
        }

        if (isLoop)
        {
            if (lineCorners.Count > 2)
            {
                for (int k = 0; k < lineSize; k++)
                {
                    if (k + 1 <= lineSize)
                    {
                        troopPositions.Add(Vector3.Lerp(lineCorners[0], lineCorners[lineCorners.Count - 1], (1f / (lineSize + 1f)) * (k + 1)));
                        troopPositions[troopPositions.Count - 1] = SetRotation(0, troopPositions[troopPositions.Count - 1]);
                    }
                }
            }
        }

        return troopPositions;
    }

    private List<Vector3> ManualFormation()
    {
        List<Vector3> troopPositions = new List<Vector3>();

        for (int i = 0; i < manualPositions.Count; i++)
        {
            troopPositions.Add(manualPositions[i]);
            troopPositions[troopPositions.Count - 1] = SetRotation(0, troopPositions[troopPositions.Count - 1]);
        }

        return troopPositions;
    }

    private Vector3 SetRotation(int _operationType, Vector3 _troopPosition)
    {
        if(_operationType == 0)
        {
            Quaternion q = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up);
            _troopPosition = q * _troopPosition;

            return new Vector3(transform.position.x + _troopPosition.x,
                                 transform.position.y,
                                 transform.position.z + _troopPosition.z);
        }
        else
        {
            Quaternion q = Quaternion.AngleAxis(-transform.eulerAngles.y, Vector3.up);
            _troopPosition = q * _troopPosition;

            return new Vector3(transform.position.x - _troopPosition.x,
                                 transform.position.y,
                                 transform.position.z + _troopPosition.z);
        }
    }

    public void DrawButtons()
    {
        GUILayout.BeginHorizontal("Box");
        if (GUILayout.Button("How Many Soldiers Will Spawn?"))
        {
            DebugManager.ToolConfiguration(formationType, GetTroopList().Count, gameObject);
        }
        GUILayout.EndHorizontal();
    }
}

[System.Serializable]
public class Troops
{
    [Range(1, 25)]
    public int rowCount = 1;

    [Range(-25f, 25f)]
    public float rowOffset = 0;
}
