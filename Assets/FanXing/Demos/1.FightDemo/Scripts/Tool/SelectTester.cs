using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FanXing.FightDemo
{
    public class SelectTester : MonoBehaviour
    {
        [SerializeField] ScriptableObject_UnitSimpleDescription scriptableObject_UnitSimpleDescription;
        void Start()
        {
            scriptableObject_UnitSimpleDescription = Instantiate(scriptableObject_UnitSimpleDescription);
        }
        public ScriptableObject_UnitSimpleDescription GetUnitSimpleDescription()
        {
            return scriptableObject_UnitSimpleDescription;
        }
    }
}