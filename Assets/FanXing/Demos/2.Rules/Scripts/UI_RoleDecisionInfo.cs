using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
namespace FanXing.Demos.Rules
{
    public class UI_RoleDecisionInfo : SubUI_Panel
    {
        [SerializeField]
        Button btn_action;
        [SerializeField]
        Button btn_move;
       
        GameObject who;
        void Start()
        {
            
            gameObject.SetActive(false);
            btn_action.interactable = false;
            btn_action.onClick.AddListener(() =>
            {
                if(!who)return;
                ScriptableObject_UnitAttributes su = who.GetComponent<Playable_Unit>().Attributes;
                Events.InvokeWhoProssesBar_Reset(su);
                btn_action.interactable = false;
            });
            btn_move.onClick.AddListener(() =>
            {
                if(!who)return;
                Events.InvokeMovePanelOpen(who,true);
            });
            Events.RoleDecisionInfo += SetPos;
        }
        void SetPos(GameObject who,bool isOpen, Vector3 worldPosition)
        {
            
            if (isOpen)
            {
                if(who.GetComponent<Playable_Unit>().Attributes.unitType != ScriptableObject_UnitAttributes.UnitType.Role)return;
                this.who = who;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);
                transform.position = screenPos;
                if (screenPos.x < Screen.width / 2)
                {
                    // Debug.Log("Target is on the left side of the screen.");
                    transform.GetComponent<RectTransform>().pivot = new Vector2(0, 0f);
                }
                else
                {
                    // Debug.Log("Target is on the right side of the screen.");
                    transform.GetComponent<RectTransform>().pivot = new Vector2(1, 0f);
                }
                if(screenPos.y < Screen.height / 2)
                {
                    transform.GetComponent<RectTransform>().pivot = new Vector2(transform.GetComponent<RectTransform>().pivot.x, 0);
                }
                else
                {
                    transform.GetComponent<RectTransform>().pivot = new Vector2(transform.GetComponent<RectTransform>().pivot.x, 1);
                }
                Events.InvokeOpenUIWindow(gameObject);
                // transform.DOScale(Vector3.one, 0.2f);
            }
            else
            {
                // Tween close = transform.DOScale(Vector3.zero, 0.2f);
                // close.onComplete = () =>
                // {
                //     Events.InvokeCloseCurrentUIWindow();
                // };
                // Events.InvokeCloseCurrentUIWindow();
            }
        }
        
        
    }
}
