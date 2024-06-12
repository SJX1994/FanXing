using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FanXing.Demos.Rules
{
    public class UI_Rules_Visualization : MonoBehaviour
    {
        [SerializeField]
        Button btn_CreateUnit,btn_2D_Geometry,btn_TimeCount;
        [SerializeField]
        TextMeshProUGUI txt_CreatedUnit,txt_2D_Geometry,txt_TimeCount;
        
        
        void Start()
        {
            btn_CreateUnit.onClick.AddListener(CreateUnit);
            btn_2D_Geometry.onClick.AddListener(Geometry_2D);
            btn_TimeCount.onClick.AddListener(TimeCount);
        }
        void Update()
        {
            bool isHovering_CreateUnit = btn_CreateUnit.GetComponent<UI_Rules_Visualization_ButtonListener>().IsHovering;
            txt_CreatedUnit.gameObject.SetActive(isHovering_CreateUnit);
            bool isHovering_2D_Geometry = btn_2D_Geometry.GetComponent<UI_Rules_Visualization_ButtonListener>().IsHovering;
            txt_2D_Geometry.gameObject.SetActive(isHovering_2D_Geometry);
            bool isHovering_TimeCount = btn_TimeCount.GetComponent<UI_Rules_Visualization_ButtonListener>().IsHovering;
            txt_TimeCount.gameObject.SetActive(isHovering_TimeCount);
        }
        void TimeCount()
        {
            TemporaryStorage.Rules_Constituative = TemporaryStorage.Rules_Constituative_State.Rule_three;
            Events.InvokeOnTimeCount();
        }
        void Geometry_2D()
        {
            // Events.InvokeOn2DGeometry();
            TemporaryStorage.Rules_Constituative = TemporaryStorage.Rules_Constituative_State.Rule_two;
            Events.InvokeOnCreate_2D_Geometry();
        }
        void CreateUnit()
        {
            TemporaryStorage.Rules_Constituative = TemporaryStorage.Rules_Constituative_State.Rule_one;
            Events.InvokeOnCreateUnit();
        }
        
    }
}