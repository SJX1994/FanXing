using UnityEngine;
using TMPro;
using QFramework;
using System.Collections.Generic;
namespace FanXing.Demos.Rules
{
    [CreateAssetMenu(fileName = "ConstituativeRules_UnitAttributes_", menuName = "ConstituativeRules/UnitAttributes", order = 1)]

    public class ScriptableObject_UnitAttributes : ScriptableObject
    {
        [Header("自身基础属性")]
        public Color MainColor = Color.white;
        public string UnitName = "Unit";
        public int Health = 100;
        private int health_Current;
        [HideInInspector]
        public int Health_Current{
            get{
                return health_Current;
            }
            set{
                health_Current = value;
                if (health_Current <= 0)
                {
                    health_Current = 0;
                    UnitStateCurrent = UnitState.Dead;
                }
            }
        }
        public int Attack = 10;
        public Vector2 Position = Vector2.zero;
        public TextMeshPro TextMeshPro;
        public enum UnitType
        {
            NotSet,
            Role,
            Enemy
        }
        public UnitType unitType = UnitType.NotSet;
        public enum UnitState
        {
            Idle,
            Moving,
            Attacking,
            Dead
        }
        public UnitState UnitStateCurrent = UnitState.Idle;
        public float ThreatLevel = 0.0f;
        public bool IsInherentTarget = false;
        public float DistancePriority = 1.0f;
        [Header("进度条")]
        public float ProgressDuration = 3.0f;
        public Sprite ProgressSprite;
        [HideInInspector]
        public bool CanUseAction = false;


        [Header("他者交流属性")]
        public GameObject Target;
        public float AutoAttackAttackDuration = 0.3f;
        public float AutoAttackInterval = 1.0f;
        public float AutoAttackRange = 1.0f;
        public float AutoPatrolRange = 5.0f;
        public List<GameObject> potentialTargets; // 潜在攻击目标列表
        public GameObject FindEnemy()
        {
            if(UnitStateCurrent == UnitState.Dead)return null;
            Collider[] colliders = Physics.OverlapSphere(new Vector3(Position.x,0.5f,Position.y), AutoAttackRange); // 在范围内进行球形检测

            GameObject closestUnit = null;
            float closestDistanceSqr = Mathf.Infinity;

            foreach (Collider collider in colliders)
            {
                // 检查是否挂载了目标脚本
                if (collider.gameObject.TryGetComponent(out UnitBehavior unit))
                {
                    Playable_Unit playable_Unit = unit as Playable_Unit;
                    // 检查是否是敌人
                    if ( playable_Unit.Attributes.unitType == unitType || playable_Unit.Attributes.UnitStateCurrent == UnitState.Dead)continue;
                    
                    float distanceSqr = (collider.transform.position - new Vector3(Position.x,0.5f,Position.y)).sqrMagnitude;
                    if (distanceSqr < closestDistanceSqr)
                    {
                        closestDistanceSqr = distanceSqr;
                        closestUnit = collider.gameObject;
                    }
                }
               
            }

            // 输出找到的最近单位
            if (closestUnit != null)
            {
                // Debug.Log(closestUnit.name);
                Target = closestUnit;
                return closestUnit;
            }else
            {
                // Debug.Log("No Enemy Found");
                return null;
            }
        }
        public GameObject FindBestTarget()
        {
            if(UnitStateCurrent == UnitState.Dead)return null;
            GameObject selectedTarget = null;
            float maxPriority = float.MinValue;

            foreach (GameObject target in potentialTargets)
            {
                Playable_Unit playable_Unit = target.GetComponent<Playable_Unit>();
                
                if(playable_Unit.Attributes.unitType == unitType || playable_Unit.Attributes.UnitStateCurrent == UnitState.Dead)continue;

                float distancePriority = 1 / Vector3.Distance(new Vector3(Position.x,0.5f,Position.y), target.transform.position);
                float threatPriority = playable_Unit.Attributes.ThreatLevel;
                float inherentPriority = playable_Unit.Attributes.IsInherentTarget ? 1.0f : 0.0f;

                float totalPriority = distancePriority + threatPriority + inherentPriority;

                if (totalPriority > maxPriority)
                {
                    maxPriority = totalPriority;
                    selectedTarget = target;
                }
            }

            return selectedTarget;
        }
        public void MoveTo(Vector2 targetPosition)
        {
            if(UnitStateCurrent == UnitState.Dead)return;
            UnitStateCurrent = UnitState.Moving;
            Position = Vector2.MoveTowards(Position, targetPosition, 0.1f);
            if (Vector2.Distance(Position, targetPosition) < AutoAttackRange - Random.Range(0.01f,0.1f))
            {
                UnitStateCurrent = UnitState.Idle;
            }
        }
    }
}