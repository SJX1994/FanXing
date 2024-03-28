using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace FanXing
{
    public class SceneChangeCommand : AbstractCommand
    {
        private readonly int mSceneIndex;
        public SceneChangeCommand(int sceneIndex)
        {
            mSceneIndex = sceneIndex;
        }
        protected override void OnExecute()
        {var sceneLoaderSystem = this.GetSystem<ISceneLoaderSystem>();
            sceneLoaderSystem.SceneIndex.Value = mSceneIndex;
            
        }
    }
}

