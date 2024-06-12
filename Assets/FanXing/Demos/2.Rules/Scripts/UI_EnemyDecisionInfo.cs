using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using TMPro;
namespace FanXing.Demos.Rules
{
    public class UI_EnemyDecisionInfo : SubUI_Panel
    {
        [SerializeField]
        TextMeshProUGUI textMeshPro;
    
        GameObject who;
        void Start()
        {
            gameObject.SetActive(false);
            Events.RoleDecisionInfo += SetPos;
        }
        void SetPos(GameObject who,bool isOpen, Vector3 worldPosition)
        {
            
            if (isOpen)
            {
                ScriptableObject_UnitAttributes attribute = who.GetComponent<Playable_Unit>().Attributes;
                if(attribute.unitType != ScriptableObject_UnitAttributes.UnitType.Enemy)return;
                this.who = who;
                textMeshPro.text = attribute.UnitName +"\n放大招啦";
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
