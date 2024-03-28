using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace FanXing
{
    public class Fanxing : Architecture<Fanxing>
    {
        protected override void Init()
        {
            RegisterSystem<ISceneLoaderSystem>(new SceneLoaderSystem());
            RegisterSystem<IDialogueSystem>(new DialogueSystem());
            
            RegisterModel<ISceneInformationModel>(new SceneInformationModel());
            RegisterModel<IDialogueModel>(new DialogueModel());
        }
    }
}
