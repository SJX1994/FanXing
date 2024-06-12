using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FanXing.FightDemo
{
    public class FightLayer_Roles_Role_Status : MonoBehaviour
    {
        
        public enum State
        {
            Idle,
            MovePreparation,
            Moveing
        }
        public State currentState;
        
        public void UpdateStatusLogic()
        {
            switch (currentState)
            {
                case State.Idle:
                    break;
                case State.MovePreparation:
                    Pathfinding_MovePreparation_Logic();
                    break;
                case State.Moveing:
                    Pathfinding_Moveing_Logic();
                    break;
                default:
                    Debug.LogError("Invalid state");
                    break;
            }
        }
        void Pathfinding_MovePreparation_Logic()
        {
            TemporaryStorage.Path_start_position = transform.position;
        }
        void Pathfinding_Moveing_Logic()
        {
            
        }
    }
}

