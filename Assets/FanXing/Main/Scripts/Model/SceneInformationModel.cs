using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
namespace FanXing
{
    public interface ISceneInformationModel : IModel
    {
        List<SceneReference> AllScene { get; set; }
        BindableProperty<SceneReference> CurrentScene { get; set; }
    }
    public class SceneInformationModel: AbstractModel, ISceneInformationModel
    {
        protected override void OnInit()
        {
            
        }

        public BindableProperty<SceneReference> CurrentScene { get; set;} = new BindableProperty<SceneReference>()
        {
            Value = new SceneReference()
        };
        public List<SceneReference> AllScene { get; set; } = new List<SceneReference>();
    }
}
