using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace FanXing.FightDemo
{
    public class EnvironmentLayer_PostProcessing : MonoBehaviour
    {
        [SerializeField] Volume postProcessing;
        ColorAdjustments colorAdjustments;
        void Start()
        {
            TemporaryStorage.OnOperating += (b) =>
            {
                if(postProcessing.profile.TryGet(out colorAdjustments))
                {
                    colorAdjustments.active = b;
                }   
            };
        }
    }
}

