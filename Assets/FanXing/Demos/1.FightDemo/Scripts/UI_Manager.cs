using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
namespace FanXing.FightDemo
{
    public class UI_Manager : MonoBehaviour
    {
        [SerializeField] UI_Manager_UnitSimpleDescription unitSimpleDescription;
        [SerializeField] Canvas uiCanvas;
        [SerializeField] UI_Manager_CommandSelect uI_CommandSelect;
        public UI_Manager_CommandSelect UI_CommandSelect => uI_CommandSelect;
        private GameObject selectedObjectAnimation;
        private GameObject SelectedObjectAnimation
        {
            get => selectedObjectAnimation;
            set
            {
                if(selectedObjectAnimation == value){isSameObject = true; return;}
                unitSimpleDescription.transform.localScale = new Vector3(1, 0, 1);
                unitSimpleDescription.transform.DOScaleY(1, 0.3f).SetEase(Ease.OutFlash).OnComplete(() =>
                {
                    isSameObject = false;
                });
                
            }
        }
        bool isSameObject = false;
        void Start()
        {
            uI_CommandSelect.Hide();
            TemporaryStorage.OnConfirmKeyPressed += () =>
            {
                if(TemporaryStorage.buoyState != OperateLayer_Buoy.State.Idle)return;
                uI_CommandSelect.Show();
            };
            TemporaryStorage.OnMove += (graph) =>
            {
                uI_CommandSelect.Hide();
            };
        }
        public void ShowUnitDescription(Camera targetCamera, GameObject gameObject,ScriptableObject_UnitSimpleDescription scriptableObject_unitSimpleDescription)
        {
            TemporaryStorage.BuoySelectedObject = gameObject;

            SelectedObjectAnimation = gameObject;
            if(isSameObject)return;
            // 更新 UI 元素内容
            unitSimpleDescription.UpdateDescription(scriptableObject_unitSimpleDescription);

            unitSimpleDescription.gameObject.SetActive(true);
            // 获取世界坐标
            Vector3 worldPos = gameObject.transform.position;

            // 将世界坐标转换为屏幕坐标
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

            // 将屏幕坐标转换为 UI 坐标
            Vector2 uiPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiCanvas.transform as RectTransform, screenPos, targetCamera, out uiPos);

            // 更新 UI 元素位置
            unitSimpleDescription.GetComponent<RectTransform>().anchoredPosition = uiPos;

        }

        void Update()
        {
            if(SelectedObjectAnimation == null)
            {
                unitSimpleDescription.gameObject.SetActive(false);
            }  
        }
    }
}
