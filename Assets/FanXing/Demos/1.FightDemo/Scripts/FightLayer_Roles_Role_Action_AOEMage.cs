using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace FanXing.FightDemo
{
    public class FightLayer_Roles_Role_Action_AOEMage : FightLayer_Roles_Role_Action
    {
        [SerializeField] Color selectedTargetColor , unselectedTargetColor;
        [SerializeField] LineRenderer lineRenderer_action;
        [SerializeField] int lineRenderer_segments = 50;
        [SerializeField] List<GameObject> targetList = new();
        bool updatePreparationDisplay = false;
        List<LineRenderer> tempLineRenderers = new();
        Dictionary<GameObject,bool> collections = new();
        Dictionary<GameObject,bool> Collections
        {
            get
            {
                return collections;
            }
            set
            {
                Dictionary<GameObject, bool> original = collections;
                Dictionary<GameObject, bool> modified = value;

                Dictionary<GameObject, bool> added, removed;
                DictionaryDiff.FindDiff(original, modified, out added, out removed);
                // 列出新增的元素
                foreach (var item in added)
                {
                    item.Key.GetComponent<FightLayer_Roles_Role_BeenSelected>().OnSelected();
                }

                // 列出被删除的元素
                foreach (var item in removed)
                {
                    item.Key.GetComponent<FightLayer_Roles_Role_BeenSelected>().OnCanceled();
                }
                collections = value;
            }
        }
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
            
        }
        List<GameObject> CreateSkillRange()
        {
            targetList.Clear();
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
                    CreateCircleShape_Display();
                    targetList = CreateCircleShape_Logic();
                    updatePreparationDisplay = true;
                    break;
                case ScriptableObject_Action.Action.SquareShape:
                    // targetList = CreateSquareShape();
                    break;
                case ScriptableObject_Action.Action.WholeMap:
                    // targetList = CreateWholeMap();
                    break;
            }
            return targetList;
        }
        void InitDisplay()
        {
            lineRenderer_action.positionCount = 0;
            lineRenderer_action.sharedMaterial.SetColor("_EmissionColor", unselectedTargetColor);
            updatePreparationDisplay = false;
            tempLineRenderers.ForEach(lr=>Destroy(lr.gameObject));
            tempLineRenderers.Clear();
            Collections.Clear();
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
            
            updatePreparationDisplay = true;
            LineRenderer lineRenderer_temp_for_Ranger = Instantiate(lineRenderer_action,transform);
            lineRenderer_temp_for_Ranger.positionCount = lineRenderer_segments+1;
            lineRenderer_temp_for_Ranger.loop = true;
            tempLineRenderers.Add(lineRenderer_temp_for_Ranger);
            LineRenderer lineRenderer_temp_for_Path = Instantiate(lineRenderer_action,transform);
            lineRenderer_temp_for_Path.loop = false;
            tempLineRenderers.Add(lineRenderer_temp_for_Path);
            CreateSelfCircleShape_Display();
        }
        List<GameObject> CreateCircleShape_Logic()
        {
            List<GameObject> targetList = new();
            
            return targetList;
        }
        void UpdateCircleShape_Logic(Vector3 circleCenter)
        {
            if(!updatePreparationDisplay)return;
            Vector3 centerOfCircle = circleCenter;
            Collider[] colliders = Physics.OverlapSphere(centerOfCircle, scriptableObject_Action.Range);
            Collections = colliders.ToDictionary(collider => collider.gameObject, playedAnimation => false);
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
            if(Collections.Count > 0)
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
                positions[i] = CalculateParabolaPoint(pointA, pointB, pointC, t);
            }

            lineRenderer_path.positionCount = numberOfPoints;
            lineRenderer_path.SetPositions(positions);
        }
        private Vector3 CalculateParabolaPoint(Vector3 pointA, Vector3 pointB, Vector3 pointC, float t)
        {
            float oneMinusT = 1f - t;
            return oneMinusT * oneMinusT * pointA + 2f * oneMinusT * t * pointC + t * t * pointB;
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
    }
}

