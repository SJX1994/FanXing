using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
namespace FanXing.Demos.Rules
{
    public class UI_RoleDec_Move : SubUI_Panel
    {
        void Start()
        {
            gameObject.SetActive(false);
            Events.OnMovePanelOpen += OpenMovePanel;
        }
        void OpenMovePanel(GameObject who,bool isOpen)
        {
            if (isOpen)
            {
                Events.InvokeOpenUIWindow(gameObject);
                
            }
            else
            {
                // Events.InvokeCloseCurrentUIWindow();
            }
        }
        
        
    }
}
