using System.Collections;
using System.Collections.Generic;
using DijkstrasPathfinding;
using UnityEngine;
using DG.Tweening;
namespace FanXing.FightDemo
{

public class RoleMove : MonoBehaviour
{
	[SerializeField] LineRenderer lineRenderer;
	[SerializeField] float m_Speed = 0.01f;
	protected Path m_Path = new Path ();
	protected Node m_Current;
    protected bool m_IsMoving = false;

    void Start()
    {
		
		TemporaryStorage.OnMovePreparation += (path) =>	
		{
			m_Path = path;
			MovePreparation();
		};
        TemporaryStorage.OnMove += (graph) =>
        {
			if(m_IsMoving)return;
            // m_Graph = graph;
            Move();
        };
        TemporaryStorage.OnMoveFinish += () =>
		{
            m_Path = null;
			InitDisplay();
		};
           
        
    }
    
    public void Follow ( Path path )
	{
		StopCoroutine (nameof(FollowPath));
		m_Path = path;
		transform.position = m_Path.nodes [ 0 ].transform.position;
		StartCoroutine (nameof(FollowPath));
	}
	private void InitDisplay()
    {
        lineRenderer.positionCount = 0;
    }
	void MovePreparation()
    {
        // m_End.transform.position = TemporaryStorage.path_end_position;
        // m_Start.transform.position = TemporaryStorage.path_start_position;
        // m_Path = m_Graph.GetShortestPath( m_Start, m_End );
        lineRenderer.positionCount = m_Path.nodes.Count;
        for ( int i = 0; i < m_Path.nodes.Count; i++ )
        {
            Vector3 pos = m_Path.nodes [ i ].transform.position;
            pos.y = 0.6f;
            lineRenderer.SetPosition( i, pos );
        }
    }
	void Move()
    {
        // m_Path = m_Graph.GetShortestPath ( m_Start, m_End );
		Follow ( m_Path );
    }
    IEnumerator FollowPath ()
	{
        m_IsMoving = true;
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
        m_IsMoving = false;
        TemporaryStorage.InvokeOnMoveFinish();
	}
    public void UpdateMoving ()
	{
        
		if ( m_Current != null)
		{
			transform.position = Vector3.MoveTowards ( transform.position, m_Current.transform.position, m_Speed );
		}
	}
	
}
}