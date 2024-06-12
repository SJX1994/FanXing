using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
namespace FanXing.Demos.Rules
{
    public class Playable_Unit : UnitBehavior
    {
        [SerializeField]
        bool CanAttack = true;
        public ScriptableObject_UnitAttributes Attributes
        {
            get
            {
                return attributes;
            }
        }
        private Vector3 previousPosition
        {
            get
            {
                return attributes.Position;
            }
            set
            {
                Vector2 pos2D = new Vector2(value.x, value.z);
                attributes.Position = pos2D;
            }
        }
        [SerializeField]
        LineRenderer lineRenderer;
        private bool isAttacking = false;
        public bool IsAttacking
        {
            get
            {
                return isAttacking;
            }
            set
            {
                if (!TemporaryStorage.GameTimeIsRunning)
                {
                    lineRenderer.positionCount = 0;
                    return;
                }
                // if (value == isAttacking)return;
                isAttacking = value;
                if (isAttacking)
                {
                    
                    lineRenderer.positionCount = 2; // 设置线段端点数量为2
                    lineRenderer.startColor = attributes.MainColor; // 设置起始颜色
                    lineRenderer.endColor = attributes.MainColor; // 设置结束颜色
                    lineRenderer.startWidth = 0.0f; // 设置起始宽度
                    lineRenderer.endWidth = 0.3f; // 设置结束宽度
                    lineRenderer.SetPosition(0, new Vector3(attributes.Position.x,0.5f,attributes.Position.y) ); // 设置起点位置
                    lineRenderer.SetPosition(1, attributes.Target.transform.position); // 设置终点位置
                    attributes.Target.GetComponent<Playable_Unit>().Attributes.Health_Current -= attributes.Attack; // 攻击目标
                }
                else
                {
                    lineRenderer.positionCount = 0;
                    
                }
            }
        }
        void Start()
        {
            Init();
            DisplayAttributes(); 
            if(!CanAttack)return;
            StartCoroutine(AttackLoop());
        
        }
        void Update()
        {
            if (previousPosition != transform.position)
            {
                previousPosition = transform.position;
            }
            DisplayAttributes();
        }
        void Init()
        {
            attributes = Instantiate(attributes);
            attributes.TextMeshPro = textMeshPro;
            attributes.Health_Current = attributes.Health;
            previousPosition = transform.position;
            isAttacking = false;
            Events.OnWhoProssesBar_Action += WhoAction;
        }
        void WhoAction(ScriptableObject_UnitAttributes attributeIn)
        {
            if(attributeIn.UnitName != attributes.UnitName)return;
            if(attributes.UnitStateCurrent == ScriptableObject_UnitAttributes.UnitState.Dead)return;
            Events.InvokeRoleDecisionInfo( gameObject , true, transform.position);
        }
        private IEnumerator AttackLoop()
        {
            
            while (true)
            {
                if(TemporaryStorage.GameTimeIsRunning)
                {
                    IsAttacking = false;
                    yield return new WaitForSeconds(0.1f);
                }
                if (!IsAttacking && attributes.FindEnemy() != null)
                {
                    IsAttacking = true;
                    // 执行攻击动作
                    
                    yield return new WaitForSeconds(attributes.AutoAttackAttackDuration); // 模拟攻击执行时间
                    IsAttacking = false;
                }
                yield return new WaitForSeconds(attributes.AutoAttackInterval);// 模拟攻击间隔
            }
        }
        public override void DisplayAttributes()
        {
            attributes.TextMeshPro.text = attributes.UnitName + "\n HP:" + attributes.Health_Current + "\n Atk:" + attributes.Attack + "\n Pos:" + attributes.Position;
        }

    }
}