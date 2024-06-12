using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FanXing.FightDemo
{
    public class ConstantRotate : MonoBehaviour
    {
        enum RotateAxis
        {
            X,
            Y,
            Z
        }
        [SerializeField]
        RotateAxis rotateAxis;
        void Update()
        {
            switch (rotateAxis)
            {
                case RotateAxis.X:
                    transform.Rotate (-90*Time.deltaTime,0,0); //rotates 50 degrees per second around x axis
                    break;
                case RotateAxis.Y:
                    transform.Rotate (0,-90*Time.deltaTime,0); //rotates 50 degrees per second around y axis
                    break;
                case RotateAxis.Z:
                    transform.Rotate (0,0,-90*Time.deltaTime); 
                    break;
            }
        }
    }
}
