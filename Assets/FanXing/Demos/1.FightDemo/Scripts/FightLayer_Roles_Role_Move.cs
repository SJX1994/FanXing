using System.Collections;
using System.Collections.Generic;
using DijkstrasPathfinding;
using UnityEngine;
using DG.Tweening;
namespace FanXing.FightDemo
{

public class FightLayer_Roles_Role_Move : MonoBehaviour
{
	[SerializeField] LineRenderer lineRenderer;
	[SerializeField] float m_Speed = 0.01f;
	public Path m_Path = new Path ();
	protected Node m_Current;
    public bool m_IsMoving = false;
	private Node tempFrom;
	private Node tempTo;
	private bool stop = false;
    void Start()
    {

    }
    public void OnMovePreparation(DijkstrasPathfinding.Path path)
	{
		stop = false;
		m_Path = path;
		MovePreparation();
	}
	public bool OnMove(DijkstrasPathfinding.Graph graph, DijkstrasPathfinding.Path path, DijkstrasPathfinding.Node from, DijkstrasPathfinding.Node to)
	{
		// if(!TemporaryStorage.Dictionarys_role_Path.ContainsKey(this))
		// {   
		//     TemporaryStorage.Dictionarys_role_Path.Add(this,path);
		// }
		// m_Path = TemporaryStorage.Dictionarys_role_Path_FindPath(this);
		m_Path = path;
		Move(graph, from, to);
		return m_IsMoving;
	}
	
	public void OnMoveFinish()
	{
		// TemporaryStorage.Dictionarys_role_Path_FindAndRemoveValue(this);
		stop = true;
		StopCoroutine (nameof(FollowPath));
		m_Path = null;
		InitDisplay();
	}
    public void Follow ( Path path )
	{
		StopCoroutine (nameof(FollowPath));
		m_Path = path;
		transform.position = m_Path.nodes [ 0 ].transform.position;
		StartCoroutine (nameof(FollowPath));
	}
	public void InitDisplay()
    {
        lineRenderer.positionCount = 0;
		// if(tempFrom)Destroy(tempFrom.gameObject);
		// if(tempTo)Destroy(tempTo.gameObject);
    }
	private void MovePreparation()
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
	private void Move(Graph m_Graph, Node m_Start, Node m_End)
    {
		tempFrom = m_Start;
		tempTo = m_End;
		DOVirtual.DelayedCall(0.1f, () =>
		{
			m_Path = m_Graph.GetShortestPath( m_Start, m_End );
			Debug.Log ( m_Path );
			Follow ( m_Path );
		});
        
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
        TemporaryStorage.InvokeOnMoveFinish(this);
	}
    public void UpdateMoving ()
	{
        
		if ( m_Current != null && stop == false)
		{
			transform.position = Vector3.MoveTowards ( transform.position, m_Current.transform.position, m_Speed );
		}
	}
	
}
}