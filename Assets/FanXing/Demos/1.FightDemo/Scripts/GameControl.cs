using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace FanXing.FightDemo
{
public class GameControl : MonoBehaviour
{
    [SerializeField]
    FightPaths fightPaths;
    void Start()
    {
        fightPaths.GetPathVertexs();
        FindObjectOfType<UI_CommandSelect>().btn_Command_Move.onClick.AddListener(() =>
        {
            FindObjectOfType<OperateBuoy>().ExecuteCommand(OperateBuoy.Command.Reset);
            FindObjectOfType<OperateBuoy>().ExecuteCommand(OperateBuoy.Command.MoveOmen);
            DOVirtual.DelayedCall(0.2f, () =>
            {
                FindObjectOfType<Role>().ExecuteCommand(Role.Command.MovePreparation);
                FindObjectOfType<Pathfinding>().ExecuteCommand(Pathfinding.Command.MovePreparation);
            });
        });
        FindObjectOfType<UI_CommandSelect>().btn_Command_Fight.onClick.AddListener(() =>
        {
            FindObjectOfType<OperateBuoy>().ExecuteCommand(OperateBuoy.Command.Fight);
        });
        FindObjectOfType<UI_CommandSelect>().btn_Command_Defense.onClick.AddListener(() =>
        {
            FindObjectOfType<OperateBuoy>().ExecuteCommand(OperateBuoy.Command.Reset);
        });
        TemporaryStorage.OnCancelKeyPressed += () =>
        {
            FindObjectOfType<OperateBuoy>().ExecuteCommand(OperateBuoy.Command.Reset);
            FindObjectOfType<OperateBuoy>().ExecuteCommand(OperateBuoy.Command.Null);
            FindObjectOfType<Role>().ExecuteCommand(Role.Command.Null);
            FindObjectOfType<Pathfinding>().ExecuteCommand(Pathfinding.Command.Null);
        };
    }
    void OnDestroy()
    {
        TemporaryStorage.ClearValues();
    }
}
}
