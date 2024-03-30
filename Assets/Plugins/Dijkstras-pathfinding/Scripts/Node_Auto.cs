using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace DijkstrasPathfinding
{
/// <summary>
/// The Node.
/// </summary>
public class Node_Auto : Node
{
	List<Node> m_Connections_temp = new List<Node>();
	Tuple<Node, Node> m_tuple_nodes;
	Tuple<Node, Node> tuple_nodes
	{
		get
		{
			return m_tuple_nodes;
		}
		set
		{
			if(m_tuple_nodes!=null)
			{
				m_tuple_nodes.Item1.connections.Remove(this);
				m_tuple_nodes.Item2.connections.Remove(this);
			}
			m_tuple_nodes = value;
			m_tuple_nodes.Item1.connections.Add(this);
			m_tuple_nodes.Item2.connections.Add(this);		
		}
	}
    public Node FindClosestPoint(List<Node> m_Connections_temp)
    {
        if (m_Connections_temp.Count == 0)
        {
            Debug.LogError("点集为空");
            return null;
        }

        Node closestPoint = null;
        float minDistance = float.MaxValue;

        for (int i = 1; i < m_Connections_temp.Count; i++)
        {
			if(m_Connections_temp[i] == transform.GetComponent<Node>())continue;
            float distance = Vector3.Distance(m_Connections_temp[i].transform.position, transform.position);
            if (distance < minDistance)
            {
                closestPoint = m_Connections_temp[i];
                minDistance = distance;
            }
        }

        return closestPoint;
    }
	
	public Tuple<Node, Node> FindClosestPoints()
    {
        if (m_Connections_temp.Count < 2)
        {
            Debug.LogError("至少需要两个点才能找到最近的两个点");
            return null;
        }

        Node closestPoint1 = null;
        Node closestPoint2 = null;
        float minDistance1 = float.MaxValue;
        float minDistance2 = float.MaxValue;

        for (int i = 0; i < m_Connections_temp.Count; i++)
        {
            if (m_Connections_temp[i] != this) // 排除目标点自身
            {
                float distance = Vector3.Distance(m_Connections_temp[i].transform.position, transform.position);
                if (distance < minDistance1)
                {
                    closestPoint2 = closestPoint1;
                    minDistance2 = minDistance1;
                    closestPoint1 = m_Connections_temp[i];
                    minDistance1 = distance;
                }
                else if (distance < minDistance2)
                {
                    closestPoint2 = m_Connections_temp[i];
                    minDistance2 = distance;
                }
            }
        }

        return new Tuple<Node, Node>(closestPoint1, closestPoint2);
    }
	void Start()
	{
		m_Connections_temp = FindObjectsOfType<Node>().ToList();
	}
	void Update()
	{
		m_Connections.Clear();
		tuple_nodes = FindClosestPoints();
		m_Connections.Add(tuple_nodes.Item1);
		m_Connections.Add(tuple_nodes.Item2);
	}	
}
}