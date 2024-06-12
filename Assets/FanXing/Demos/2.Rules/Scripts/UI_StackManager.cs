using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FanXing.Demos.Rules
{
    public class UI_StackManager : MonoBehaviour
    {
        private Stack<GameObject> uiStack = new Stack<GameObject>();
        void Start()
        {
            Events.OnOpenUIWindow += OpenUIWindow;
            Events.OnCloseCurrentUIWindow += CloseCurrentUIWindow;
            Events.OnBackToPreviousUIWindow += BackToPreviousUIWindow;
            Events.OnCloseAllUIWindow += CloseAllUIWindow;
        }

        public void OpenUIWindow(GameObject uiWindow)
        {
            if (uiStack.Count > 0)
            {
                // 关闭当前UI窗口
                GameObject currentWindow = uiStack.Peek();
                currentWindow.SetActive(false);
            }
            // 打开新的UI窗口
            uiWindow.SetActive(true);
            // 将其添加到堆栈中
            uiStack.Push(uiWindow);
            Events.InvokeToggleTime(true);
        }
        public void CloseAllUIWindow()
        {
            while (uiStack.Count > 0)
            {
                GameObject currentWindow = uiStack.Pop();
                currentWindow.SetActive(false);
            }
            Events.InvokeToggleTime(false);
        }

        public void CloseCurrentUIWindow()
        {
            if (uiStack.Count > 0)
            {
                // 关闭当前UI窗口
                GameObject currentWindow = uiStack.Pop();
                currentWindow.SetActive(false);
                currentWindow = uiStack.Count > 0 ? uiStack.Peek() : null;
                if (currentWindow != null)
                {
                    currentWindow.SetActive(true);
                }
                Events.InvokeToggleTime(true);
            }
            
            TemporaryStorage.Is_UI_StackManagerEmpty = uiStack.Count == 0;
            // Debug.Log("UI_StackManager: " + TemporaryStorage.Is_UI_StackManagerEmpty);
        }

        public void BackToPreviousUIWindow()
        {
            if (uiStack.Count > 1)
            {
                // 关闭当前UI窗口
                GameObject currentWindow = uiStack.Pop();
                currentWindow.SetActive(false);

                // 返回到上一级UI窗口
                GameObject previousWindow = uiStack.Peek();
                previousWindow.SetActive(true);

                Events.InvokeToggleTime(true);
            }else
            {
                Events.InvokeToggleTime(false);
            }
        }
      
    }
}
