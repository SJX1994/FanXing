using UnityEngine;
using System;

namespace FanXing.Demos.Rules
{
    public static class Events
    {
        public static event Action OnClearAllWorldObjects;
        public static event Action OnCreateUnit;
        public static event Action OnTimeCount;
        public static event Action OnCreate_2D_Geometry;
        public static void InvokeOnTimeCount()
        {
            OnTimeCount?.Invoke();
        }
        public static void InvokeOnCreate_2D_Geometry()
        {
            OnCreate_2D_Geometry?.Invoke();
        }
        public static void InvokeOnCreateUnit()
        {
            OnCreateUnit?.Invoke();
        }
        public static void InvokeOnClearAllWorldObjects()
        {
            OnClearAllWorldObjects?.Invoke();
        }
        public static void Reset() 
        {
            InvokeOnClearAllWorldObjects();
            OnCreateUnit = null;
            OnClearAllWorldObjects = null;
            OnCreate_2D_Geometry = null;
            OnTimeCount = null;
        }
        // playable
        public static event Action<bool> OnUnitAuto;
        public static void InvokeEnemyAuto(bool auto = true)
        {
            OnUnitAuto?.Invoke(auto);
        }
        public static event Action<bool> OnToggleTime;
        public static void InvokeToggleTime(bool pause = false)
        {
            if(pause)
            {
                TemporaryStorage.GameTimeIsRunning = false;
                TemporaryStorage.inputState = TemporaryStorage.InputState.Operate;
                //Events.InvokeEnemyAuto(false);
            }
            else
            {
                TemporaryStorage.GameTimeIsRunning = true;
                TemporaryStorage.inputState = TemporaryStorage.InputState.Auto;
                TemporaryStorage.PausedTimeStr = "--:--";
                //Events.InvokeEnemyAuto(true);
            }
            OnToggleTime?.Invoke(pause);
            OnUnitAuto?.Invoke(!pause);
        }
        public static event Action<GameObject,bool,Vector3> RoleDecisionInfo;
        public static void InvokeRoleDecisionInfo(GameObject who ,bool open = true,Vector3 worldPosition = default)
        {
            RoleDecisionInfo?.Invoke(who,open,worldPosition);
        }
        public static event Action<GameObject> OnOpenUIWindow;
        public static void InvokeOpenUIWindow(GameObject uiWindow)
        {
            OnOpenUIWindow?.Invoke(uiWindow);
        }
        public static event Action OnCloseCurrentUIWindow;
        public static void InvokeCloseCurrentUIWindow()
        {
            OnCloseCurrentUIWindow?.Invoke();
        }
        public static event Action OnBackToPreviousUIWindow;
        public static void InvokeBackToPreviousUIWindow()
        {
            OnBackToPreviousUIWindow?.Invoke();
        }
        public static event Action OnCloseAllUIWindow;
        public static void InvokeCloseAllUIWindow()
        {
            OnCloseAllUIWindow?.Invoke();
        }
        public static event Action<GameObject,bool> OnMovePanelOpen;
        public static void InvokeMovePanelOpen(GameObject who, bool open)
        {
            OnMovePanelOpen?.Invoke(who,open);
        }
        public static event Action<ScriptableObject_UnitAttributes[],Sprite[],float[]> OnInitProssesBar;
        public static void InvokeInitProssesBar(ScriptableObject_UnitAttributes[] scriptableObject_UnitAttributes,Sprite[] characterIcons,float[] durations)
        {
            OnInitProssesBar?.Invoke(scriptableObject_UnitAttributes,characterIcons,durations);
        }
        public static event Action<ScriptableObject_UnitAttributes> OnWhoProssesBar_Reset;
        public static void InvokeWhoProssesBar_Reset(ScriptableObject_UnitAttributes scriptableObject_UnitAttributes)
        {
            OnWhoProssesBar_Reset?.Invoke(scriptableObject_UnitAttributes);
        }
        public static event Action<ScriptableObject_UnitAttributes> OnWhoProssesBar_Action;
        public static void InvokeWhoProssesBar_Action(ScriptableObject_UnitAttributes scriptableObject_UnitAttributes)
        {
            OnWhoProssesBar_Action?.Invoke(scriptableObject_UnitAttributes);
        }
        public static void Reset_Playable()
        {
            OnToggleTime = null;
            RoleDecisionInfo = null;
            OnOpenUIWindow = null;
            OnCloseCurrentUIWindow = null;
            OnBackToPreviousUIWindow = null;
            OnMovePanelOpen = null;
            OnCloseAllUIWindow = null;
            OnUnitAuto = null;
            OnInitProssesBar = null;
            OnWhoProssesBar_Action = null;
        }
    }
}