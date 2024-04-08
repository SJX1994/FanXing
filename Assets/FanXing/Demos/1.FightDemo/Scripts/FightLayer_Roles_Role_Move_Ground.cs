using System.Collections;
using System.Collections.Generic;
using DijkstrasPathfinding;
using UnityEngine;
using DG.Tweening;
using System.Linq;
namespace FanXing.FightDemo
{
public class FightLayer_Roles_Role_Move_Ground : FightLayer_Roles_Role_Move
{
	public Path m_Path = new Path ();
	protected Node m_Current;
	private Node tempFrom;
	private Node tempTo;
	List<Node> tempNodesFormMovingLineRender = new List<Node>();
    void Start()
    {

    }
    public override void OnMovePreparation(DijkstrasPathfinding.Path path)
	{
		base.OnMovePreparation(path);
		m_Path = path;
		MovePreparation_Ground();
	}
	public override bool OnMove(FightLayer_Roles_Role_Move who,DijkstrasPathfinding.Graph graph, DijkstrasPathfinding.Path path, DijkstrasPathfinding.Node from, DijkstrasPathfinding.Node to)
	{
		// base.OnMove(who,graph,path,from,to);
		if(!who)return false;
		m_Path = path;
		Move_Ground(graph, from, to);
		return m_IsMoving;
	}
	
	public override void OnMoveFinish()
	{
		base.OnMoveFinish();
		timer = 0f;
		StopCoroutine (nameof(FollowPath_Ground));
		m_Path = null;
		InitDisplay();
	}
    public void Follow ( Path path )
	{
		StopCoroutine (nameof(FollowPath_Ground));
		m_Path = path;
		tempNodesFormMovingLineRender = m_Path.nodes.ToList();
		transform.position = m_Path.nodes [ 0 ].transform.position;
		StartCoroutine (nameof(FollowPath_Ground));
	}
	
	public override void InitDisplay()
    {
		lineRenderer.gameObject.SetActive(false);

        lineRenderer.positionCount = 0;
		if(tempFrom)
		{
			Node_Auto node_Auto = tempFrom as Node_Auto;
			node_Auto.RemoveAutoNode();
		}
		if(tempTo)
		{
			Node_Auto node_Auto = tempTo as Node_Auto;
			node_Auto.RemoveAutoNode();
		}
    }
	private void MovePreparation_Ground()
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
		timer += Time.deltaTime;
        if (timer < 0.2f)return;
		lineRenderer.gameObject.SetActive(true);
    }
	private void LineRenderUpdate()
	{
		if (tempNodesFormMovingLineRender == null || tempNodesFormMovingLineRender.Count == 0 || !lineRenderer)return;
		if(tempNodesFormMovingLineRender.Contains(m_Current) && Vector3.Distance(transform.position,m_Current.transform.position)<0.05f)tempNodesFormMovingLineRender.Remove(m_Current);
		lineRenderer.positionCount = tempNodesFormMovingLineRender.Count;
		for (int i = 0; i < tempNodesFormMovingLineRender.Count; i++)
		{
			if(tempNodesFormMovingLineRender[i] == null)continue;
			Vector3 pos = tempNodesFormMovingLineRender[i].transform.position;
			pos.y = 0.6f;
			lineRenderer.SetPosition(i, pos);
		}
		if(lineRenderer.positionCount == 0)return;
		lineRenderer.SetPosition(0, transform.position);
		
	}
	private void Move_Ground(Graph m_Graph, Node m_Start, Node m_End)
    {
		lineRenderer.gameObject.SetActive(true);

		tempFrom = m_Start;
		tempTo = m_End;
		
		DOVirtual.DelayedCall(0.1f, () =>
		{
			stop = false;
			m_Path = m_Graph.GetShortestPath( m_Start, m_End );
			Follow ( m_Path );
		});
        
    }
    IEnumerator FollowPath_Ground ()
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
    public override void UpdateMoving ()
	{
        
		if ( m_Current == null || stop)return;
		LineRenderUpdate();
		// transform.position = Vector3.MoveTowards ( transform.position, m_Current.transform.position, m_Speed );
		Vector3 movement = m_Current.transform.position - transform.position;
		Vector3 currentPosition = transform.position;
		Vector3 newPosition = currentPosition + movement * m_Speed * Time.deltaTime;
		transform.LookAt(newPosition);
		transform.localRotation = Quaternion.Euler(0,transform.localRotation.eulerAngles.y,0);
		transform.position = Vector3.MoveTowards ( transform.position, m_Current.transform.position, m_Speed );
		
	}
}
}