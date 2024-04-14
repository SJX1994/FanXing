using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace FanXing.FightDemo
{
    public class FightLayer_Roles_Role_Action_AOEMage : FightLayer_Roles_Role_Action
    {
        [SerializeField] Color selectedTargetColor , unselectedTargetColor;
        [SerializeField] LineRenderer lineRenderer_action;
        [SerializeField] int lineRenderer_segments = 50;
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
        Vector3 relaseCenterOfCircle = Vector3.zero;
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
                Vector3 targetPosition = RandomPointInCircle(relaseCenterOfCircle,scriptableObject_Action.Range);
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
                    CreateCircleShape_Logic();
                    CreateCircleShape_Display();
                    break;
                case ScriptableObject_Action.Action.SquareShape:
                    // targetList = CreateSquareShape();
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
            foreach (var item in TargetCollections)
            {
                if(!item.Key.TryGetComponent<FightLayer_Roles_Role_BeenSelected>(out var b))continue;
                b.OnSelect = false;
            }
            TargetCollections.Clear();
        }
        void CreateSelfCircleShape_Display()
        {
            
            LineRenderer lineRenderer_temp_for_Self = Instantiate(lineRenderer_action,transform);
            lineRenderer_temp_for_Self.positionCount = lineRenderer_segments+1;
            lineRenderer_temp_for_Self.loop = true;
            tempLineRenderers.Add(lineRenderer_temp_for_Self);

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
        void CreateCircleShape_Display()
        {
            LineRenderer lineRenderer_temp_for_Ranger = Instantiate(lineRenderer_action,transform);
            lineRenderer_temp_for_Ranger.positionCount = lineRenderer_segments+1;
            lineRenderer_temp_for_Ranger.loop = true;
            tempLineRenderers.Add(lineRenderer_temp_for_Ranger);
            LineRenderer lineRenderer_temp_for_Path = Instantiate(lineRenderer_action,transform);
            lineRenderer_temp_for_Path.loop = false;
            tempLineRenderers.Add(lineRenderer_temp_for_Path);
            CreateSelfCircleShape_Display();
        }
        void CreateCircleShape_Logic()
        {
            updatePreparationDisplay = true;
        }
        void UpdateCircleShape_Logic(Vector3 circleCenter)
        {
            if(!updatePreparationDisplay)return;
            Vector3 centerOfCircle = circleCenter;
            relaseCenterOfCircle = circleCenter;
            Collider[] colliders = Physics.OverlapSphere(centerOfCircle, scriptableObject_Action.Range);
            TargetCollections = colliders.ToDictionary(collider => collider.gameObject, playedAnimation => false);
        }
        void UpdateCircleShape_Display(LineRenderer lineRenderer,Vector3 circleCenter,int lineRenderer_segments = 50)
        {
            if(!updatePreparationDisplay)return;
            float x;
            float z;
            float angle = 20f;
            for (int i = 0; i < (lineRenderer_segments + 1); i++)
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * scriptableObject_Action.Range;
                z = Mathf.Cos(Mathf.Deg2Rad * angle) * scriptableObject_Action.Range;
                lineRenderer.SetPosition(i, new Vector3(x+circleCenter.x,circleCenter.y, z+circleCenter.z));
                angle += 360f / lineRenderer_segments;
            }
            // Debug.Log(Collections.Count);
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
                        Vector3 circleCenter = new Vector3(TemporaryStorage.BuoyPosition.x,0.6f,TemporaryStorage.BuoyPosition.z);
                        UpdateCircleShape_Logic(circleCenter);
                        UpdateCircleShape_Display(tempLineRenderers[0],circleCenter,lineRenderer_segments);
                        DrawParabola(tempLineRenderers[1],transform.position,circleCenter,10);
                        break;
                    case ScriptableObject_Action.Action.SquareShape:
                        // UpdateSquareShape();
                        break;
                    case ScriptableObject_Action.Action.WholeMap:
                        // UpdateWholeMap();
                        break;
                }
            }
            
        }
        public Vector3 RandomPointInCircle(Vector3 center, float radius)
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2); // 随机角度
            float distance = Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f)) * radius; // 随机距离

            float x = center.x + distance * Mathf.Cos(angle);
            float z = center.z + distance * Mathf.Sin(angle);
            float y = center.y; // 在XZ平面上生成，所以y坐标保持不变

            return new Vector3(x, y, z);
        }
    }
}

