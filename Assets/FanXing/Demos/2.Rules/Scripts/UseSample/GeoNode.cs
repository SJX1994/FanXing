using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace FanXing.Demos.Rules
{
    public class GeoNode : MonoBehaviour
    {
        [SerializeField]
        float link_Radius = 5f;
        [SerializeField]
        LineRenderer lineRenderer;
        [SerializeField]
        TextMeshPro textMeshPro;
        public ScriptableObject_GeoNodeData data;
        public void Init()
        {
            data = Instantiate(data);
            data.Init();
            data.Self = gameObject;
            data.textMeshPro = textMeshPro;
            data.FindClosestNode(link_Radius);
            foreach (var item in data.Link_Nodes)
            {
                DrawLine(item.Self.transform.position);
            }
        }
        void DrawLine(Vector3 targetPos)
        {
            LineRenderer lr = Instantiate(lineRenderer, transform);
            lr.positionCount = 2;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, targetPos);
        }
    }
}
