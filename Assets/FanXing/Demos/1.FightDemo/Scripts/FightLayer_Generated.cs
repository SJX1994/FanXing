using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DijkstrasPathfinding;
using System;
using DG.Tweening;

namespace FanXing.FightDemo
{
    public class FightLayer_Generated : MonoBehaviour
    {   
        List<FightLayer_Generated_Missile> missiles = new();
        void Start()
        {
            TemporaryStorage.OnOperating += Pause;
            TemporaryStorage.OnGenerateMissile += Generate;
            TemporaryStorage.OnDestoryMissile += Destory;
        }
        void Generate(ScriptableObject_Action scriptableObject_Action,Vector3 startPostion,Vector3 endPosition)
        {
            if(!scriptableObject_Action.Particle.TryGetComponent(out FightLayer_Generated_Missile missile))return;
            missile = Instantiate(missile,startPostion,Quaternion.identity,transform);
            missile.StartPosition = startPostion;
            missile.EndPosition = endPosition;
            missile.Speed = UnityEngine.Random.Range(5.0f,5.5f);
            missile.Delay = UnityEngine.Random.Range(0f,0.2f);
            missile.Creat();
            missiles.Add(missile);
            missile.Operating = true;
        }
        void Destory(FightLayer_Generated_Missile fightLayer_Generated_Missile)
        {
            fightLayer_Generated_Missile.Operating = false;
            if(missiles.Find(x=>x==fightLayer_Generated_Missile)==null)return;
            if(missiles.Contains(fightLayer_Generated_Missile))
            {
                fightLayer_Generated_Missile = missiles.Find(x=>x==fightLayer_Generated_Missile);
                if(!fightLayer_Generated_Missile)return;
                GameObject go =  missiles.Find(x=>x==fightLayer_Generated_Missile).gameObject;
                missiles.Remove(fightLayer_Generated_Missile);
                Destroy(go);
            }
            
        }
        void Pause(bool b)
        {
            foreach (var item in missiles)
            {
                item.Operating = b;
            }
        }
       
    }
}