using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DG.Tweening;
namespace FanXing
{
    public class View_SceneManager : MonoBehaviour,IController
    {
        [SerializeField]
        float logoDisplayTime = 0.5f;
        [SerializeField]
        List<SceneReference> allScenes;
        [SerializeField]
        bool playedLogo = false;
        void Start()
        {
            if(playedLogo)return;
            playedLogo = true;
            DontDestroyOnLoad(this.gameObject);
            this.SendCommand(new ScenesLoadCommand(allScenes));
            this.SendCommand(new SceneChangeCommand(0));
            DOVirtual.DelayedCall(logoDisplayTime, () =>
            {
                this.SendCommand(new SceneChangeCommand(1));
            });
            
        }
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return Fanxing.Interface;
        }
    }
}

