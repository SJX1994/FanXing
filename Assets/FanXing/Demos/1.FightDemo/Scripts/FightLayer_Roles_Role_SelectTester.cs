using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FanXing.FightDemo
{
    public class FightLayer_Roles_Role_SelectTester : MonoBehaviour
    {
        [SerializeField] ScriptableObject_UnitSimpleDescription scriptableObject_UnitSimpleDescription;
        [SerializeField] ScriptableObject_UI_Manager_DisplayOptions scriptableObject_UI_Manager_DisplayOptions;
        void Start()
        {
            scriptableObject_UnitSimpleDescription = Instantiate(scriptableObject_UnitSimpleDescription);
            scriptableObject_UI_Manager_DisplayOptions = Instantiate(scriptableObject_UI_Manager_DisplayOptions);
        }
        public ScriptableObject_UnitSimpleDescription GetUnitSimpleDescription()
        {
            return scriptableObject_UnitSimpleDescription;
        }
        public ScriptableObject_UI_Manager_DisplayOptions GetUI_Manager_DisplayOptions()
        {
            return scriptableObject_UI_Manager_DisplayOptions;
        }

    }
}