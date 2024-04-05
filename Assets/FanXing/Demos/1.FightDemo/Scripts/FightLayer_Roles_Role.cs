using System.Collections;
using System.Collections.Generic;
using FanXing.FightDemo;
using UnityEngine;
using DG.Tweening;
using System.Linq;
namespace FanXing.FightDemo
{
public class FightLayer_Roles_Role : MonoBehaviour
{
    public enum Command
    {
        Null,
        MovePreparation,
        Moveing,
    }
    public FightLayer_Roles_Role_Move roleMove;
    [SerializeField] FightLayer_Roles_Role_Status roleStatus;
    public FightLayer_Roles_Role_SelectTester selectTester;

    void Start()
    {
        ExecuteCommand(Command.Null);

        TemporaryStorage.OnMovePreparation += (who,path) =>	
		{
			who.OnMovePreparation(path);
		};
        TemporaryStorage.OnMove += (who,graph,path,from,to) =>
        {
			
            ExecuteCommand(Command.Moveing);
            who.OnMove(graph,path,from,to);
            
        };
        TemporaryStorage.OnMoveFinish += (who) =>
		{
            who.OnMoveFinish();
		};
       
    }
    public void ExecuteCommand(Command command)
    {
        switch (command)
        {
            case Command.Null:
                roleStatus.currentState = FightLayer_Roles_Role_Status.State.Idle;
                break;
            case Command.MovePreparation:
                roleStatus.currentState = FightLayer_Roles_Role_Status.State.MovePreparation;
                break;
            case Command.Moveing:
                roleStatus.currentState = FightLayer_Roles_Role_Status.State.Moveing;
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
            case FightLayer_Roles_Role_Status.State.Idle:
                break;
            case FightLayer_Roles_Role_Status.State.MovePreparation:
                Pathfinding_MovePreparation_Display();
                break;
            case FightLayer_Roles_Role_Status.State.Moveing:
                Pathfinding_Moveing_Display();
                break;
            default:
                Debug.LogError("Invalid state");
                break;
        }
    }
    private void Pathfinding_MovePreparation_Display()
    {

        TemporaryStorage.Path_start_position = transform.position;
    }
    private void Pathfinding_Moveing_Display()
    {
        roleMove.UpdateMoving();
    }
}
}