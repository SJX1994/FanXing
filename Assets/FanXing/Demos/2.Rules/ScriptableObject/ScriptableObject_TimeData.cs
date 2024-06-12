using UnityEngine;
using TMPro;
namespace FanXing.Demos.Rules
{
    [CreateAssetMenu(fileName = "ConstituativeRules_TimeData_", menuName = "ConstituativeRules/TimeData", order = 1)]

    public class ScriptableObject_TimeData : ScriptableObject
    {
        [Header("时间数据")]
        public int Discrete = 0;
        private string timeStr = "";
        public string TimeStr
        {
            get
            {
                return timeStr;
            }
            set
            {
                timeStr = value;
                
            }
        }
        public TextMeshPro textMeshPro;
    }
}