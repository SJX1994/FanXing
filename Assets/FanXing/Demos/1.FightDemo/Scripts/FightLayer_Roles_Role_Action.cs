using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
namespace FanXing.FightDemo
{
    public class FightLayer_Roles_Role_Action : MonoBehaviour
    {
        protected ScriptableObject_Action scriptableObject_Action;
        void Start()
        {
            TemporaryStorage.OnActionSelected += OnActionSelected;
            TemporaryStorage.OnActionCanceled += OnActionCanceled;
            TemporaryStorage.OnConfirmKeyPressed += () =>
            {
                if( TemporaryStorage.BuoyState != OperateLayer_Buoy.State.Action )return;
                if( TemporaryStorage.BuoySelectingObject != gameObject)return;
                ActionRelease();
            };
        }
        void OnActionSelected(GameObject who_been_selected, ScriptableObject_Action action)
        {
            if(who_been_selected!=gameObject)return;
            // Debug.Log(transform.name+"Action Selected:"+action.name);
            scriptableObject_Action = action;
            ActionPreparation();
        }
        void OnActionCanceled(GameObject who_been_selected)
        {
            if(who_been_selected!=gameObject)return;
            scriptableObject_Action = null;
            // Debug.Log(transform.name+"Action Canceled");
            ActionCanceled();
        }
        // 技能取消
        protected virtual void ActionCanceled()
        {
            
        }
        // 技能预示
        protected virtual void ActionPreparation()
        {
            
        }
        // 技能释放
        protected virtual void ActionRelease()
        {
            TemporaryStorage.InvokeOnActionRelease();
        }
        
    }
}

