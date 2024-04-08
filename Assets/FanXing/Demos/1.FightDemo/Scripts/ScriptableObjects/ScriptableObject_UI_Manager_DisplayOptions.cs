

using System;
using UnityEngine;
namespace FanXing.FightDemo
{
    [CreateAssetMenu(fileName = "UI_Manager_DisplayOptions_", menuName = "ScriptableObjects/UI_Manager_DisplayOptions", order = 1)]
    public class ScriptableObject_UI_Manager_DisplayOptions : ScriptableObject
    {
        public bool CommandSelectSystem_Button_Fight = true;
        public bool CommandSelectSystem_Button_Move = true;
        public bool CommandSelectSystem_Button_Defense = true;
        [System.Serializable]
        public struct ActionUI_DisplayOptions
        {
            public bool Display;
            public ScriptableObject_Action Content;
            
            public ActionUI_DisplayOptions(bool display, ScriptableObject_Action content)
            {
                Display = display;
                Content = content;
            }
        }
        public ActionUI_DisplayOptions ActionUI_Button_0 = new ActionUI_DisplayOptions(true, null);
        public ActionUI_DisplayOptions ActionUI_Button_1 = new ActionUI_DisplayOptions(true, null);
        public ActionUI_DisplayOptions ActionUI_Button_2 = new ActionUI_DisplayOptions(true, null);
        public ActionUI_DisplayOptions ActionUI_Button_3 = new ActionUI_DisplayOptions(true, null);
        public ActionUI_DisplayOptions ActionUI_Button_4 = new ActionUI_DisplayOptions(true, null);
        public ActionUI_DisplayOptions ActionUI_Button_5 = new ActionUI_DisplayOptions(true, null);
    }
}
