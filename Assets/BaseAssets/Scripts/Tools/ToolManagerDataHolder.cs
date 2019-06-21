using UnityEngine;
using UnityEditor;

namespace BaseAssets.Tools
{
    public class ToolManagerDataHolder
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

        public static bool GizmoInEditMode
        {
            get
            {
                return EditorPrefs.GetBool("GizmosInEditMode", true);
            }
            set
            {
                SpawnManager[] sm = (SpawnManager[])Object.FindObjectsOfType(typeof(SpawnManager));
                foreach (SpawnManager s in sm)
                {
                    s.gizmoInEditMode = value;
                }

                FormationPlanner[] fp = (FormationPlanner[])Object.FindObjectsOfType(typeof(FormationPlanner));
                foreach (FormationPlanner f in fp)
                {
                    f.gizmoInEditMode = value;
                }

                EditorPrefs.SetBool("GizmosInEditMode", value);
            }
        }

        public static bool ProjectileDebugRay
        {
            get
            {
                return EditorPrefs.GetBool("ProjectileDebugRay");
            }
            set
            {
                EditorPrefs.SetBool("ProjectileDebugRay", value);
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
}