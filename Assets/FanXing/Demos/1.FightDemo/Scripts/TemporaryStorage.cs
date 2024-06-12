using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using DijkstrasPathfinding;
namespace FanXing.FightDemo
{
    public static class TemporaryStorage
    {
        public static Camera UI_Camera;
       
        public static List<Vector3> PathPoints;
        public static Vector3 Path_end_position;
        public static Vector3 Path_start_position;
        public static ScriptableObject_Role_Infomation.RoleType CurrentBuoy_RoleType;
        public static Vector3 BuoyPosition;
        public static OperateLayer_Buoy.State BuoyState
        {
            get => _buoyState;
            set
            {
                if(_buoyState == value)return;
                _buoyState = value;
                OnBuoyStateChanged?.Invoke(value);
            }
        }
        private static OperateLayer_Buoy.State _buoyState;
        public static event Action<OperateLayer_Buoy.State> OnBuoyStateChanged;
        public static event Action<Vector3> OnRestBuoyPosition;
        public static void InvokeOnRestBuoyPosition(Vector3 position)
        {
            OnRestBuoyPosition?.Invoke(position);
        }
        public static GameObject BuoySelectingObject;
        public static GameObject BuoySelectedObject;
        public static event Action<GameObject> OnBuoySelectedObject;
        public static void InvokeOnBuoyStateSelected(GameObject go)
        {
            OnBuoySelectedObject?.Invoke(go);
        }
        
        public static event Action<FightLayer_Roles_Role_Move,DijkstrasPathfinding.Path> OnMovePreparation;
        public static void InvokeOnMovePreparation(FightLayer_Roles_Role_Move who,DijkstrasPathfinding.Path path)
        {
            OnMovePreparation?.Invoke(who,path);
        }
       
        public static event Action<FightLayer_Roles_Role_Move,DijkstrasPathfinding.Graph,DijkstrasPathfinding.Path,DijkstrasPathfinding.Node,DijkstrasPathfinding.Node> OnMove;
        public static void InvokeOnMove(FightLayer_Roles_Role_Move who,DijkstrasPathfinding.Graph graph,DijkstrasPathfinding.Path path,DijkstrasPathfinding.Node tempFrom,DijkstrasPathfinding.Node tempTo)
        {
            OnMove?.Invoke(who,graph,path,tempFrom,tempTo);
        }
        public static event Action<FightLayer_Roles_Role_Move> OnMoveFinish;
        public static void InvokeOnMoveFinish(FightLayer_Roles_Role_Move who)
        {
            OnMoveFinish?.Invoke(who);
        }
        public static event Action OnConfirmKeyPressed;
        public static void InvokeOnConfirmKeyPressed()
        {
            OnConfirmKeyPressed?.Invoke();
        }
        public static event Action OnCancelKeyPressed;
        public static void InvokeOnCancelKeyPressed()
        {
            OnCancelKeyPressed?.Invoke();
        }
        public static event Action<GameObject,ScriptableObject_UI_Manager_DisplayOptions> OnShow_UI_Manager;
        public static void InvokeOnShow_UI_Manager(GameObject go,ScriptableObject_UI_Manager_DisplayOptions scriptableObject_UI_Manager_DisplayOptions)
        {
            OnShow_UI_Manager?.Invoke(go,scriptableObject_UI_Manager_DisplayOptions);
        }
        public static event Action OnShow_UI_MovePreparation;
        public static void InvokeOnShow_UI_MovePreparation()
        {
            OnShow_UI_MovePreparation?.Invoke();
        }
        public static event Action OnHide_UI_MovePreparation;
        public static void InvokeOnHide_UI_MovePreparation()
        {
            OnHide_UI_MovePreparation?.Invoke();
        }
        public static event Action<GameObject,ScriptableObject_UnitSimpleDescription> OnShowUnitDescription;
        public static void InvokeOnShowUnitDescription(GameObject go,ScriptableObject_UnitSimpleDescription scriptableObject_UnitSimpleDescription)
        {
            OnShowUnitDescription?.Invoke(go,scriptableObject_UnitSimpleDescription);
        }
        public static event Action OnHideUnitDescription;
        public static void InvokeOnHideUnitDescription()
        {
            OnHideUnitDescription?.Invoke();
        }
        public static event Action<bool> OnOperating;
        public static void InvokeOnOperating(bool active)
        {
            OnOperating?.Invoke(active);
        }
        public static event Action<GameObject,ScriptableObject_Action> OnActionSelected;
        public static void InvokeOnActionSelected(GameObject who_been_selected,ScriptableObject_Action scriptableObject_Action)
        {
            OnActionSelected?.Invoke(who_been_selected,scriptableObject_Action);
        }
        public static event Action<GameObject> OnActionCanceled;
        public static void InvokeOnActionCanceled(GameObject who_been_selected)
        {
            OnActionCanceled?.Invoke(who_been_selected);
        }
        public static event Action OnActionRelease;
        public static void InvokeOnActionRelease()
        {
            OnActionRelease?.Invoke();
        }
        public static event Action<ScriptableObject_Action,Vector3,Vector3> OnGenerateMissile;
        public static void InvokeOnGenerateMissile(ScriptableObject_Action scriptableObject_Action,Vector3 startPostion,Vector3 endPosition)
        {
            OnGenerateMissile?.Invoke(scriptableObject_Action,startPostion,endPosition);
        }
        public static event Action<FightLayer_Generated_Missile> OnDestoryMissile;
        public static void InvokeOnDestoryMissile(FightLayer_Generated_Missile fightLayer_Generated_Missile)
        {
            OnDestoryMissile?.Invoke(fightLayer_Generated_Missile);
        }
        public static event Action<FightLayer_Roles_Role_Action,float> OnCoolDown;
        public static void InvokeOnCoolDown(FightLayer_Roles_Role_Action who,float time)
        {
            OnCoolDown?.Invoke(who,time);
        }
        public enum UnitName
        {
            AOE_Mage,
            Dodge_Tank,
        }
        public enum UnitType
        {
            Ground,
            Air,
        }
        public enum UnitCamp
        {
            Enemy,
            Friendly,
        }
        public static void ClearValues()
        {
            Path_end_position = Vector3.zero;
            Path_start_position = Vector3.zero;
            PathPoints = null;
            BuoyState = OperateLayer_Buoy.State.Idle;
            OnBuoyStateChanged = null;
            OnMove = null;
            OnConfirmKeyPressed = null;
            OnCancelKeyPressed = null;
            OnMoveFinish = null;
            OnBuoySelectedObject = null;
            BuoySelectedObject = null;
            BuoySelectingObject = null;
            OnRestBuoyPosition = null;
            OnMovePreparation = null;
            OnShowUnitDescription = null;
            OnShow_UI_Manager = null;
            UI_Camera = null;
            OnHide_UI_MovePreparation = null;
            OnHideUnitDescription = null;
            OnShow_UI_MovePreparation = null;
            OnOperating = null;
            BuoyPosition = Vector3.zero;
            CurrentBuoy_RoleType = ScriptableObject_Role_Infomation.RoleType.Null;
            OnActionSelected = null;
            OnActionCanceled = null;
            OnGenerateMissile = null;

            
        }
        
    }
}
