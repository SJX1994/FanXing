using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FanXing.Demos.Rules
{
    public class InputControl : MonoBehaviour
    {
        
        
        string substringToCheck_RoleName = "Role_No"; // 要检查的字符串
       
        void Start()
        {
          
        }
        
        void Update()
        {
            switch (TemporaryStorage.inputState)
            {
                case TemporaryStorage.InputState.Placement:
                    break;
                case TemporaryStorage.InputState.Operate:
                    Operate_Update();
                    break;
                case TemporaryStorage.InputState.Auto:
                    Auto_Update();
                    break;
            }
        }
        void Operate_Update()
        {
            if (Input.GetMouseButtonDown(1)) // 在鼠标右键按下时执行射线检测
            {
                // Events.InvokeRoleDecisionInfo(false);
                Events.InvokeCloseCurrentUIWindow();
                // Events.InvokeToggleTime(false);
            }
        }
        void Auto_Update()
        {
            if (Input.GetMouseButtonDown(0)) // 在鼠标左键按下时执行射线检测
            {
                // 创建射线
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // 检测射线是否击中世界坐标下的物体
                if (Physics.Raycast(ray, out hit))
                {
                    string input = hit.collider.gameObject.name;
                    if (input.Contains(substringToCheck_RoleName))
                    {
                        // 检测到击中物体
                        // Debug.Log("Hit object: " + hit.collider.gameObject.name);
                        // Events.InvokeToggleTime(true);
                        Events.InvokeRoleDecisionInfo( hit.collider.gameObject , true, hit.point);
                    }
                }
            }
            if (Input.GetMouseButtonDown(1)) // 在鼠标右键按下时执行射线检测
            {
                Events.InvokeCloseCurrentUIWindow();
                // Events.InvokeToggleTime(false);
            }
        }
    }
}
