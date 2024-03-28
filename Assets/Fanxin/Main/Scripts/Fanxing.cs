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

            RegisterModel<ISceneInformationModel>(new SceneInformationModel());
        }
    }
}
