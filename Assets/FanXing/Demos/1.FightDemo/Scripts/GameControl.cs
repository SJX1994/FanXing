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
        TemporaryStorage.OnShow_UI_MovePreparation += () =>
        {
            if(TemporaryStorage.BuoySelectedObject == null)return;
            TemporaryStorage.InvokeOnRestBuoyPosition(TemporaryStorage.BuoySelectedObject.transform.position);
            operateBuoy.ExecuteCommand(OperateLayer_Buoy.Command.MoveOmen);
            pathfinding.ExecuteCommand(FightLayer_Pathfinding.Command.MovePreparation);
            TemporaryStorage.BuoySelectedObject.GetComponent<FightLayer_Roles_Role>().ExecuteCommand(FightLayer_Roles_Role.Command.MovePreparation);
            
        };
        TemporaryStorage.OnHide_UI_MovePreparation += () =>
        {
            if(TemporaryStorage.BuoySelectedObject == null)return;
            TemporaryStorage.InvokeOnRestBuoyPosition(TemporaryStorage.BuoySelectedObject.transform.position);
            operateBuoy.ExecuteCommand(OperateLayer_Buoy.Command.Null);
            pathfinding.ExecuteCommand(FightLayer_Pathfinding.Command.Null);
            TemporaryStorage.BuoySelectedObject.GetComponent<FightLayer_Roles_Role>().ExecuteCommand(FightLayer_Roles_Role.Command.Null);
            
        };
        
    }
    void OnDestroy()
    {
        TemporaryStorage.ClearValues();
        DOTween.KillAll();
    }
    private void OnApplicationQuit() { DOTween.KillAll(); }
}
}
