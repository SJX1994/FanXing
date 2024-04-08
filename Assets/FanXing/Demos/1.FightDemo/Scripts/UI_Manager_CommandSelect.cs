using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MonsterLove.StateMachine;
using QFramework;

namespace FanXing.FightDemo
{
    public class UI_Manager_CommandSelect : MonoBehaviour
    {
        [SerializeField] RectTransform command_buttons_panel;
        [SerializeField] RectTransform action_buttons_panel;
        public Button btn_Command_Move,btn_Command_Action,btn_Command_Defense;
        public Button btn_Action_0,btn_Action_1,btn_Action_2,btn_Action_3,btn_Action_4,btn_Action_5;
        private StateMachine<States, Driver> fsm;
        public enum Command
        {
            Null,
            DisplayOptions,
            Idle,
            Move,
            Action,
            Defense,
            
        }
        public enum States
        {
            Null,
            Idle,
            Action,
            Move
        }
        public class Driver
        {
            public StateEvent Update;
        }
        void Awake()
        {
            //Initialize the state machine
            fsm = new StateMachine<States, Driver>(this);
            fsm.ChangeState(States.Null); //Remember to set an initial state!
        }

        private void Update()
        {
            fsm.Driver.Update.Invoke(); //Tap the state machine into Unity's update loop. We could choose to call this from anywhere though!
        }
        void Start()
        {
            btn_Command_Move.onClick.AddListener(() =>
            {
                ExecuteCommand(Command.Move);
            });
            btn_Command_Action.onClick.AddListener(() =>
            {
                ExecuteCommand(Command.Action);
            });
            TemporaryStorage.OnCancelKeyPressed += () =>
            {
                switch(fsm.State)
                {
                    case States.Idle:
                        ExecuteCommand(Command.Null);
                        break;
                    case States.Move:
                        ExecuteCommand(Command.Idle);
                        break;
                    case States.Action:
                        ExecuteCommand(Command.Idle);
                        break;
                    case States.Null:
                        break;
                    default:
                        break;
                }
            };
        }
        void Null_Enter()
        {
            TemporaryStorage.InvokeOnOperatePostProcessing(false);
            Hide_Command_Buttons();
            Hide_Action_Buttons();
        }
        void Idle_Enter()
        {
            TemporaryStorage.InvokeOnOperatePostProcessing(true);
            Show_Command_Buttons();
            Hide_Move_Buttons();
            Hide_Action_Buttons();
        }
        void Action_Enter()
        {
            TemporaryStorage.InvokeOnOperatePostProcessing(true);
            Hide_Command_Buttons();
            Hide_Move_Buttons();
            Show_Action_Buttons();
        }
        void Move_Enter()
        {
            TemporaryStorage.InvokeOnOperatePostProcessing(true);
            Show_Move_Buttons();
            Hide_Command_Buttons();
            Hide_Action_Buttons();
        }
        public void ExecuteCommand(Command command)
        {
            switch (command)
            {
                case Command.Idle:
                    fsm.ChangeState(States.Idle);
                    break;
                case Command.Move:
                    fsm.ChangeState(States.Move);
                    break;
                case Command.Action:
                    fsm.ChangeState(States.Action);
                    break;
                case Command.Defense:
                    break;
                case Command.Null:
                    fsm.ChangeState(States.Null);
                    break;
            
                default:
                    break;
            }
        }
        public States GetCurrentState()
        {
            return fsm.State;
        }
        void Show_Move_Buttons()
        {
            TemporaryStorage.InvokeOnShow_UI_MovePreparation();
            TemporaryStorage.InvokeOnHideUnitDescription();
        }
        void Hide_Move_Buttons()
        {
            TemporaryStorage.InvokeOnHide_UI_MovePreparation();
        }
        void Show_Action_Buttons()
        {
            if(action_buttons_panel.gameObject.activeSelf)return;
            action_buttons_panel.gameObject.SetActive(true);
            action_buttons_panel.localScale = new Vector3(1, 0, 1);
            action_buttons_panel.DOScaleY(1, 0.3f).SetEase(Ease.OutSine);
        }
        void Hide_Action_Buttons()
        {
            if(!action_buttons_panel.gameObject.activeSelf)return;
            action_buttons_panel.DOScaleY(0, 0.3f).SetEase(Ease.InSine).OnComplete(() =>
            {
                action_buttons_panel.gameObject.SetActive(false);
            });
        }
        void Show_Command_Buttons()
        {
           if(command_buttons_panel.gameObject.activeSelf)return;
            // gameObject.SetActive(true);
            // gameObject.transform.GetComponent<Image>().enabled = true;
            command_buttons_panel.gameObject.SetActive(true);
            command_buttons_panel.localScale = new Vector3(1, 0, 1);
            command_buttons_panel.DOScaleY(1, 0.3f).SetEase(Ease.OutSine);
        }
        void Hide_Command_Buttons()
        {
            if(!command_buttons_panel.gameObject.activeSelf)return;
            command_buttons_panel.DOScaleY(0, 0.3f).SetEase(Ease.InSine).OnComplete(() =>
            {
                command_buttons_panel.gameObject.SetActive(false);
            });
        }
        
    }

}

