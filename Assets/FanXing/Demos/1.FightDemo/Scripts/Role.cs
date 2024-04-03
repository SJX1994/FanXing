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
    [SerializeField] RoleMove roleMove;
    [SerializeField] RoleStatus roleStatus;

    void Start()
    {
        ExecuteCommand(Command.Null);

        TemporaryStorage.OnMove += (graph) =>
        {
            ExecuteCommand(Command.Moveing);
        };
    }
    public void ExecuteCommand(Command command)
    {
        switch (command)
        {
            case Command.Null:
                roleStatus.currentState = RoleStatus.State.Idle;
                break;
            case Command.MovePreparation:
                roleStatus.currentState = RoleStatus.State.MovePreparation;
                break;
            case Command.Moveing:
                roleStatus.currentState = RoleStatus.State.Moveing;
                break;
            default:
                Debug.LogError("Invalid command");
                break;
        }
    }
    void Update()
    {
        roleStatus.UpdateStatusLogic();
        switch (roleStatus.currentState)
        {
            case RoleStatus.State.Idle:
                break;
            case RoleStatus.State.MovePreparation:
                Pathfinding_MovePreparation_Display();
                break;
            case RoleStatus.State.Moveing:
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
        roleMove.UpdateMoving();
    }
}
}