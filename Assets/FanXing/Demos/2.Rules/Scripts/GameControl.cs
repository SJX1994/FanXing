using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FanXing.Demos.Rules
{
    public class GameControl : MonoBehaviour
    {
        [SerializeField]
        bool pause = false;
        [SerializeField]
        ScriptableObject_UnitAttributes[] role_attributes;
        
        void Awake()
        {
            TemporaryStorage.Reset_Playable();
            TemporaryStorage.MaxCharacters = 2;
            Events.Reset_Playable();
        }
        void Start()
        {
            Invoke(nameof(LateStart), 0.1f);
        }
        void LateStart()
        {
            Events.InvokeToggleTime(pause); 
            Sprite[] characterIcons = new Sprite[role_attributes.Length];
            float[] durations = new float[role_attributes.Length];
            for (int i = 0; i < role_attributes.Length; i++)
            {
                characterIcons[i] = role_attributes[i].ProgressSprite;
                durations[i] = role_attributes[i].ProgressDuration;
            }
            Events.InvokeInitProssesBar(role_attributes,characterIcons, durations);
        }
    }
}