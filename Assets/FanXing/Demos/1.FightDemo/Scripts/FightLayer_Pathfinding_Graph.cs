using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DijkstrasPathfinding;

namespace FanXing.FightDemo
{

    public class FightLayer_Pathfinding_Graph : MonoBehaviour
    {
        [SerializeField] Graph m_Graph;
        void FixedUpdate()
        {
            m_Graph.nodes.Clear ();
            foreach ( Transform child in m_Graph.transform )
            {
                Node node = child.GetComponent<Node> ();
                if ( node != null )
                {
                    m_Graph.nodes.Add ( node );
                }
            }
            if ( m_Graph == null )return;
            for ( int i = 0; i < m_Graph.nodes.Count; i++ )
            {
                if(m_Graph.nodes[i] == null)
                {
                    m_Graph.nodes.RemoveAt(i);
                    continue;
                }
                Node node = m_Graph.nodes [ i ];
                for ( int j = 0; j < node.connections.Count; j++ )
                {
                    Node connection = node.connections [ j ];
                    if ( connection == null )
                    {
                        continue;
                    }
                    float distance = Vector3.Distance ( node.transform.position, connection.transform.position );
                    Vector3 diff = connection.transform.position - node.transform.position;
                  
                }
            }
        }
    }
}