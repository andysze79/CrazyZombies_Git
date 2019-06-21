using UnityEngine;
using PathCreation;

namespace BaseAssets.Tools
{
    public class FormationMover : MonoBehaviour
    {
        private bool isActive = false;
        private bool loop = false;
        private float initialDelayBeforeMove = 5f;
        private float formationMoveSpeed = 5f;
        private float distanceTravelled = 0f;
        private float timer = 0f;
        private PathCreator pathCreator = null;
        private FormationPlanner formationPlanner = null;

        public void SetValues(bool _isActive, bool _loop, float _initialDelayBeforeMove, float _formationMoveSpeed)
        {
            isActive = _isActive;
            loop = _loop;
            initialDelayBeforeMove = _initialDelayBeforeMove;
            formationMoveSpeed = _formationMoveSpeed;
        }

        private void Start()
        {
            pathCreator = transform.GetComponentInChildren<PathCreator>();
            formationPlanner = GetComponent<FormationPlanner>();

            if (!pathCreator) return;

            if (pathCreator.bezierPath.Space != PathSpace.xyz)
            {
                pathCreator.bezierPath.Space = PathSpace.xyz;
            }

            pathCreator.bezierPath.GlobalNormalsAngle = 90;
        }

        private void Update()
        {
            if (!isActive) return;

            timer += Time.deltaTime;

            if (timer < initialDelayBeforeMove) return;
            if (!loop && distanceTravelled >= pathCreator.path.length - 0.5f)
            {
                formationPlanner.isFormationMoverActive = false;
                distanceTravelled = 0f;
                timer = 0f;
                return;
            }

            distanceTravelled += formationMoveSpeed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        }

        public void DrawButtons()
        {
            GUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("I want to be able to move this formation!"))
            {
                if (!GetComponentInChildren<PathCreator>())
                {
                    GameObject pathCreator = new GameObject("FormationPathCreator");
                    pathCreator.transform.SetParent(transform);
                    pathCreator.AddComponent(typeof(PathCreator));
                    if (formationPlanner == null)
                    {
                        formationPlanner = GetComponent<FormationPlanner>();
                    }
                    formationPlanner.isFormationMoverActive = true;
                }
            }
            GUILayout.EndHorizontal();
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(FormationMover))]
    public class FormationMoverInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            FormationMover _target = target as FormationMover;
            _target.DrawButtons();
            base.OnInspectorGUI();
        }
    }
#endif
}