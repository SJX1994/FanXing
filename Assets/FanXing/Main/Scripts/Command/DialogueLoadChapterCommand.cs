using UnityEngine;
using QFramework;
namespace FanXing
{
    public class DialogueLoadChapterCommand : AbstractCommand
    {
        private int mChapterIndex;
        public DialogueLoadChapterCommand(int chapterIndex)
        {
            mChapterIndex = chapterIndex;
        }
        protected override void OnExecute()
        {
            Debug.Log("Load Chapter " + mChapterIndex);
            var dialogueSystem = this.GetModel<IDialogueModel>();
            dialogueSystem.ParseChapter(mChapterIndex);
        }
    }
}