using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FanXing.FightDemo
{
    public static class TemporaryStorage
    {

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
        public static event Action<DijkstrasPathfinding.Graph,DijkstrasPathfinding.Path,DijkstrasPathfinding.Node,DijkstrasPathfinding.Node> OnMove;
        public static void InvokeOnMove(DijkstrasPathfinding.Graph graph, DijkstrasPathfinding.Path path, DijkstrasPathfinding.Node from, DijkstrasPathfinding.Node to)
        {
            OnMove?.Invoke(graph, path, from, to);
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
        }
    }
}
