using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DijkstrasPathfinding;
using System;
using DG.Tweening;

namespace FanXing.FightDemo
{
    public class FightLayer_Generated_Missile : MonoBehaviour
    {
        [SerializeField] LineRenderer lineRenderer_path;
        [SerializeField] ParticleSystem endParticle;
        bool operating = false;
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }
        public float Speed { get; set; }
        public float Delay { get; set; }
        public bool Operating
        {
           get => operating;
           set
           {
               operating = value;
           }
        }
        float timer_dealy = 0;
        private float distanceTraveled = 0f; // 移动距离
        public void Creat()
        {
            DrawParabola(lineRenderer_path,StartPosition,EndPosition,31);
        }
        void Update()
        {
            if(operating)return;
            timer_dealy += Time.deltaTime;
            if(timer_dealy<Delay)return;
            if (distanceTraveled < lineRenderer_path.positionCount - 1)
            {
                Vector3 startPosition = lineRenderer_path.GetPosition(Mathf.FloorToInt(distanceTraveled));
                Vector3 endPosition = lineRenderer_path.GetPosition(Mathf.CeilToInt(distanceTraveled));
                Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, distanceTraveled % 1);
                transform.position = newPosition;
                distanceTraveled += Speed * Time.deltaTime;
                
            }
            else
            {
                var tempEndParticle = Instantiate(endParticle,transform.position,Quaternion.identity);
                tempEndParticle.Play();
                TemporaryStorage.InvokeOnDestoryMissile(this);
            }

        }
        void DrawParabola(LineRenderer lineRenderer_path, Vector3 pointA, Vector3 pointB,int numberOfPoints)
        {
            // 计算顶点C
            Vector3 pointC = (pointA + pointB) / 2 + Vector3.up * Mathf.Abs(pointA.y - pointB.y)*16f;

            Vector3[] positions = new Vector3[numberOfPoints];

            for (int i = 0; i < numberOfPoints; i++)
            {
                float t = (float)i / (numberOfPoints - 1);
                positions[i] = Shap.CalculateParabolaPoint(pointA, pointB, pointC, t);
            }
            lineRenderer_path.positionCount = numberOfPoints;
            lineRenderer_path.SetPositions(positions);
        }
    }
}