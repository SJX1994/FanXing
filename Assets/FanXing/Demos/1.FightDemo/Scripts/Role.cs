using System.Collections;
using System.Collections.Generic;
using FanXing.FightDemo;
using UnityEngine;
using DG.Tweening;
namespace FanXing.FightDemo
{
public class Role : MonoBehaviour
{
    public enum Command
    {
        Null,
        MovePreparation,
        Moveing,
    }
    public enum State
    {
        Idle,
        MovePreparation,
        Moveing
    }
    public State currentState;

    void Start()
    {
        currentState = State.Idle;
        TemporaryStorage.OnMove += (graph,path, startNode, endNode) =>
        {
            ExecuteCommand(Command.Moveing);
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
            case Command.Moveing:
                currentState = State.Moveing;
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
            case State.Moveing:
                Pathfinding_Moveing_Display();
                break;
            default:
                Debug.LogError("Invalid state");
                break;
        }
    }
    private void Pathfinding_MovePreparation_Display()
    {
        TemporaryStorage.path_start_position = transform.position;
    }
    private void Pathfinding_Moveing_Display()
    {
        
    }
}
}