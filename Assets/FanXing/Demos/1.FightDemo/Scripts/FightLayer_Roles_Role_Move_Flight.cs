using System.Collections;
using System.Collections.Generic;
using DijkstrasPathfinding;
using UnityEngine;
using DG.Tweening;
using System.Linq;
namespace FanXing.FightDemo
{
public class FightLayer_Roles_Role_Move_Flight : FightLayer_Roles_Role_Move
{
	public Vector3 m_From_Flight;
	public Vector3 m_Destination_Flight;

    public override void OnMovePreparation(DijkstrasPathfinding.Path path)
	{
		base.OnMovePreparation(path);
		MovePreparation_Flight();
	}
	public override bool OnMove(FightLayer_Roles_Role_Move who,DijkstrasPathfinding.Graph graph, DijkstrasPathfinding.Path path, DijkstrasPathfinding.Node from, DijkstrasPathfinding.Node to)
	{
		m_IsMoving = true;
		return m_IsMoving;
	}
	
	public override void OnMoveFinish()
	{
		base.OnMoveFinish();
	}
	
	public override void InitDisplay()
    {
		lineRenderer.gameObject.SetActive(false);
        lineRenderer.positionCount = 0;
		m_From_Flight = Vector3.zero;
		m_Destination_Flight = Vector3.zero;
    }
	private void MovePreparation_Flight()
	{
		lineRenderer.positionCount = 2;
		Vector3 start = transform.position;
		start.y = 0.6f;
		Vector3 end = TemporaryStorage.BuoyPosition;
		end.y = 0.6f;
		
		lineRenderer.SetPosition(0, start);
		lineRenderer.SetPosition(1, end);
		timer += Time.deltaTime;
		if (timer < 0.2f)return;
		lineRenderer.gameObject.SetActive(true);
	}
	private void LineRenderUpdate()
	{
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, m_Destination_Flight);
	}
	
	
    
	public override void UpdateMoving()
	{
		
		if(stop)return;
		if (!m_IsMoving)return;
		
        // 计算物体需要移动的方向
        Vector3 direction = m_Destination_Flight - m_From_Flight;

        // 移动物体
        transform.position = Vector3.MoveTowards(transform.position,m_Destination_Flight, m_Speed * Time.deltaTime);
		LineRenderUpdate();
		// 旋转朝向
		Vector3 movement = m_Destination_Flight - transform.position;
		Vector3 currentPosition = transform.position;
		Vector3 newPosition = currentPosition + movement * m_Speed * Time.deltaTime;
		transform.LookAt(newPosition);
		transform.localRotation = Quaternion.Euler(0,transform.localRotation.eulerAngles.y,0);

        // 检测是否到达目标位置
        if (Vector3.Distance(transform.position, m_Destination_Flight) < 0.1f)
        {
            m_IsMoving = false;
			TemporaryStorage.InvokeOnMoveFinish(this);
        }
        
        
	}

}
}