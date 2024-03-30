using System.Collections;
using System.Collections.Generic;
using DijkstrasPathfinding;
using UnityEngine;
namespace FanXing.FightDemo
{

public class RoleMove : MonoBehaviour
{
    [SerializeField]
	protected Graph m_Graph;
	[SerializeField]
	protected Node m_Start;
	[SerializeField]
	protected Node m_End;
	[SerializeField]
	protected float m_Speed = 0.01f;
	protected Path m_Path = new Path ();
	protected Node m_Current;

    void Start()
    {
        TemporaryStorage.OnMove += (graph,path, startNode, endNode) =>
        {
            m_Graph = graph;
            m_Path = path;
            m_Start = startNode;
            m_End = endNode;
            Move();
        }; 
           
        
    }
    void Move()
    {
        m_Path = m_Graph.GetShortestPath ( m_Start, m_End );
		Follow ( m_Path );
    }
    public void Follow ( Path path )
	{
		StopCoroutine (nameof(FollowPath));
		m_Path = path;
		transform.position = m_Path.nodes [ 0 ].transform.position;
		StartCoroutine (nameof(FollowPath));
	}
    IEnumerator FollowPath ()
	{
		var e = m_Path.nodes.GetEnumerator ();
		while ( e.MoveNext () )
		{
			m_Current = e.Current;
			
			// Wait until we reach the current target node and then go to next node
			yield return new WaitUntil ( () =>
			{
				return transform.position == m_Current.transform.position;
			} );
		}
		m_Current = null;
        TemporaryStorage.InvokeOnMoveFinish();
	}
    void Update ()
	{
        
		if ( m_Current != null && TemporaryStorage.buoyState == OperateBuoy.State.MoveExecute)
		{
			transform.position = Vector3.MoveTowards ( transform.position, m_Current.transform.position, m_Speed );
		}
	}
}
}