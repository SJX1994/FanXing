using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FanXing.Demos.Rules
{
    public class TimeControl : MonoBehaviour
    {
        public ScriptableObject_TimeData gameTimeData;
        public ScriptableObject_TimeData pauseTimeData;
        private float totalSeconds = 0f;
        private bool isPaused = true;
        private float pauseTime = 0f;
        private float DiscreteTimekeeping = 0;
        
     
        void Start()
        {
            Events.OnToggleTime += TogglePause;
        }
        void Update()
        {
            Update_TimeCount();
        }
        void Update_TimeCount()
        {
            if (isPaused)
            {
                // 继续计时
                pauseTime = Time.time - pauseTime;
                
            }
            else
            {
                // 暂停计时
                pauseTime = Time.time;
                totalSeconds += Time.deltaTime;
            }
                

            DisplayTime();
        }
        void TogglePause(bool pause = false)
        {
            if(isPaused == pause)return;
            isPaused = pause;
            if(isPaused) DiscreteTimekeeping = 0;
        }
        void DisplayTime()
        {
            int hours = Mathf.FloorToInt(totalSeconds / 3600);
            int minutes = Mathf.FloorToInt(totalSeconds % 3600 / 60);
            int seconds = Mathf.FloorToInt(totalSeconds % 60);
            string timeString = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            gameTimeData.TimeStr = timeString;
            TemporaryStorage.GameTime = totalSeconds;
            TemporaryStorage.GameTimeStr = timeString;
            // Debug.Log("Time Elapsed: " + timeString);

            if(!isPaused)return;
            // 开始暂停计时
            DiscreteTimekeeping += Time.deltaTime;
            int pauseHours = Mathf.FloorToInt(DiscreteTimekeeping / 3600);
            int pauseMinutes = Mathf.FloorToInt(DiscreteTimekeeping % 3600 / 60);
            int pauseSeconds = Mathf.FloorToInt(DiscreteTimekeeping % 60);
            string pauseTimeString = string.Format("{0:00}:{1:00}:{2:00}", pauseHours, pauseMinutes, pauseSeconds);
            pauseTimeData.TimeStr = pauseTimeString;
            TemporaryStorage.PausedTime = DiscreteTimekeeping;
            TemporaryStorage.PausedTimeStr = pauseTimeString;
            // Debug.Log("Time Paused: " + pauseTimeString);
        }
    }
}