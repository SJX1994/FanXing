using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DijkstrasPathfinding;
using System;
using DG.Tweening;

namespace FanXing.FightDemo
{
public class FightLayer_Pathfinding : MonoBehaviour
{
    [SerializeField] Node From;
    [SerializeField] Node To;
    [SerializeField] Graph graph;
    
    Path m_Path = new Path ();
    public enum Command
    {
        Null,
        MovePreparation,
       
    }
    public enum State
    {
        Idle,
        MovePreparation,
     
    }
    public State currentState;
    void Start()
    {
        currentState = State.Idle;
        TemporaryStorage.OnConfirmKeyPressed += () =>
        {
            DOVirtual.DelayedCall(0.1f, () =>OnConfirmKeyPressed());
        };
       
    }
    public void ExecuteCommand(Command command)
    {
        
        switch (command)
        {
            case Command.Null:
                currentState = State.Idle;
                break;
            case Command.MovePreparation:
                currentState = State.MovePreparation;
                break;
            default:
                Debug.LogError("Invalid command");
                break;
        }
    }

    

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                
                break;
            case State.MovePreparation:
                Pathfinding_MovePreparation_Display();
                break;
            default:
                break;
        }
    }
    void Pathfinding_MovePreparation_Display()
    {
        To.transform.position = TemporaryStorage.Path_end_position;
        From.transform.position = TemporaryStorage.Path_start_position;
        m_Path = graph.GetShortestPath( From, To );
        FightLayer_Roles_Role_Move roleMove = TemporaryStorage.BuoySelectedObject.GetComponent<FightLayer_Roles_Role>().roleMove;
        TemporaryStorage.InvokeOnMovePreparation(roleMove,m_Path);
    }
    int i = 0;
    void OnConfirmKeyPressed()
    {
        switch (TemporaryStorage.BuoyState)
        {
            case OperateLayer_Buoy.State.MoveExecute:
                ExecuteCommand(Command.Null);
                FightLayer_Roles_Role_Move roleMove = TemporaryStorage.BuoySelectedObject.GetComponent<FightLayer_Roles_Role>().roleMove;
                i++;
                // Node tempFrom = From;
                // Node tempTo = To;
                Node tempFrom = Instantiate(From,transform);
                tempFrom.name = "From"+i;
                Node tempTo = Instantiate(To,transform);
                tempTo.name = "To"+i;
                m_Path = graph.GetShortestPath( tempFrom, tempTo );
                TemporaryStorage.InvokeOnMove(roleMove,graph,m_Path,tempFrom,tempTo);
                break;
            default:
                break;
        }
    }
}
}