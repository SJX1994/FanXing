using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FanXing.FightDemo
{
    public class OperateBuoy : MonoBehaviour
    {
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] Buoy buoy;
        [SerializeField] LineRenderer lineRenderer_buoy_path;
        [SerializeField] Transform path_end;
        [SerializeField] int numberOfPoints = 10;
        public enum Command
        {
            Null,
            MoveOmen,
            MoveExecute,
            Fight,
            Reset
        }
        public enum State
        {
            Idle,
            MoveOmen,
            MoveExecute,
            Fighting
        }
        public State currentState;
        private float timer = 0f;

        void Start()
        {
            currentState = State.Idle;
            TemporaryStorage.OnConfirmKeyPressed += () =>
            {
                switch(TemporaryStorage.buoyState)
                {
                    case State.MoveOmen:
                        ExecuteCommand(Command.MoveExecute);
                        break;
                    default:
                        break;
                }
            };
            TemporaryStorage.OnMoveFinish += () =>
            {
                ExecuteCommand(Command.Null);
            };
        }
        void Update()
        {
            
            float moveX = Input.GetAxis("Horizontal"); // A 和 D 键
            float moveZ = Input.GetAxis("Vertical"); // W 和 S 键

            Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed * Time.deltaTime;
            buoy.transform.Translate(movement, Space.Self);
            switch (currentState)
            {
                case State.Idle:
                    buoy.UpdateSelector();
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
                case State.Fighting:
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
                case Command.Fight:
                    TransitionToState(State.Fighting);
                    break;
                case Command.Reset:
                    List<Vector3> rolePos = new();
                    foreach (Role role in GameObject.FindObjectsOfType<Role>())
                    {
                        rolePos.Add(role.transform.position);
                    }
                    Vector3 clostestPoint = FindClosestPoint(rolePos, buoy.transform.position);
                    buoy.transform.position = new Vector3(clostestPoint.x, buoy.transform.position.y, clostestPoint.z);
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
            TemporaryStorage.buoyState = currentState;
        }
        private void MoveOmeningDisplay()
        {
            Vector3 closestPoint =  FindClosestPoint(TemporaryStorage.pathPoints, buoy.transform.position);
            DrawParabola(buoy.transform.position, closestPoint);
            path_end.gameObject.SetActive(true);
            path_end.position = closestPoint;
            TemporaryStorage.path_end_position = closestPoint;
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
