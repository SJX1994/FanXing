using System.Collections;
using System.Collections.Generic;
using DijkstrasPathfinding;
using UnityEngine;
using DG.Tweening;
using System.Linq;
namespace FanXing.FightDemo
{
public class FightLayer_Roles_Role_Move : MonoBehaviour
{
	
	[SerializeField]protected LineRenderer lineRenderer;
	public LineRenderer Role_Move_LineRenderer
	{
		get
		{
			return lineRenderer;
		}
		set
		{
			lineRenderer = value;
		}
	}
	[SerializeField]protected float m_Speed = 0.01f;
	public bool m_IsMoving = false;
	protected bool stop = false;
	protected float timer = 0f;
    void Start()
    {

    }
	public virtual void InitDisplay()
	{
		
	}
    public virtual void OnMovePreparation(DijkstrasPathfinding.Path path)
	{
		stop = false;
	}
	public virtual bool OnMove(FightLayer_Roles_Role_Move who,DijkstrasPathfinding.Graph graph, DijkstrasPathfinding.Path path, DijkstrasPathfinding.Node from, DijkstrasPathfinding.Node to)
	{
		return m_IsMoving;
	}
	
	public virtual void OnMoveFinish()
	{
		stop = true;
		
	}
	public virtual void UpdateMoving ()
	{

	}
}
}