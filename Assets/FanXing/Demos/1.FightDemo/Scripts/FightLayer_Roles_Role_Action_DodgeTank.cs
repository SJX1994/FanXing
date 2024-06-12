using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FanXing.FightDemo
{
    public class FightLayer_Roles_Role_Action_DodgeTank : FightLayer_Roles_Role_Action
    {
        [SerializeField] Color selectedTargetColor , unselectedTargetColor;
        [SerializeField] LineRenderer lineRenderer_action;
        [SerializeField] int lineRenderer_segments = 50;
        [SerializeField] GameObject squareArea;
        [SerializeField] Transform skillEndPoint;
        bool updatePreparationDisplay = false;
        List<LineRenderer> tempLineRenderers = new();
        Dictionary<GameObject,bool> targetCollections = new();
        Dictionary<GameObject,bool> TargetCollections
        {
            get
            {
                return targetCollections;
            }
            set
            {
                Dictionary<GameObject, bool> original = targetCollections;
                Dictionary<GameObject, bool> modified = value;

                Dictionary<GameObject, bool> added, removed;
                DictionaryDiff.FindDiff(original, modified, out added, out removed);
                // 列出新增的元素
                foreach (var item in added)
                {
                    if(!item.Key.TryGetComponent<FightLayer_Roles_Role_BeenSelected>(out var b))continue;
                    b.OnSelect = true;
                }

                // 列出被删除的元素
                foreach (var item in removed)
                {
                    if(!item.Key.TryGetComponent<FightLayer_Roles_Role_BeenSelected>(out var b))continue;
                    b.OnSelect = false;

                }
                targetCollections = value;
            }
        }
        float rectangleWidth; // 矩形的宽度
        BoxCollider squearBoxCollider;
        // 技能取消
        protected override void ActionCanceled()
        {
            InitDisplay();
        }
        // 技能预示
        protected override void ActionPreparation()
        {
            CreateSkillRange();
            
        }
        // 技能释放
        protected override void ActionRelease()
        {
            base.ActionRelease();
            for(int i = scriptableObject_Action.MissileCount; i>0; i--)
            {
                Vector3 targetPosition = skillEndPoint.position;
                TemporaryStorage.InvokeOnGenerateMissile(scriptableObject_Action,transform.position,targetPosition);
            }
            TemporaryStorage.InvokeOnCoolDown(this,scriptableObject_Action.cooldown);
            InitDisplay();
        }
        void CreateSkillRange()
        {
            
            switch(scriptableObject_Action.action)
            {
                case ScriptableObject_Action.Action.FanShape:
                    // targetList = CreateFanShape();
                    break;
                case ScriptableObject_Action.Action.LineShape:
                    // targetList = CreateLineShape();
                    break;
                case ScriptableObject_Action.Action.SingleTarget:
                    // targetList = CreateSingleTarget();
                    break;
                case ScriptableObject_Action.Action.PointToPoint:
                    // targetList = CreatePointToPoint();
                    break;
                case ScriptableObject_Action.Action.CircleShape:
                    break;
                case ScriptableObject_Action.Action.SquareShape:
                    CreateSquareShape_Logic();
                    CreateSquareShape_Display();
                    break;
                case ScriptableObject_Action.Action.WholeMap:
                    // targetList = CreateWholeMap();
                    break;
            }
           
        }
        void InitDisplay()
        {
            lineRenderer_action.positionCount = 0;
            lineRenderer_action.sharedMaterial.SetColor("_EmissionColor", unselectedTargetColor);
            updatePreparationDisplay = false;
            tempLineRenderers.ForEach(lr=>Destroy(lr.gameObject));
            tempLineRenderers.Clear();
            TargetCollections.Clear();
        }
        void CreateSquareShape_Logic()
        {
            updatePreparationDisplay = true;
            rectangleWidth = scriptableObject_Action.Range;
            if(!squearBoxCollider)
            {
                squearBoxCollider = squareArea.GetComponentInChildren<BoxCollider>();
            }
            squearBoxCollider.size = new Vector3(squearBoxCollider.size.x,squearBoxCollider.size.y,rectangleWidth);
        }
        void CreateSquareShape_Display()
        {
            LineRenderer lineRenderer_temp_for_Self = Instantiate(lineRenderer_action,transform);
            lineRenderer_temp_for_Self.positionCount = lineRenderer_segments+1;
            lineRenderer_temp_for_Self.loop = true;
            tempLineRenderers.Add(lineRenderer_temp_for_Self); // 0
            CreateSelfCircleShape_Display(lineRenderer_temp_for_Self);

            LineRenderer lineRenderer_temp_for_Square = Instantiate(lineRenderer_action,transform);
            lineRenderer_temp_for_Square.positionCount = 5;
            lineRenderer_temp_for_Square.useWorldSpace = true;
            tempLineRenderers.Add(lineRenderer_temp_for_Square); // 1
        }
        void CreateSelfCircleShape_Display(LineRenderer lineRenderer_temp_for_Self)
        {
            float x;
            float z;
            float angle = 20f;
            for (int i = 0; i < (lineRenderer_segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * 2;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * 2;
                lineRenderer_temp_for_Self.SetPosition(i, new Vector3(x, 1.6f, z) + transform.position);
                angle += 360f / lineRenderer_segments;
            }
        }
        void DrawParabola(LineRenderer lineRenderer_path, Vector3 pointA, Vector3 pointB,int numberOfPoints)
        {
            // 计算顶点C
            Vector3 pointC = (pointA + pointB) / 2 + Vector3.up * Mathf.Abs(pointA.y - pointB.y)*16f;

            Vector3[] positions = new Vector3[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                float t = (float)i / (numberOfPoints - 1);
                positions[i] = Shap.CalculateParabolaPoint(pointA, pointB, pointC, t);
            }

            lineRenderer_path.positionCount = numberOfPoints;
            lineRenderer_path.SetPositions(positions);
        }
        
        void Update()
        {
            if(updatePreparationDisplay)
            {
                switch(scriptableObject_Action.action)
                {
                    case ScriptableObject_Action.Action.FanShape:
                        // UpdateFanShape();
                        break;
                    case ScriptableObject_Action.Action.LineShape:
                        // UpdateLineShape();
                        break;
                    case ScriptableObject_Action.Action.SingleTarget:
                        // UpdateSingleTarget();
                        break;
                    case ScriptableObject_Action.Action.PointToPoint:
                        // UpdatePointToPoint();
                        break;
                    case ScriptableObject_Action.Action.CircleShape:
                        break;
                    case ScriptableObject_Action.Action.SquareShape:
                        UpdateSquareShape();
                        break;
                    case ScriptableObject_Action.Action.WholeMap:
                        // UpdateWholeMap();
                        break;
                }
            }
        }
        void UpdateSquareShape()
        {
            
            UpdateSquareShape_Display(tempLineRenderers[1]);
            UpdateSquareShape_Logic(tempLineRenderers[1]);
            // DrawParabola();
        }
        void UpdateSquareShape_Logic(LineRenderer lineRenderer)
        {
            // 计算需要对准的方向
            Vector3 direction = (TemporaryStorage.BuoyPosition - transform.position).normalized;

            // 将方向中的X和Z分量赋值给新的向量
            Vector3 targetDirection = new Vector3(direction.x, 0f, direction.z).normalized;

            // 计算需要旋转的角度
            Quaternion targetRotation = targetDirection ==Vector3.zero? Quaternion.identity :Quaternion.LookRotation(targetDirection, Vector3.up);

            // 平滑过渡旋转
            squareArea.transform.rotation = Quaternion.Lerp(squareArea.transform.rotation, targetRotation, Time.deltaTime * 9.2f);

            // 碰撞计算
            Collider[] colliders = Physics.OverlapBox(squearBoxCollider.bounds.center, squearBoxCollider.bounds.extents, squearBoxCollider.transform.rotation);
            colliders = colliders.Where(
                collider => 
                collider.gameObject.transform.parent.gameObject != gameObject
                && 
                collider.gameObject != squearBoxCollider.gameObject 
                ).ToArray();
         
            TargetCollections = colliders.ToDictionary(collider => collider.gameObject, playedAnimation => false);
        }
        void UpdateSquareShape_Display(LineRenderer lineRenderer)
        {
            

            Vector3 bottomLeft = squearBoxCollider.transform.TransformPoint(squearBoxCollider.center + new Vector3(-squearBoxCollider.size.x / 2, -squearBoxCollider.size.y / 2, -squearBoxCollider.size.z / 2));
            Vector3 bottomRight = squearBoxCollider.transform.TransformPoint(squearBoxCollider.center + new Vector3(squearBoxCollider.size.x / 2, -squearBoxCollider.size.y / 2, -squearBoxCollider.size.z / 2));
            Vector3 topLeft = squearBoxCollider.transform.TransformPoint(squearBoxCollider.center + new Vector3(-squearBoxCollider.size.x / 2, -squearBoxCollider.size.y / 2, squearBoxCollider.size.z / 2));
            Vector3 topRight = squearBoxCollider.transform.TransformPoint(squearBoxCollider.center + new Vector3(squearBoxCollider.size.x / 2, -squearBoxCollider.size.y / 2, squearBoxCollider.size.z / 2));
            lineRenderer.SetPosition(0, bottomLeft);
            lineRenderer.SetPosition(1, bottomRight);
            lineRenderer.SetPosition(2, topRight);
            lineRenderer.SetPosition(3, topLeft);
            lineRenderer.SetPosition(4, bottomLeft);
            
            
            if(TargetCollections.Count > 0)
            {
                lineRenderer.sharedMaterial.SetColor("_EmissionColor", selectedTargetColor);
                lineRenderer.sharedMaterial.SetFloat("_Alpha", selectedTargetColor.a);
            }else
            {
                lineRenderer.sharedMaterial.SetColor("_EmissionColor", unselectedTargetColor);
                lineRenderer.sharedMaterial.SetFloat("_Alpha", unselectedTargetColor.a);
            }
        }
    }
}

