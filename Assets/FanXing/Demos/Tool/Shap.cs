using UnityEngine;
namespace FanXing.FightDemo
{
    public static class Shap 
    {
        public static Vector3 CalculateParabolaPoint(Vector3 pointA, Vector3 pointB, Vector3 pointC, float t)
        {
            float oneMinusT = 1f - t;
            return oneMinusT * oneMinusT * pointA + 2f * oneMinusT * t * pointC + t * t * pointB;
        }
    }
}