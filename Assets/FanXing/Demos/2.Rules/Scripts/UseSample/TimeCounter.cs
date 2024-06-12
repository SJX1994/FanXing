using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace FanXing.Demos.Rules
{
    public class TimeCounter : MonoBehaviour
    {
        [SerializeField]
        public ScriptableObject_TimeData timeData;
        [SerializeField]
        TextMeshPro textMeshPro;
        public void Init()
        {
            timeData = Instantiate(timeData);
            timeData.Discrete = 0;
            timeData.TimeStr = "0";
            timeData.textMeshPro = textMeshPro;
        }
    }
}