using UnityEngine.AI;
using System.Linq;
using UnityEngine;
using BaseAssets.Tools;

namespace BaseAssets.AI
{
    public static class AIStateHelperMethods
    {
        // ENEMY
        public static bool IsEnemyTooFar(AIDataHolder _data, Vector3 _casterPosition, AIReferenceKeeper _reference)
        {
            if (_data.enemy)
            {
                if (Vector3.Distance(_data.enemy.transform.position, _casterPosition) > _data.AttackDistance)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public static bool IsEnemyDead(AIDataHolder _data)
        {
            if (!_data.enemy) return true;

            if (_data.enemy.CurrentHealth <= 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // ENEMY FIND RELATED
        public static void FindEnemy(AIDataHolder _data, AIReferenceKeeper _reference, AIStateManager _owner)
        {
            AIDataHolder enemy = null;

            if (_data.Spawner && _data.Spawner.targetingType == SpawnManager.TargetingType.BoxCast)
            {
                enemy = AIStateHelperMethods.FindEnemyInBox(_data, _data.Spawner.transform.position + _data.Spawner.targetAreaPosition, _data.Spawner.targetAreaDimension, 1000f, _data.searchLayerMask, _data.PrioritizeNotTargeted);
            }
            else
            {
                enemy = AIStateHelperMethods.FindEnemyInCircle(_data, _data.searchPosition, _data.SearchRadius, Vector3.up, 1000f, _data.searchLayerMask, _data.PrioritizeNotTargeted);
            }

            if (_data.enemy || enemy)
            {
                if (_data.enemy == null)
                {
                    _data.enemy = enemy;
                }

                _data.currentMoveTo = _data.enemy.transform;
                float distanceToEnemy = Vector3.Distance(_data.currentMoveTo.position, _data.transform.position);
                _reference.agent.speed = AIStateHelperMethods.GetAgentSpeedBasedOnDistance(_data, distanceToEnemy);

                if (distanceToEnemy <= _data.AttackDistance)
                {
                    _owner.ChangeState(AIStateKeeper.States.Attack);
                }
                else if (distanceToEnemy > _data.SearchRadius)
                {
                    _data.enemy = null;
                }
                else
                {
                    if(_data.useSetDestination)
                        _reference.agent.SetDestination(_data.currentMoveTo.position);
                    else
                        AIStateHelperMethods.SetAgentPath(_data.transform.position, _data.currentMoveTo.position, _reference.agent);
                    _owner.ChangeState(AIStateKeeper.States.Move);
                }
            }
        }

        public static AIDataHolder FindEnemyInCircle(AIDataHolder _caster, Vector3 _center, float _radius, Vector3 _direction, float _maxDistance, LayerMask _layerMask, bool _attackNotTargeted)
        {
            if (_caster.fightingMode == AIDataHolder.FightingMode.Passive || _caster.fightingMode == AIDataHolder.FightingMode.Neutral) return null;
            if (_caster.fightingMode == AIDataHolder.FightingMode.PassiveAggressive && _caster.attackerList.Count == 0) return null;

            RaycastHit[] enemiesInSearchArea = Physics.SphereCastAll(_caster.transform.position + _center, _radius, _direction, _maxDistance, _layerMask);

            if (_caster.enemy)
            {
                _caster.enemy.attackerList.Remove(_caster);
                if (_caster.enemy.attackerList.Count == 0)
                {
                    _caster.enemy.IsTargeted = false;
                }
            }

            Transform bestTarget = FindClosest(_attackNotTargeted, enemiesInSearchArea, _caster);

            if (bestTarget == null)
            {
                bestTarget = FindClosest(false, enemiesInSearchArea, _caster);
            }

            if (bestTarget)
            {
                return bestTarget.transform.GetComponentInParent<AIDataHolder>();
            }
            else
            {
                return null;
            }
        }

        public static AIDataHolder FindEnemyInBox(AIDataHolder _caster, Vector3 _center, Vector3 _size, float _maxDistance, LayerMask _layerMask, bool _attackNotTargeted)
        {
            if (_caster.fightingMode == AIDataHolder.FightingMode.Passive || _caster.fightingMode == AIDataHolder.FightingMode.Neutral) return null;
            if (_caster.fightingMode == AIDataHolder.FightingMode.PassiveAggressive && _caster.attackerList.Count == 0) return null;

            RaycastHit[] enemiesInSearchArea = Physics.BoxCastAll(_center, _size / 2f, Vector3.up, _caster.Spawner.transform.rotation, _maxDistance, _layerMask);

            if (_caster.enemy)
            {
                _caster.enemy.attackerList.Remove(_caster);
                if (_caster.enemy.attackerList.Count == 0)
                {
                    _caster.enemy.IsTargeted = false;
                }
            }

            Transform bestTarget = FindClosest(_attackNotTargeted, enemiesInSearchArea, _caster);

            if (bestTarget == null)
            {
                bestTarget = FindClosest(false, enemiesInSearchArea, _caster);
            }

            if (bestTarget)
            {
                return bestTarget.transform.GetComponentInParent<AIDataHolder>();
            }
            else
            {
                return null;
            }
        }

        private static Transform FindClosest(bool _findNotTargeted, RaycastHit[] _enemiesInSearchArea, AIDataHolder _caster)
        {
            AIDataHolder bestTarget = null;

            // float closestDistanceSqr = Mathf.Infinity;

            // AIDataHolder[] potentialTargetDatas = new AIDataHolder[_enemiesInSearchArea.Length];

            // for (int i = 0; i < potentialTargetDatas.Length; i++)
            // {
            //     potentialTargetDatas[i] = _enemiesInSearchArea[i].transform.GetComponentInParent<AIDataHolder>();
            // }

            // foreach (AIDataHolder potentialTargetData in potentialTargetDatas)
            // {
            //     Vector3 directionToTarget = potentialTargetData.transform.position - _caster.transform.position;
            //     float dSqrToTarget = directionToTarget.sqrMagnitude;
            //     if (dSqrToTarget < closestDistanceSqr)
            //     {
            //         if (potentialTargetData.fightingMode != AIDataHolder.FightingMode.Neutral)
            //         {
            //             if (_findNotTargeted)
            //             {
            //                 if (!potentialTargetData.IsTargeted)
            //                 {
            //                     closestDistanceSqr = dSqrToTarget;
            //                     bestTarget = potentialTargetData;
            //                 }
            //             }
            //             else
            //             {
            //                 closestDistanceSqr = dSqrToTarget;
            //                 bestTarget = potentialTargetData;
            //             }
            //         }
            //     }
            // }

            AIDataHolder[] datas = CreateArrayBasedOnDistance(_enemiesInSearchArea, _caster);

            if (datas.Length == 0)
            {
                return null;
            }

            if (_caster.Spawner)
            {
                if (bestTarget == null)
                {
                    for (int i = 0; i < datas.Length; i++)
                    {
                        if (datas[i].fightingMode != AIDataHolder.FightingMode.Neutral)
                        {
                            if (!datas[i].IsTargeted)
                            {
                                if (datas[i].troopType == _caster.Spawner.Priority1)
                                {
                                    bestTarget = datas[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                if (bestTarget == null)
                {
                    for (int i = 0; i < datas.Length; i++)
                    {
                        if (datas[i].fightingMode != AIDataHolder.FightingMode.Neutral)
                        {
                            if (!datas[i].IsTargeted)
                            {
                                if (datas[i].troopType == _caster.Spawner.Priority2)
                                {
                                    bestTarget = datas[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                if (bestTarget == null)
                {
                    for (int i = 0; i < datas.Length; i++)
                    {
                        if (datas[i].fightingMode != AIDataHolder.FightingMode.Neutral)
                        {
                            if (!datas[i].IsTargeted)
                            {
                                if (datas[i].troopType == _caster.Spawner.Priority3)
                                {
                                    bestTarget = datas[i];
                                    break;
                                }
                            }
                        }
                    }
                }
                if (bestTarget == null)
                {
                    for (int i = 0; i < datas.Length; i++)
                    {
                        if (datas[i].fightingMode != AIDataHolder.FightingMode.Neutral)
                        {
                            if (datas[i].troopType == _caster.Spawner.Priority1)
                            {
                                bestTarget = datas[i];
                                break;
                            }
                        }
                    }
                }
                if (bestTarget == null)
                {
                    for (int i = 0; i < datas.Length; i++)
                    {
                        if (datas[i].fightingMode != AIDataHolder.FightingMode.Neutral)
                        {
                            if (datas[i].troopType == _caster.Spawner.Priority2)
                            {
                                bestTarget = datas[i];
                                break;
                            }
                        }
                    }
                }
                if (bestTarget == null)
                {
                    for (int i = 0; i < datas.Length; i++)
                    {
                        if (datas[i].fightingMode != AIDataHolder.FightingMode.Neutral)
                        {
                            if (datas[i].troopType == _caster.Spawner.Priority3)
                            {
                                bestTarget = datas[i];
                                break;
                            }
                        }
                    }
                }
            }
            else // No spawner
            {
                if (bestTarget == null)
                {
                    for (int i = 0; i < datas.Length; i++)
                    {
                        if (datas[i].fightingMode != AIDataHolder.FightingMode.Neutral)
                        {
                            if (!datas[i].IsTargeted)
                            {
                                bestTarget = datas[i];
                                break;
                            }
                        }
                    }
                }
                if (bestTarget == null)
                {
                    for (int i = 0; i < datas.Length; i++)
                    {
                        if (datas[i].fightingMode != AIDataHolder.FightingMode.Neutral)
                        {
                            bestTarget = datas[i];
                            break;
                        }
                    }
                }
            }

            if (bestTarget == null)
            {
                for (int i = 0; i < datas.Length; i++)
                {
                    if (datas[i].fightingMode != AIDataHolder.FightingMode.Neutral)
                    {
                        bestTarget = datas[i];
                        break;
                    }
                }
            }

            if (bestTarget)
            {
                AIDataHolder enemyData = bestTarget.transform.GetComponentInParent<AIDataHolder>();

                if (!enemyData.attackerList.Contains(_caster))
                {
                    enemyData.attackerList.Add(_caster);
                }

                enemyData.IsTargeted = true;

                return bestTarget.transform;
            }
            else
            {
                return null;
            }
        }

        private static AIDataHolder[] CreateArrayBasedOnDistance(RaycastHit[] _enemiesInSearchArea, AIDataHolder _caster)
        {
            AIDataHolder[] potentialTargetDatas = new AIDataHolder[_enemiesInSearchArea.Length];

            for (int i = 0; i < potentialTargetDatas.Length; i++)
            {
                potentialTargetDatas[i] = _enemiesInSearchArea[i].transform.GetComponentInParent<AIDataHolder>();
            }

            return potentialTargetDatas.OrderBy(x => Vector3.Distance(_caster.transform.position, x.transform.position)).ToArray();
        }

        public static void CheckAttackerList(AIDataHolder _data)
        {
            for (int i = 0; i < _data.attackerList.Count; i++)
            {
                if (_data.attackerList[i] == null || (_data.attackerList[i].enemy && _data.attackerList[i].enemy.CurrentHealth <= 0))
                {
                    _data.attackerList.RemoveAt(i);
                }
            }

            if (_data.attackerList.Count == 0)
            {
                _data.IsTargeted = false;
            }
        }

        public static void RemoveFromAttackerList(AIDataHolder _data)
        {
            if (_data.enemy)
            {
                _data.enemy.attackerList.Remove(_data);
                if (_data.enemy.attackerList.Count == 0)
                {
                    _data.enemy.IsTargeted = false;
                }
            }
        }

        // NAVMESH RELATED
        public static void SetAgentPath(Vector3 _from, Vector3 _to, NavMeshAgent _agent)
        {
            if(!_agent.enabled) return;
            
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(_from, _to, NavMesh.AllAreas, path))
            {
                _agent.SetPath(path);
            }
        }

        public static bool HasReachedDestination(NavMeshAgent _agent)
        {
            if (!_agent.enabled) return false;

            if (!_agent.pathPending)
            {
                if (_agent.remainingDistance <= _agent.stoppingDistance)
                {
                    if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static float GetAgentSpeedBasedOnDistance(AIDataHolder _data, float _distance)
        {
            float agentSpeed = 0f;

            if (_distance > _data.maxAccelerationDistance)
            {
                agentSpeed = _data.maxPossibleSpeed;
            }
            else if (_distance < _data.minDecelarationDistance)
            {
                agentSpeed = _data.minPossibleSpeed;
            }
            else
            {
                var distRatio = (_distance - _data.minDecelarationDistance) / (_data.maxAccelerationDistance - _data.minDecelarationDistance);
                var diffSpeed = _data.maxPossibleSpeed - _data.minPossibleSpeed;
                agentSpeed = (distRatio * diffSpeed) + _data.minPossibleSpeed;
            }

            return agentSpeed;
        }

        // ANIMATOR & ANIMATION RELATED
        public static void PlayAnimation(Animator _animator, string _stateName)
        {
            if (_animator == null) return;

            AnimatorControllerParameter[] parameters = _animator.parameters;

            for (int i = 0; i < parameters.Length; ++i)
            {
                if (parameters[i].type == UnityEngine.AnimatorControllerParameterType.Trigger)
                {
                    _animator.ResetTrigger(parameters[i].name);
                }
            }

            _animator.SetTrigger(_stateName);
        }

        public static void SetAnimationParameters(Animator _animator, string _parameterName, float _value)
        {
            if (_animator == null) return;

            _animator.SetFloat(_parameterName, _value);
        }

        // GUI RELATED
        public static void DrawString(string _text, Vector3 _worldPos, Color? _colour = null)
        {
            UnityEditor.Handles.BeginGUI();

            Color restoreColor = GUI.color;

            if (_colour.HasValue) GUI.color = _colour.Value;
            UnityEditor.SceneView view = UnityEditor.SceneView.currentDrawingSceneView;
            Vector3 screenPos = view.camera.WorldToScreenPoint(_worldPos);

            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                GUI.color = restoreColor;
                UnityEditor.Handles.EndGUI();
                return;
            }

            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(_text));

            GUI.skin.label.fontSize = 12;

            if(screenPos.z > 35)
            {
                _text = "";
            }

            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height - 10, size.x, size.y), _text);
            GUI.color = restoreColor;
            UnityEditor.Handles.EndGUI();
        }

        // MISC
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}