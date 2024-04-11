using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FanXing.FightDemo
{
    public class OperateLayer_Buoy : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] OperateLayer_Buoy_Selector buoySelector;
        [SerializeField] LineRenderer lineRenderer_buoy_path;
        [SerializeField] Transform path_end;
        [SerializeField] int numberOfPoints = 10;
        public enum Command
        {
            Null,
            MoveOmen,
            MoveExecute,
            Action
        }
        public enum State
        {
            Idle,
            MoveOmen,
            MoveExecute,
            Action
        }
        public State currentState;
        private float timer = 0f;

        void Start()
        {
            currentState = State.Idle;
            TemporaryStorage.OnConfirmKeyPressed += () =>
            {
                switch(TemporaryStorage.BuoyState)
                {
                    case State.MoveOmen:
                        ExecuteCommand(Command.MoveExecute);
                        break;
                    
                    default:
                        break;
                }
            };
            TemporaryStorage.OnBuoyStateChanged += (state) =>
            {
                currentState = state;
            };
            TemporaryStorage.OnRestBuoyPosition += (position) =>
            {
                buoySelector.transform.position = new Vector3(position.x, buoySelector.transform.position.y, position.z);
            };
        }
        void Update()
        {
            // Debug.Log("OperateLayer_Buoy"+ TemporaryStorage.BuoyState);

            float moveX = Input.GetAxis("Horizontal"); // A 和 D 键
            float moveZ = Input.GetAxis("Vertical"); // W 和 S 键
            TemporaryStorage.BuoyPosition = buoySelector.transform.position;
            Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed * Time.deltaTime;
            buoySelector.transform.Translate(movement, Space.Self);
            switch (TemporaryStorage.BuoyState)
            {
                case State.Idle:
                    buoySelector.UpdateSelector();
                    break;
                case State.MoveOmen:
                    MoveOmeningDisplay();
                    break;
                case State.MoveExecute:
                    timer += Time.deltaTime;
                    if (timer > 0.5f)
                    {
                        timer = 0f;
                        ExecuteCommand(Command.Null);
                    }
                    
                    break;
                case State.Action:
                    buoySelector.UpdateSelector();
                    break;
                
                default:
                    break;
            }
        }
        public void ExecuteCommand(Command command)
        {
            InitDisplay();
            switch (command)
            {
                case Command.Null:
                    TransitionToState(State.Idle);
                    break;
                case Command.MoveOmen:
                    TransitionToState(State.MoveOmen);
                    break;
                case Command.MoveExecute:
                    TransitionToState(State.MoveExecute);
                    break;
                case Command.Action:
                    TransitionToState(State.Action);
                    break;
                default:
                    Debug.LogError("Invalid command");
                    break;
            }
        }
        void InitDisplay()
        {
            path_end.gameObject.SetActive(false);
            lineRenderer_buoy_path.positionCount = 0;
        }
        private void TransitionToState(State newState)
        {
            currentState = newState;
            TemporaryStorage.BuoyState = currentState;
        }
        private void MoveOmeningDisplay()
        {
            switch(TemporaryStorage.CurrentBuoy_RoleType)
            {
                case ScriptableObject_Role_Infomation.RoleType.Flight:
                    MoveOmeningDisplay_Flight();
                    break;
                case ScriptableObject_Role_Infomation.RoleType.LongDistance:
                    MoveOmeningDisplay_Ground();
                    break;
                default:
                    break;
            }
            // Vector3 closestPoint =  FindClosestPoint(TemporaryStorage.PathPoints, buoySelector.transform.position);
            // DrawParabola(buoySelector.transform.position, closestPoint);
            // path_end.gameObject.SetActive(true);
            // path_end.position = closestPoint;
            // TemporaryStorage.Path_end_position = closestPoint;
        }
        private void MoveOmeningDisplay_Flight()
        {
            path_end.gameObject.SetActive(true);
            path_end.position = TemporaryStorage.BuoyPosition;
        }
        private void MoveOmeningDisplay_Ground()
        {
            Vector3 closestPoint =  FindClosestPoint(TemporaryStorage.PathPoints, buoySelector.transform.position);
            DrawParabola(buoySelector.transform.position, closestPoint);
            path_end.gameObject.SetActive(true);
            path_end.position = closestPoint;
            TemporaryStorage.Path_end_position = closestPoint;
        }
        public Vector3 FindClosestPoint(List<Vector3> points, Vector3 targetPoint)
        {
            if (points.Count == 0)
            {
                Debug.LogError("The list of points is empty");
                return Vector3.zero;
            }

            Vector3 closestPoint = points[0];
            float closestDistance = Vector3.Distance(points[0], targetPoint);

            for (int i = 1; i < points.Count; i++)
            {
                float distance = Vector3.Distance(points[i], targetPoint);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = points[i];
                }
            }

            return closestPoint;
        }
        void DrawParabola(Vector3 pointA, Vector3 pointB)
        {
            // 计算顶点C
            Vector3 pointC = (pointA + pointB) / 2 + Vector3.up * Mathf.Abs(pointA.y - pointB.y)*6f;

            Vector3[] positions = new Vector3[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                float t = (float)i / (numberOfPoints - 1);
                positions[i] = CalculateParabolaPoint(pointA, pointB, pointC, t);
            }

            lineRenderer_buoy_path.positionCount = numberOfPoints;
            lineRenderer_buoy_path.SetPositions(positions);
        }
        

        private Vector3 CalculateParabolaPoint(Vector3 pointA, Vector3 pointB, Vector3 pointC, float t)
        {
            float oneMinusT = 1f - t;
            return oneMinusT * oneMinusT * pointA + 2f * oneMinusT * t * pointC + t * t * pointB;
        }
    }
}
