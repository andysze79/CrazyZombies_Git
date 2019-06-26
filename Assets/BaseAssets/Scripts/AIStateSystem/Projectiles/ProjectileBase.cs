using System.Collections.Generic;
using UnityEngine;
using BaseAssets.Tools;

namespace BaseAssets.AI.Projectile
{
    public class ProjectileBase : MonoBehaviour
    {
        protected bool homingProjectile = true;
        protected float projectileSpeedInSeconds = 1f;
        protected float projectileTrajectoryHeight = 1f;
        protected float projectileTargetOffsetY = 0.5f;
        protected float timer = 0f;
        protected GameObject targetTrajectory = null;
        protected Vector3 targetPosition = Vector3.zero;
        protected Vector3 startPosition = Vector3.zero;

        private bool canDamage = true;
        private AIDataHolder projectileCaster = null;
        private AIDataHolder projectileReceiver = null;
        private LayerMask damagable = 0;

        private ProjectileMover projectileMover = null;

        private void Start()
        {
            startPosition = transform.position;
            projectileMover = GetComponentInChildren<ProjectileMover>();
        }

        protected Vector3 Curve(Vector3 initialPos, Vector3 targetPos, float t)
        {
            float durationRef = t / projectileSpeedInSeconds;
            Vector3 nPos = Vector3.zero;

            if (t <= projectileSpeedInSeconds)
            {
                nPos = Vector3.Lerp(initialPos, targetPos, durationRef);
                nPos.y += (1 - Mathf.Pow(2 * durationRef - 1, 2)) * projectileTrajectoryHeight;
            }
            else
            {
                nPos = targetPos;
            }

            return nPos;
        }

        private void OnDrawGizmosSelected()
        {
            if (homingProjectile && targetTrajectory == null) return;

            Gizmos.color = Color.red;

            List<Vector3> allCurvePos = new List<Vector3>();

            for (int j = 0; j < projectileSpeedInSeconds * 100; j++)
            {
                if (Application.isPlaying)
                {
                    Vector3 pos = Curve(startPosition, new Vector3(targetTrajectory.transform.position.x, targetTrajectory.transform.position.y + projectileTargetOffsetY, targetTrajectory.transform.position.z), (float)j / 50);
                    allCurvePos.Add(pos);
                }
                else
                {
                    Vector3 pos = Curve(transform.position, new Vector3(targetTrajectory.transform.position.x, targetTrajectory.transform.position.y + projectileTargetOffsetY, targetTrajectory.transform.position.z), (float)j / 50);
                    allCurvePos.Add(pos);
                }
            }

            for (int j = 0; j < allCurvePos.Count; j++)
            {
                int prev = 0;
                if (j - 1 < 0)
                {
                    prev = 0;
                }
                else
                {
                    prev = j - 1;
                }

                Gizmos.DrawLine(allCurvePos[prev], allCurvePos[j]);
            }
        }

        protected void CheckIfCanDoDamage()
        {
            if (!canDamage) return;
            if (ToolManagerDataHolder.ProjectileDebugRay) Debug.DrawRay(transform.position, transform.forward, Color.cyan);

            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, transform.forward, out hit, 4f, damagable))
            {
                if (projectileMover) projectileMover.PlayOnHit(hit);
                projectileCaster.Attack.DealDamage(null);
                canDamage = false;
            }
        }

        protected void CheckIfProjectileHasReachedItsEndPosition()
        {
            if (timer / projectileSpeedInSeconds >= 1f)
            {
                Destroy(gameObject);
            }
        }

        public void SetTarget(GameObject _target)
        {
            targetTrajectory = _target;
            homingProjectile = true;
        }

        public void SetTarget(Vector3 _targetPosition)
        {
            targetPosition = _targetPosition;
            homingProjectile = false;
        }

        public void SetProjectileSpeedInSeconds(float _projectileSpeedInSeconds)
        {
            projectileSpeedInSeconds = _projectileSpeedInSeconds;
        }

        public void SetProjectileTrajectoryHeight(float _projectileTrajectoryHeight)
        {
            projectileTrajectoryHeight = _projectileTrajectoryHeight;
        }

        public void SetProjectileTargetOffsetY(float _projectileTargetOffsetY)
        {
            projectileTargetOffsetY = _projectileTargetOffsetY;
        }

        public void SetDamagableLayerMask(LayerMask _damagable)
        {
            damagable = _damagable;
        }

        public void SetProjectileCasterAndReceiver(AIDataHolder _caster, AIDataHolder _receiever)
        {
            projectileCaster = _caster;
            projectileReceiver = _receiever;
        }
    }
}