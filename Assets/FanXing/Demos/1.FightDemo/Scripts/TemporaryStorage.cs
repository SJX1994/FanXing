using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FanXing.FightDemo
{
    public static class TemporaryStorage
    {
        private static Camera ui_camera;
        public static Camera UI_Camera
        {
            get
            {
                if (ui_camera != null)return                
                ui_camera = GameObject.Find("UI Camera").GetComponent<Camera>();
                return ui_camera;
            }
        }
        public static List<Vector3> pathPoints;
        public static Vector3 path_end_position;
        public static Vector3 path_start_position;
        public static OperateBuoy.State buoyState
        {
            get => _buoyState;
            set
            {
                _buoyState = value;
                OnBuoyStateChanged?.Invoke(value);
            }
        }
        private static OperateBuoy.State _buoyState;
        public static event Action<OperateBuoy.State> OnBuoyStateChanged;
        public static event Action<GameObject> OnBuoySelectedObject;
        public static void InvokeOnBuoyStateSelected(GameObject go)
        {
            OnBuoySelectedObject?.Invoke(go);
        }
        public static GameObject BuoySelectedObject;
        public static event Action<DijkstrasPathfinding.Path> OnMovePreparation;
        public static void InvokeOnMovePreparation(DijkstrasPathfinding.Path path)
        {
            OnMovePreparation?.Invoke(path);
        }
       
        public static event Action<DijkstrasPathfinding.Graph> OnMove;
        public static void InvokeOnMove(DijkstrasPathfinding.Graph graph)
        {
            OnMove?.Invoke(graph);
        }
        public static event Action OnMoveFinish;
        public static void InvokeOnMoveFinish()
        {
            OnMoveFinish?.Invoke();
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
            path_end_position = Vector3.zero;
            path_start_position = Vector3.zero;
            pathPoints = null;
            buoyState = OperateBuoy.State.Idle;
            OnBuoyStateChanged = null;
            OnMove = null;
            OnConfirmKeyPressed = null;
            OnCancelKeyPressed = null;
            OnMoveFinish = null;
            OnBuoySelectedObject = null;
            BuoySelectedObject = null;
        }
    }
}
