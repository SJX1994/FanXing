using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace FanXing.FightDemo
{
public class GameControl : MonoBehaviour
{
    [SerializeField]
    OperateBuoy operateBuoy;
    [SerializeField]
    FightPaths fightPaths;
    [SerializeField]
    UI_Manager uI_Manager;
    [SerializeField]
    Camera uiCamera;
    private ScriptableObject_UnitSimpleDescription temp_unitSimpleDescription;
    void Start()
    {
        fightPaths.GetPathVertexs();
        uI_Manager.UI_CommandSelect.btn_Command_Move.onClick.AddListener(() =>
        {
            operateBuoy.ExecuteCommand(OperateBuoy.Command.Reset);
            operateBuoy.ExecuteCommand(OperateBuoy.Command.MoveOmen);
            FindObjectOfType<Role>().ExecuteCommand(Role.Command.MovePreparation);
            FindObjectOfType<Pathfinding>().ExecuteCommand(Pathfinding.Command.MovePreparation);
            
            DOVirtual.DelayedCall(0.1f, () =>
            {
                
                
            });
        });
        uI_Manager.UI_CommandSelect.btn_Command_Fight.onClick.AddListener(() =>
        {
            operateBuoy.ExecuteCommand(OperateBuoy.Command.Fight);
        });
        uI_Manager.UI_CommandSelect.btn_Command_Defense.onClick.AddListener(() =>
        {
            operateBuoy.ExecuteCommand(OperateBuoy.Command.Reset);
        });
        // TemporaryStorage.OnCancelKeyPressed += () =>
        // {
        //     operateBuoy.ExecuteCommand(OperateBuoy.Command.Reset);
        //     operateBuoy.ExecuteCommand(OperateBuoy.Command.Null);
        //     FindObjectOfType<Role>().ExecuteCommand(Role.Command.Null);
        //     FindObjectOfType<Pathfinding>().ExecuteCommand(Pathfinding.Command.Null);
        //     uI_Manager.UI_CommandSelect.Hide();
        // };
        TemporaryStorage.OnBuoySelectedObject += (go) =>
        {
            if(go.TryGetComponent(out SelectTester selectTest))
            {
                if(temp_unitSimpleDescription != selectTest.GetUnitSimpleDescription())
                {
                    temp_unitSimpleDescription = selectTest.GetUnitSimpleDescription();
                }
                uI_Manager.ShowUnitDescription(uiCamera,go,temp_unitSimpleDescription);
            }
        };
    }
    void OnDestroy()
    {
        TemporaryStorage.ClearValues();
    }
}
}
