using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
namespace FanXing.FightDemo
{
    public class UI_Manager : MonoBehaviour
    {
        [SerializeField] UI_Manager_UnitSimpleDescription uI_UnitSimpleDescription;
        [SerializeField] Canvas uiCanvas;
        [SerializeField] UI_Manager_CommandSelect uI_CommandSelect;
        public UI_Manager_CommandSelect UI_CommandSelect => uI_CommandSelect;
        
        void Start()
        {
            uI_CommandSelect.ExecuteCommand(UI_Manager_CommandSelect.Command.Null);
            TemporaryStorage.OnMove += (who,graph,path,from,to) =>
            {
                uI_CommandSelect.ExecuteCommand(UI_Manager_CommandSelect.Command.Null);
            };
            TemporaryStorage.OnShowUnitDescription += Show_UI_UnitSimpleDescription;
            TemporaryStorage.OnHideUnitDescription += () =>
            {
                if(!uI_UnitSimpleDescription.isShowing)return;
                uI_UnitSimpleDescription.ExecuteCommand(UI_Manager_UnitSimpleDescription.Command.Hide);
            };
            TemporaryStorage.OnShow_UI_Manager += Show_UI_CommandSelect_Data;
        }
        void Show_UI_CommandSelect_Data(GameObject gameObject,ScriptableObject_UI_Manager_DisplayOptions displayOptions)
        {
            if(TemporaryStorage.BuoyState != OperateLayer_Buoy.State.Idle)return;
            uI_CommandSelect.SetData(gameObject,displayOptions);
            uI_CommandSelect.ExecuteCommand(UI_Manager_CommandSelect.Command.Idle);
        }
        void Show_UI_UnitSimpleDescription(GameObject gameObject,ScriptableObject_UnitSimpleDescription scriptableObject_unitSimpleDescription)
        {
            
            Camera targetCamera = TemporaryStorage.UI_Camera;
            // 更新 UI 元素内容
            uI_UnitSimpleDescription.UpdateDescription(scriptableObject_unitSimpleDescription);
            // 获取世界坐标
            Vector3 worldPos = gameObject.transform.position;

            // 将世界坐标转换为屏幕坐标
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            // 将屏幕坐标转换为 UI 坐标
            Vector2 uiPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.transform as RectTransform, screenPos, targetCamera, out uiPos);

            // 更新 UI 元素位置
            uI_UnitSimpleDescription.GetComponent<RectTransform>().anchoredPosition = uiPos;
            if(uI_UnitSimpleDescription.isShowing)return;
            uI_UnitSimpleDescription.ExecuteCommand(UI_Manager_UnitSimpleDescription.Command.Show);
        }

        void Update()
        {
            
        }
    }
}
