using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FanXing.FightDemo
{
    public static class TemporaryStorage
    {
        public static List<Vector3> pathPoints;
        public static Vector3 path_end_position;
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
        public static void ClearValues()
        {
            path_end_position = Vector3.zero;
            pathPoints = null;
            buoyState = OperateBuoy.State.Idle;
        }
    }
}
