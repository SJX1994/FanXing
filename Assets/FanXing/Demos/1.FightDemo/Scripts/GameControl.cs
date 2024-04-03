using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace FanXing.FightDemo
{
public class GameControl : MonoBehaviour
{
    [SerializeField]
    FightLayer_Pathfinding pathfinding;
    [SerializeField]
    OperateLayer_Buoy operateBuoy;
    [SerializeField]
    FightLayer_Paths fightPaths;
    [SerializeField]
    UI_Manager uI_Manager;
    [SerializeField]
    Camera uiCamera;
   
    private ScriptableObject_UnitSimpleDescription temp_unitSimpleDescription;
    void Start()
    {
        TemporaryStorage.UI_Camera = uiCamera;
        fightPaths.GetPathVertexs();
        uI_Manager.UI_CommandSelect.btn_Command_Move.onClick.AddListener(() =>
        {
            if(TemporaryStorage.BuoySelectingObject == null)return;
            operateBuoy.ExecuteCommand(OperateLayer_Buoy.Command.MoveOmen);
            pathfinding.ExecuteCommand(FightLayer_Pathfinding.Command.MovePreparation);
            TemporaryStorage.BuoySelectingObject.GetComponent<FightLayer_Roles_Role>().ExecuteCommand(FightLayer_Roles_Role.Command.MovePreparation);
           
        });
        uI_Manager.UI_CommandSelect.btn_Command_Fight.onClick.AddListener(() =>
        {
            //operateBuoy.ExecuteCommand(OperateLayer_Buoy.Command.Fight);
        });
        uI_Manager.UI_CommandSelect.btn_Command_Defense.onClick.AddListener(() =>
        {
            //operateBuoy.ExecuteCommand(OperateLayer_Buoy.Command.Reset);
        });
        // TemporaryStorage.OnCancelKeyPressed += () =>
        // {
        //     operateBuoy.ExecuteCommand(OperateBuoy.Command.Reset);
        //     operateBuoy.ExecuteCommand(OperateBuoy.Command.Null);
        //     FindObjectOfType<Role>().ExecuteCommand(Role.Command.Null);
        //     FindObjectOfType<Pathfinding>().ExecuteCommand(Pathfinding.Command.Null);
        //     uI_Manager.UI_CommandSelect.Hide();
        // };
        // TemporaryStorage.OnBuoySelectedObject += (go) =>
        // {
        //     if(go.TryGetComponent(out SelectTester selectTest))
        //     {
        //         if(temp_unitSimpleDescription != selectTest.GetUnitSimpleDescription())
        //         {
        //             temp_unitSimpleDescription = selectTest.GetUnitSimpleDescription();
        //         }
        //         uI_Manager.ShowUnitDescription(go,temp_unitSimpleDescription);
        //     }
        // };
    }
    void OnDestroy()
    {
        TemporaryStorage.ClearValues();
    }
}
}
