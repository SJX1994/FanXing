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
        MoveFinish,
    }
    public enum State
    {
        Idle,
        MovePreparation,
        MoveFinish
    }
    public State currentState;
    void Start()
    {
        currentState = State.Idle;
        TemporaryStorage.OnConfirmKeyPressed += () =>
        {
            DOVirtual.DelayedCall(0.2f, () =>OnConfirmKeyPressed());
        };
        TemporaryStorage.OnMoveFinish += () =>
        {
            ExecuteCommand(Command.MoveFinish);
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
            case Command.MoveFinish:
                currentState = State.MoveFinish;
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
            case State.MoveFinish:
                break;
            default:
                break;
        }
    }
    void Pathfinding_MovePreparation_Display()
    {
        To.transform.position = TemporaryStorage.path_end_position;
        From.transform.position = TemporaryStorage.path_start_position;
        m_Path = graph.GetShortestPath( From, To );
        TemporaryStorage.InvokeOnMovePreparation(m_Path);
    }
    void OnConfirmKeyPressed()
    {
        switch (TemporaryStorage.buoyState)
        {
            case OperateLayer_Buoy.State.MoveExecute:
                TemporaryStorage.InvokeOnMove(graph);
                break;
            default:
                break;
        }
    }
}
}