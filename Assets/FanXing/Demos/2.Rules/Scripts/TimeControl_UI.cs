using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace FanXing.Demos.Rules
{
    public class TimeControl_UI : MonoBehaviour
    {
        [SerializeField]
        TimeType timeType;
        enum TimeType
        {
            GameTime,
            PausedTime
        }
        void FixedUpdate()
        {
            switch (timeType)
            {
                case TimeType.GameTime:
                    GetComponent<TextMeshProUGUI>().text = TemporaryStorage.GameTimeStr;
                    break;
                case TimeType.PausedTime:
                    GetComponent<TextMeshProUGUI>().text = TemporaryStorage.PausedTimeStr;
                    break;
            }
        }
    }
}