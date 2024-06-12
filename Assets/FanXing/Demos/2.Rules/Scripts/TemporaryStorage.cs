using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using DijkstrasPathfinding;
namespace FanXing.Demos.Rules
{
    public static class TemporaryStorage
    {
        public enum Rules_Constituative_State
        {
            Rule_notSet,
            Rule_one,
            Rule_two,
            Rule_three
        }
        private static Rules_Constituative_State rules_Constituative = Rules_Constituative_State.Rule_notSet;
        public static Rules_Constituative_State Rules_Constituative
        {
            get
            {
                return rules_Constituative;
            }
            set
            {
                if(rules_Constituative==value)return;
                Events.InvokeOnClearAllWorldObjects();
                rules_Constituative = value;
            }
        }
        public static void Reset()
        {
            Rules_Constituative = Rules_Constituative_State.Rule_notSet;
        }
        // playable
        public enum InputState
        {
            Placement,
            Operate,
            Auto
        }
        public static InputState inputState = InputState.Placement;
        static bool is_UI_StackManagerEmpty = false;
        public static bool Is_UI_StackManagerEmpty 
        {
            get
            {
                return is_UI_StackManagerEmpty;
            }
            set
            {
                // if(is_UI_StackManagerEmpty==value)return;
                // Debug.Log("UI_StackManager: " + value);
                is_UI_StackManagerEmpty = value;
                if(is_UI_StackManagerEmpty == true)
                {
                    Events.InvokeToggleTime(false);
                }
            }
        
        }
        public static int MaxCharacters = 2;
        private static int currentCharacter = 0;
        public static int CurrentCharacter
        {
            get
            {
                
                return currentCharacter;
            }
            set
            {
                if(currentCharacter==value)return;
                if(value == MaxCharacters)
                {
                    Events.InvokeToggleTime(false);
                }
                currentCharacter = value;
            }
        }
        private static bool gameTimeIsRunning = false;
        public static bool GameTimeIsRunning
        {
            get
            {
                return gameTimeIsRunning;
            }
            set
            {
                if(gameTimeIsRunning==value)return;
                gameTimeIsRunning = value;
            }
        }
        public static float PausedTime;
        public static string PausedTimeStr;
        public static float GameTime;
        public static string GameTimeStr;
        public static void Reset_Playable()
        {
            GameTimeIsRunning = false;
            Is_UI_StackManagerEmpty = false;
            PausedTimeStr = "";
            GameTimeStr = "";
            PausedTime = 0f;
            GameTime = 0f;
            currentCharacter = 0;
            MaxCharacters = 0;
        }
    }
}
