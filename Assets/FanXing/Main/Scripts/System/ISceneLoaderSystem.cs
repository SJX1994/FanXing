using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using FanXing;
using QFramework;
namespace FanXing
{
    public interface ISceneLoaderSystem : ISystem
    {
        BindableProperty<int> SceneIndex { get; set;}
        void LoadSceneIndex(int index);
    }
    public class SceneLoaderSystem : AbstractSystem,ISceneLoaderSystem
    {
       
        public BindableProperty<int> SceneIndex { get; set;} = new BindableProperty<int>()
        {
            Value = 0
        };
        protected override void OnInit()
        {
            SceneIndex.Register(index =>
            {
                LoadSceneIndex(index);
            });
        }
        public void LoadSceneIndex(int index)
        {
            var sceneModel = this.GetModel<ISceneInformationModel>();
            SceneManager.LoadScene(sceneModel.AllScene[index].ScenePath);
            sceneModel.CurrentScene.Value = sceneModel.AllScene[index];
            // Debug.Log(sceneModel.CurrentScene.Value.ScenePath);
        }
    }
}


