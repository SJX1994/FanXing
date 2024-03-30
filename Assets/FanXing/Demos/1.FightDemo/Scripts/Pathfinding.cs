using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DijkstrasPathfinding;
using System;
using DG.Tweening;
namespace FanXing.FightDemo
{
public class Pathfinding : MonoBehaviour
{
    [SerializeField] Node From;
    [SerializeField] Node To;
    [SerializeField] Graph graph;
    [SerializeField] LineRenderer lineRenderer;
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
        InitDisplay();
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

    private void InitDisplay()
    {
        lineRenderer.positionCount = 0;
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
        lineRenderer.positionCount = m_Path.nodes.Count;
        for ( int i = 0; i < m_Path.nodes.Count; i++ )
        {
            Vector3 pos = m_Path.nodes [ i ].transform.position;
            pos.y = 0.6f;
            lineRenderer.SetPosition( i, pos );
        }
    }
    void OnConfirmKeyPressed()
    {
        switch (TemporaryStorage.buoyState)
        {
            case OperateBuoy.State.MoveExecute:
                TemporaryStorage.InvokeOnMove(graph,m_Path, From, To);
                break;
            default:
                break;
        }
    }
}
}