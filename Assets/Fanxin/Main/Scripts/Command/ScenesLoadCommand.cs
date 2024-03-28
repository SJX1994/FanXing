using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace FanXing
{
    public class ScenesLoadCommand : AbstractCommand
    {
        private readonly List<SceneReference> mAllScene;
        public ScenesLoadCommand(List<SceneReference> AllScene)
        {
            mAllScene = AllScene;
        }
        protected override void OnExecute()
        {
            var sceneModel = this.GetModel<ISceneInformationModel>();
            sceneModel.AllScene = mAllScene;
            // foreach (var scene in mAllScene)
            // {
            //     Debug.Log(scene.ScenePath);
            // }
        }
    }
}

