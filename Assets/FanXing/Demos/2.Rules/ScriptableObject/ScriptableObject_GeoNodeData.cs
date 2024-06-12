using UnityEngine;
using TMPro;
using QFramework.PointGame;
using System.Collections.Generic;
namespace FanXing.Demos.Rules
{
    [CreateAssetMenu(fileName = "ConstituativeRules_GeoNodeData_", menuName = "ConstituativeRules/GeoNodeData", order = 1)]

    public class ScriptableObject_GeoNodeData : ScriptableObject
    {
        [Header("节点属性")]
        public string NodeName = "Node";
        [HideInInspector]
        public Vector3 Position = Vector3.zero;
        [HideInInspector]
        public GameObject Self;
        [HideInInspector] 
        public List<ScriptableObject_GeoNodeData> Link_Nodes = new();
        [HideInInspector]
        public TextMeshPro textMeshPro;
        public void Init()
        {
            Link_Nodes = new();
        }

        public void FindClosestNode(float radius)
        {
            Collider[] hitColliders = Physics.OverlapSphere(Self.transform.position, radius);
            
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].gameObject != Self)
                {
                    // Debug.Log("Found object: " + hitColliders[i].gameObject.name);
                    // 在这里可以对找到的其他物体进行进一步处理
                    Link_Nodes.Add(hitColliders[i].gameObject.GetComponent<GeoNode>().data);
                }
            }
        }
    }
}