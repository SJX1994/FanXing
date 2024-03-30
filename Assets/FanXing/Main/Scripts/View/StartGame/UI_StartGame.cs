using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DG.Tweening;
namespace FanXing.StartGame
{
    public class UI_StartGame : MonoBehaviour,IController
    {
        [SerializeField]
        Panel_StartGame panel_StartGame;
        [SerializeField]
        Panel_NewGame panel_NewGame;
        [SerializeField]
        Panel_SetUp panel_SetUp;
        [SerializeField]
        Panel_Extras panel_Extras;
        
        void Start()
        {
            panel_StartGame.btn_NewGame.onClick.AddListener(LoadPanel_NewGame);
            panel_StartGame.btn_SetUp.onClick.AddListener(LoadPanel_SetUp);
            panel_StartGame.btn_Extras.onClick.AddListener(LoadPanel_Extras);
            panel_SetUp.btn_Back.onClick.AddListener(LoadPanel_StartGame);
            panel_Extras.btn_Back.onClick.AddListener(LoadPanel_StartGame); 
        }
        void LoadPanel_NewGame()
        {
            panel_NewGame.gameObject.SetActive(true);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                this.SendCommand(new SceneChangeCommand(2));
            });
            panel_StartGame.gameObject.SetActive(false);
        }
        void LoadPanel_SetUp()
        {
            panel_SetUp.gameObject.SetActive(true);
            panel_StartGame.gameObject.SetActive(false);
        }
        void LoadPanel_Extras()
        {
            panel_Extras.gameObject.SetActive(true);
            panel_StartGame.gameObject.SetActive(false);
        }
        void LoadPanel_StartGame()
        {
            panel_StartGame.gameObject.SetActive(true);
            panel_NewGame.gameObject.SetActive(false);
            panel_SetUp.gameObject.SetActive(false);
            panel_Extras.gameObject.SetActive(false);
        }
        void OnDestroy()
        {
            panel_StartGame.btn_NewGame.onClick.RemoveAllListeners();
            panel_StartGame.btn_SetUp.onClick.RemoveAllListeners();
            panel_StartGame.btn_Extras.onClick.RemoveAllListeners();
            panel_SetUp.btn_Back.onClick.RemoveAllListeners();
            panel_Extras.btn_Back.onClick.RemoveAllListeners();
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return Fanxing.Interface;
        }
    }
}
