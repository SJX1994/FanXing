

using UnityEngine;
namespace FanXing.FightDemo
{
    [CreateAssetMenu(fileName = "UI_Manager_DisplayOptions_", menuName = "ScriptableObjects/UI_Manager_DisplayOptions", order = 1)]
    public class ScriptableObject_UI_Manager_DisplayOptions : ScriptableObject
    {
        public bool CommandSelectSystem_Button_Fight = true;
        public bool CommandSelectSystem_Button_Move = true;
        public bool CommandSelectSystem_Button_Defense = true;
    }
}
