using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProjectManagerDataHolder
{
    public static bool GizmoInPlayMode
    {
        get
        {
            return EditorPrefs.GetBool("GizmosInPlayMode", true);
        }
        set
        {
            SpawnManager[] sm = (SpawnManager[])Object.FindObjectsOfType(typeof(SpawnManager));
            foreach (SpawnManager s in sm)
            {
                s.gizmoInPlayMode = value;
            }

            FormationPlanner[] fp = (FormationPlanner[])Object.FindObjectsOfType(typeof(FormationPlanner));
            foreach (FormationPlanner f in fp)
            {
                f.gizmoInPlayMode = value;
            }

            EditorPrefs.SetBool("GizmosInPlayMode", value);
        }
    }

    public static bool ShowInfoLog
    {
        get
        {
            return EditorPrefs.GetBool("ShowInfoLog");
        }
        set
        {
            EditorPrefs.SetBool("ShowInfoLog", value);
        }
    }

    public static bool ShowErrorLog
    {
        get
        {
            return EditorPrefs.GetBool("ShowErrorLog");
        }
        set
        {
            EditorPrefs.SetBool("ShowErrorLog", value);
        }
    }
}