using UnityEngine;
using QFramework;
namespace FanXing
{
    public class DialogueNextTalkCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            var dialogueSystem = this.GetModel<IDialogueModel>();
            dialogueSystem.CurrentTakingIndex.Value++;
        }
    }
}
