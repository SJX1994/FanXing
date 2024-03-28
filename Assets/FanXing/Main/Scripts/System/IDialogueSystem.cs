using UnityEngine;
using QFramework;
namespace FanXing
{
    public interface IDialogueSystem : ISystem
    {
        
    }
    public class DialogueSystem : AbstractSystem,IDialogueSystem
    {
        protected override void OnInit()
        {
            var gameModel = this.GetModel<IDialogueModel>();
            
        }
    }
}