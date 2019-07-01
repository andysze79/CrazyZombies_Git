using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using BaseAssets.AI;

namespace BaseAssets.Tools
{
    class AOEControlled : MonoBehaviour 
    {
        private bool canMove = false;
        private float kickbackSpeedInSeconds = 1f;
        private float kickbackTrajectoryHeight = 1f;
        private float kickbackDistance = 1f;
        private float damageToInflict = 0f;
        private Vector3 startTrajectory = Vector3.zero;
        private Vector3 targetTrajectory = Vector3.zero;
        private Vector3 aoeCenter = Vector3.zero;
        private AIReferenceKeeper reference = null;
        private AIDataHolder data = null;

        private UnityEvent OnKickbackStart;
        private UnityEvent OnKickbackUpdate;
        private UnityEvent OnKickbackEnd;

        private float timer = 0f;

        public void Setup(Vector3 _aoeCenter, float _kickbackSpeedInSeconds, float _kickbackTrajectoryHeight, float _kickbackDistance, float _damageToInflict, UnityEvent _OnKickbackStart, UnityEvent _OnKickbackUpdate, UnityEvent _OnKickbackEnd)
        {
            reference = GetComponent<AIReferenceKeeper>();
            data = GetComponent<AIDataHolder>();
            
            startTrajectory = transform.position;
            aoeCenter = _aoeCenter;
            kickbackSpeedInSeconds = _kickbackSpeedInSeconds;
            kickbackTrajectoryHeight = _kickbackTrajectoryHeight;
            damageToInflict = _damageToInflict;
            OnKickbackStart = _OnKickbackStart;
            OnKickbackUpdate = _OnKickbackUpdate;
            OnKickbackEnd = _OnKickbackEnd;

            //targetTrajectory = startTrajectory - ((aoeCenter - startTrajectory) * _kickbackDistance);
            targetTrajectory = new Vector3(transform.position.x + _kickbackDistance, 24f, transform.position.z);

            AIStateHelperMethods.PlayAnimation(reference.animator, "Hit");
            reference.agent.enabled = false;

            OnKickbackStart.Invoke();

            canMove = true;
        }

        private void Update() 
        {
            if(!canMove) return;

            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, Curve(startTrajectory, targetTrajectory, timer), timer / kickbackSpeedInSeconds);

            Vector3 lookDirection = (2f * transform.position - Curve(startTrajectory, targetTrajectory, timer)) - transform.position;
            if(lookDirection !=  Vector3.zero)
            {
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), timer);
            }

            OnKickbackUpdate.Invoke();

            if (timer / kickbackSpeedInSeconds >= 1f)
            {
                transform.rotation = Quaternion.LookRotation(startTrajectory - transform.position);
                canMove = false;
                data.Attack.DealAOEDamageToSelf(damageToInflict);
                if(reference.agent && data.CurrentHealth > 0) reference.agent.enabled = true;

                OnKickbackEnd.Invoke();

                Destroy(this);
            }
        }

        private Vector3 Curve(Vector3 _initialPos, Vector3 _targetPos, float t)
        {
            float durationRef = t / kickbackSpeedInSeconds;
            Vector3 nPos = Vector3.zero;

            if (t <= kickbackSpeedInSeconds)
            {
                nPos = Vector3.Lerp(_initialPos, _targetPos, durationRef);
                nPos.y += (1 - Mathf.Pow(2 * durationRef - 1, 2)) * kickbackTrajectoryHeight;
            }
            else
            {
                nPos = _targetPos;
            }

            return nPos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            List<Vector3> allCurvePos = new List<Vector3>();

            for (int j = 0; j < kickbackSpeedInSeconds * 100; j++)
            {
                Vector3 pos = Curve(startTrajectory, targetTrajectory, (float)j / 50);
                allCurvePos.Add(pos);
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
    }
}