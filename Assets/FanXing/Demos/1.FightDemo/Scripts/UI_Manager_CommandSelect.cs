using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MonsterLove.StateMachine;
using TMPro;
using System;

namespace FanXing.FightDemo
{
    public class UI_Manager_CommandSelect : MonoBehaviour
    {
        [SerializeField] RectTransform command_buttons_panel;
        [SerializeField] RectTransform action_buttons_panel;
        [SerializeField] Button btn_Command_Move,btn_Command_Action,btn_Command_Defense;
        [SerializeField] Button btn_Action_0,btn_Action_1,btn_Action_2,btn_Action_3,btn_Action_4,btn_Action_5;
        Dictionary<Button,ScriptableObject_Action> dic_ActionUI_DisplayOptions = new();
        private StateMachine<States, Driver> fsm;
        private GameObject who_been_selected;
        public enum Command
        {
            Null,
            DisplayOptions,
            Idle,
            Move,
            Action,
            ActionInOperation,
            ActionRelease,
            Defense,
            
        }
        public enum States
        {
            Null,
            Idle,
            Action,
            ActionInOperation,
            ActionRelease,
            Move
        }
        public class Driver
        {
            public StateEvent Update;
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
            List<Button> action_buttons = new List<Button>(){btn_Action_0,btn_Action_1,btn_Action_2,btn_Action_3,btn_Action_4,btn_Action_5};
            foreach(Button btn in action_buttons)
            {
                btn.onClick.AddListener(() =>
                {
                    if(dic_ActionUI_DisplayOptions.ContainsKey(btn))
                    {
                        TemporaryStorage.InvokeOnActionSelected(who_been_selected,dic_ActionUI_DisplayOptions[btn]);
                        ExecuteCommand(Command.ActionInOperation);
                    }
                });
            }
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
                    case States.ActionInOperation:
                        ExecuteCommand(Command.Action);
                        break;
                    case States.Null:
                        break;
                    default:
                        break;
                }
            };
            TemporaryStorage.OnActionRelease += () =>
            {
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    ExecuteCommand(Command.ActionRelease);
                });
                
            };
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
                case Command.ActionInOperation:
                    fsm.ChangeState(States.ActionInOperation);
                    break;
                case Command.ActionRelease:
                    fsm.ChangeState(States.ActionRelease);
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
        public void SetData(GameObject who,ScriptableObject_UI_Manager_DisplayOptions displayOptions)
        {
            who_been_selected = who;
            btn_Command_Move.interactable = displayOptions.CommandSelectSystem_Button_Move;
            btn_Command_Action.interactable = displayOptions.CommandSelectSystem_Button_Fight;
            btn_Command_Defense.interactable = displayOptions.CommandSelectSystem_Button_Defense;
            Action_Data_Processing(btn_Action_0,displayOptions.ActionUI_Button_0);
            Action_Data_Processing(btn_Action_1,displayOptions.ActionUI_Button_1);
            Action_Data_Processing(btn_Action_2,displayOptions.ActionUI_Button_2);
            Action_Data_Processing(btn_Action_3,displayOptions.ActionUI_Button_3);
            Action_Data_Processing(btn_Action_4,displayOptions.ActionUI_Button_4);
            Action_Data_Processing(btn_Action_5,displayOptions.ActionUI_Button_5);
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
        
        void Null_Enter()
        {
            TemporaryStorage.BuoyState = OperateLayer_Buoy.State.Idle;
            TemporaryStorage.InvokeOnOperating(false);
            Hide_Command_Buttons();
            Hide_Action_Buttons();
        }
        void Idle_Enter()
        {
            TemporaryStorage.BuoyState = OperateLayer_Buoy.State.Idle;
            TemporaryStorage.InvokeOnOperating(true);
            Show_Command_Buttons();
            Hide_Move_Buttons();
            Hide_Action_Buttons();
        }
        void Action_Enter()
        {
            TemporaryStorage.InvokeOnOperating(true);
            Hide_Command_Buttons();
            Hide_Move_Buttons();
            Show_Action_Buttons();
        }
        void ActionInOperation_Enter()
        {
            TemporaryStorage.InvokeOnOperating(true);
            TemporaryStorage.InvokeOnHideUnitDescription();
            Hide_Command_Buttons();
            Hide_Move_Buttons();
            Hide_Action_Buttons();
        }
        void ActionInOperation_Update()
        {
            if(fsm.State != States.ActionInOperation)return;
            TemporaryStorage.BuoyState = OperateLayer_Buoy.State.Action;
            
        }
        void ActionInOperation_Exit()
        {
            TemporaryStorage.InvokeOnActionCanceled(who_been_selected);
        }
        void ActionRelease_Enter()
        {
            TemporaryStorage.InvokeOnOperating(false);
            TemporaryStorage.BuoyState = OperateLayer_Buoy.State.ActionExecute;
        }
        void Move_Enter()
        {
            TemporaryStorage.BuoyState = OperateLayer_Buoy.State.MoveOmen;
            TemporaryStorage.InvokeOnOperating(true);
            Show_Move_Buttons();
            Hide_Command_Buttons();
            Hide_Action_Buttons();
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
        void Action_Data_Processing(Button target_Btn_Action,ScriptableObject_UI_Manager_DisplayOptions.ActionUI_DisplayOptions action_DisplayOptions)
        {
            TextMeshProUGUI textMeshProUI = target_Btn_Action.GetComponentInChildren<TextMeshProUGUI>(true);
            textMeshProUI.text = "";
            target_Btn_Action.interactable = action_DisplayOptions.Display;
            if(!action_DisplayOptions.Content)
            {
                textMeshProUI.text = "未解锁栏位";
            }else
            {
                textMeshProUI.text = action_DisplayOptions.Content.ActionName;
                if(!dic_ActionUI_DisplayOptions.ContainsKey(target_Btn_Action))
                {
                    dic_ActionUI_DisplayOptions.Add(target_Btn_Action,action_DisplayOptions.Content);
                }else
                {
                    dic_ActionUI_DisplayOptions[target_Btn_Action] = action_DisplayOptions.Content;
                }
                if(action_DisplayOptions.Content.IsUnlocked)return;
                textMeshProUI.text += " (锁定中)";
                target_Btn_Action.interactable = false;
            }
        }
    }

}

