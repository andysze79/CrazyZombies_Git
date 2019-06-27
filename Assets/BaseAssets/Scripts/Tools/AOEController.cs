using UnityEngine;
using UnityEngine.Events;

namespace BaseAssets.Tools
{
    public class AOEController : MonoBehaviour
    {
        [Header("Tool Settings")]
        public bool showGizmoOnSelect = true;

        public Color gizmoColor = Color.white;
        public LayerMask layerMask = 0;
        public float destroySelfTime = 0f;

        [Header("Radius Settings")]
        public float radius = 0f;
        public float maxRadius = 10f;
        public float radiusExpandSpeed = 1f;

        [Header("Kickback Settings")]
        public float kickbackSpeedInSeconds = 1f;
        public float kickbackTrajectoryHeight = 1f;
        public float kickbackDistance = 1f;

        [Header("Damage Settings")]
        public float damageToInflict = 0f;

        [Header("Callbacks")]
        public UnityEvent OnKickbackStart;
        public UnityEvent OnKickbackUpdate;
        public UnityEvent OnKickbackEnd;

        private void Update() 
        {
            radius += Time.deltaTime * radiusExpandSpeed;
            radius = Mathf.Clamp(radius, 0f, maxRadius);
            Collider[] enemiesInSearchArea = Physics.OverlapSphere(transform.position, radius, layerMask);

            for (int i = 0; i < enemiesInSearchArea.Length; i++)
            {
                if(!enemiesInSearchArea[i].transform.parent.GetComponent<AOEControlled>())
                {
                    AOEControlled controlled = enemiesInSearchArea[i].transform.parent.gameObject.AddComponent<AOEControlled>();
                    controlled.Setup(transform.position, kickbackSpeedInSeconds, kickbackTrajectoryHeight, kickbackDistance, damageToInflict, OnKickbackStart, OnKickbackUpdate, OnKickbackEnd);
                }
            }

            if(destroySelfTime != 0f)
            {
                Destroy(gameObject, destroySelfTime);
            }
        }

        private void OnDrawGizmos() 
        {
            if (showGizmoOnSelect) return;
            DrawGizmos();
        }

        private void OnDrawGizmosSelected() 
        {
            if (!showGizmoOnSelect) return;
            DrawGizmos();
        }

        private void DrawGizmos()
        {
            UnityEditor.Handles.color = gizmoColor;
            radius = Mathf.Clamp(radius, 0f, maxRadius);
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, radius);
        }
    }
}

