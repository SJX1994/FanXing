using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QFramework;
namespace FanXing.Dialogue
{
    public class Panel_Dialogue : MonoBehaviour,IController
    {
        public TextMeshProUGUI Text_Content;
        public TextMeshProUGUI Text_RoleName;

        public Button Button_Next;
        IDialogueModel mDialogueModel;

        void Start()
        {
            mDialogueModel = this.GetModel<IDialogueModel>();
            LoadDialogue();
            Button_Next.onClick.AddListener(() =>
            {
                NextTalk();
            });
            this.RegisterEvent<OnDialogueChapterLastTalkEvent>(e =>
            {
                this.SendCommand(new SceneChangeCommand(1));
            });
        }
        void LoadDialogue()
        {
            this.SendCommand(new DialogueLoadChapterCommand(mDialogueModel.CurrentChapter.Value));
            Text_Content.text = mDialogueModel.CurrentContent.Value;
            Text_RoleName.text = mDialogueModel.CurrentName.Value;
        }
        void NextTalk()
        {
            this.SendCommand<DialogueNextTalkCommand>();
            Text_Content.text = mDialogueModel.CurrentContent.Value;
            Text_RoleName.text = mDialogueModel.CurrentName.Value;
        }
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return Fanxing.Interface;
        }
    }
}
