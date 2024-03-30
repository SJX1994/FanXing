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
            FindObjectOfType<OperateBuoy>().ExecuteCommand(OperateBuoy.Command.Move);
            DOVirtual.DelayedCall(0.2f, () =>
            {
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
    }
    void OnDestroy()
    {
        TemporaryStorage.ClearValues();
    }
}
}
